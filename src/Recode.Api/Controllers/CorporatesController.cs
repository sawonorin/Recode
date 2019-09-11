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

namespace Recode.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CorporatesController : ControllerBase
    {
        private readonly ICorporateManager _corporateManager;
        public CorporatesController(ICorporateManager corporateManager)
        {
            _corporateManager = corporateManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetCorporateId()
        {
            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _corporateManager.GetById()
            });
        }

        [Authorize(Roles = "ADD_FILE, clientadmin")]
        [HttpPost("files/add/{name}")]
        public async Task<ActionResult> AddFile(string name, IFormFile file, [FromQuery] int documentId)
        {
            if (file == null)
            {
                throw new Exception("No File supplied");
            }

            var fileModel = new FileModel(name, file.FileName, file.OpenReadStream());
            var upload = await _corporateManager.AddFile(fileModel, documentId);
            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = upload,
                Message = $"{name} has been uploaded successfully"
            });
        }

        [HttpGet("files")]
        public async Task<IActionResult> GetFiles()
        {
            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _corporateManager.GetDocument()
            });
        }

        [Authorize(Roles = "UPDATE_CORPORATE, clientadmin")]
        [HttpPut("{corporateId}")]
        public async Task<IActionResult> UpdateCorporate([FromBody] CorporateRequestModel model, long corporateId)
        {
            model.Validate();

            if (corporateId == default(long))
            {
                throw new BadRequestException("Corporate is required");
            }

            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = $"Corporate update was successfully",
                ResponseData = await _corporateManager.Update(new CorporateModel
                {
                    AlternateContactPhoneNumber = model.AlternateContactPhoneNumber,
                    BusinessAddress = model.BusinessAddress,
                    BusinessEmail = model.BusinessEmail,
                    City = model.City,
                    CorporateRcNumber = model.RCNumber,
                    Id = corporateId,
                    PrimaryContactPhoneNumber = model.PrimaryContactPhoneNumber,
                    State = model.State
                })
            }); ;

        }
    }
}