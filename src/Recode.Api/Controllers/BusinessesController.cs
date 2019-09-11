using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recode.Core.Exceptions;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Api.RequestModels;
using static Recode.Core.Utilities.Constants;

namespace Recode.Api.Controllers
{
    [Authorize]
    [Route("api/Businesses")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly IBusinessManager _businessManager;
        public BusinessesController(IBusinessManager businessManager)
        {
            _businessManager = businessManager;
        }

        [Authorize(Roles = "ACTIVATE_BUSINESS, clientadmin")]
        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody]BaseRequestModel model)
        {
            if (model.Id == default(long))
            {
                throw new BadRequestException("Business is required");
            }

            bool result = await _businessManager.Activate(model.Id);

            if (result) return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Business has been activated successfully."
            });
            else return BadRequest(new ResponseModel<object>
            {
                ResponseCode = ResponseCodes.Failed,
                Message = "Failed. Please try again or contact your administrator"
            });
        }

        [Authorize(Roles = "DEACTIVATE_BUSINESS, clientadmin")]
        [HttpPost("deactivate")]
        public async Task<IActionResult> DeActivate([FromBody]BaseRequestModel model)
        {
            if (model.Id == default(long))
            {
                throw new BadRequestException("Business is required");
            }

            bool result = await _businessManager.Deactivate(model.Id);

            if (result) return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Business has been deactivated successfully."
            });
            else return BadRequest(new ResponseModel<object>
            {
                ResponseCode = ResponseCodes.Failed,
                Message = "Failed. Please try again or contact your administrator"
            });
        }

        [Authorize(Roles = "CREATE_BUSINESS, clientadmin")]
        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromBody] BusinessRequestModel model)
        {
            model.Validate();

            bool result = await _businessManager.CreateBusiness(new BusinessModel
            {
                BusinessCode = model.BusinessCode,
                BusinessName = model.BusinessName,
                Description = model.Description
            });

            if (result) return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Business has been created successfully."
            });
            else return BadRequest(new ResponseModel<object>
            {
                ResponseCode = ResponseCodes.Failed,
                Message = "Failed. Please try again or contact your administrator"
            });
        }

        [Authorize(Roles = "ADD_USER_TO_BUSINESS, clientadmin")]
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] UserBusinessRequestModel model)
        {
            model.Validate();
            bool result = await _businessManager.AddUser(model.UserId, model.BusinessId);

            if (result) return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "User has been added to the business successfully."
            });
            else return BadRequest(new ResponseModel<object>
            {
                ResponseCode = ResponseCodes.Failed,
                Message = "Failed. Please try again or contact your administrator"
            });
        }

        [Authorize(Roles = "UPDATE_BUSINESS, clientadmin")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromBody] BusinessRequestModel model, [FromRoute] long Id)
        {
            model.Validate();

            var result = await _businessManager.UpdateBusiness(new BusinessModel
            {
                BusinessCode = model.BusinessCode,
                BusinessName = model.BusinessName,
                Description = model.Description,
                Id = Id
            });

            if (result) return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Business has been updated successfully."
            });
            else return BadRequest(new ResponseModel<object>
            {
                ResponseCode = ResponseCodes.Failed,
                Message = "Failed. Please try again or contact your administrator"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _businessManager.Get(pageSize, pageNumber)
            });
        }

    }
}