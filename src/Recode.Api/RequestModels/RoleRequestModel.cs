using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class RoleRequestModel : Model
    {
        [MinLength(2, ErrorMessage = "Role name should be atleast 6 characters")]
        [MaxLength(100, ErrorMessage = "Role name should not be more than 100 characters")]
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        [MinLength(2, ErrorMessage = "Description should be atleast 6 characters")]
        [MaxLength(200, ErrorMessage = "Description should not be more than 200 characters")]
        public string Description { get; set; }
        public long Id { get; set; }
    }
}
