using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Contracts.Dtos.Requests.FiltersRequest;

namespace SpendingTrackerApp.Infrastructure.Interfaces
{
	public interface ISpendingCategoryService
	{
		Task<HttpResponseMessage> Get10(SpendingCategoryFilterRequest request, bool useDatesFromToo = false);
		Task<HttpResponseMessage> AddSpendingCategory(SpendingCategoryRequest request);
		Task<HttpResponseMessage> DeleteSpendingCategory(Guid id);
		Task<HttpResponseMessage> EditSpendingCategory(SpendingCategoryRequest request);
	}
}
