
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendLess.Shared;

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
            var transactions = await _context.Transactions.ToListAsync();
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
        public async Task<ActionResult<int?>> AddTransaction([FromBody] Transactions? transaction)
        {
            var header = Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            await _context.SaveChangesAsync();

            return Ok(transaction.Id);
        }

        [HttpPost("AddPeriodicTransaction")]
        public async Task<ActionResult<List<Transactions?>>> AddPeriodicTransaction([FromBody] List<Transactions?> transactions)
        {
            foreach (var transaction in transactions)
            {
                _context.Transactions.Add(transaction);
            }
            //_context.SaveChanges();
            await _context.SaveChangesAsync();

            return Ok(transactions);
        }

        [HttpDelete("{id}")]
        public async Task DeleteTransaction(int id)
        {
            var transaction = new Transactions(id, 0, "null", DateTime.MinValue);
            _context.Transactions.Attach(transaction);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
