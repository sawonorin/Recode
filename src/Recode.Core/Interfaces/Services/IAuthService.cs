using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;
using Recode.Service;

namespace Recode.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Claim[]> ValidateUser(string userId);
        Task<bool> VerifyConfirmationToken(string userId, string token);
        Task<bool> ForgetPassword(string email);
        Task<bool> ResetPassword(string token, string newPassword, string userId);
        Task<bool> ChangePassword(string currentPassword, string newPassword);
        Task<LoginResponseModel> Login(string email, string password);
    }
}
