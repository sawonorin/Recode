using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Recode.Api.RequestModels
{
    public class OnboardRequestModel : Model
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email Address")]
        public string ContactEmail { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "The Contact First Name is required")]
        public string ContactFirstName { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "The Contact Last Name is required")]
        public string ContactLastName { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "The Company Name is required")]
        public string CompanyName { get; set; }

        [MinLength(6, ErrorMessage = "Password should be atleast 6 characters")]
        [MaxLength(12, ErrorMessage = "Password should not be more than 12 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?:(?=.*[a-z])(?=.*[$-/:-?{-~!^_`\[\]])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{8,}$", ErrorMessage = "Password should be a minimum of 8 characters with a number, an upper case letter, a lower case and a special character")]
        [Required(ErrorMessage = "The Password is required")]
        public string Password { get; set; }

        [MinLength(6, ErrorMessage = "Password should be atleast 6 characters")]
        [MaxLength(12, ErrorMessage = "Password should not be more than 12 characters")]
        [Required(ErrorMessage = "The Confirm password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
