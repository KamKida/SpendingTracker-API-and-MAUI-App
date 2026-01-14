using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Requests.FiltersRequests;
using SpendingTracker.Contracts.Dtos.Responses;

namespace SpendingTracker.Application.Interfaces.Services
{
	public interface ISpendingCategoryService
	{
		Task<List<SpendingCategoryResponse>> GetByFilter(SpendingCategoryFilterRequest request);
		Task AddSpendingCategory(SpendingCategoryRequest request);
		Task DeleteSpendingCategory(Guid spendingCategoryId);
		Task EditSpendingCategory(SpendingCategoryRequest spendingCategoryRequest);
	}

}
