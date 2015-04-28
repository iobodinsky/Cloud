using System.Web.Mvc;
using System.Web.Routing;

namespace Cloud.WebApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{urlparams}",
                defaults: new { controller = "Cloud", action = "Cloud", urlparams = UrlParameter.Optional }
            );
        }
    }
}
