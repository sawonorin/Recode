using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Recode.Service
{

    public class SSOUserData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberConfirmed { get; set; }
        public string TwoFactorEnabled { get; set; }
        public string LockoutEndDateUtc { get; set; }
        public string LockoutEnabled { get; set; }
        public string AccessFailedCount { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<SSOClaim> Claims { get; set; }
    }

    public class SSOUser
    {
        //public List<SSOUserData> Data { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool Exists { get; set; }
        public SSOUserDtoResponse Response { get; set; }
        public object CustomValue { get; set; }
        public int TotalRecords { get; set; }
    }

    public class SSOUserDtoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public object LastUpdate { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public object LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsGoogleAuthenticatorEnabled { get; set; }
        public List<SSOClaim> Claims { get; set; }
    }

    public class SSOUserDto
    {
        [MaxLength(64)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(64)]
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool ConfirmEmail { get; set; }
        [Required]
        public List<SSOClaim> Claims { get; set; }
    }

    public class SSOClaim
    {
        public SSOClaim(string type, string value)
        {
            Type = type;
            Value = value;
        }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class SSOUpdateUserDto
    {
        [MaxLength(64)]
        public string FirstName { get; set; }
        [MaxLength(64)]
        public string LastName { get; set; }
        [MaxLength(10)]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class SSOChangePasswordRequestModel
    {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ClearanceModel
    {
        public string UserId { get; set; }

        public bool Enabled { get; set; }
        public List<string> ClientIds { get; set; }
    }

    public class UserClaimModel
    {
        public string UserId { get; set; }
        public List<SSOClaim> Claims { get; set; }
    }

    public enum ClaimAction
    {
        Add,
        Remove
    }
    //public class SSOClaim
    //{
    //    [Required]
    //    public string Type { get; set; }
    //    [Required]
    //    public string Value { get; set; }
    //}

    public class ForgotPwdResponse
    {
        public string UserId { get; set; }
        public string PasswordToken { get; set; }
    }

    public class SSOForgotPasswordResponse
    {
        public bool EmailConfirmed { get; set; }
        public bool Exists { get; set; }
        public ForgotPwdResponse Response { get; set; }
        public string CustomValue { get; set; }
        public int TotalRecords { get; set; }
    }

    public class GetUserClaimsResModel
    {
        public bool EmailConfirmed { get; set; }
        public bool Exists { get; set; }
        public List<SSOClaim> Response { get; set; }
        public string CustomValue { get; set; }
        public int TotalRecords { get; set; }
    }

    public class SSOResetPasswordRequestModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class SSOConfirmUserModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}

