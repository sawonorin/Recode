using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Service.Utilities;
using static Recode.Core.Utilities.Constants;

namespace Recode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users
        /// (vgg_admin) gets users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) gets users within their company
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModelPage>))]
        public async Task<IActionResult> GetAll(string email = "", string firstName = "", string lastName = "", string userName = "", long roleId = 0, int pageSize = 10, int pageNo = 1)
        {
            try
            {
                var response = await _userService.GetUsers(email: email, firstName: firstName, lastName: lastName, userName: userName, roleId: roleId, pageSize: pageSize, pageNo: pageNo);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModelPage>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModelPage>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModelPage>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get users by email
        /// (vgg_admin) gets users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) gets users within their company
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("GetUserByEmail/{email}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel>))]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            try
            {
                var response = await _userService.GetUserByEmail(email);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get users by an array of Ids
        /// (vgg_admin) gets users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) gets users within their company
        /// </summary>
        /// <param name="UserIds"></param>
        /// <returns></returns>
        [HttpPost("GetUserByIds")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel[]>))]
        public async Task<IActionResult> GetUsersByIds(long[] UserIds)
        {
            try
            {
                var response = await _userService.GetUsers(UserIds);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel[]>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel[]>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get users by roleId
        /// (vgg_admin) gets users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) gets users within their company
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("GetUsersByRoleId/{roleId}")]
        [AllowAnonymous]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel[]>))]
        public async Task<IActionResult> GetUsersByRoleId(long roleId)
        {
            try
            {
                var response = await _userService.GetUsersByRoleId(roleId);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel[]>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel[]>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get user by Id
        /// (vgg_admin) gets users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) gets users within their company
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetUserById/{Id}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel>))]
        public async Task<IActionResult> GetUsersById(long Id)
        {
            try
            {
                var response = await _userService.GetUser(Id);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get users by SSO Id
        /// (vgg_admin) gets users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) gets users within their company
        /// </summary>
        /// <param name="ssoUserId"></param>
        /// <returns></returns>
        [HttpGet("GetUserBySSOUserId/{ssoUserId}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel>))]
        public async Task<IActionResult> GetUsersBySSOUserId(string ssoUserId)
        {
            try
            {
                var response = await _userService.GetUser(ssoUserId);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// After User creation, if user does not receive confirmation email and EmailConfirmed = false
        /// Admin can call this endpoint to resend email confirmation mail
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [Authorize(Roles = "CompanyAdmin, clientadmin, vgg_admin")]
        [HttpGet("ResendEmailConfirmation/{UserId}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<bool>))]
        public async Task<IActionResult> ResendEmailConfirmation(long UserId)
        {
            try
            {
                var response = await _userService.ResendEmailConfirmation(UserId);
                if (response == false)
                {
                    return Ok(WebApiResponses<object>.ErrorOccured("Could not send email confirmation"));
                }
                return Ok(WebApiResponses<object>.Successful(response));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Toggle activate user.
        /// If user is active, he/she becomes inactive and vice versa
        /// (vgg_admin) for all users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) for only users within their company
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [Authorize(Roles = "CompanyAdmin, clientadmin, vgg_admin")]
        [HttpGet("ToggleActivateUser/{UserId}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel>))]
        public async Task<IActionResult> ToggleActivateUser(long UserId)
        {
            try
            {
                var response = await _userService.ToggleActivateUser(UserId);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Create a new user and assign he/she a role
        /// (vgg_admin) for all companies
        /// (CompanyAdmin, Recruiter, Interviewer) adds users to their company
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CompanyAdmin, clientadmin, vgg_admin")]
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel>))]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _userService.CreateUser(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Update an existing user and assign he/she a role
        /// (vgg_admin) for all users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) for only users within their company
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CompanyAdmin, clientadmin, vgg_admin")]
        [HttpPut("Update")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel>))]
        public async Task<IActionResult> Update(UserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _userService.UpdateUser(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Add role to a specific user
        /// (vgg_admin) for all users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) for only users within their company
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CompanyAdmin, clientadmin, vgg_admin")]
        [HttpPost("AddUserRole")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel>))]
        public async Task<IActionResult> AddUserRole(UserRoleModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _userService.AddUserRole(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Remove role from a specific user
        /// (vgg_admin) for all users in the database
        /// (CompanyAdmin, Recruiter, Interviewer) for only users within their company
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CompanyAdmin, clientadmin, vgg_admin")]
        [HttpPost("RemoveUserRole")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel>))]
        public async Task<IActionResult> RemoveUserRole(UserRoleModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _userService.RemoveUserRole(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<UserModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<UserModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<UserModel>.ErrorOccured(ex.Message));
            }
        }
    }
}