using SpendLess.Shared;

namespace SpendLess.Client.Services
{
	public interface IGoalService
	{
		List<Goal> Goals { get; set; }

		Task AddGoal(int userId, string name, double? amount, DateTime endDate);
		Task GetGoals(LogException logException);
	}
}