using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Service.Models
{
    public class OrgForgetPasswordResponseModel
    {
        public OrgFPSubResponseModel Response { get; set; }
    }

    public class OrgFPSubResponseModel
    {
        public string PasswordToken { get; set; }
        public string UserId { get; set; }
    }
}
