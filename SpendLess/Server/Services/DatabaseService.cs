using Microsoft.EntityFrameworkCore;
using SpendLess.Shared;

namespace SpendLess.Server.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SpendLessContext _context;


        public DatabaseService(SpendLessContext context)
        {
            _context = context;
        }

        public async Task<bool> FindEmail(string email) =>
            await _context.Users.AnyAsync(o => o.Email == email);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
         
        public async Task AddNewUserAsync(User newUser) =>
            await _context.Users.AddAsync(newUser);

        public async Task<byte[]?> GetUserPasswordHashAsync(UserDto request) =>
                 await _context.Users
                .Where(user => user.Email.ToLower().Contains(request!.Email!.ToLower()))
                .Select(user => user.PasswordHash)
                .FirstOrDefaultAsync();

        public async Task<byte[]?> GetUserPasswordSaltAsync(UserDto request) =>
         await _context.Users
        .Where(user => user.Email.ToLower().Contains(request!.Email!.ToLower()))
        .Select(user => user.PasswordSalt)
        .FirstOrDefaultAsync();
    }
}
