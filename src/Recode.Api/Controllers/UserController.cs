using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModelPage>))]
        public async Task<IActionResult> GetAll(string email = "", string firstName = "", string lastName = "", string userName = "", int pageSize = 10, int pageNo = 1)
        {
            try
            {
                var response = await _userService.GetUsers(email: email, firstName: firstName, lastName: lastName, userName: userName, pageSize: pageSize, pageNo: pageNo);
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

        [HttpGet("GetUserByRoleId/{roleId}")]
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

        [HttpGet("GetUserById/{userId}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<UserModel>))]
        public async Task<IActionResult> GetUsersById(long userId)
        {
            try
            {
                var response = await _userService.GetUser(userId);
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

        [HttpPost("Update")]
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