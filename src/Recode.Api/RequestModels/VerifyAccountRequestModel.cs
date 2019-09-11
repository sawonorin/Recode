using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class VerifyAccountRequestModel : Model
    {
        [MinLength(30, ErrorMessage = "User details should be atleast 6 characters")]
        [Required(ErrorMessage = "User details is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
    }
}
