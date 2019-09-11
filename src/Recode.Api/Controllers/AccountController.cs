using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Recode.Core.Models;
using Recode.Api.RequestModels;
using static Recode.Core.Utilities.Constants;
using Recode.Core.Interfaces.Services;

namespace Recode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IAuthService _authManager;
        public AccountController(IAuthService authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("login")]
        [ProducesDefaultResponseType(typeof(ResponseModel<LoginResponseModel>))]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            model.Validate();
            var result = await _authManager.Login(model.Email, model.Password);
            return Ok(new ResponseModel<LoginResponseModel>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Login Successful.",
                ResponseData = result
            });
        }

        [HttpGet("ForgotPassword")]
        [ProducesDefaultResponseType(typeof(ResponseModel<bool>))]
        public async Task<IActionResult> ForgotPassword([FromQuery]string email)
        {
            var result = await _authManager.ForgetPassword(email);
            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Kindly follow the link sent to your email to change your password.",
                ResponseData = result
            });
        }

        [HttpPost("ResetPassword")]
        [ProducesDefaultResponseType(typeof(ResponseModel<bool>))]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestModel model)
        {
            model.Validate();
            var result = await _authManager.ResetPassword(model.Token, model.NewPassword, model.UserId);

            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Your password has been reset successfully",
                ResponseData = result
            });
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        [ProducesDefaultResponseType(typeof(ResponseModel<bool>))]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            model.Validate();
            var result = await _authManager.ChangePassword(model.CurrentPassword, model.NewPassword);
            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Your password has been reset successfully",
                ResponseData = result
            });
        }

        [HttpPost("verifyaccount")]
        [ProducesDefaultResponseType(typeof(ResponseModel<bool>))]
        public async Task<IActionResult> VerifyAccount(VerifyAccountRequestModel model)
        {
            model.Validate();
            var result = await _authManager.VerifyConfirmationToken(model.UserId, model.Token);
            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "User Account verification successful.",
                ResponseData = result
            });
        }

    }
}
