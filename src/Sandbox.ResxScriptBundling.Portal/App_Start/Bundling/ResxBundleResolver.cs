using System.Collections.Generic;
using System.Web.Optimization;

namespace Sandbox.ResxScriptBundling.Portal.Bundling
{
    public class ResxBundleResolver : IBundleResolver
    {
        readonly BundleResolver _wrapped;

        readonly BundleCollection _bundles;

        public ResxBundleResolver(
            BundleCollection bundles)
        {
            _bundles = bundles;
            _wrapped = new BundleResolver();
        }

        public bool IsBundleVirtualPath(string virtualPath)
        {
            return _wrapped.IsBundleVirtualPath(virtualPath);
        }

        public IEnumerable<string> GetBundleContents(string virtualPath)
        {
            return IsLocal(virtualPath)
                       ? new[] {virtualPath}
                       : _wrapped.GetBundleContents(virtualPath);
        }

        public string GetBundleUrl(string virtualPath)
        {
            var bundle = _bundles.GetBundleFor(virtualPath) as ResxScriptBundle;
            return bundle != null
                       ? bundle.LocalPath
                       : _wrapped.GetBundleUrl(virtualPath);
        }

        ResxScriptBundle GetLocal(string virtualPath)
        {
            return _bundles.GetBundleFor(virtualPath) as ResxScriptBundle;
        }

        bool IsLocal(string virtualPath)
        {
            return GetLocal(virtualPath) != null;
        }
    }
}