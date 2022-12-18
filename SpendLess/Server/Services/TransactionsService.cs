using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendLess.Server.Models;
using SpendLess.Shared;
using System.Security.Claims;

namespace SpendLess.Server.Services
{
    public class TransactionsService : ITransactionsService   
    {
        public async Task<List<Transactions>> GetTransactions(SpendLessContext _context, HttpContext _httpContext)
        {

            var identity = _httpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;
            string email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            var transactions = await _context.Transactions.Where(t => t.UserId == user.Id).ToListAsync();

            return transactions;
        }

        public async Task<int?> AddTransaction(Transactions? transaction, SpendLessContext _context, HttpContext _httpContext)
        {
            var identity = _httpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;
            string email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            transaction.UserId = user.Id;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction.Id;
        }

        public async Task<List<Transactions?>> AddPeriodicTransaction(List<Transactions?> transactions, SpendLessContext _context, HttpContext _httpContext)
        {
            var identity = _httpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;
            string email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            foreach (var transaction in transactions)
            {
                transaction.UserId = user.Id;
                _context.Transactions.Add(transaction);
            }
            await _context.SaveChangesAsync();

            return transactions;
        }

        public async Task DeleteTransaction(int id, SpendLessContext _context)
        {
            var transaction = new Transactions(id, 0, "null", DateTime.MinValue);
            _context.Transactions.Attach(transaction);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
