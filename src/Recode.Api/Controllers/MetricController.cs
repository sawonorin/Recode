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
    public class MetricController : BaseApiController
    {
        private readonly IMetricService _metricService;

        public MetricController(IMetricService metricService)
        {
            _metricService = metricService;
        }

        /// <summary>
        /// Get All Metrics within company
        /// Filter by name
        /// paginated request with pageNo and pageSize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<MetricModelPage>))]
        public async Task<IActionResult> GetAll(string name = "", int pageSize = 10, int pageNo = 1)
        {
            try
            {
                var response = await _metricService.GetMetrics(name: name, pageSize: pageSize, pageNo: pageNo);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<MetricModelPage>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<MetricModelPage>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<MetricModelPage>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Get Metric by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetMetricById/{Id}")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<MetricModel>))]
        public async Task<IActionResult> GetMetricsById(long Id)
        {
            try
            {
                var response = await _metricService.GetMetric(Id);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<MetricModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<MetricModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<MetricModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Create a new metric
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<MetricModel>))]
        public async Task<IActionResult> Create(MetricModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _metricService.CreateMetric(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<MetricModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<MetricModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<MetricModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Update an existing metric 
        /// Be sure to pass the metric Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesDefaultResponseType(typeof(APIResponseModel<MetricModel>))]
        public async Task<IActionResult> Update(MetricModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrors(ModelState));

                var response = await _metricService.UpdateMetric(model);
                if (response.ResponseCode != ResponseCode.Ok)
                {
                    return Ok(WebApiResponses<MetricModel>.ErrorOccured(response.Message));
                }
                return Ok(WebApiResponses<MetricModel>.Successful(response.ResponseData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Ok(WebApiResponses<MetricModel>.ErrorOccured(ex.Message));
            }
        }

        /// <summary>
        /// Delete an existing metric by Id
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

                var response = await _metricService.DeleteMetric(Id);
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