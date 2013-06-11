using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPaging;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using IDSM.Models;
using IDSM.Model;
using IDSM.Model.Entities;

namespace IDSM.Repository
{
    /// <summary>
    /// This class extracts information that Log4Net stores so that we can report on it
    /// </summary>
    public class Log4NetRepository : ILogReportingRepository
    {
        ErrorLogsEntities _context = null;

        /// <summary>
        /// Default Constructor uses the default Entity Container
        /// </summary>
        public Log4NetRepository()
        {
            _context = new ErrorLogsEntities();
        }

        /// <summary>
        /// Overloaded constructor that can take an EntityContainer as a parameter so that it can be mocked out by our tests
        /// </summary>
        /// <param name="context">The Entity context</param>
        public Log4NetRepository(ErrorLogsEntities context)
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
            IQueryable<LogEvent> list = (from b in _context.Log4Net_Error
                                         where b.Date >= start && b.Date <= end
                                         && (b.Level == logLevel || logLevel == "All")
                                         select new LogEvent
                                         {
                                             IdType = "number"
                                             ,
                                             Id = ""
                                             ,
                                             IdAsInteger = 0 //b.Id  think log4net has changed and now uses GUID for it's ID?
                                             ,
                                             IdAsGuid = b.Id // Guid.NewGuid()
                                             ,
                                             LoggerProviderName = "Log4Net"
                                             ,
                                             LogDate = b.Date
                                             ,
                                             MachineName = b.Thread
                                             ,
                                             Message = b.Message
                                             ,
                                             Type = ""
                                             ,
                                             Level = b.Level
                                             ,
                                             Source = b.Thread
                                             ,
                                             StackTrace = ""
                                         });

            return list;
        }

        /// <summary>
        /// Returns a single Log event
        /// </summary>
        /// <param name="id">Id of the log event as a string</param>
        /// <returns>A single Log event</returns>
        public LogEvent GetById(string id)
        {
            //int logEventId = Convert.ToInt32(id);
            Guid logEventId = new Guid(id);

            LogEvent logEvent = (from b in _context.Log4Net_Error
                                 where b.Id == logEventId
                                 select new LogEvent
                                 {
                                     IdType = "number"
                                     ,
                                     IdAsInteger = 0 //b.Id  think log4net has changed and now uses GUID for it's ID?
                                      ,
                                     IdAsGuid = b.Id // Guid.NewGuid()
                                     ,
                                     LoggerProviderName = "Log4Net"
                                     ,
                                     LogDate = b.Date
                                     ,
                                     MachineName = b.Thread
                                     ,
                                     Message = b.Message
                                     ,
                                     Type = ""
                                     ,
                                     Level = b.Level
                                     ,
                                     Source = b.Thread
                                     ,
                                     StackTrace = ""
                                     ,
                                     AllXml = ""
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
            string logLevelList = "";
            foreach (string logLevel in logLevels)
            {
                logLevelList += ",'" + logLevel + "'";
            }
            if (logLevelList.Length > 0)
            {
                logLevelList = logLevelList.Substring(1);
            }

            string commandText = "delete from Log4Net_Error WHERE [Date] >= @p0 and [Date] <= @p1 and Level in (@p2)";

            SqlParameter paramStartDate = new SqlParameter { ParameterName = "p0", Value = start.ToUniversalTime(), DbType = System.Data.DbType.DateTime };
            SqlParameter paramEndDate = new SqlParameter { ParameterName = "p1", Value = end.ToUniversalTime(), DbType = System.Data.DbType.DateTime };
            SqlParameter paramLogLevelList = new SqlParameter { ParameterName = "p2", Value = logLevelList };

            //_context.ExecuteStoreCommand(commandText, paramStartDate, paramEndDate, paramLogLevelList);
            //required because this code (from http://dotnetdarren.wordpress.com/2010/07/29/logging-in-mvc-part-5-the-model-and-data-layer/)
            // was created with an older version of EF, where an objectcontext was used rather than a DBContext (which was created by EF)
            var adapter = (IObjectContextAdapter)_context;
            var objectContext = adapter.ObjectContext;
            objectContext.ExecuteStoreCommand(commandText, paramStartDate, paramEndDate, paramLogLevelList);
        }

    }
}