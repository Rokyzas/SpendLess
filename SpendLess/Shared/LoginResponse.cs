using System.Text.Json.Serialization;

namespace SpendLess.Shared
{
    public class LoginResponse
    {
        public string? token { get; set; }
        public string message { get; set; }

        [JsonConstructor]
        public LoginResponse(string? token, string message)
        {
            this.token = token;
            this.message = message;

        }
    }


}
