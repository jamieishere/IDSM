using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.Unity.Utility;
using IDSM.Wrapper;
using IDSM.Logging;
using IDSM.Logging.Repository;

namespace IDSM.ServiceLayer
{
    ///<summary>
    /// ModelContainer class
    ///</summary>
    ///<remarks>
    /// See video  HTML5 and JSON, video 5/6/7.
    ///</remarks>
    //The instance of UnityContainer is created in the constructor 
    //rather than checking in the Instance property and performing a lock if needed
    public static class ModelContainer
    {
        private static IUnityContainer _Instance;

        static ModelContainer()
        {
            _Instance = new UnityContainer();
        }

        public static IUnityContainer Instance
        {
            get
            {
                // HierarchicalLifetimeManager is necessary for disposal per request
                // See http://unitymvc3.codeplex.com/
                // "important to note that any types that you want to be disposed at the end of the request must be given a lifetime of HierarchicalLifetimeManager"

                _Instance.RegisterType<IWebSecurityWrapper, WebSecurityWrapper>(new HierarchicalLifetimeManager())
                         .RegisterType<ILogReportingFacade, LogReportingFacade>(new HierarchicalLifetimeManager())
                         //.RegisterType<IService, Service>(new PerThreadLifetimeManager()) //don't use this, causes all sorts of problems.
                         .RegisterType<IService, Service>(new HierarchicalLifetimeManager())
                         .RegisterType<IUnitOfWork, UnitOfWork>();

                return _Instance;
            }
        }
    }
}
