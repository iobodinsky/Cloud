using System.Net;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;

namespace Cloud.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<WebClient>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}