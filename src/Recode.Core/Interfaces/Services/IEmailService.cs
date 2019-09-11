using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Interfaces.Services
{
    public interface IEmailService
    {
        bool EmailConfirmation(string token, string email, string fullName, string userId);
        bool ForgetPasswordEmail(string token, string email, string fullName, string userId);
        bool SendMail(string subject, string email, string content);
    }
}
