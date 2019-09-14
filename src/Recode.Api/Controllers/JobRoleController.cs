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
    [Authorize(Roles = "CompanyAdmin")]
    public class JobRoleController : BaseApiController
    {
        private readonly IJobRoleService _jobRoleService;

        public JobRoleController(IJobRoleService jobRoleService)
        {
            _jobRoleService = jobRoleService;
        }

        /// <summary>
        /// Get All Job Roles within company
        /// Filter by name and departmentId
        /// paginated request with pageNo and pageSize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="departmentId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<JobRoleModelPage>))]
        public async Task<IActionResult> GetAll(string name = "", long departmentId = 0, int pageSize = 10, int pageNo = 1)
        {
            try
            {
                var response = await _jobRoleService.GetJobRoles(name: name, departmentId: departmentId , pageSize: pageSize, pageNo: pageNo);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<JobRoleModelPage>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<JobRoleModelPage>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<JobRoleModelPage>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get JobRole by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetJobRoleById/{Id}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<JobRoleModel>))]
        public async Task<IActionResult> GetJobRolesById(long Id)
        {
            try
            {
                var response = await _jobRoleService.GetJobRole(Id);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<JobRoleModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<JobRoleModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<JobRoleModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Create a new jobRole
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<JobRoleModel>))]
        public async Task<IActionResult> Create(UpdateJobRoleModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _jobRoleService.CreateJobRole(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<JobRoleModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<JobRoleModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<JobRoleModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Update an existing jobRole 
        /// Be sure to pass the jobRole Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<JobRoleModel>))]
        public async Task<IActionResult> Update(UpdateJobRoleModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _jobRoleService.UpdateJobRole(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<JobRoleModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<JobRoleModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<JobRoleModel>.ErrorOccured(ex.Message));
            }
        }

        [HttpDelete("Delete")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<bool>))]
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _jobRoleService.DeleteJobRole(Id);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<object>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<object>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<object>.ErrorOccured(ex.Message));
            }
        }
    }
}