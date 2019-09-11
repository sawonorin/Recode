using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class DocumentRequestModel : Model
    {
        [MinLength(2, ErrorMessage = "Document name should be atleast 4 characters")]
        [MaxLength(200, ErrorMessage = "Document name should not be more than 200 characters")]
        [Required(ErrorMessage = "Document name is required")]
        public string DocumentName { get; set; }

        [MinLength(2, ErrorMessage = "Document code should be atleast 2 characters")]
        [MaxLength(20, ErrorMessage = "Document code should not be more than 20 characters")]
        [Required(ErrorMessage = "Document Code is required")]
        public string DocumentCode { get; set; }

        public bool IsRequired { get; set; }
    }
}
