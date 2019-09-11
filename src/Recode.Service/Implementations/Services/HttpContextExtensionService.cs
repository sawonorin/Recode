using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Interfaces.Services;

namespace Recode.Service.Implementations.Services
{
    public class HttpContextExtensionService : IHttpContextExtensionService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        public HttpContextExtensionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpAccessor = httpContextAccessor;
        }

        public long GetCurrentCompanyId()
        {
            var sub = _httpAccessor.HttpContext.User.Claims
               .FirstOrDefault(x => x.Type == "companyId");
            if (sub != null)
            {
                return long.Parse(sub.Value);
            }
            return default(long);
        }

        public string GetCurrentUserId()
        {
            var sub = _httpAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (sub != null)
            {
                return sub.Value;
            }
            return null;
        }

        public string GetSSORole()
        {
            //vigipay.orbit.role
            var sub = _httpAccessor.HttpContext.User.Claims
             .FirstOrDefault(x => x.Type == "recode.role");
            if (sub != null)
            {
                return sub.Value;
            }
            return string.Empty;
        }
    }
}
