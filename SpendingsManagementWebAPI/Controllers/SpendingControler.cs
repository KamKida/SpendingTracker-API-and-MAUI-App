using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingsManagementWebAPI.Dtos.SpendingDtos;
using SpendingsManagementWebAPI.Services;

namespace SpendingsManagementWebAPI.Controllers
{
    [Route("spendingApi/spending")]
    [ApiController]
    [Authorize]
    public class SpendingControler : ControllerBase
    {
        private readonly SpendingService _spendingService;

        public SpendingControler(SpendingService spendingService)
        {
            _spendingService = spendingService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var spendings = await _spendingService.GetAll();

            return Ok(spendings);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddSpendingDto dto)
        {
            await _spendingService.Add(dto);

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            await _spendingService.Delete(id);
            return Ok();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody]EditSpendingDto dto)
        {
            await _spendingService.Edit(dto);

            return Ok();
        }
    }
}
