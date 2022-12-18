using System.Net;
using Microsoft.Extensions.Caching.Distributed;
using SpendLess.Server.Extensions;
using SpendLess.Server.Middleware.Decorators;

namespace SpendLess.Server.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public RateLimitingMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var currentDate = DateTime.UtcNow;
            var rateLimitingDecorator = endpoint?.Metadata.GetMetadata<LimitRequests>();

            if (rateLimitingDecorator is null)
            {
                await _next(context);
                return;
            }

            var key = GenerateClientKey(context);
            var clientStatistics = await GetClientStatisticsByKey(key);

            if (clientStatistics != null && currentDate < clientStatistics.LastSuccessfulResponseTime.AddSeconds(rateLimitingDecorator.TimeWindow) && clientStatistics.NumberOfRequestsCompletedSuccessfully == rateLimitingDecorator.MaxRequests)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                return;
            }
            await UpdateClientStatisticsStorage(key, rateLimitingDecorator.MaxRequests);
            await _next(context);
        }

        private static string GenerateClientKey(HttpContext context) => 
            $"{context.Request.Method}_{context.Connection.RemoteIpAddress}";

        private async Task<ClientStatistics> GetClientStatisticsByKey(string key) => await GetCacheValueAsync(_cache, key);

        protected virtual async Task<ClientStatistics> GetCacheValueAsync(IDistributedCache cache, string key) =>
            await cache.GetCacheValueAsync<ClientStatistics>(key);

        protected virtual async Task SetCahceValueAsync(IDistributedCache cache, string key, ClientStatistics? clientStat) =>
            await cache.SetCahceValueAsync(key, clientStat);

        private async Task UpdateClientStatisticsStorage(string key, int maxRequests)
        {
            var clientStat = await GetCacheValueAsync(_cache, key);
            if (clientStat != null)
            {
                clientStat.LastSuccessfulResponseTime = DateTime.UtcNow;

                if (clientStat.NumberOfRequestsCompletedSuccessfully == maxRequests)
                    clientStat.NumberOfRequestsCompletedSuccessfully = 1;

                else
                    clientStat.NumberOfRequestsCompletedSuccessfully++;

                await SetCahceValueAsync(_cache, key, clientStat);
            }
            else
            {
                var clientStatistics = new ClientStatistics
                {
                    LastSuccessfulResponseTime = DateTime.UtcNow,
                    NumberOfRequestsCompletedSuccessfully = 1
                };
                await SetCahceValueAsync(_cache, key, clientStat);
            }
        }
    }

    public class ClientStatistics
    {
        public DateTime LastSuccessfulResponseTime { get; set; }
        public int NumberOfRequestsCompletedSuccessfully { get; set; }
    }

}
