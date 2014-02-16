using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MvcPaging;
using System.Data.Entity.Infrastructure;
using IDSM.Logging.Entities;

namespace IDSM.Logging.Repository
{
    /// <summary>
    /// This class extracts information that Elmah stores so that we can report on it
    /// </summary>
    public class ElmahRepository : ILogReportingRepository
    {
        ErrorLogsEntities _context = null;

        /// <summary>
        /// Default Constructor uses the default Entity Container
        /// </summary>
        public ElmahRepository()
        {
            _context = new ErrorLogsEntities();
        }

        /// <summary>
        /// Overloaded constructor that can take an EntityContainer as a parameter so that it can be mocked out by our tests
        /// </summary>
        /// <param name="context">The Entity context</param>
        public ElmahRepository(ErrorLogsEntities context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a filtered list of log events
        /// </summary>
        /// <param name="pageIndex">0 based page index</param>
        /// <param name="pageSize">max number of records to return</param>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <param name="logLevel">The level of the log messages</param>
        /// <returns>A filtered list of log events</returns>
        public IQueryable<LogEvent> GetByDateRangeAndType(int pageIndex, int pageSize, DateTime start, DateTime end, string logLevel)
        {
            IQueryable<LogEvent> list = null;

            //if((logLevel=="All")||(logLevel=="Error")){

                list = (from a in _context.ELMAH_Error
                                             where a.TimeUtc >= start && a.TimeUtc <= end
                                             //&& (logLevel == "All" || logLevel == "Error")
                                             //&& (logLevel == "All")
                                             select new LogEvent
                                             {
                                                 IdType = "guid"
                                                     //,
                                                     //  Id = ""
                                               ,
                                                 IdAsInteger = 0
                                               ,
                                                 IdAsGuid = a.ErrorId
                                               ,
                                                 LoggerProviderName = "Elmah"
                                               ,
                                                 LogDate = a.TimeUtc
                                               ,
                                                 MachineName = a.Host
                                               ,
                                                 Message = a.Message
                                               ,
                                                 Type = a.Type
                                               ,
                                                 Level = "Error"
                                               ,
                                                 Source = a.Source,
                                                 StackTrace = ""
                                             });
            //}else{
                  // gave cast error.
            //    list = (IQueryable<LogEvent>)Enumerable.Empty<LogEvent>();
            //}
                                        
                                         

            return list;
        }

        /// <summary>
        /// Returns a single Log event
        /// </summary>
        /// <param name="id">Id of the log event as a string</param>
        /// <returns>A single Log event</returns>
        public LogEvent GetById(string id)
        {
            Guid guid = new Guid(id);
            LogEvent logEvent = (from b in _context.ELMAH_Error
                                 where b.ErrorId == guid
                                 select new LogEvent
                                 {
                                     IdType = "guid"
                                 ,
                                     IdAsGuid = b.ErrorId
                                 ,
                                     LoggerProviderName = "Elmah"
                                 ,
                                     LogDate = b.TimeUtc
                                 ,
                                     MachineName = b.Host
                                 ,
                                     Message = b.Message
                                 ,
                                     Type = b.Type
                                 ,
                                     Level = "Error"
                                 ,
                                     Source = b.Source
                                 ,
                                     StackTrace = ""
                                 ,
                                     AllXml = b.AllXml
                                 })
                        .SingleOrDefault();

            return logEvent;
        }

        /// <summary>
        /// Clears log messages between a date range and for specified log levels
        /// </summary>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <param name="logLevels">string array of log levels</param>
        public void ClearLog(DateTime start, DateTime end, string[] logLevels)
        {
            string commandText = "delete from Elmah_Error WHERE TimeUtc >= @p0 and TimeUtc <= @p1";

            SqlParameter paramStartDate = new SqlParameter { ParameterName = "p0", Value = start.ToUniversalTime(), DbType = System.Data.DbType.DateTime };
            SqlParameter paramEndDate = new SqlParameter { ParameterName = "p1", Value = end.ToUniversalTime(), DbType = System.Data.DbType.DateTime };

           // _context.ExecuteStoreCommand(commandText, paramStartDate, paramEndDate);

            //required because this code (from http://dotnetdarren.wordpress.com/2010/07/29/logging-in-mvc-part-5-the-model-and-data-layer/)
            // was created with an older version of EF, where an objectcontext was used rather than a DBContext (which was created by EF)
            var adapter = (IObjectContextAdapter)_context;
            var objectContext = adapter.ObjectContext;
            objectContext.ExecuteStoreCommand(commandText, paramStartDate, paramEndDate);
        }

    }
}