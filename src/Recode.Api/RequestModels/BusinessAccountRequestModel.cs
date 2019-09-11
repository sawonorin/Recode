using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class BusinessAccountRequestModel : Model
    {
        [Required(ErrorMessage = "Business is required")]
        public long BusinessId { get; set; }

        [MinLength(10, ErrorMessage = "Account number should be 10 digit")]
        [MaxLength(10, ErrorMessage = "Account number should be 10 digit")]
        [Required(ErrorMessage = "Account number is required")]
        public string AccountNumber { get; set; }

        [MinLength(6, ErrorMessage = "Account Name should be atleast 6 characters")]
        [MaxLength(200, ErrorMessage = "Account Name should not be more than 200 characters")]
        [Required(ErrorMessage = "Account Name is required")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Bank is required")]
        public int BankId { get; set; }

        public string Comment { get; set; }
    }
}
