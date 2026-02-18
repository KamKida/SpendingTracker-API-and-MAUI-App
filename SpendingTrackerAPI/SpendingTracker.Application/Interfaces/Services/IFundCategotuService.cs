using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Requests.FiltersRequests;
using SpendingTracker.Contracts.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Interfaces.Services
{
	public interface IFundCategotuService
	{
		Task<List<FundCategoryResponse>> GetByFilter(FundCategoryFilterRequest request);
		Task AddFundCategory(FundCategoryRequest request);
		Task DeleteFundCategory(Guid fundId);
		Task EditFundCategory(FundCategoryRequest fundRequest);
	}
}
