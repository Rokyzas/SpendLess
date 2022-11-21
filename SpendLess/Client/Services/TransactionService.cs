using SpendLess.Shared;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SpendLess.Client.Services
{
    public class TransactionService : ITransactionService
    {

        /* private readonly HttpClient _httpClient;

         public TransactionService(HttpClient httpClient)
         {
             _httpClient = httpClient;
         }*/

        public event EventHandler<EventArgs>? TransactionsChanged;
        public void OnTransactionsChanged()
        {
            if (TransactionsChanged is not null)
                TransactionsChanged.Invoke(this, EventArgs.Empty);
        }

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
            var client = _clientFactory.CreateClient();
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7290/api/Transactions/GetTransactions");                
                string token = await _localStorage.GetItemAsStringAsync("token");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.Replace("\"", ""));

                var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
                if ((response.StatusCode) == HttpStatusCode.Unauthorized)
                {
                    await _authStateProvider.GetAuthenticationStateAsync();
                    //SnackBarService.ErrorMsg("Session has ended");
                    return;
                }
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<Transactions>>();
                    Transactions = result;
                }
            }
            catch (NullReferenceException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (JsonException ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (Exception ex)
            {
                await client.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
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
            var _httpClient = _clientFactory.CreateClient();
            try
            {
                string token = await _localStorage.GetItemAsStringAsync("token");                
                _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));


                var transaction = new Transactions(null, amount, category, date, comment);
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Transactions/AddTransaction", transaction);
                var id = await response.Content.ReadFromJsonAsync<int>();
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", new Exception());
                if (response.IsSuccessStatusCode)
                {
                    transaction.Id = id;
                    Transactions.Add(transaction);
                    //SnackBarService.SuccessMsg("Succsesfully saved data");
                }
                else
                {
                    //SnackBarService.ErrorMsg("Failed to save data!");
                }
            }
            catch(NullReferenceException ex)
            {
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch(JsonException ex)
            {
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch(Exception ex)
            {
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
        }

        public async Task AddPeriodicTransaction(double? amount, string category, DateTime date, string comment, string period, int interval, DateTime? endDate)
        {
            var _httpClient = _clientFactory.CreateClient();
            try
            {
                string token = await _localStorage.GetItemAsStringAsync("token");               
                _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));


                if (endDate == null)
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
                    default: throw new ArgumentException("Period is incorrect"); //add exception for logging here?
                }

                List<Transactions> transactions = new List<Transactions>();

                while (date <= endDate)
                {
                    transactions.Add(new Transactions(null, amount, category, date, comment, null, period, interval, endDate));

                    if (isMonthly)
                    {
                        date = date.AddMonths(interval);
                    }
                    else
                    {
                        date = date.AddDays(interval);
                    }
                }

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Transactions/AddPeriodicTransaction", transactions);
                var transactionsID = await response.Content.ReadFromJsonAsync<List<Transactions?>>();

                if (response.IsSuccessStatusCode)
                {
                    Transactions.AddRange(transactionsID);
                    //SnackBarService.SuccessMsg("Succsesfully saved data");
                }
                else
                {
                   // SnackBarService.ErrorMsg("Failed to save data!");
                }
            }
            catch (NullReferenceException ex)
            {
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (JsonException ex)
            {
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch(ArgumentException ex)
            {
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
            catch (Exception ex)
            {
                await _httpClient.PostAsJsonAsync("https://localhost:7290/api/Exception", ex);
                throw;
            }
        }

        public async Task<string> DeleteTransaction(int id)
        {
            try
            {
                string token = await _localStorage.GetItemAsStringAsync("token");
                var _httpClient = _clientFactory.CreateClient();
                _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

                var response = await _httpClient.DeleteAsync($"https://localhost:7290/api/Transactions/{id}");
                if (response.IsSuccessStatusCode)
                {
                    //SnackBarService.SuccessMsg("Transaction was successfully deleted");
                    
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
                    return "Transaction was successfully deleted";
                }
                else
                {
                    //SnackBarService.WarningMsg("Failed to delete transaction");
                    return "Failed to delete transaction";
                }
            }
            catch (NullReferenceException ex)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
