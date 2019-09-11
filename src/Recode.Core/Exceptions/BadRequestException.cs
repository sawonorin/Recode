using System;
using System.Collections.Generic;
using System.Text;
using Recode.Core.Utilities;

namespace Recode.Core.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message)
        {
            base.Code = Constants.ResponseCodes.Failed;
            base.httpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
