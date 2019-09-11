using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Recode.Core.Exceptions
{
    public class BaseException : Exception
    {
        public string Code { get; set; }
        public HttpStatusCode httpStatusCode { get; set; } = HttpStatusCode.InternalServerError;

        public BaseException(string message) : base(message)
        {

        }

        public BaseException(string code, string message, Exception exception) : base(message, exception)
        {

        }
    }
}
