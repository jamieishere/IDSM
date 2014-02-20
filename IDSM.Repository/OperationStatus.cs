using System;
using System.Diagnostics;
using IDSM.Logging.Services.Logging;

namespace IDSM.Repository
{
    /// <summary>
    /// Utility class that holds all information returned from a database operation.  Better than returning void or bool.
    /// TODO:
    /// Rewatch AccountAtAGlance WorkingWithData, 9:demo: Creating Data Repository Classes, 13mins
    /// </summary>
    [DebuggerDisplay("Status: {Status}")]
    public class OperationStatus
    {
        public bool Status { get; set; }
        public int RecordsAffected { get; set; }
        public string Message { get; set; }
        public Object OperationId { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ExceptionInnerMessage { get; set; }
        public string ExceptionInnerStackTrace { get; set; }

        public static OperationStatus CreateFromException(string message, Exception ex, bool doLog = false)
        {
            var _opStatus = new OperationStatus
            {
                Status = false,
                Message = message,
                OperationId = null
            };

            if (ex != null)
            {
                _opStatus.ExceptionMessage = ex.Message;
                _opStatus.ExceptionStackTrace = ex.StackTrace;
                _opStatus.ExceptionInnerMessage = (ex.InnerException == null) ? null : ex.InnerException.Message;
                _opStatus.ExceptionInnerStackTrace = (ex.InnerException == null) ? null : ex.InnerException.StackTrace;
            }

            if (doLog)
            {
                ILogger _logger = LogFactory.Logger();
                _logger.Error(message, ex);
            }

            return _opStatus;
        }


    }
}
