using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Requests.FiltersRequests;

namespace SpendingTracker.API.Controllers
{
	[Route("spending/fundCategory")]
	[ApiController]
	public class FundCategorieController : ControllerBase
	{
		private readonly IFundCategotuService _fundCategotuService;
		public FundCategorieController(IFundCategotuService fundCategotuService)
		{
			_fundCategotuService = fundCategotuService;
		}

		[HttpGet("get10")]
		[Authorize]
		public async Task<IActionResult> Get10([FromQuery] FundCategoryFilterRequest request)
		{
			var response = await _fundCategotuService.GetByFilter(request);

			return Ok(response);
		}

		[HttpPost("add")]
		[Authorize]
		public async Task<IActionResult> AddFundCategory([FromBody] FundCategoryRequest request)
		{
			await _fundCategotuService.AddFundCategory(request);

			return Ok();

		}

		[HttpDelete("delete/{fundId}")]
		[Authorize]
		public async Task<IActionResult> DeleteFundCategory([FromRoute] Guid fundId)
		{
			await _fundCategotuService.DeleteFundCategory(fundId);

			return Ok();
		}

		[HttpPut("edit")]
		[Authorize]
		public async Task<IActionResult> EditFundCategory([FromBody] FundCategoryRequest request)
		{
			await _fundCategotuService.EditFundCategory(request);
			return Ok();
		}
	}
}
