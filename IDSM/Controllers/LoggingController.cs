using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDSM.Logging.Models.Repository;
using IDSM.ViewModels;
using IDSM.Helpers;
using MvcPaging;


namespace IDSM.Controllers
{
        [Authorize]
        public class LoggingController : Controller
        {
            private readonly LogReportingFacade loggingRepository;

            public LoggingController()
            {
                loggingRepository = new LogReportingFacade();
            }

            public LoggingController(LogReportingFacade repository)
            {
                loggingRepository = repository;
            }

            /// <summary>
            /// Returns the Index view
            /// </summary>
            /// <param name="Period">Text representation of the date time period. eg: Today, Yesterday, Last Week etc.</param>
            /// <param name="LoggerProviderName">Elmah, Log4Net, NLog, Health Monitoring etc</param>
            /// <param name="LogLevel">Debug, Info, Warning, Error, Fatal</param>
            /// <param name="page">The current page index (0 based)</param>
            /// <param name="PageSize">The number of records per page</param>
            /// <returns></returns>
            public ActionResult Index(string Period, string LoggerProviderName, string LogLevel, int? page, int? PageSize)
            {
                // Set up our default values
                string defaultPeriod = Session["Period"] == null ? "Today" : Session["Period"].ToString();
                string defaultLogType = Session["LoggerProviderName"] == null ? "All" : Session["LoggerProviderName"].ToString();
                string defaultLogLevel = Session["LogLevel"] == null ? "Error" : Session["LogLevel"].ToString();

                // Set up our view model
                LoggingIndexModel model = new LoggingIndexModel();

                model.Period = (Period == null) ? defaultPeriod : Period;
                model.LoggerProviderName = (LoggerProviderName == null) ? defaultLogType : LoggerProviderName;
                model.LogLevel = (LogLevel == null) ? defaultLogLevel : LogLevel;
                model.CurrentPageIndex = page.HasValue ? page.Value - 1 : 0;
                model.PageSize = PageSize.HasValue ? PageSize.Value : 20;

                TimePeriod timePeriod = TimePeriodHelper.GetUtcTimePeriod(model.Period);

                // Grab the data from the database
                model.LogEvents = loggingRepository.GetByDateRangeAndType(model.CurrentPageIndex, model.PageSize, timePeriod.Start, timePeriod.End, model.LoggerProviderName, model.LogLevel);

                // Put this into the ViewModel so our Pager can get at these values
                ViewData["Period"] = model.Period;
                ViewData["LoggerProviderName"] = model.LoggerProviderName;
                ViewData["LogLevel"] = model.LogLevel;
                ViewData["PageSize"] = model.PageSize;

                // Put the info into the Session so that when we browse away from the page and come back that the last settings are rememberd and used.
                Session["Period"] = model.Period;
                Session["LoggerProviderName"] = model.LoggerProviderName;
                Session["LogLevel"] = model.LogLevel;

                return View(model);
            }

        }
}
