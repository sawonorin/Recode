using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Recode.Api.Utilities;

namespace Recode.Api.RequestModels
{
    public class UserBusinessRequestModel : Model
    {
        [MinLength(30, ErrorMessage = "User details should be atleast 6 characters")]
        [Required(ErrorMessage = "User is required")]
        public string UserId { get; set; }

        [NotEmpty(ErrorMessage = "Business is required")]
        public long[] BusinessId { get; set; }
    }
}
