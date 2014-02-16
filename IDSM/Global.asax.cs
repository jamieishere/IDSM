using IDSM.Helpers;
using IDSM.Model;
using IDSM.Models;
using IDSM.ServiceLayer;
using IDSM.Repository;
using IDSM.Repository.AutoMapperConfig;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using System.Web.Security;
using WebMatrix.WebData;
using IDSM.Logging.Services.Logging.Elmah;

namespace IDSM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //// initialise membership 
            WebSecurity.InitializeDatabaseConnection("IDSMContext", "UserProfile", "UserId", "UserName", true);
            Database.SetInitializer<IDSMContext>(new DBInitialiser());
            //Database.SetInitializer<IDSMContext>(new DropCreateDatabaseIfModelChanges<IDSMContext>());
            //Database.SetInitializer<IDSMContext>(new DropCreateDatabaseAlways<IDSMContext>());
            
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // Set Unity as our dependency resolved
            // see http://unitymvc3.codeplex.com/
            // "If you are not using the NuGet package, you will need to register the DependencyResolver and set up the Unity container yourself."
            DependencyResolver.SetResolver(new UnityDependencyResolver(ModelContainer.Instance));

            AutoMapperConfiguration.Configure();

            // For ELMAH
            // Setup our custom controller factory so that the [HandleErrorWithElmah] attribute is automatically injected into all of the controllers
            ControllerBuilder.Current.SetControllerFactory(new ErrorHandlingControllerFactory());

            // For Log4Net
            log4net.Config.XmlConfigurator.Configure();


        }
    }
}