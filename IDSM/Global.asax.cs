using IDSM.Helpers;
using IDSM.Model;
using IDSM.Models;
using IDSM.Repository;
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

namespace IDSM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<IDSMContext>(new DBInitialiser());
            //Database.SetInitializer<IDSMContext>(new DropCreateDatabaseIfModelChanges<IDSMContext>());
            //Database.SetInitializer<IDSMContext>(new DropCreateDatabaseAlways<IDSMContext>());
            
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //ViewEngines.Engines.Add(new RazorViewEngine());

            DependencyResolver.SetResolver(new UnityDependencyResolver(ModelContainer.Instance));

            WebSecurity.InitializeDatabaseConnection("IDSMContext", "UserProfile", "UserId", "UserName", true);


        }
    }
}