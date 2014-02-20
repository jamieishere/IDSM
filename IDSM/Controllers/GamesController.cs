﻿using IDSM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcPaging;
using IDSM.Logging.Services.Logging.Log4Net;
using WebMatrix.WebData;
using IDSM.Wrapper;
using IDSM.ViewModel;
using IDSM.Helpers;
using IDSM.Exceptions;
using IDSM.Logging.Services.Logging;
using AutoMapper;
using IDSM.ServiceLayer;


namespace IDSM.Controllers
{
     [Authorize]
    public class GamesController : Controller
    {
         private IService _service;

          public GamesController(IService service)
        {
            _service = service;
        }

          /// <summary>
          /// Index
          /// </summary>
          /// <returns></returns>
          public ViewResult Index()
          {
              return View();
          }

    }
}
