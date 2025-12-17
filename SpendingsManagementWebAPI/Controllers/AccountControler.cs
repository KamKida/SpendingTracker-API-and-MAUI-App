using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingsManagementWebAPI.Dtos.UserDtos;
using SpendingsManagementWebAPI.Services;
using System.Threading.Tasks;

namespace SpendingsManagementWebAPI.Controllers
{
    [Route("spendingApi/account")]
    [ApiController]
    public class AccountControler : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountControler(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody]RegisterUserDto dto)
        {
            await _accountService.RegisterUser(dto);

            return Ok();
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto dto)
        {
            var token = await _accountService.Login(dto);

            return Ok(token);
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            await _accountService.Delete();

            return Ok("Konto zostało usunięte.");
        }

        [HttpPut("edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody]EditUserDto dto)
        {
            await _accountService.Edit(dto);

            return Ok("Zmiany zostały zapisane.");
        }
    }
}
