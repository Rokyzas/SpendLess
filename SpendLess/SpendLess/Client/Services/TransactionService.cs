using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendLess.Client.Pages;
using SpendLess.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using static MudBlazor.CategoryTypes;


namespace SpendLess.Client.Services
{
    public class TransactionService : ITransactionService {

        private readonly HttpClient _httpClient;

        public TransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public async Task GetTransactions()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Transaction>>("api/finance");
            if(result != null)
            {
                Transactions = result;
                SnackBarService.SuccessMsg("Data loaded");
            }
        }

        public async Task AddTransaction(double? amount, string category, DateTime date, string comment = "Transaction")
        {
            var transaction = new Transaction(null, amount, category, date, comment);
            var response = await _httpClient.PostAsJsonAsync("api/finance", transaction);
            var id = await response.Content.ReadFromJsonAsync<int>();

            if (response.IsSuccessStatusCode)
            {
                transaction.Id = id;
                Transactions.Add(transaction);
                SnackBarService.SuccessMsg("Succsesfully saved data");
            }
            else
            {
                SnackBarService.ErrorMsg("Failed to save data!");
            }
        }

        public async Task DeleteTransaction(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/finance/{id}");
            if (response.IsSuccessStatusCode)
            {
                SnackBarService.SuccessMsg("Transaction was successfully deleted");
                int c = 0;
                foreach (var element in Transactions)
                {
                    if (element.Id.Equals(id))
                    {
                        Transactions.RemoveAt(c);
                        break;
                    }

                    c++;
                }
            }
            else
            {
                SnackBarService.WarningMsg("Failed to delete transaction");
            }
        }
    }
}
