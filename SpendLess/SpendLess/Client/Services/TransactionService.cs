﻿using Microsoft.AspNetCore.Mvc;
using SpendLess.Client.Pages;
using SpendLess.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using static SpendLess.Client.Pages.Transactions;


namespace SpendLess.Client.Services
{
    public class TransactionService : ITransactionService {

        private readonly HttpClient _httpClient;

        public TransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Transaction> Transactions { get; set; }

        public async Task<List<Transaction>> GetTransactions()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Transaction>>("api/finance");
            if(result != null)
            {
                Transactions = result;
            }
            return result;
        }

        public async Task AddTransaction(Transaction transaction)
        {
            await _httpClient.PostAsJsonAsync("api/finance", transaction);
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
