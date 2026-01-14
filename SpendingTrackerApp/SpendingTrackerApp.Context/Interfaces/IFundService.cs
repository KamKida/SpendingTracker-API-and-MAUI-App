using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
	public interface IFundService
	{
		Task<HttpResponseMessage> Get10(FundFilterRequest request, bool useDatesFromToo = false);
		Task<HttpResponseMessage> AddFund(FundRequest request);
		Task<HttpResponseMessage> DeleteFund(Guid id);
		Task<HttpResponseMessage> EditFund(FundRequest request);

	}
}
