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
    public class BusinessAccountController : ControllerBase
    {
        private readonly IBusinessAccountManager _businessAccountMgr;

        public BusinessAccountController(IBusinessAccountManager businessAccountManager)
        {
            _businessAccountMgr = businessAccountManager;
        }

        [Authorize(Roles = "GET_ACCOUNT, clientadmin")]
        [HttpGet("corporate")]
        public async Task<IActionResult> GetByCorporate()
        {
            return Ok(new ResponseModel<object[]>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _businessAccountMgr.GetByCurrentCorporate()
            }); ;
        }

        [Authorize(Roles = "GET_ACCOUNT, clientadmin")]
        [HttpGet("businesses/{businessId}")]
        public async Task<IActionResult> Get(long businessId)
        {
            return Ok(new ResponseModel<object[]>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _businessAccountMgr.GetByBusiness(businessId)
            });
        }

        [Authorize(Roles = "CREATE_ACCOUNT, clientadmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessAccountRequestModel model)
        {
            model.Validate();

            var result = await _businessAccountMgr.AddAccount(new BusinessAccountModel
            {
                AccountName = model.AccountName,
                AccountNumber = model.AccountNumber,
                BusinessId = model.BusinessId,
                BankId = model.BankId,
                Comment = model.Comment
            });

            return Ok(new ResponseModel<bool>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = result
            }); ;
        }

        [Authorize(Roles = "ACTIVATE_ACCOUNT, clientadmin")]
        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] BaseRequestModel model)
        {
            if (model.Id == default(long))
            {
                throw new BadRequestException("Invalid Permission");
            }
            var result = await _businessAccountMgr.Activate(model.Id);

            return Ok(new ResponseModel<bool>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = result
            });
        }

        [Authorize(Roles = "DEACTIVATE_ACCOUNT, clientadmin")]
        [HttpPost("deactivate")]
        public async Task<IActionResult> Deactivate(BaseRequestModel model)
        {
            if (model.Id == default(long))
            {
                throw new BadRequestException("Invalid Permission");
            }
            var result = await _businessAccountMgr.Deactivate(model.Id);

            return Ok(new ResponseModel<bool>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = result
            });
        }

        [Authorize(Roles = "UPDATE_ACCOUNT, clientadmin")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(long Id, [FromBody] BusinessAccountRequestModel model)
        {
            model.Validate();
            if (Id == default(long))
            {
                throw new BadRequestException("Invalid request. Business account is required");
            }
            var result = await _businessAccountMgr.Update(new BusinessAccountModel
            {
                AccountName = model.AccountName,
                AccountNumber = model.AccountNumber,
                BusinessId = model.BusinessId,
                BankId = model.BankId,
                Comment = model.Comment,
                Id = Id
            });

            return Ok(new ResponseModel<bool>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = result
            });
        }

        [HttpPost("{id}/files/add/{name}")]
        public async Task<IActionResult> AddFile(string name, IFormFile file, long id)
        {
            if (file == null)
            {
                throw new BadRequestException("No File supplied");
            }
            if (id == default(long))
            {
                throw new BadRequestException("Business is required");
            }

            var fileModel = new FileModel(name, file.FileName, file.OpenReadStream());
            var upload = await _businessAccountMgr.AddFile(fileModel, id);
            return Ok(new ResponseModel<object>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = upload,
                Message = $"{name} has been uploaded successfully"
            });
        }

        [Authorize(Roles = "GET_ACCOUNT_BALANCE, clientadmin")]
        [HttpGet("banks/balance")]
        public async Task<IActionResult> ByBanks()
        {
            return Ok(new ResponseModel<object[]>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _businessAccountMgr.ByBanks()
            });
        }

        [Authorize(Roles = "GET_ACCOUNT, clientadmin")]
        [HttpGet("banks/{bankId}")]
        public async Task<IActionResult> ByBanks(long bankId)
        {
            if (bankId == default(long))
            {
                throw new BadRequestException("Bank is required");
            }

            return Ok(new ResponseModel<object[]>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _businessAccountMgr.ByBanks(bankId)
            }); ;
        }
    }
}