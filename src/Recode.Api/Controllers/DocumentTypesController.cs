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
    public class DocumentTypesController : ControllerBase
    {
        private readonly IDocumentTypeManager _docManager;
        public DocumentTypesController(IDocumentTypeManager docManager)
        {
            _docManager = docManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _docManager.Get()
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long Id)
        {

            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _docManager.Get(Id)
            });
        }

        [Authorize(Roles = "CREATE_DOCUMENT, clientadmin")]
        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody]DocumentRequestModel model)
        {
            model.Validate();
            bool result = await _docManager.AddDocumentType(new DocumentTypeModel
            {
                DocumentCode = model.DocumentCode,
                DocumentName = model.DocumentName,
                IsRequired = model.IsRequired
            });

            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Document has been created successfully.",
                ResponseData = result
            });
        }

        [Authorize(Roles = "UPDATE_DOCUMENT, clientadmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(long Id, [FromBody] DocumentRequestModel model)
        {
            model.Validate();
            if (Id == default(long))
            {
                throw new BadRequestException("Invalid request. Document is required");
            }

            bool result = await _docManager.UpdateDocumentType(new DocumentTypeModel
            {
                DocumentCode = model.DocumentCode,
                DocumentName = model.DocumentName,
                IsRequired = model.IsRequired,
                Id = Id
            });

            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Document has been updated successfully.",
                ResponseData = result
            });
        }

        [Authorize(Roles = "ACTIVATE_DOCUMENT, clientadmin")]
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateDocument([FromQuery] long id)
        {
            if (id == default(long))
            {
                throw new BadRequestException("Document is required");
            }

            bool result = await _docManager.Activate(id);
            if (result) return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Activation is successful"
            });
            else return BadRequest(new ResponseModel<object>
            {
                ResponseCode = ResponseCodes.Failed,
                Message = "Failed. Please try again or contact your administrator"
            });
        }

        [Authorize(Roles = "DEACTIVATE_DOCUMENT, clientadmin")]
        [HttpPost("deactivate")]
        public async Task<IActionResult> DeActivateDocument([FromQuery] long id)
        {
            if (id == default(long))
            {
                throw new BadRequestException("Document is required");
            }

            bool result = await _docManager.Deactivate(id);
            if (result) return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                Message = "Deactivation is successful"
            });
            else return BadRequest(new ResponseModel<object>
            {
                ResponseCode = ResponseCodes.Failed,
                Message = "Failed. Please try again or contact your administrator"
            });
        }
    }
}