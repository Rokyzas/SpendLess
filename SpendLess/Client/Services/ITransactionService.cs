//using Microsoft.AspNetCore.Mvc;
using SpendLess.Shared;

namespace SpendLess.Client.Services
{
    public interface ITransactionService
    {
        public List<Transactions> Transactions { get; set; }
        Task GetTransactions();

        Task AddTransaction(double? amount, string category, DateTime date, string comment = "Transaction");

        Task AddPeriodicTransaction(double? amount, string category, DateTime date, string comment, string period, int interval, DateTime? endDate);

        Task DeleteTransaction(int id);
    }
}
