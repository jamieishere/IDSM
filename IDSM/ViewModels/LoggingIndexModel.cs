﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPaging;
using IDSM.Logging.Models;

namespace IDSM.ViewModels
{
    public class LoggingIndexModel
    {
        public IPagedList<LogEvent> LogEvents { get; set; }

        public string LoggerProviderName { get; set; }
        public string LogLevel { get; set; }
        public string Period { get; set; }

        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }

        public LoggingIndexModel()
        {
            CurrentPageIndex = 0;
            PageSize = 20;
        }
    }
}
