using SpendingTrackerApp.Contracts.Dtos.Requests;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
	public interface IFundService
	{

		Task<(int StatusCode, string Content)> AddFund(FundRequest request);
	}
}
