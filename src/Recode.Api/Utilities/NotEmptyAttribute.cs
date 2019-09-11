using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recode.Api.Utilities
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public class NotEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        public NotEmptyAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return false;
            }

            switch (value)
            {
                case Guid guid:
                    return guid != Guid.Empty;
                case long lg:
                    return lg != default(long);
                case long[] lg:
                    var res = lg.Any(x => x == default(long));
                    if (res && lg.Length > 1) ErrorMessage = "One of the {0} value is invalid";
                    return !res;
                default:
                    return true;
            }
        }
    }
}
