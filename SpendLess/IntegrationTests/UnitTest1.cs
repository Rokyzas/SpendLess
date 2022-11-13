using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;
using SpendLess.Shared;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IntegrationTests
{
    public class UnitTest1
    {
        public UnitTest1()
        { 
            var appFactory = new WebApplicationFactory<Program>();
            _httpClient = appFactory.CreateClient();    
        }
        private HttpClient _httpClient;
        [Fact]
        public async void Test1()
        {
            var user = new UserDto
            {
                Email = "petras@gmail.com",
                Password= "Petras123"
            };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7290/api/User/login", user);

            var success = response.IsSuccessStatusCode;
        }
    }
}