using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Interfaces.Services
{
    public interface ILoggerService
    {
        void Error(Exception ex);
        void Info2(string data);
        void Info(object data, string message = "Default");
        void Write(string msg);
    }
}
