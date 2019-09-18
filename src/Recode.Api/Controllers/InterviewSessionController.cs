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
    [Authorize(Roles = "Recruiter, Interviewer")]
    public class InterviewSessionController : BaseApiController
    {
        private readonly IInterviewSessionService _interviewSessionService;

        public InterviewSessionController(IInterviewSessionService interviewSessionService)
        {
            _interviewSessionService = interviewSessionService;
        }

        /// <summary>
        /// Get All InterviewSessions within company
        /// Filter by name
        /// paginated request with pageNo and pageSize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<InterviewSessionModelPage>))]
        public async Task<IActionResult> GetAll(string subject = "", long jobRoleId = 0, long recruiterId = 0, long venueId = 0, string status = "", int pageSize = 10, int pageNo = 1)
        {
            try
            {
                var response = await _interviewSessionService.GetInterviewSessions(subject: subject, jobRoleId: jobRoleId, recruiterId: recruiterId, venueId: venueId, status: status, pageSize: pageSize, pageNo: pageNo);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<InterviewSessionModelPage>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<InterviewSessionModelPage>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<InterviewSessionModelPage>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Set Interview Candidates
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SetInterviewCandidates")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<bool>))]
        public async Task<IActionResult> SetInterviewCandidates(InterviewSessionVariable model)
        {
            try
            {
                var response = await _interviewSessionService.SetInterviewCandidates(model.Ids, model.InterviewSessionId);
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

        /// <summary>
        /// Set Interview Metrics
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SetInterviewMetrics")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<bool>))]
        public async Task<IActionResult> SetInterviewMetrics(InterviewSessionVariable model)
        {
            try
            {
                var response = await _interviewSessionService.SetInterviewMetrics(model.Ids, model.InterviewSessionId);
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

        /// <summary>
        /// Set Interview Interviwers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SetInterviewInterviewers")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<bool>))]
        public async Task<IActionResult> SetInterviewInterviewers(InterviewSessionVariable model)
        {
            try
            {
                var response = await _interviewSessionService.SetInterviewInterviewers(model.Ids, model.InterviewSessionId);
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

        /// <summary>
        /// Get InterviewSession by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetInterviewSessionById/{Id}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<InterviewSessionModel>))]
        public async Task<IActionResult> GetInterviewSessionsById(long Id)
        {
            try
            {
                var response = await _interviewSessionService.GetInterviewSession(Id);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<InterviewSessionModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<InterviewSessionModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<InterviewSessionModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Create a new interviewSession
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<InterviewSessionModel>))]
        public async Task<IActionResult> Create(InterviewSessionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _interviewSessionService.CreateInterviewSession(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<InterviewSessionModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<InterviewSessionModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<InterviewSessionModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Update an existing interviewSession 
        /// Be sure to pass the interviewSession Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<InterviewSessionModel>))]
        public async Task<IActionResult> Update(InterviewSessionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _interviewSessionService.UpdateInterviewSession(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<InterviewSessionModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<InterviewSessionModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<InterviewSessionModel>.ErrorOccured(ex.Message));
            }
        }
    }
}