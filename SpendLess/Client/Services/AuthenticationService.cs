using Newtonsoft.Json;
using Serilog;
using SpendLess.Shared;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;

namespace SpendLess.Client.Services
{
    public class AuthenticationService
    {
        public AuthenticationService(IHttpClientFactory clientFactory, ILocalStorageService LocalStorage, AuthenticationStateProvider authStateProvider)
        {
            _clientFactory = clientFactory;
            _localStorage = LocalStorage;
            _authStateprovider = authStateProvider;
        }

        private readonly ILogger<AuthenticationService> _logger;
        private readonly AuthenticationStateProvider _authStateprovider;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalStorageService _localStorage;
        private UserDto? UserInfo;

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
            if (string.IsNullOrWhiteSpace(email))
            {
                return "Email is required";

            }
            if (!Regex.IsMatch(email, @"^([\w]+)@([\w]+)\.(((\w){2,3})+)$"))
                return "Email format is incorrect";
            else
                return null;

        }
        public async Task<bool> CreateAccount(UserDto user)
        {
            var client = _clientFactory.CreateClient();
            try
            {
                string serializedUser = JsonConvert.SerializeObject(user);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7290/api/User/register");
                requestMessage.Content = new StringContent(serializedUser);
                requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                
                var response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    //SnackBarService.WarningMsg(response.Content.ToString());
                    var content = response.Content.ReadAsStringAsync();
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    if (result != null)
                    {
                        if (result.token != null)
                        {
                            await _localStorage.SetItemAsync("token", result.token);
                            await _authStateprovider.GetAuthenticationStateAsync();
                            //SnackBarService.SuccessMsg("Account has been created");
                            return true;
                        }
                        else
                        {
                            //SnackBarService.ErrorMsg(result.message);
                            return false;
                        }
                    }
                    else
                    {
                        //SnackBarService.ErrorMsg("Unexpected error 1");
                        return false;
                    }
                }
                else
                {
                    //SnackBarService.ErrorMsg("Unexpected error 2");
                    return false;
                }
            }
            catch(ArgumentNullException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (NullReferenceException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (JsonException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (Exception ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }



        }


        public async Task<bool> GetLoginAuthentication(UserDto user)
        {

            var client = _clientFactory.CreateClient();
            try
            {
                string serializedUser = JsonConvert.SerializeObject(user);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7290/api/User/login");
                requestMessage.Content = new StringContent(serializedUser);
                requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");               
                var response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    if (result.token != null)
                    {
                        await _localStorage.SetItemAsync("token", result.token);
                        await _authStateprovider.GetAuthenticationStateAsync();
                        //SnackBarService.SuccessMsg("Logged in");
                        return true;
                    }
                    else
                    {
                        //SnackBarService.ErrorMsg(result.message);
                        return false;
                    }
                }
                else
                {
                   // SnackBarService.ErrorMsg("Server could not be reached.");
                    return false;
                }
            }
            catch (ArgumentNullException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (NullReferenceException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (JsonException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (Exception ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }


        }

    }
}
