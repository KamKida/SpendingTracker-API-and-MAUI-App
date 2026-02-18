using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
	public interface IFundCategoryService
	{
		Task<HttpResponseMessage> Get10(FundCategoryFilterRequest request, bool useDatesFromToo = false);
		Task<HttpResponseMessage> AddFundCategory(FundCategoryRequest request);
		Task<HttpResponseMessage> DeleteFundCategory(Guid id);

		Task<HttpResponseMessage> EditFundCategory(FundCategoryRequest request);
	}
}
