using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Recode.Service.Models
{
    public class OrgUserModel
    {
        public string ssoUserId { get; set; }
        public long corporateId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string Email { get; set; }
        public Claim[] Claims { get; set; } = new Claim[] { };
        public string EmailConfirmationToken { get; set; }
    }
}
