using System;
using System.Collections.Generic;
using System.Text;
using Recode.Core.Utilities;

namespace Recode.Core.Exceptions
{
    public class AlreadyExistException : BaseException
    {
        public AlreadyExistException(string message) : base(message)
        {
            base.Code = Constants.ResponseCodes.AlreadyExist;
            base.httpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
