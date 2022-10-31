using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using SpendLess.Shared;

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

            var transactions =  await _context.Finances.ToListAsync();
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<ActionResult> AddTransaction(Transaction transaction)
        {
            _context.Finances.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok();
        } 
    }
}
