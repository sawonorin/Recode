using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class ResetPasswordRequestModel : Model
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [MinLength(6, ErrorMessage = "Password should be atleast 6 characters")]
        [MaxLength(12, ErrorMessage = "Password should not be more than 12 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?:(?=.*[a-z])(?=.*[$-/:-?{-~!^_`\[\]])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{8,}$", ErrorMessage = "Password should be a minimum of 8 characters with a number, an upper case letter, a lower case and a special character")]
        [Required(ErrorMessage = "The Password is required")]
        //[StringLength(12, MinimumLength = 6, ErrorMessage = "Password cannot be less than 6 characters")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "The Confirm password is required")]
        [MinLength(6, ErrorMessage = "Confirm Password should be atleast 6 characters")]
        [MaxLength(12, ErrorMessage = "Confirm Password should not be more than 12 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
