using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace SpendLess.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _clientFactory;
        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, IHttpClientFactory clientFactory)
        {
            _localStorage = localStorage;

            _clientFactory = clientFactory;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // string token = await _localStorage.GetItemAsStringAsync("token");

            //string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoia2F6eXNAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2Njc1NzczODV9.Rzqx7jDdTN_NP5Wfinp51iCqNSMel4oW3PKMkDkOmdqBNxyOzDDji3IIbXiaMyE--MUvIF28f8IjYyVfhVwrNg";
            string token = await _localStorage.GetItemAsStringAsync("token");
            //string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.1sp43CTKXre7LRlfiRtLw56UiPmeYphY6cMYiXhk1d8";
            var httpClient = _clientFactory.CreateClient();
            var identity = new ClaimsIdentity();


            httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token.Replace("\"", ""));
                var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tokenExp)).DateTime;

                if (expirationTime > DateTime.UtcNow)
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                    httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
                }
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
         
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
