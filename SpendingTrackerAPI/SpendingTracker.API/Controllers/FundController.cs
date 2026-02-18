using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Requests.FiltersRequests;

namespace SpendingTracker.API.Controllers
{
	[Route("spending/fund")]
	[ApiController]
	public class FundController : ControllerBase
	{
		private readonly IFundService _fundService;

		public FundController(IFundService fundService)
		{
			_fundService = fundService;
		}

		[HttpGet("get10")]
		[Authorize]
		public async Task<IActionResult> Get10([FromQuery] FundFilterRequest request)
		{
			var response = await _fundService.GetByFilter(request);

			return Ok(response);
		}

		[HttpPost("add")]
		[Authorize]
		public async Task<IActionResult> AddFund([FromBody] FundRequest request)
		{
			await _fundService.AddFund(request);

			return Ok();

		}

		[HttpDelete("delete/{fundId}")]
		[Authorize]
		public async Task<IActionResult> DeleteFund([FromRoute] Guid fundId)
		{
			await _fundService.DeleteFund(fundId);

			return Ok();
		}

		[HttpPut("edit")]
		[Authorize]
		public async Task<IActionResult> EditFund([FromBody] FundRequest request)
		{
			await _fundService.EditFund(request);
			return Ok();
		}
	}
}
