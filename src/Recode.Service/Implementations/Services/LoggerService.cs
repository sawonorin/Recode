using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using Recode.Core.Interfaces.Services;
using Recode.Data;

namespace Recode.Service.Implementations
{
    public class LoggerService : ILoggerService
    {
        private static readonly Logger _log = LogManager.GetLogger("APPLog");
        public void Error(Exception ex)
        {
            _log.Error(ex, string.Format("Message : {0}  Source :{1}  StackTrace:{2} InnerException :{3}", ex.Message, ex.Source, ex.StackTrace, ex.InnerException));
            StackifyLib.Logger.QueueException("Error", ex, (ex.Message + ex.Source + ex.StackTrace));
        }

        public void Info(object data, string message = "Default")
        {
            StackifyLib.Logger.Queue("INFO", message, data);
        }

        public void Info2(string data)
        {
            _log.Info(data);
            StackifyLib.Logger.QueueException(message: data, exceptionObject: null);
        }

        public void Write(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
