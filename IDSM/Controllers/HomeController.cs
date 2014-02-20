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
using IDSM.ServiceLayer;

namespace IDSM.Controllers
{
    public class HomeController : Controller
    {
        //private IPlayerRepository _repository;
        private IService _service;

        public HomeController()
        {
            //_repository = new PlayerRepository();
        }

        public HomeController(IService service)
        {
            _service = service;
        }

        public ViewResult Index()
        {

            ViewBag.Title = Resources.labels.HomeTitle;
            ViewBag.Message = Resources.labels.HomeSubTitle;

            return View();
        }

        public ViewResult About()
        {
            ViewBag.Title = Resources.labels.AboutTitle;
            ViewBag.Message = Resources.labels.AboutSubTitle;

            return View();
        }

        public ViewResult Contact()
        {
            ViewBag.Title = Resources.labels.ContactTitle;
            ViewBag.Message = Resources.labels.ContactSubTitle;

            return View();
        }

        //
        // GET: /Upload/

        public ActionResult Upload()
        {
            var opStatus = new OperationStatus() { Status = false };
            ViewBag.OperationStatus = opStatus;
            return View();
        }

        /// <summary>
        /// Upload
        /// Takes a correctly formatted csv containing Player data & uploads
        /// </summary>
        /// <param name="FileUpload"></param>
        /// <returns>View</returns>
        /// <remarks>
        /// TODO:
        /// Currently only works for a small number of rows.  Real file size is 100,000+ rows.  This breaks the upload.
        /// Need to ensure it works for at least the full set of Premiership clubs.
        /// </remarks>
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase FileUpload)
        {
            var opStatus = new OperationStatus() { Status = false };

            if (FileUpload != null && FileUpload.ContentLength > 0)
            {
                string fileName = Path.GetFileName(FileUpload.FileName);
               
                string path = Path.Combine(ConfigurationManager.AppSettings["AppDataUploadsPath"], fileName);

                // take the upload file and save to the app_data/uploads folder.
                try
                {
                    opStatus.Status = true;
                    FileUpload.SaveAs(path);
                }
                catch (Exception ex)
                {
                    opStatus = OperationStatus.CreateFromException("Error saving CSV file to "+path, ex);
                    ILogger _logger = LogFactory.Logger();
                    _logger.Error(opStatus.Message, ex);
                }

                // process the csv and save players to database
                if (opStatus.Status)
                {
                    try
                    {
                        //opStatus = PlayerRepository.UploadPlayersCSV(path); 
                    }
                    catch (Exception ex)
                    {
                        opStatus = OperationStatus.CreateFromException("Error saving Players from csv file to database", ex);
                        ILogger _logger = LogFactory.Logger();
                        _logger.Error(opStatus.Message, ex);
                    }
                }
            }
            else
            {
                opStatus = OperationStatus.CreateFromException("Please select a file", null);
            }

            ViewBag.OperationStatus = opStatus;
            return View();
        }

        

    }
}
