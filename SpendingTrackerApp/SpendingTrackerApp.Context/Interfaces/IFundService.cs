using SpendingTrackerApp.Contracts.Dtos.Requests;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
	public interface IFundService
	{
		Task<HttpResponseMessage> Get10(FundFilterRequest? request);
		Task<(int StatusCode, string Content)> AddFund(FundRequest request);
		Task<(int StatusCode, string Content)> DeleteFund(Guid id);
		Task<HttpResponseMessage> EditFund(FundRequest request);

	}
}
