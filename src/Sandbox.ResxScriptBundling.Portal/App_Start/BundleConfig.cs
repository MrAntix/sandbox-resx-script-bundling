using System.Web.Optimization;
using Sandbox.ResxScriptBundling.Portal.Bundling;
using Sandbox.ResxScriptBundling.Portal.Properties;

namespace Sandbox.ResxScriptBundling.Portal
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleResolver.Current = new ResxBundleResolver(bundles);

            BundleTable.EnableOptimizations = true;

            bundles.Add(
                new StyleBundle("~/bundles/css")
                    .Include("~/Styles/site.css")
                );

            bundles.Add(
                new ScriptBundle("~/bundles/app")
                    .Include("~/Scripts/app/*.js")
                );

            bundles.Add(
                new ResxScriptBundle("~/bundles/local")
                    .Include<JsResources>("app")
                    .Include<CommonResources>("common")
                );
        }
    }
}