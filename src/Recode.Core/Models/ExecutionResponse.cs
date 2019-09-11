using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class ExecutionResponse<T> where T : class
    {
        public string Message { get; set; }

        public ResponseCode ResponseCode { get; set; }

        public T ResponseData { get; set; }

        public object extraResult { get; set; }
    }


    public enum ResponseCode
    {
        Ok = 0,
        ValidationError = 1,
        NotFound = 2,
        ServerException = 3,
        AuthorizationFaild = 4,
        RetryTransaction = 5
    }
}
