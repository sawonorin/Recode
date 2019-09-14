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
    public class DepartmentController : BaseApiController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Get All Departments within company
        /// Filter by name
        /// paginated request with pageNo and pageSize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<DepartmentModelPage>))]
        public async Task<IActionResult> GetAll(string name = "", int pageSize = 10, int pageNo = 1)
        {
            try
            {
                var response = await _departmentService.GetDepartments(name: name, pageSize: pageSize, pageNo: pageNo);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<DepartmentModelPage>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<DepartmentModelPage>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<DepartmentModelPage>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get Department by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetDepartmentById/{Id}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<DepartmentModel>))]
        public async Task<IActionResult> GetDepartmentsById(long Id)
        {
            try
            {
                var response = await _departmentService.GetDepartment(Id);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<DepartmentModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<DepartmentModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<DepartmentModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Create a new department
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<DepartmentModel>))]
        public async Task<IActionResult> Create(DepartmentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _departmentService.CreateDepartment(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<DepartmentModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<DepartmentModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<DepartmentModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Update an existing department 
        /// Be sure to pass the department Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<DepartmentModel>))]
        public async Task<IActionResult> Update(DepartmentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _departmentService.UpdateDepartment(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<DepartmentModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<DepartmentModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<DepartmentModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Delete an existing department by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<bool>))]
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _departmentService.DeleteDepartment(Id);
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