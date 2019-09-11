
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Recode.Data;
using Recode.Data.AppEntity;
using Recode.Data.MongoDB.Interfaces;
using Recode.Service.AuditHelper;

namespace Recode.Service.AspNetCoreHelper
{
    public class APIClientRequestFilter : IAsyncActionFilter, IAutoDependencyService
    {
        private readonly IClientInfoProvider _HttpContextClientInfoProvider;
        private readonly IClientRequestLogService _clientRequestLogService;

        public APIClientRequestFilter(IClientInfoProvider HttpContextClientInfoProvider, IClientRequestLogService clientRequestLogService)
        {
            _HttpContextClientInfoProvider = HttpContextClientInfoProvider;
            _clientRequestLogService = clientRequestLogService;
        }

        //public APIClientRequestFilter()
        //{
        //    _HttpContextClientInfoProvider = new HttpContextClientInfoProvider(new HttpContextAccessor());
        //}


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //if (!ShouldSaveAudit(context))
            //{
            //    await next();
            //    return;
            //}
            var auditInfo=ProcessCLientRequest(context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.AsType(),
                    context.ActionDescriptor.AsControllerActionDescriptor().MethodInfo,
                    context.ActionArguments);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var result = await next();
                try
                {
                    var responseObject = result.Result as OkObjectResult;
                    auditInfo.ResponseBody = JsonConvert.SerializeObject(responseObject.Value);
                }
                catch { }

                if (result.Exception != null && !result.ExceptionHandled)
                {
                    auditInfo.Exception = $"message : {result.Exception.Message} Exception : {result.Exception.InnerException}  StackTrace : {result.Exception.StackTrace}";
                }
            }
            catch (Exception ex)
            {
                auditInfo.Exception = $"message : {ex.Message} Exception : {ex.InnerException}  StackTrace : {ex.StackTrace}";
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                 _clientRequestLogService.SaveClientReQuest(auditInfo);
            }

        }


        private string ConvertArgumentsToJson(IDictionary<string, object> arguments)
        {
            try
            {
                if (arguments.IsNullOrEmpty())
                {
                    return "{}";
                }

                var dictionary = new Dictionary<string, object>();

                foreach (var argument in arguments)
                {
                    if (argument.Value != null)
                    {
                        dictionary[argument.Key] = argument.Value;
                    }
                }

                return JsonConvert.SerializeObject(dictionary, Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }); 
            }
            catch
            {
                return "{}";
            }
        }

        private ClientRequestLog ProcessCLientRequest(Type type, MethodInfo method, IDictionary<string, object> arguments)
        {
            var auditInfo = new ClientRequestLog()
            {
                ServiceName = type != null ? type.FullName : "",
                MethodName = method.Name,
                Parameters = ConvertArgumentsToJson(arguments),
                ExecutionTime = DateTime.Now,
                ClientIpAddress = _HttpContextClientInfoProvider.ClientIpAddress,
                BrowserInfo = _HttpContextClientInfoProvider.BrowserInfo
            };
            return auditInfo;
        }
        

    }
}
