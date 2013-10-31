using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace IDSM.Logging.Services.Logging.Log4Net
{
     public class Log4NetLogger : ILogger
     {
          private ILog _logger;
 
         public Log4NetLogger()
         {
             // create a singleton instance (variation) 
             // http://stackoverflow.com/questions/15341878/how-does-a-log4net-instance-get-found-after-instantiating
            _logger = LogManager.GetLogger(this.GetType());
         }
 
         public void Info(string message)
         {
            _logger.Info(message);
         }
 
         public void Warn(string message)
         {
            _logger.Warn(message);
         }
 
         public void Debug(string message)
         {
            _logger.Debug(message);
         }
 
         public void Error(string message)
         {
            _logger.Error(message);
         }
 
         public void Error(Exception x)
         {
            Error(LogUtility.BuildExceptionMessage(x));
         }
 
         public void Error(string message, Exception x)
         {
            _logger.Error(message, x);
         }
 
         public void Fatal(string message)
         {
            _logger.Fatal(message);
         }
 
         public void Fatal(Exception x)
         {
            Fatal(LogUtility.BuildExceptionMessage(x));
         }
    }
}