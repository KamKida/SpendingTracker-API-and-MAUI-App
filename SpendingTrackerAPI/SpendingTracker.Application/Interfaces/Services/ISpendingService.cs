using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Requests.FiltersRequests;
using SpendingTracker.Contracts.Dtos.Responses;

namespace SpendingTracker.Application.Interfaces.Services
{
	public interface ISpendingService
	{
		Task<List<SpendingReponse>> GetByFilter(SpendingFilterRequest request);
		Task<SpendingReponse> AddSpending(SpendingRequest request);
		Task DeleteSpending(Guid spendingId);
		Task EditSpending(SpendingRequest request);
	}
}
