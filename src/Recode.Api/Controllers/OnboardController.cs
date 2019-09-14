using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recode.Core.Exceptions;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Models;
using Recode.Api.RequestModels;
using static Recode.Core.Utilities.Constants;
using Recode.Core.Interfaces.Services;

namespace Recode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnboardController : BaseApiController
    {
        private readonly IUserService _userService;
        public OnboardController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Onboard a new vgg_admin. This endpoint should be called automatically from SSOUI.
        /// In the event that the callback wasn't received, do it manually
        /// Note that only one vgg_admin is expected to be on the platform
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Admin([FromBody] OnboardUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            var result = await _userService.OnboardAdmin(model);

            if(result.ResponseCode == ResponseCode.Ok)
            {
                return Ok(WebApiResponses<UserModel>.Successful(result.ResponseData));
            }
            else
            {
                return Ok(WebApiResponses<UserModel>.ErrorOccured(result.Message));
            }
        }
    }
}