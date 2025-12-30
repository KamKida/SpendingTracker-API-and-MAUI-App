using SpendingTrackerApp.Contracts.Dtos.Requests;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
	public interface IFundService
	{
		Task<(int StatusCode, string Content)> GetTop10();
		Task<(int StatusCode, string Content)> AddFund(FundRequest request);
		Task<(int StatusCode, string Content)> DeleteFund(Guid id);
	}
}
