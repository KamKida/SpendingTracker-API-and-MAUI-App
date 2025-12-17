using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingsManagementWebAPI.Dtos.GroupDtos;
using SpendingsManagementWebAPI.Dtos.SpendingLimitDtos;
using SpendingsManagementWebAPI.Services;

namespace SpendingsManagementWebAPI.Controllers
{
    [Route("spendingApi/group")]
    [ApiController]
    [Authorize]
    public class GroupControler : ControllerBase
    {
        private readonly GroupService _groupService;

        public GroupControler(GroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _groupService.GetAll();

            return Ok(groups);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddGroupDto dto)
        {
            await _groupService.Add(dto);

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _groupService.Delete(id);
            return Ok();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] EditGroupDto dto)
        {
            await _groupService.Edit(dto);

            return Ok();
        }
    }
}
