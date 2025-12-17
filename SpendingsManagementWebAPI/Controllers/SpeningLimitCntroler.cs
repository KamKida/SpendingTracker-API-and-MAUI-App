using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingsManagementWebAPI.Dtos.SpendingDtos;
using SpendingsManagementWebAPI.Dtos.SpendingLimitDtos;
using SpendingsManagementWebAPI.Services;

namespace SpendingsManagementWebAPI.Controllers
{
    [Route("spendingApi/spendingLimit")]
    [ApiController]
    [Authorize]
    public class SpeningLimitCntroler : ControllerBase
    {
        private readonly SpendingLimitService _spendingLimitService;

        public SpeningLimitCntroler(SpendingLimitService spendingLimitService)
        {
            _spendingLimitService = spendingLimitService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var limits = await _spendingLimitService.GetAll();

            return Ok(limits);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]AddSpendingLimitDto dto)
        {
            await _spendingLimitService.Add(dto);

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _spendingLimitService.Delete(id);
            return Ok();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody]EditSpendingLimitDto dto)
        {
            await _spendingLimitService.Edit(dto);

            return Ok();
        }
    }
}
