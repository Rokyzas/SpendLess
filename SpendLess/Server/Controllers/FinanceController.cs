
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendLess.Shared;
using System.Security.Claims;

namespace SpendLess.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FinanceController : ControllerBase
    {
        private readonly SpendLessContext _context;

        public FinanceController(SpendLessContext context)
        {
            _context = context;
        }

        [HttpGet("GetTransactions")]
        public async Task<ActionResult<List<Transaction>>> GetTransactions()
        {
            Dictionary<string, string> requestHeaders =
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
            return Ok(listas);
        }

        [HttpPost]
        public async Task<ActionResult<int?>> AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            await _context.SaveChangesAsync();

            return Ok(transaction.Id);
        }

        [HttpDelete("{id}")]
        public async Task DeleteTransaction(int id)
        {
            var transaction = new Transaction(id, 0, "null", DateTime.MinValue);
            _context.Transactions.Attach(transaction);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
