using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingsManagementWebAPI.Dtos.FundsDtos;
using SpendingsManagementWebAPI.Dtos.UserDtos;
using SpendingsManagementWebAPI.Services;

namespace SpendingsManagementWebAPI.Controllers
{
    [Route("spendingApi/fund")]
    [ApiController]
    [Authorize]
    public class FundControler : ControllerBase
    {
        private readonly FundService _fundsService;

        public FundControler(FundService fundsService)
        {
            _fundsService = fundsService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var allfunds = await _fundsService.GetAll();

            return Ok(allfunds);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddFundDto dto)
        {
            await  _fundsService.Add(dto);

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            await _fundsService.Delete(id);

            return Ok();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] EditFundDto dto)
        {
            await _fundsService.Edit(dto);

            return Ok();
        }
    }
}
