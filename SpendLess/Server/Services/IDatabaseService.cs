using SpendLess.Shared;

namespace SpendLess.Server.Services
{
    public interface IDatabaseService
    {
        Task AddNewUserAsync(User newUser);
        Task<byte[]?> GetUserPasswordHashAsync(UserDto request);
        Task<byte[]?> GetUserPasswordSaltAsync(UserDto request);
        Task<bool> FindEmail(string email);
        Task SaveChangesAsync();
    }
}