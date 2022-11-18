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

namespace SpendLess.UnitTests.MockingServices
{
    internal class HttpMessageHandlerMock<T> : HttpMessageHandler
    {
        StringContent istringContent;
        public HttpMessageHandlerMock(T instance) 
        {
            string json = JsonConvert.SerializeObject(instance);
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