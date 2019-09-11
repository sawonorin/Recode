using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Recode.Api.Utilities;

namespace Recode.Api.RequestModels
{
    public class RolePermissionRequestModel : Model
    {
        [Required(ErrorMessage = "Role is required")]
        [NotEmpty(ErrorMessage = "Role is required")]
        public long RoleId { get; set; }

        [MinLength(1, ErrorMessage = "Permission is required")]
        [NotEmpty(ErrorMessage = "Permission is required")]
        public long[] PermissionIds { get; set; }
    }
}
