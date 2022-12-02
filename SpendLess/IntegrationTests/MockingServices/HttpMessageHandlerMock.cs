using MudBlazor;
using Newtonsoft.Json;
using SpendLess.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IntegrationTests.MockingServices
{
    internal class HttpMessageHandlerMock : HttpMessageHandler
    {
        StringContent istringContent;
        public HttpMessageHandlerMock() 
        {
            string json = JsonConvert.SerializeObject(new LoginResponse("success", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJqb25hc0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY2ODU1MDIxNX0.WRDJFuhBLpbeWK8q4fx4CNJ9TOZeq_owRbuuqt8CQrt-97ctoAfTff2vdyCBpgeXWY8bPW0sMJWVmZu5Q2fMWA"));
            istringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        }
        

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = istringContent
            });
        }

        
    }
}