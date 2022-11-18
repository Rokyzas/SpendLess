using Blazored.LocalStorage;
using SpendLess.Client.Services;
using SpendLess.Server.Services;
using SpendLess.Shared;
using SpendLess.UnitTests.MockingServices;
using Moq;
using Moq.Protected;
using System.Net.Http;
using System.Net.Http.Json;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SpendLess.Server.Controllers;
using System.IO;
using UnitTests.MockingServices;
using Microsoft.AspNetCore.Components.Authorization;
using SpendLess.Client;

namespace SpendLess.UnitTests
{
    public class Tests
    {
        private AuthServices _serverAuthServices;
        private AuthenticationService _clientAuthServices;

        private IHttpClientFactory _clientFactoryLogin = new HttpClientFactoryMock<LoginResponse>(
            new LoginResponse("success", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJqb25hc0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY2ODU1MDIxNX0.WRDJFuhBLpbeWK8q4fx4CNJ9TOZeq_owRbuuqt8CQrt-97ctoAfTff2vdyCBpgeXWY8bPW0sMJWVmZu5Q2fMWA"));
        private IHttpClientFactory _clientFactoryInt = new HttpClientFactoryMock<int>(0);
        private IHttpClientFactory _clientFactoryTransactionList = new HttpClientFactoryMock<List<Transaction?>>
        ( new List<Transaction?>{new Transaction(null, 10, "Food", DateTime.Now), null} );
        private ILocalStorageService _localStorage = new LocalStorage();
        private ILocalStorageService _localStorageAuth = new LocalStorage();
        private AuthenticationStateProvider _authProviderMock = new CustomAuthStateProviderMock();
        private TransactionService _transactionServiceDelete;
        private TransactionService _transactionServiceAdd;
        private TransactionService _transactionServiceAddPeriodic;
        private AuthenticationStateProvider _authStateProvider;


        [SetUp]
        public void Setup()
        {
            _authStateProvider = new CustomAuthenticationStateProvider(_localStorageAuth, _clientFactoryLogin);
            _transactionServiceAddPeriodic = new TransactionService(_clientFactoryTransactionList, _localStorage, _authProviderMock);
            _transactionServiceAdd = new TransactionService(_clientFactoryInt, _localStorage, _authProviderMock);
            _transactionServiceDelete = new TransactionService(_clientFactoryLogin, _localStorage, _authProviderMock);
            _serverAuthServices = new AuthServices(null);
            _clientAuthServices = new AuthenticationService(_clientFactoryLogin, _localStorage, _authProviderMock);
        }

        [Test]
        public void ParsingJwtReturnsIEnumerableWithClaims()
        {
            string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJqb25hc0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY2ODU1MDIxNX0.WRDJFuhBLpbeWK8q4fx4CNJ9TOZeq_owRbuuqt8CQrt-97ctoAfTff2vdyCBpgeXWY8bPW0sMJWVmZu5Q2fMWA";
            var state = CustomAuthenticationStateProvider.ParseClaimsFromJwt(token);
            var email =  state.Select(s => s.Value).FirstOrDefault();
            Assert.That(email,Is.EqualTo("jonas@gmail.com"));

        }


        [Test]
        public async Task GettingAuthenticationStateWithExpiredTokenNonAuthentificatedState()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var isAuthenticated = state.User.Identity.IsAuthenticated;
            Assert.False(isAuthenticated);
        }
        
        [Test]
        public async Task GettingTransactionsListChangesTransactionList()
        {
            var listCountBefore = _transactionServiceAddPeriodic.Transactions.Count;
            await _transactionServiceAddPeriodic.AddPeriodicTransaction(10, "Food", DateTime.Now, "Pizza", "week(s)", 3, DateTime.Now.AddMonths(2));
            var listCountAfter = _transactionServiceAddPeriodic.Transactions.Count;
            Assert.True(listCountBefore < listCountAfter);
        }

        [Test]
        public async Task AddingTransactionsListChangesTransactionList()
        {
            var listCountBefore = _transactionServiceAddPeriodic.Transactions.Count;
            await _transactionServiceAddPeriodic.GetTransactions();
            var listCountAfter = _transactionServiceAddPeriodic.Transactions.Count;
            Assert.True(listCountBefore < listCountAfter);
        }

        [Test]
        public async Task SuccessfulyCreatingAccountReturnsTrue()
        {
            var result = await _clientAuthServices.CreateAccount(new UserDto()
            {
                Email = "name@gmail.com",
                Password = "password",
                Username = "username"
            });
            Assert.IsTrue(result);
        }

        [Test]
        public async Task SuccessfulyCreatingAccountAddsTokenToLocalStorage()
        {
            var result = await _clientAuthServices.CreateAccount(new UserDto()
            {
                Email = "name@gmail.com",
                Password = "password",
                Username = "username"
            });
        
            Assert.IsNotNull(_localStorage.GetItemAsStringAsync("token"));
        }

        [Test]
        public async Task CheckingForExistingAccountReturnsTrue()
        {
            var result = await _clientAuthServices.GetLoginAuthentication(new UserDto()
            {
                Email = "name@gmail.com",
                Password = "password",
            });
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeletingTransactionReturnsMessage()
        {

            Assert.That(await _transactionServiceDelete.DeleteTransaction(5),
                Is.EqualTo("Transaction was successfully deleted"));
        }

        [Test]
        public async Task AddingTransactionAppendsToList()
        {
            int count = _transactionServiceAdd.Transactions.Count;
            await _transactionServiceAdd.AddTransaction(50, "Food", DateTime.Now, "Taco");

            Transaction item = _transactionServiceAdd.Transactions.Last();

            Assert.That(_transactionServiceAdd.Transactions.Count, Is.EqualTo(count + 1));
        }


        [Test]
        public void ProvidingInvalidRequestReturnsFalse()
        {
            Assert.IsFalse(_serverAuthServices.VerifyRequest(null));
        }

        [Test]

        public void CreatingTokenWhenArgumentsAreInvalidReturnsNull()
        {
            var user = new UserDto
            {
                Email = "email@gmail.com",
                Password = "password",
                Username = "username"
            };
            Assert.Throws<NullReferenceException>(() => _serverAuthServices.CreateToken(user, null));
        }

        [Test]
        public void CreatingHashedPasswordReturnsTwoBiteArrays()
        {
            var password = "password";

            _serverAuthServices.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            Assert.IsInstanceOf<byte[]>(passwordHash);
            Assert.IsInstanceOf<byte[]>(passwordSalt);
        }

        [Test]
        public void ProvidingEmptyPasswordReturnsMessage()
        {
            Assert.That(_clientAuthServices.CheckPassword(""), Is.EqualTo("Password is required!"));

        }

        [Test]
        public void ProvidingTooShortPasswordReturnsMessage()
        {
            Assert.That(_clientAuthServices.CheckPassword("short"),
                Is.EqualTo("Password must be at least of length 8"));
        }

        [Test]
        public void ProvidingPasswordWithoutCapitalLettersReturnsMessage()
        {
            Assert.That(_clientAuthServices.CheckPassword("no_capital"),
                Is.EqualTo("Password must contain at least one capital letter"));

            Assert.That(_clientAuthServices.CheckPassword("NO_LOWERCASE"),
                Is.EqualTo("Password must contain at least one lowercase letter"));

            Assert.That(_clientAuthServices.CheckPassword("No_Digits"),
                Is.EqualTo("Password must contain at least one digit"));
        }

        [Test]
        public void ProvidingPasswordWithoutLowercaseLettersReturnsMessage()
        {
            Assert.That(_clientAuthServices.CheckPassword("NO_LOWERCASE"),
                Is.EqualTo("Password must contain at least one lowercase letter"));

            Assert.That(_clientAuthServices.CheckPassword("No_Digits"),
                Is.EqualTo("Password must contain at least one digit"));
        }

        [Test]
        public void ProvidingPasswordWithoutDigitsReturnsMessage()
        {
            Assert.That(_clientAuthServices.CheckPassword("No_Digits"),
                Is.EqualTo("Password must contain at least one digit"));
        }

        [Test]
        public void ProvidingEmptyEmailReturnsMessage()
        {
            Assert.That(_clientAuthServices.CheckEmail(""),
                Is.EqualTo("Email is required"));

        }

        [Test]
        [TestCase("email")]
        [TestCase("email@")]
        [TestCase("email@com")]
        [TestCase("@gmail.com")]
        public void ProvidingInvalidEmailReturnsMessage(string email)
        {

            Assert.That(_clientAuthServices.CheckEmail(email)!,
                Is.EqualTo("Email format is incorrect"));

        }
    }
}