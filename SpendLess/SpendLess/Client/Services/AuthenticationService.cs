using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using SpendLess.Shared;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SpendLess.Client.Services
{
    public class AuthenticationService
    { 
        public AuthenticationService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        private readonly IHttpClientFactory clientFactory;
        private UserConnect? UserInfo;

        public string? CheckPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return "Password is required!";
            }
            if (password.Length < 8)
                return "Password must be at least of length 8";
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(password, @"[a-z]"))
                return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(password, @"[0-9]"))
                return "Password must contain at least one digit";
            else
                return null;
        }


        public string? CheckEmail(string email)
        {
            if (email == null)
            {
                return "Email is required";

            }
            if (!Regex.IsMatch(email, @"^([\w]+)@([\w]+)\.(((\w){2,3})+)$"))
                return "Email format is incorrect";
            else
                return null;

        }
        public async Task<bool> CreateAccount(string? username, string? email, string? password)
        {
            UserConnect UserInfo = new UserConnect();
            UserInfo.username = username;
            UserInfo.emailAddress = email;
            UserInfo.password = password;

            string serializedUser = JsonConvert.SerializeObject(UserInfo);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7008/api/User/CreateAccount");
            requestMessage.Content = new StringContent(serializedUser);
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);
            if (await response.Content.ReadFromJsonAsync<bool>())
            {
                return true;
            }
            return false;
        }
        

        public async Task<bool> GetLoginAuthentication(string? email, string? password)
        {
            UserConnect UserInfo = new UserConnect();
            UserInfo.emailAddress = email;
            UserInfo.password = password;
            string serializedUser = JsonConvert.SerializeObject(UserInfo);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7008/api/User/CheckLogin");
            requestMessage.Content = new StringContent(serializedUser);
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);
            if (await response.Content.ReadFromJsonAsync<bool>())
            {
                return true;
            }
            return false;

        }
        
    }
}
