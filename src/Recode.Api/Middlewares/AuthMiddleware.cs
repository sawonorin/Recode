using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Recode.Core.Exceptions;
using Recode.Core.Interfaces.Managers;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Api.Filters;
using Recode.Service.Utilities;

namespace Recode.Api.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate next;
        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            try
            {
                if (context.Request.Headers.ContainsKey("Authorization") && context.User.Identity.IsAuthenticated)
                {
                    // get sub
                    IHttpContextExtensionService httpExtSrv = (IHttpContextExtensionService)serviceProvider.GetService(typeof(IHttpContextExtensionService));
                    string userId = httpExtSrv.GetCurrentSSOUserId();
                    if (string.IsNullOrEmpty(userId))
                    {
                        throw new SecurityTokenValidationException();
                    }

                    IAuthService authMgr = (IAuthService)serviceProvider.GetService(typeof(IAuthService));
                    Claim[] claims = await authMgr.ValidateUser(userId);
                    if (claims.Length > 0)
                    {
                        context.User.AddIdentity(new ClaimsIdentity(claims));
                    }
                }
                await next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                var content = GlobalExceptionFilter.GetStatusCode<object>(ex);
                var res = JsonConvert.SerializeObject(content.responseModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                HttpResponse response = context.Response;
                context.Response.ContentType = "application/json";
                response.StatusCode = (int)content.statusCode;
                await context.Response.WriteAsync(res);
            }
        }
    }
}
