using SpendLess.Shared;

namespace SpendLess.Client.Services
{
    public interface IAuthenticationService
    {
        public string? CheckEmail(string email);
        public string? CheckPassword(string password);
        public Task<bool> CreateAccount(UserDto user);
        public Task<bool> GetLoginAuthentication(UserDto user);
        public Task<bool> ValidateLogin(bool success, string email, string username, string password);
    }
}