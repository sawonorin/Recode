using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Recode.Service.Utilities
{
    public static class Log
    {
        private static readonly Logger _log = LogManager.GetLogger("APPLog"); //Specific Logger
        public static void Error(Exception ex)
        {
            _log.Error(ex,string.Format("Message : {0}  Source :{1}  StackTrace:{2} InnerException :{3}", ex.Message, ex.Source, ex.StackTrace, ex.InnerException));
            StackifyLib.Logger.QueueException("Error", ex, (ex.Message + ex.Source + ex.StackTrace));
        }

        public static void Info2(string data)
        {
           _log.Info(data);
            StackifyLib.Logger.QueueException(message: data, exceptionObject: null);
        }

        public static void Info(object data, string message = "Default")
        {
            StackifyLib.Logger.Queue("INFO", message, data);
        }
        
        public static void Write(string msg)
        {
            //LogWriter.LogWrite(msg);
        }
    }
}
