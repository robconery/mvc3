using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace VidPub.Web.Infrastructure.Logging {
    public class NLogger : VidPub.Web.Infrastructure.Logging.ILogger {
        Logger _logger;
        public NLogger() {
            _logger = LogManager.GetCurrentClassLogger();
        }
        public void LogInfo(string message) {
            _logger.Info(message);
        }

        public void LogWarning(string message) {
            _logger.Warn(message);
        }

        public void LogDebug(string message) {
            _logger.Debug(message);
        }

        public void LogError(string message) {
            _logger.Error(message);
        }
        public void LogError(Exception x) {
            LogError(BuildExceptionMessage(x));
        }
        public void LogFatal(string message) {
            _logger.Fatal(message);
        }
        public void LogFatal(Exception x) {
            LogFatal(BuildExceptionMessage(x));
        }
        string BuildExceptionMessage(Exception x) {

            Exception logException = x;
            if (x.InnerException != null)
                logException = x.InnerException;

            string strErrorMsg = Environment.NewLine + "Error in Path :" + System.Web.HttpContext.Current.Request.Path;

            // Get the QueryString along with the Virtual Path
            strErrorMsg += Environment.NewLine + "Raw Url :" + System.Web.HttpContext.Current.Request.RawUrl;


            // Get the error message
            strErrorMsg += Environment.NewLine + "Message :" + logException.Message;

            // Source of the message
            strErrorMsg += Environment.NewLine + "Source :" + logException.Source;

            // Stack Trace of the error

            strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;

            // Method where the error occurred
            strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;
            return strErrorMsg;
        }
    }
}