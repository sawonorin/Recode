using Recode.Core.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Recode.Service.SSO
{
    public interface ISSOService
    {
        Task<ExecutionResponse<LoginResDto>> Login(LoginDto loginDto);
        Task<ExecutionResponse<SSOEmailConfirmationTokenResponseDto>> Register(SSOUserDto registerDto);
        Task<ExecutionResponse<SSOEmailConfirmationTokenResponseDto>> GetConfirmationToken(string UserId);
        Task<ExecutionResponse<SSOUser>> GetUser(string userId);
        Task<ExecutionResponse<SSOUpdateUserResDto>> UpdateUser(SSOUpdateUserDto sSOUserDto);
        Task<ExecutionResponse<object>> AddRemoveClaims(UserClaimModel claims, ClaimAction action);
        Task<ExecutionResponse<SSOForgotPasswordResponse>> ForgotPassword(string email);
        Task<ExecutionResponse<object>> ChangePassword(SSOChangePasswordRequestModel model);
        Task<ExecutionResponse<object>> ResetPassword(SSOResetPasswordRequestModel model);
        Task<ExecutionResponse<object>> ConfirmUser(SSOConfirmUserModel model);
        Task<ExecutionResponse<List<Claim>>> GetUserClaims(string ssoUserId);
    }
}
