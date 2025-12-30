using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Contracts.Dtos.Requests;

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

		[HttpGet("top10")]
		[Authorize]
		public async Task<IActionResult> GetTop10Funds()
		{
			var response = await _fundService.GetTop10Funds();

			return Ok(response);
		}

		[HttpPost("add")]
		[Authorize]
		public async Task<IActionResult> AddFund([FromBody] FundRequest request)
		{
			var response = await _fundService.AddFund(request);

			return Ok(response);

		}

		[HttpDelete("delete/{fundId}")]
		[Authorize]
		public async Task<IActionResult> DeleteFund([FromRoute] Guid fundId)
		{
			await _fundService.DeleteFund(fundId);

			return Ok();
		}
	}
}
