using System;
using System.Collections.Generic;
using System.Text;
using Recode.Core.Utilities;

namespace Recode.Core.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message)
        {
            base.Code = Constants.ResponseCodes.NotFound;
            base.httpStatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}
