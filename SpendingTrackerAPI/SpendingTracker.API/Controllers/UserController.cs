using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Responses;

namespace SpendingTracker.API.Controllers
{
    [Route("spending/account")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequest request)
        {
            await _userService.CreateUser(request);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserRequest request)
        {
            string token = await _userService.LoginUser(request);

            return Ok(token);
        }

        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] UserRequest request)
        {
            await _userService.ResetPassword(request);

            return Ok();
        }

        [HttpGet("getBaseInfo")]
       //[Authorize]
        public async Task<IActionResult> GetUserBaseData()
        {
            UserResponse response = await _userService.GetUserBaseData();

            return Ok(response);
        }
    }
}
