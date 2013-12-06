using System.Web.Mvc;
using System.Web.Routing;

namespace Sandbox.ResxScriptBundling.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new {culture = "en-GB", controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}