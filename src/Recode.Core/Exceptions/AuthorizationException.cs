using System;
using System.Collections.Generic;
using System.Text;
using Recode.Core.Utilities;

namespace Recode.Core.Exceptions
{
    public class AuthorizationException : BaseException
    {

        public AuthorizationException(string message) : base(message)
        {
            base.Code = Constants.ResponseCodes.Unauthorized;
            base.httpStatusCode = System.Net.HttpStatusCode.Unauthorized;
        }
    }
}
