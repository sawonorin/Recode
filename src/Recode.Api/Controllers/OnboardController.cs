using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets.Internal;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Models;
using Recode.Api.RequestModels;
using Recode.Data.AppEntity;
using static Recode.Core.Utilities.Constants;

namespace Recode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnboardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOnboardManager _OnboardMgr;

        public OnboardController(IOnboardManager onboardManager,
            IMapper mapper)
        {
            _mapper = mapper;
            _OnboardMgr = onboardManager;
        }

        [HttpPost]
        public async Task<IActionResult> Onboard([FromBody] OnboardRequestModel model)
        {
            model.Validate();
            var result = await _OnboardMgr.Onboard(new OnboardModel
            {
                ContactEmail = model.ContactEmail,
                ContactFirstName = model.ContactFirstName,
                ContactLastName = model.ContactLastName,
                CorporateName = model.CompanyName,
                Password = model.Password
            });

            if (result.isSuccessful) return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = result.msg
            });
            else return BadRequest(new ResponseModel<object>
            {
                ResponseCode = ResponseCodes.Failed,
                Message = "Onboarding failed. Please try again or contact your administrator"
            });
        }

        [Authorize(Roles = "CREATE_USER, clientadmin")]
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestModel model)
        {
            model.Validate();

            var result = await _OnboardMgr.CreateUserAndAssignToCorporate(new UserModel
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
               // Password = model.Password
            }, default(long));

            if (result.usermodel != null) return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = result.verifySent
               ? "Kindly verify your account by following the link sent to your email address." : $"{result.usermodel.LastName} {result.usermodel.FirstName} has been created successfully."
        });
            else return BadRequest(new ResponseModel<object>
            {
                ResponseCode = ResponseCodes.Failed,
                Message = "User Registration failed. Please try again or contact your administrator"
            });
        }

        [Authorize(Roles = "GET_USERS, clientadmin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery]int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _OnboardMgr.GetUsers(pageSize, pageNumber)
            });
        }
    }
}