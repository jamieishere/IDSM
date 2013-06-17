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

namespace IDSM.Controllers
{
    public class HomeController : Controller
    {
        private IPlayerRepository _repository;

        public HomeController()
        {
            _repository = new PlayerRepository();
        }

        public HomeController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ViewResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ViewResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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

        //
        // POST: /Upload/

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase FileUpload)
        {

            // Set up DataTable place holder
           // DataTable dt = new DataTable();
            var opStatus = new OperationStatus() { Status = false };

            if (FileUpload != null && FileUpload.ContentLength > 0)
            {
                string fileName = Path.GetFileName(FileUpload.FileName);
                
                //string path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
                string path = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings.Get("AppDataUploadsPath")), fileName);

                try
                {
                    opStatus.Status = true;
                    FileUpload.SaveAs(path);
                    //ViewData["Feedback"] = PlayerRepository.ProcessCSVHelper(path, new IDSMContext());
                }
                catch (Exception ex)
                {
                    opStatus = OperationStatus.CreateFromException("Error saving CSV file to "+path, ex);
                }

                if (opStatus.Status)
                {
                    opStatus = PlayerRepository.UploadPlayersCSV(path);  //PlayerRepository.ProcessCSVHelper(path, new IDSMContext());
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
