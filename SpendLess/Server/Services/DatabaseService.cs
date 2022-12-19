using Microsoft.EntityFrameworkCore;
using SpendLess.Server.Models;
using SpendLess.Shared;
using System.Runtime.CompilerServices;

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

        public async Task AddTransaction(Transactions transaction) =>
            _context.Transactions.Add(transaction);

        public async Task<User> GetUser(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<List<Transactions>> GetTransactionsAsync(int userId) =>
            await _context.Transactions.Where(t => t.UserId == userId).ToListAsync();

        public async Task RemoveTransaction(Transactions transaction){
            _context.Transactions.Attach(transaction);
            _context.Transactions.Remove(transaction);
        }
    }
}
