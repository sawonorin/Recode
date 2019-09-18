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
    [Authorize(Roles = "Recruiter")]
    public class CandidateController : BaseApiController
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        /// <summary>
        /// Get All Candidates within company
        /// Filter by name
        /// paginated request with pageNo and pageSize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<CandidateModelPage>))]
        public async Task<IActionResult> GetAll(string firstName = "", string lastName = "", string email = "", long jobRole = 0, int pageSize = 10, int pageNo = 1)
        {
            try
            {
                var response = await _candidateService.GetCandidates(firstName: firstName, lastName: lastName, email: email, jobRole: jobRole, pageSize: pageSize, pageNo: pageNo);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<CandidateModelPage>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<CandidateModelPage>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<CandidateModelPage>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get Candidate by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetCandidateById/{Id}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<CandidateModel>))]
        public async Task<IActionResult> GetCandidatesById(long Id)
        {
            try
            {
                var response = await _candidateService.GetCandidate(Id);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<CandidateModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<CandidateModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<CandidateModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Create a new candidate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<CandidateModel>))]
        public async Task<IActionResult> Create(IFormFile resume, [FromForm] UpdateCandidateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                if (resume == null)
                    return Ok(WebApiResponses<CandidateModel>.ErrorOccured("No resume found"));

                var response = await _candidateService.CreateCandidate(resume.OpenReadStream(), resume.ContentType, model);

                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<CandidateModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<CandidateModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<CandidateModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Update an existing candidate 
        /// Be sure to pass the candidate Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<CandidateModel>))]
        public async Task<IActionResult> Update(IFormFile resume, [FromForm] CandidateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _candidateService.UpdateCandidate(resume?.OpenReadStream(), resume?.ContentType, model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<CandidateModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<CandidateModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<CandidateModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Delete an existing candidate by Id
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

                var response = await _candidateService.DeleteCandidate(Id);
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