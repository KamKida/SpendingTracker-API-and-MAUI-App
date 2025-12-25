using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Responses;

namespace SpendingTracker.Application.Interfaces.Services
{
	public interface IFundService
	{
		Task<FundResponse> AddFund(FundRequest request);
	}
}
