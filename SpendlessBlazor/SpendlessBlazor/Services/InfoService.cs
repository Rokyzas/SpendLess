using SpendlessBlazor.Shared;
using System.Net.NetworkInformation;
using System.Text.Json;

using System.Text.Json.Serialization;
using static SpendlessBlazor.Pages.Expenses;

namespace SpendlessBlazor.Services
{
    public class InfoService : IInfoService <Info>
    {
        private List<Shared.Info>? infoList = new List<Shared.Info>();
        /*
        public void writeToJson(Shared.Info info)
        {

            infoList = readJson();
            infoList.Add(info);


            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(infoList, options);

            System.IO.File.WriteAllText($"{System.IO.Directory.GetCurrentDirectory()}{"\\wwwroot\\data.json"}", jsonString);
        }
        */

        public List<Shared.Info> ReadJson(SnackBarService snackbar)
        {
            //String someString = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{"\\wwwroot\\data.json"}");
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
                snackbar.ErrorMsg("Failed to load data!");
                return null;
            }


            //deserialize fails on empty string
            //will probably become redundant when data is no longer deleted manually
            if (someString == "")
            {
                someString = "[]";
            }

            List<Shared.Info> list = JsonSerializer.Deserialize<List<Shared.Info>>(someString)!;

            return list;
        }

    }
}
