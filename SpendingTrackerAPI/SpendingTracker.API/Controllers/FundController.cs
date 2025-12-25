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

		[HttpPost("add")]
		[Authorize]
		public async Task<IActionResult> AddFund([FromBody] FundRequest request)
		{
			var response = await _fundService.AddFund(request);

			return Ok(response);

		}
	}
}
