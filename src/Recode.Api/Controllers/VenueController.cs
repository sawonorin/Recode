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
    public class VenueController : BaseApiController
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        /// <summary>
        /// Get All Venues within company
        /// Filter by name
        /// paginated request with pageNo and pageSize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<VenueModelPage>))]
        public async Task<IActionResult> GetAll(string name = "", int pageSize = 10, int pageNo = 1)
        {
            try
            {
                var response = await _venueService.GetVenues(name: name, pageSize: pageSize, pageNo: pageNo);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<VenueModelPage>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<VenueModelPage>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<VenueModelPage>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get available venues by startDate and endDate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("GetAvailableVenues")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<VenueModelPage>))]
        public async Task<IActionResult> GetAvailableVenues(DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _venueService.GetAvailableVenue(startDate, endDate);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<VenueModel[]>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<VenueModel[]>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<VenueModel[]>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get Venue by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetVenueById/{Id}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<VenueModel>))]
        public async Task<IActionResult> GetVenuesById(long Id)
        {
            try
            {
                var response = await _venueService.GetVenue(Id);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<VenueModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<VenueModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<VenueModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Create a new venue
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<VenueModel>))]
        public async Task<IActionResult> Create(VenueModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _venueService.CreateVenue(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<VenueModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<VenueModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<VenueModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Update an existing venue 
        /// Be sure to pass the venue Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<VenueModel>))]
        public async Task<IActionResult> Update(VenueModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _venueService.UpdateVenue(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<VenueModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<VenueModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<VenueModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Delete an existing venue by Id
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

                var response = await _venueService.DeleteVenue(Id);
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