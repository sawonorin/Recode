using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class OnboardModel
    {
        public string CorporateName { get;set;}
        public string CorporateCode { get; set; }
        public string RCNumberCode { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
