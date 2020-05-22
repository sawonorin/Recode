using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Exceptions;
using Recode.Api.Filters;
using Recode.Service.Utilities;

namespace Recode.Api.Extensions
{
    public static class SecurityExtensions
    {
        public static void AddSecurityServices(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts =>
            {
                opts.Authority = configuration.GetValue<string>("SSoSetting:SSOIdentityUrl") + "/identity";
                opts.RequireHttpsMetadata = false;
                opts.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidAudiences = new[]
                    {
                        $"{opts.Authority}/resources"
                    }
                };
                opts.IncludeErrorDetails = true;
                opts.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = async context =>
                    {
                        Exception ex = context.Exception;
                        Log.Error(ex);

                        var result = GlobalExceptionFilter.GetStatusCode<object>(ex);
                        context.Response.StatusCode = (int)result.statusCode;
                        context.Response.ContentType = "application/json";

                        var responseJson = JsonConvert.SerializeObject(result.responseModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                        await context.Response.WriteAsync(responseJson);
                    }
                };
                opts.Events.OnChallenge = async context =>
                {
                    context.HandleResponse();
                    Exception ex = new AuthorizationException(context.ErrorDescription ?? "User is unauthorised");
                    Log.Error(ex);

                    var result = GlobalExceptionFilter.GetStatusCode<object>(ex);
                    context.Response.StatusCode = (int)result.statusCode;
                    context.Response.ContentType = "application/json";

                    var responseJson = JsonConvert.SerializeObject(result.responseModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    await context.Response.WriteAsync(responseJson);
                };
            });
        }
    }
}
