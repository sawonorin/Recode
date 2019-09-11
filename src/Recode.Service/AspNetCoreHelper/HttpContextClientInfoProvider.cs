using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Recode.Service.AuditHelper;

namespace Recode.Service.AspNetCoreHelper
{
    public class HttpContextClientInfoProvider : IClientInfoProvider
    {
        public string BrowserInfo => GetBrowserInfo();

        public string ClientIpAddress => GetClientIpAddress();

        public string ComputerName => GetComputerName();
        

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        /// <summary>
        /// Creates a new <see cref="HttpContextClientInfoProvider"/>.
        /// </summary>
        public HttpContextClientInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = httpContextAccessor.HttpContext;
        }

        protected virtual string GetBrowserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
            return httpContext?.Request?.Headers?["User-Agent"];
        }

        protected virtual string GetClientIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
                return httpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        protected virtual string GetComputerName()
        {
            return null; //TODO: Implement!
        }
    }
}
