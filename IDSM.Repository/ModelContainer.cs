using IDSM.Repository;
using Microsoft.Practices.Unity;
using IDSM.Model;

namespace IDSM.Model
{
    ///<summary>
    /// ModelContainer class
    ///</summary>
    ///<remarks>
    /// See video  HTML5 and JSON, video 7.
    ///</remarks>
    //This is a simplified version of the code shown in the videos
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
                // HierarchicalLifetimeManager is necessary for disposal issues caused by unity... or something - 'serving html5/json content, vid 5/6'
                _Instance.RegisterType<IUserTeamRepository, UserTeamRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IPlayerRepository, PlayerRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IGameRepository, GameRepository>(new HierarchicalLifetimeManager());

               // _Instance.RegisterType<ILogReportingFacade, LogReportingFacade>(new HierarchicalLifetimeManager());
                return _Instance;
            }
        }
    }
}
