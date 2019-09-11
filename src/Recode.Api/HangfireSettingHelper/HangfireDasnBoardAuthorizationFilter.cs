using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Recode.Api.HangfireSettingHelper
{
    public class HangfireDasnBoardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var currentcontext = context.GetHttpContext();
            var userRole = currentcontext.User.FindFirst(ClaimTypes.Role)?.Value;
            return userRole == "vgg_admin";
        }
    }
}
