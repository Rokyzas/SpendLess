﻿using SpendlessBlazor.Shared;
using System.Net.NetworkInformation;
using System.Text.Json;

using System.Text.Json.Serialization;
using static SpendlessBlazor.Pages.Expenses;
using Info = SpendlessBlazor.Pages.Expenses.Info;

namespace SpendlessBlazor.Services
{
    public class InfoService : IInfoService
    {
        private List<Shared.Info>? infoList = new List<Shared.Info>();
        public void writeToJson(Shared.Info info)
        {

            infoList = readJson();
            infoList.Add(info);

            

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(infoList, options);

            System.IO.File.WriteAllText($"{System.IO.Directory.GetCurrentDirectory()}{"\\wwwroot\\data.json"}", jsonString);
        }

        public List<Shared.Info> readJson()
        {
            String someString = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{"\\wwwroot\\data.json"}");
            return JsonSerializer.Deserialize<List<Shared.Info>>(someString)!;
        }

    }
}
