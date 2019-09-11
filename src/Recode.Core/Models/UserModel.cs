using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace Recode.Core.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string SSOUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public RoleModel[] Roles { get; set; }
        public long CompanyId { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserModelPage
    {
        public UserModel[] Users { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
    }

    public class CreateUserModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public long RoleId { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string UserName { get; set; }

        [MinLength(6, ErrorMessage = "Password should be atleast 6 characters")]
        [MaxLength(12, ErrorMessage = "Password should not be more than 12 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?:(?=.*[a-z])(?=.*[$-/:-?{-~!^_`\[\]])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{6,}$", ErrorMessage = "Password should be a minimum of 6 characters with a number, an upper case letter, a lower case and a special character")]
        [Required(ErrorMessage = "The Password is required")]
        public string Password { get; set; }

        [MinLength(6, ErrorMessage = "Password should be atleast 6 characters")]
        [MaxLength(12, ErrorMessage = "Password should not be more than 12 characters")]
        [Required(ErrorMessage = "The Confirm password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public long CompanyId { get; set; }
    }

    public class UserRoleModel
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}
