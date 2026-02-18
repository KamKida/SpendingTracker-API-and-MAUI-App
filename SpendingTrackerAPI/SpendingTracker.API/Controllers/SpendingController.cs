using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Requests.FiltersRequests;

namespace SpendingTracker.API.Controllers
{
	[Route("spending/spending")]
	[ApiController]
	public class SpendingController : ControllerBase
	{
		private readonly ISpendingService _spendingService;

		public SpendingController(ISpendingService spendingService)
		{
			_spendingService = spendingService;
		}

		[HttpGet("get10")]
		[Authorize]
		public async Task<IActionResult> Get10([FromQuery] SpendingFilterRequest request)
		{
			var response = await _spendingService.GetByFilter(request);

			return Ok(response);
		}

		[HttpPost("add")]
		[Authorize]
		public async Task<IActionResult> AddSpending([FromBody] SpendingRequest request)
		{
			await _spendingService.AddSpending(request);

			return Ok();

		}

		[HttpDelete("delete/{spendingId}")]
		[Authorize]
		public async Task<IActionResult> DeleteSpending([FromRoute] Guid spendingId)
		{
			await _spendingService.DeleteSpending(spendingId);

			return Ok();
		}

		[HttpPut("edit")]
		[Authorize]
		public async Task<IActionResult> EditSpending([FromBody] SpendingRequest request)
		{
			await _spendingService.EditSpending(request);
			return Ok();
		}
	}
}
