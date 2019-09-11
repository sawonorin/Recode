using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Recode.Api.Controllers
{
    public class BaseApiController : ControllerBase
    {
        public static string GetModelStateErrors(ModelStateDictionary modelState)
        {
            StringBuilder result = new StringBuilder();
            var err = modelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
            foreach (var item in err)
            {
                result.Append(item + Environment.NewLine);
            }
            return result.ToString().Replace(Environment.NewLine, string.Empty);
        }
    }
}