using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Recode.Core.Exceptions;
using Recode.Core.Models;
using Recode.Core.Utilities;
using Recode.Service.Utilities;

namespace Recode.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // _log.Information($"Error occured. Error details: {context.Exception.Message}, Stack Trace: {context.Exception.StackTrace}, Inner Exception Details: " + (context.Exception.InnerException == null ? context.Exception.Message : context.Exception.InnerException.Message));
            Log.Error(context.Exception);
            var content = GetStatusCode<object>(context.Exception);
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)content.Item2;
            response.ContentType = "application/json";

            context.Result = new JsonResult(content.responseModel);
        }

        public static (ResponseModel<T> responseModel, HttpStatusCode statusCode) GetStatusCode<T>(Exception exception)
        {
            switch (exception)
            {
                case BaseException bex:
                    return (new ResponseModel<T>
                    {
                        ResponseCode = bex.Code,
                        Message = bex.Message,
                        RequestSuccessful = false,
                    }, bex.httpStatusCode);
                case SecurityTokenExpiredException bex:
                    return (new ResponseModel<T>
                    {
                        ResponseCode = Constants.ResponseCodes.TokenExpired,
                        Message = "Session expired",
                        RequestSuccessful = false,
                    }, HttpStatusCode.Unauthorized);
                case SecurityTokenValidationException bex:
                    return (new ResponseModel<T>
                    {
                        ResponseCode = Constants.ResponseCodes.TokenValidationFailed,
                        Message = "Invalid authentication parameters",
                        RequestSuccessful = false,
                    }, HttpStatusCode.Unauthorized);
                default:
                    return (new ResponseModel<T>
                    {
                        ResponseCode = Constants.ResponseCodes.Failed,
                        Message = exception.Message,
                        RequestSuccessful = false
                    }, HttpStatusCode.InternalServerError);
            }
        }
    }
}
