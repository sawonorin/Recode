using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Exceptions;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Interfaces.Repositories;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Core.Utilities;
using Recode.Data.AppEntity;
using Recode.Repository.CoreRepositories;
using Recode.Service;
using Recode.Service.SSO;

namespace Recode.Core.Managers
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextExtensionService _httpContextService;
        private readonly IRepositoryQuery<User, long> _userQueryRepo;
        private readonly ISSOService _ssoService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public AuthService(
            IRepositoryQuery<User, long> userQueryRepo,
            ISSOService ssoService,
            IUserService userService,
            IEmailService emailService, IHttpContextExtensionService httpContextExtensionService)
        {
            _httpContextService = httpContextExtensionService;
            _ssoService = ssoService;
            _userService = userService;
            _emailService = emailService;
            _userQueryRepo = userQueryRepo;
        }

        public async Task<bool> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new BadRequestException("Email is required");
            }
            if (!email.IsValidEmail())
            {
                throw new BadRequestException("Email is not valid");
            }

            var model = _userQueryRepo.GetAll().FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower());

            if (model == null)
                throw new NotFoundException("User does not exist");

            var response = await _ssoService.ForgotPassword(email);

            if (response.ResponseCode == ResponseCode.Ok)
            {
                bool result = _emailService.ForgetPasswordEmail(response.ResponseData.Response.PasswordToken,
                     email, $"{model.LastName} {model.FirstName}",
                     model.SSOUserId);

                if (!result)
                {
                    throw new BadRequestException("Forget password request failed");
                }

                return result;
            }
            else
                return false;
        }

        public async Task<bool> ResetPassword(string token, string newPassword, string userId)
        {
            var response = await _ssoService.ResetPassword(new SSOResetPasswordRequestModel { NewPassword = newPassword, Token = token, UserId = userId });
            if (response.ResponseCode != ResponseCode.Ok)
            {
                throw new BadRequestException("Reset password request failed");
            }

            return true;
        }

        public async Task<bool> ChangePassword(string currentPassword, string newPassword)
        {
            var result = await _ssoService.ChangePassword(new SSOChangePasswordRequestModel { UserId = _httpContextService.GetCurrentUserId(), CurrentPassword = currentPassword, NewPassword = newPassword });
            if (result.ResponseCode != ResponseCode.Ok)
            {
                throw new BadRequestException("Change password request failed");
            }

            return true;
        }

        public async Task<Claim[]> ValidateUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new BadRequestException("User is required");

            var user = _userQueryRepo.GetAll().FirstOrDefault(x => x.SSOUserId == userId);
            if (user == null)
                throw new BadRequestException("User does not exist on recode");

            if (!user.IsActive)
                throw new BadRequestException("User is not active. Please contact your administrator.");

            string ssoRole = _httpContextService.GetSSORole();
            //var clms = await _permissionRepository.GetClaims(userId);
            var claims = new Claim[]
            {
                    new Claim("companyId", user.CompanyId.ToString())
                    , new Claim(ClaimTypes.Role, _httpContextService.GetSSORole())
            };
            return claims;
        }

        public async Task<bool> VerifyConfirmationToken(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new BadRequestException("User is invalid");
            }

            var response = await _ssoService.ConfirmUser(new SSOConfirmUserModel { UserId = userId, Token = token });

            if (response.ResponseCode != ResponseCode.Ok)
            {
                throw new BadRequestException("User account confirmation failed. Please try again");
            }

            // await _emailService.EmailConfirmation();

            return true;
        }

        public async Task<LoginResponseModel> Login(string email, string password)
        {
            var response = await _userService.GetUserByEmail(email);
            if (response.ResponseCode == ResponseCode.NotFound)
            {
                throw new BadRequestException("User does not exist");
            }
            else if (response.ResponseCode == ResponseCode.Ok)
            {
                var result = await _ssoService.Login(new LoginDto { UserName = email, Password = password });

                if (result.ResponseCode != ResponseCode.Ok)
                {
                    throw new BadRequestException(result.Message);
                }

                return new LoginResponseModel { Token = result.ResponseData.token_type, User = response.ResponseData };
            }
            throw new Exception("An error occured");
        }
    }
}
