

using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendLess.Server.Interceptor;
using SpendLess.Server.Middleware.Decorators;
using SpendLess.Shared;
using System.Security.Claims;
using System.Text.Json;

namespace SpendLess.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly SpendLessContext _context;

        public TransactionsController(SpendLessContext context)
        {
            _context = context;
        }

        [HttpGet("GetTransactions")]
        public async Task<ActionResult<List<Transactions>>> GetTransactions()
        {
            var header = Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
            var token = header.Value.ToString();
            var email = ParseEmailFromJwt(token);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            var transactions = await _context.Transactions.Where(t => t.UserId == user.Id).ToListAsync();
            
            return Ok(transactions);
            /*Dictionary<string, string> requestHeaders =
             new Dictionary<string, string>();
            foreach (var header in Request.Headers)
            {
                requestHeaders.Add(header.Key, header.Value);
            }
            string word1 = requestHeaders.GetValueOrDefault("Authorization");


            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;
            string word = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
            if (word == null)
            {
                word = "not auth";
            }
            Transaction? transaction = new Transaction(35, 55.5, word, DateTime.Today, "kebab");


            //   List<Transaction> listas2 = await _context.Transactions.ToListAsync();
            List<Transaction> listas = new List<Transaction>();
            listas.Add(transaction);
            return Ok(listas);*/
        }

        [HttpPost("AddTransaction")]
        [LimitRequests(MaxRequests = 1, TimeWindow = 1)]
        public async Task<ActionResult<int?>> AddTransaction([FromBody] Transactions? transaction)
        {
            var header = Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
            var token = header.Value.ToString();
            var email = ParseEmailFromJwt(token);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            transaction.UserId = user.Id;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok(transaction.Id);
        }

        [HttpPost("AddPeriodicTransaction")]
        public async Task<ActionResult<List<Transactions?>>> AddPeriodicTransaction([FromBody] List<Transactions?> transactions)
        {
            var header = Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
            var token = header.Value.ToString();
            var email = ParseEmailFromJwt(token);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            foreach (var transaction in transactions)
            {
                transaction.UserId = user.Id;
                _context.Transactions.Add(transaction);
            }
            await _context.SaveChangesAsync();

            return Ok(transactions);
        }

        [HttpDelete("{id}")]
        [LimitRequests(MaxRequests = 3, TimeWindow = 1)]
        public async Task DeleteTransaction(int id)
        {
            var transaction = new Transactions(id, 0, "null", DateTime.MinValue);
            _context.Transactions.Attach(transaction);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public static string ParseEmailFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var email = keyValuePairs["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].ToString();

            return email;
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
