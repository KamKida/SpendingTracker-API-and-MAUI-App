using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Requests.FiltersRequests;

namespace SpendingTracker.API.Controllers
{
	[Route("spending/spendingCategory")]
	[ApiController]
	public class SpendingCategoryController : ControllerBase
	{
		private readonly ISpendingCategoryService _spendingCategoryService;
		public SpendingCategoryController(ISpendingCategoryService spendingCategoryService)
		{
			_spendingCategoryService = spendingCategoryService;
		}

		[HttpGet("get10")]
		[Authorize]
		public async Task<IActionResult> Get10([FromQuery] SpendingCategoryFilterRequest request)
		{
			var response = await _spendingCategoryService.GetByFilter(request);
			return Ok(response);
		}

		[HttpPost("add")]
		[Authorize]
		public async Task<IActionResult> AddSpendingCategory([FromBody] SpendingCategoryRequest request)
		{
			await _spendingCategoryService.AddSpendingCategory(request);
			return Ok();
		}

		[HttpDelete("delete/{spendingCategoryId}")]
		[Authorize]
		public async Task<IActionResult> DeleteSpendingCategory([FromRoute] Guid spendingCategoryId)
		{
			await _spendingCategoryService.DeleteSpendingCategory(spendingCategoryId);
			return Ok();
		}

		[HttpPut("edit")]
		[Authorize]
		public async Task<IActionResult> EditSpendingCategory([FromBody] SpendingCategoryRequest request)
		{
			await _spendingCategoryService.EditSpendingCategory(request);
			return Ok();
		}
	}
}
