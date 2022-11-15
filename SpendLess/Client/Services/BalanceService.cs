using SpendLess.Shared;
using SpendLess.Client.Services;
using SpendLess.Client.Pages;
using SpendLess.Client.Shared;
using System.Net.Http;


namespace SpendLess.Client.Services
{
    public class BalanceService : IBalanceService
    {
        public event Action? BalanceChanged;

        public async Task RefreshBalance()
        {
            if (BalanceChanged != null)
                BalanceChanged.Invoke();
        }
    }
}