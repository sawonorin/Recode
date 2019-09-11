using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class LoginResponseModel
    {
        public UserModel User { get; set; }
        public string Token { get; set; }
    }
}
