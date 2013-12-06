using System.Web.Optimization;

namespace Sandbox.ResxScriptBundling.Portal.Bundling
{
    public class ResxScriptBundleOptions
    {
        public ResxScriptBundleOptions()
        {
            ScriptNamespace = "resources";
            EnableOptimizations = BundleTable.EnableOptimizations;
        }

        public string ScriptNamespace { get; set; }
        public bool EnableOptimizations { get; set; }
    }
}