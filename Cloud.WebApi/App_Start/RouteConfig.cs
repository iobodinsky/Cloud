using System.Web.Mvc;
using System.Web.Routing;
using Cloud.WebApi.Resources;

namespace Cloud.WebApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: Routes.Default,
                url: "{urlparams}",
                defaults: new {controller = "Cloud", action = "Cloud", urlparams = UrlParameter.Optional}
                );
        }
    }
}