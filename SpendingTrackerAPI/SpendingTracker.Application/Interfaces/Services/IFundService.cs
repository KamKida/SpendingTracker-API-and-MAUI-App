using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Requests.FiltersRequests;
using SpendingTracker.Contracts.Dtos.Responses;

namespace SpendingTracker.Application.Interfaces.Services
{
	public interface IFundService
	{
		Task<List<FundResponse>> GetByFilter(FundFilterRequest request);
		Task<FundResponse> AddFund(FundRequest request);
		Task DeleteFund(Guid fundId);
		Task EditFund(FundRequest fundRequest);
	}
}
