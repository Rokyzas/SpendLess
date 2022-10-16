using SpendlessBlazor.Data;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;
using static SpendlessBlazor.Pages.Expenses;

namespace SpendlessBlazor.Services
{
    public class TransactionService : ITransactionService <Transaction>
    {

        public List<Transaction> ReadJson()
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

    }
}
