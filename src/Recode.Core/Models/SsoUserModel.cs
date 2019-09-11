using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Recode.Core.Models
{
    public class SsoUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool ConfirmEmail { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
