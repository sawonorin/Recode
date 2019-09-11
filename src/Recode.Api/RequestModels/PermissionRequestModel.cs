using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class PermissionRequestModel : Model
    {
        [MinLength(4, ErrorMessage = "Permission name must be atleast 6 characters")]
        [MaxLength(50, ErrorMessage = "Permission name should not be more than 50 characters")]
        [Required(ErrorMessage = "Permission Name is required")]
        public string PermissionName { get; set; }

        [MinLength(4, ErrorMessage = "Description should be atleast 6 characters")]
        [MaxLength(200, ErrorMessage = "Description should not be more than 200 characters")]
        public string PermissionDescription { get; set; }
    }
}
