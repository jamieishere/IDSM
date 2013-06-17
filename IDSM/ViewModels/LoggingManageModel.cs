using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using IDSM.Logging;
using IDSM.Logging.Services.Paging;
using IDSM.Model;

namespace IDSM.ViewModels
{
    public class LoggingManageModel
    {
        [DataType("DateTime")]
        public DateTime StartDate { get; set; }

        [DataType("DateTime")]
        public DateTime EndDate { get; set; }

        public string LogSourceName { get; set; }

        public string LogLevels { get; set; }

        public LoggingManageModel()
        {            
        }        
    }
}