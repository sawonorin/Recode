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
    public class CompanyController : BaseApiController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<CompanyModelPage>))]
        public async Task<IActionResult> GetAll(string name = "", string code = "", int pageSize = 10, int pageNo = 1)
        {
            try
            {
                var response = await _companyService.GetCompanys(name: name, code: code, pageSize: pageSize, pageNo: pageNo);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<CompanyModelPage>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<CompanyModelPage>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<CompanyModelPage>.ErrorOccured(ex.Message));
            }
        }

        [HttpGet("GetCompanyById/{Id}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<CompanyModel>))]
        public async Task<IActionResult> GetCompanysById(long Id)
        {
            try
            {
                var response = await _companyService.GetCompany(Id);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<CompanyModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<CompanyModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<CompanyModel>.ErrorOccured(ex.Message));
            }
        }

        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<CompanyModel>))]
        public async Task<IActionResult> Create(CreateCompanyModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _companyService.CreateCompany(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<CompanyModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<CompanyModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<CompanyModel>.ErrorOccured(ex.Message));
            }
        }

        [HttpPost("Update")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<CompanyModel>))]
        public async Task<IActionResult> Update(CompanyModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _companyService.UpdateCompany(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<CompanyModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<CompanyModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<CompanyModel>.ErrorOccured(ex.Message));
            }
        }
    }
}