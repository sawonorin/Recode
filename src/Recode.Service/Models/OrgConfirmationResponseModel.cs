using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Service.Models
{
    public class OrgConfirmationResponseModel
    {
        public string UserId { get; set; }
        public string EmailConfirmationToken { get; set; }
        public string PasswordResetToken { get; set; }
    }
}
