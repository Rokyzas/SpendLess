using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;
using SpendLess.Shared;
using SpendLess.Client.Services;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Hosting;
using Blazored.LocalStorage;
using SpendLess.Client.Services;
using IntegrationTests.MockingServices;
using Microsoft.AspNetCore.Components.Authorization;

namespace IntegrationTests

{
    public class UnitTest1
    {
        //private readonly WebApplicationFactory<Program> _factory;
        private IHttpClientFactory _clientFactory = new HttpClientFactoryMock();
        private ILocalStorageService _localStorage = new LocalStorage();
        private TransactionService _transactionService;
        public UnitTest1()      
        {
            _transactionService = new TransactionService(_clientFactory, _localStorage, null);
        }
        //private HttpClient _httpClient;
        [Fact]
        public async void Test1()
        {
            //Assert.Equal("Failed to delete transaction", await _transactionService.DeleteTransaction(5));
        }


    }
}