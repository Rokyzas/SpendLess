using Microsoft.AspNetCore.Mvc;
using SpendLess.Shared;

namespace SpendLess.Client.Services
{
    public interface ITransactionService
    {
        public List<Transaction> Transactions { get; set; }
        Task GetTransactions();

        Task AddTransaction(double? amount, string category, DateTime date, string comment = "Transaction");

        Task DeleteTransaction(int id);
    }
}
