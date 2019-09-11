using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class BusinessRequestModel : Model
    {
        [MinLength(2, ErrorMessage = "Business code should be atleast 2 characters")]
        [MaxLength(20, ErrorMessage = "Business code should not be more than 20 characters")]
        [Required(ErrorMessage = "Business code is required")]
        public string BusinessCode { get; set; }

        [MinLength(3, ErrorMessage = "Description should be atleast 3 characters")]
        [MaxLength(250, ErrorMessage = "Description should not be more than 250 characters")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [MinLength(3, ErrorMessage = "Business Name should be atleast 3 characters")]
        [MaxLength(200, ErrorMessage = "Business Name should not be more than 200 characters")]
        [Required(ErrorMessage = "Business Name is required")]
        public string BusinessName { get; set; }
    }
}
