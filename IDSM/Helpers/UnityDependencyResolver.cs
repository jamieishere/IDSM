using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.Mvc;

namespace IDSM.Helpers
{
    ///<summary>
    /// UnityDependencyResolver
    ///</summary>
    ///<remarks>
    /// Implements the IDependencyResolver interface which is required by DependencyResolver.SetResolver in global asax
    ///     this is required so we can avoid having a default nul nul nul (etc) constructor in our controller for unity - basically to tidy up unity & dependency stuff
    ///     see html5 & json vid 6.
    ///</remarks>
    public class UnityDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _Container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            _Container = container;
        }

        public object GetService(Type serviceType)
        {
            if (!_Container.IsRegistered(serviceType) && (serviceType.IsAbstract || serviceType.IsInterface))
                return null;

            return _Container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _Container.ResolveAll(serviceType);
        }
    }
}