﻿using SpendLess.Server.Middleware;

namespace SpendLess.Server.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitingMiddleware>();
    }
}

