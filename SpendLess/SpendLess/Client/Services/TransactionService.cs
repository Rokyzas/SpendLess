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

        public async Task<List<Transaction>> GetTransactions()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Transaction>>("api/finance");
            if(result != null)
            {
                Transactions = result;
            }
            return result;
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

        /*
public static List<Transaction> ReadJson()
{
   String someString;
   String path = $"{System.IO.Directory.GetCurrentDirectory()}{"\\wwwroot\\data.json"}";

   //throws error if file not found
   try
   {
       using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
       using System.IO.StreamReader streamReader = new System.IO.StreamReader(fileStream);

       someString = streamReader.ReadToEnd();
   }
   catch (Exception)
   {
       SnackBarService.ErrorMsg("Failed to load data!");
       return null;
   }


   if (someString == "")
   {
       someString = "[]";
   }
   List<Transaction> list = JsonSerializer.Deserialize<List<Transaction>>(someString)!;

   return list;
}

public static void WriteToJson()
{
   var options = new JsonSerializerOptions { WriteIndented = true };
   string jsonString = JsonSerializer.Serialize(transactions, options);
   string path = $"{System.IO.Directory.GetCurrentDirectory()}{"\\wwwroot\\data.json"}";

   try
   {
       using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
       {
           using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fileStream))
           {
               streamWriter.Write(jsonString);
           }
       }
   }
   catch (Exception)
   {
       SnackBarService.ErrorMsg("Failed to save data");
   }
}
*/
    }
}
