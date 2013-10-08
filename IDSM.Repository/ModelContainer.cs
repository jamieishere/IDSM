using IDSM.Repository;
using Microsoft.Practices.Unity;
using IDSM.Model;
using IDSM.Wrapper;

namespace IDSM.Model
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
                // HierarchicalLifetimeManager is necessary for disposal issues caused by unity... 
                // See http://unitymvc3.codeplex.com/
                // "important to note that any types that you want to be disposed at the end of the request must be given a lifetime of HierarchicalLifetimeManager"
                _Instance.RegisterType<IUserTeamRepository, UserTeamRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IPlayerRepository, PlayerRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IGameRepository, GameRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IWebSecurityWrapper, WebSecurityWrapper>(new HierarchicalLifetimeManager());
               // _Instance.RegisterType<ILogReportingFacade, LogReportingFacade>(new HierarchicalLifetimeManager());
                return _Instance;
            }
        }
    }
}
