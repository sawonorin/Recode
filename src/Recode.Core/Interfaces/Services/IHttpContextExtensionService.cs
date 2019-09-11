using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recode.Core.Interfaces.Services
{
    public interface IHttpContextExtensionService
    {
        string GetCurrentUserId();
        long GetCurrentCompanyId();
        string GetSSORole();
    }
}
