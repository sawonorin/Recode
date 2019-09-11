using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class UserRoleRequestModel : Model
    {
        [Required(ErrorMessage = "User is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public long RoleId { get; set; }
    }
}
