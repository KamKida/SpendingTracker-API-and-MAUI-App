using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
	public interface ISpendingService
	{
		Task<HttpResponseMessage> Get10(SpendingFilterRequest request, bool useDatesFromToo = false);
		Task<HttpResponseMessage> AddSpending(SpendingRequest request);
		Task<HttpResponseMessage> DeleteSpending(Guid spendingId);
		Task<HttpResponseMessage> EditSpending(SpendingRequest request);
	}
}
