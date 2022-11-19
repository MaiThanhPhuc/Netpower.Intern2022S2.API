using Microsoft.AspNetCore.Mvc;
using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Services;
using Netpower.Intern2022S2.Services.Interfaces;

namespace Netpower.Intern2022S2.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserLoginDTO user)
        {
            var result = await _accountServices.Login(user);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDTO user)
        {
           
            var result = await _accountServices.Register(user);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("forgot-password")]

        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return StatusCode(400,new ApiResponse(400, "Email is required", null!));
            }
            var result = await _accountServices.ForgotPassword(email);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("verifyEmail")]
        public async Task<IActionResult> VerifyEmail(Guid id, string token)
        {
            var result = await _accountServices.CheckVerifyEmail(id, token);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("resendVerifyEmail")]
        public async Task<IActionResult> ResendEmail(Guid id)
        {
            var result = await _accountServices.ResendVerifyMail(id);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword( Guid id, string token, string password)
        {
            var result = await _accountServices.ResetPassword(id, token, password);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("resendResetPasswordEmail")]
        public async Task<IActionResult> ResendResetPassword(Guid id)
        {
            var result = await _accountServices.ResendResetPasswordMail(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
