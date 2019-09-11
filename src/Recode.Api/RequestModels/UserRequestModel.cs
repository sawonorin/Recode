using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.RequestModels
{
    public class UserRequestModel : Model
    {
        [MinLength(2, ErrorMessage = "First name should be atleast 6 characters")]
        [MaxLength(100, ErrorMessage = "First name should not be more than 100 characters")]
        [Required(ErrorMessage = "The First Name is required")]
        public string FirstName { get; set; }

        [MinLength(2, ErrorMessage = "Last name should be atleast 6 characters")]
        [MaxLength(100, ErrorMessage = "Last name should not be more than 100 characters")]
        [Required(ErrorMessage = "The Last Name is required")]
        public string LastName { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "The Email is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?:(?=.*[a-z])(?=.*[$-/:-?{-~!^_`\[\]])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{8,}$", ErrorMessage = "Password should be a minimum of 8 characters with a number, an upper case letter, a lower case and a special character")]
        [Required(ErrorMessage = "The Password is required")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Password cannot be less than 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "The Confirm password is required")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Password cannot be less than 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
