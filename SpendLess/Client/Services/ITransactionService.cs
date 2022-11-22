//using Microsoft.AspNetCore.Mvc;
using SpendLess.Shared;
namespace SpendLess.Client.Services
{
    public delegate void LogException(HttpClient client, string str, Exception ex);

    public interface ITransactionService
    {
        public List<Transactions> Transactions { get; set; }

        event EventHandler<EventArgs>? TransactionsChanged;

        public Task OnTransactionsChanged();
        Task GetTransactions(LogException logexception);
        Task GetTransactions();

        Task AddTransaction(double? amount, string category, DateTime date, string comment = "Transaction");

        Task AddPeriodicTransaction(double? amount, string category, DateTime date, string comment, string period, int interval, DateTime? endDate);

        Task<string> DeleteTransaction(int id);
    }
}
