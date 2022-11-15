using Newtonsoft.Json;
using SpendLess.Client.Pages;
using SpendLess.Shared;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace SpendLess.Client.Services
{
    public class TransactionService : ITransactionService
    {

        /* private readonly HttpClient _httpClient;

         public TransactionService(HttpClient httpClient)
         {
             _httpClient = httpClient;
         }*/

        public TransactionService(IHttpClientFactory clientFactory, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _clientFactory = clientFactory;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        private readonly IHttpClientFactory _clientFactory;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        public List<SpendLess.Shared.Transactions> Transactions { get; set; } = new List<SpendLess.Shared.Transactions>();

        public async Task GetTransactions()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7290/api/Transactions/GetTransactions");
            var client = _clientFactory.CreateClient();
            string token = await _localStorage.GetItemAsStringAsync("token");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.Replace("\"", ""));

            var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            if ((response.StatusCode) == HttpStatusCode.Unauthorized)
            {
                await _authStateProvider.GetAuthenticationStateAsync();
                SnackBarService.ErrorMsg("Session has ended");
                return;
            }
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<SpendLess.Shared.Transactions>>();
                Transactions = result;
            }


            /*var _httpClient = clientFactory.CreateClient();
            var result = await _httpClient.GetFromJsonAsync<List<Transaction>>("api/finance/GetTransactions");
            if(result != null)
            {
                Transactions = result;
                SnackBarService.SuccessMsg("Data loaded");
            }*/
        }

        public async Task AddTransaction(double? amount, string category, DateTime date, string comment)
        {

            string token = await _localStorage.GetItemAsStringAsync("token");
            var _httpClient = _clientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));


            var transaction = new SpendLess.Shared.Transactions(null, amount, category, date, comment);
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Transactions/AddTransaction", transaction);
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

        public async Task AddPeriodicTransaction(double? amount, string category, DateTime date, string comment, string period, int interval, DateTime? endDate)
        {
            string token = await _localStorage.GetItemAsStringAsync("token");
            var _httpClient = _clientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));


            if(endDate == null)
            {
                endDate = date.AddMonths(12);
            }

            bool isMonthly = false;

            switch (period)
            {
                case "day(s)":
                    break;
                case "week(s)":
                    interval = interval * 7;
                    break;
                case "month(s)":
                    isMonthly = true;
                    break;
                default://add exception for logging here?
                    break;
            }

            List<SpendLess.Shared.Transactions> transactions = new List<SpendLess.Shared.Transactions>();

            while (date <= endDate)
            {
                transactions.Add(new SpendLess.Shared.Transactions(null, amount, category, date, comment, null , period, interval, endDate));

                if (isMonthly)
                {
                    date = date.AddMonths(interval);
                }
                else
                {
                    date =  date.AddDays(interval);
                }
            }

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Transactions/AddPeriodicTransaction", transactions);
            var transactionsID = await response.Content.ReadFromJsonAsync<List<SpendLess.Shared.Transactions?>>();

            if (response.IsSuccessStatusCode)
            {
                Transactions.AddRange(transactionsID);
                SnackBarService.SuccessMsg("Succsesfully saved data");
            }
            else
            {
                SnackBarService.ErrorMsg("Failed to save data!");
            }
        }

        public async Task DeleteTransaction(int id)
        {
            string token = await _localStorage.GetItemAsStringAsync("token");
            var _httpClient = _clientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

            var response = await _httpClient.DeleteAsync($"https://localhost:7290/api/Transactions/{id}");
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
