using IDSM.Model;
using IDSM.Repository;
using IDSM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using IDSM.Logging.Services.Logging;
using System.Net;

namespace IDSM.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult AccessDenied()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            Response.TrySkipIisCustomErrors = true;

            if (Request.IsAjaxRequest())
            {
                // return Json friendly response here
            }

            return View();
        }

        public ActionResult NotFound(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            if (Request.IsAjaxRequest())
            {
                // return Json friendly response here
            }

            ViewBag.Message = message; 
            return View();
        }

        //public ActionResult OperationFailed(string message)
        //{
        //    Response.StatusCode = (int)HttpStatusCode.
        //    Response.TrySkipIisCustomErrors = true;

        //    if (Request.IsAjaxRequest())
        //    {
        //        // return Json friendly response here
        //    }

        //    ViewBag.Message = message;
        //    return View();
        //}

        public ActionResult ApplicationError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;

            if (Request.IsAjaxRequest())
            {
                // return Json friendly response here
            }

            return View();
        }
    }
}
