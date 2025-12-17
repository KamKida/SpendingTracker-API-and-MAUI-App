using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingsManagementWebAPI.Dtos.FundsDtos;
using SpendingsManagementWebAPI.Dtos.PlanedSpendingDtos;
using SpendingsManagementWebAPI.Services;

namespace SpendingsManagementWebAPI.Controllers
{
    [Route("spendingApi/planedSpending")]
    [ApiController]
    [Authorize]
    public class PlanedSpendingService : ControllerBase
    {
        private PlanedSpendingsService _planedSpendingsService;

        public PlanedSpendingService(PlanedSpendingsService planedSpendingsService)
        {
            _planedSpendingsService = planedSpendingsService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var allfunds = await _planedSpendingsService.GetAll();

            return Ok(allfunds);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]AddPlanedSpendingDto dto)
        {
            await _planedSpendingsService.Add(dto);

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _planedSpendingsService.Delete(id);

            return Ok();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody]EditPlanedSpendingDto dto)
        {
            await _planedSpendingsService.Edit(dto);

            return Ok();
        }
    }
}
