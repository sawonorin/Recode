using System;
namespace Recode.Service
{
    public class SSOEmailConfirmationTokenResponseDto
    {
        public string UserId { get; set; }
        public string EmailConfirmationToken { get; set; }
        public string PasswordResetToken { get; set; }
    }
}
