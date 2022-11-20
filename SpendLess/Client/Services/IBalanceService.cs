using SpendLess.Shared;
namespace SpendLess.Client.Services
{
    public interface IBalanceService
    {
		event Action BalanceChanged;

		//public delegate void BalanceChanged();
		public Task RefreshBalance();
    }
}

