using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using SpendLess.Client.Pages;
using SpendLess.Shared;
using static MudBlazor.CategoryTypes;

namespace SpendLess.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly SpendLessContext _context;

        public FinanceController(SpendLessContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetTransactions(){

            var transactions =  await _context.Transactions.ToListAsync();
            return Ok(transactions);
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
