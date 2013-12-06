using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace Sandbox.ResxScriptBundling.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CultureActionFilter());
        }

        public class CultureActionFilter : IActionFilter
        {
            const string CultureKey = "culture";
            
            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (!filterContext.RouteData.Values.ContainsKey(CultureKey)) return;

                var cultureInfo = CultureInfo
                    .GetCultureInfo((string) filterContext.RouteData.Values["culture"]);

                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }

            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
            }
        }
    }
}