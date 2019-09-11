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

    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionManager _permissionManager;

        public PermissionsController(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        [Authorize(Roles = "clientadmin, GET_PERMISSIONS")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new ResponseModel<object[]>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = await _permissionManager.Get()
            }); ;
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] PermissionRequestModel[] model)
        //{
        //    if (model.Length <= 0)
        //    {
        //        throw new BadRequestException("Permission is empty");
        //    }

        //    var validate = model.FirstOrDefault(x => string.IsNullOrEmpty(x.PermissionName.Trim()));
        //    if (validate != null)
        //    {
        //        throw new BadRequestException("One of the permission is empty");
        //    }

        //    var result = await _permissionManager.AddPermission(model.Select(x => new PermissionModel
        //    {
        //        Description = x.PermissionDescription,
        //        PermissionName = x.PermissionName
        //    }).ToArray());

        //    return Ok(new ResponseModel<bool>
        //    {
        //        RequestSuccessful = true,
        //        ResponseCode = ResponseCodes.Successful,
        //        ResponseData = result
        //    }); ;
        //}
        [Authorize(Roles = "vggadmin")]
        [HttpPost("{permisssionId}/activate")]
        public async Task<IActionResult> Activate(long permisssionId)
        {
            if (permisssionId == default(long))
            {
                throw new BadRequestException("Invalid Permission");
            }
            var result = await _permissionManager.Activate(permisssionId);

            return Ok(new ResponseModel<bool>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = result
            });
        }
        [Authorize(Roles = "vggadmin")]
        [HttpPost("{permisssionId}/deactivate")]
        public async Task<IActionResult> Deactivate(long permisssionId)
        {
            if (permisssionId == default(long))
            {
                throw new BadRequestException("Invalid Permission");
            }
            var result = await _permissionManager.Deactivate(permisssionId);

            return Ok(new ResponseModel<bool>
            {
                RequestSuccessful = true,
                ResponseCode = ResponseCodes.Successful,
                ResponseData = result
            });
        }

        //[HttpPut("{permisssionId}")]
        //public async Task<IActionResult> Update(long permisssionId, [FromBody] PermissionRequestModel model)
        //{
        //    model.Validate();
        //    if (permisssionId == default(long))
        //    {
        //        throw new BadRequestException("Invalid Permission");
        //    }
        //    var result = await _permissionManager.Update(new PermissionModel
        //    {
        //        Description = model.PermissionDescription,
        //        PermissionName = model.PermissionName
        //    });

        //    return Ok(new ResponseModel<bool>
        //    {
        //        RequestSuccessful = true,
        //        ResponseCode = ResponseCodes.Successful,
        //        ResponseData = result
        //    });
        //}

    }
}