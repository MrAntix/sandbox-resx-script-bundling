using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Optimization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sandbox.ResxScriptBundling.Portal.Bundling
{
    public class ResxScriptBundle : ScriptBundle
    {
        readonly IDictionary<string, Type> _types = new Dictionary<string, Type>();
        readonly IDictionary<string, BundleContent> _contents = new Dictionary<string, BundleContent>();
        readonly ResxScriptBundleOptions _options;

        static readonly object LockObject = new object();

        public ResxScriptBundle(string virtualPath)
            : this(virtualPath, new ResxScriptBundleOptions())
        {
        }

        public ResxScriptBundle(string virtualPath, ResxScriptBundleOptions options)
            : base(virtualPath)
        {
            _options = options;
        }

        public string LocalPath
        {
            get
            {
                var cultureName = Thread.CurrentThread.CurrentUICulture.Name;

                return string.Format("{0}?v={1}&c={2}",
                                     Path.TrimStart('~'),
                                     GetContent(_contents, _types, cultureName, _options.ScriptNamespace).Hash,
                                     cultureName);
            }
        }

        public ResxScriptBundle Include<T>(string name = null)
        {
            var type = GetValidType<T>();

            _types.Add(name ?? type.Name, type);

            return this;
        }

        public override BundleResponse GenerateBundleResponse(BundleContext context)
        {
            context.UseServerCache = false;

            var content = GetContent(
                _contents, _types,
                GetRequestParameter(context, "c"),
                _options.ScriptNamespace);

            return
                _options.EnableOptimizations
                    ? ApplyTransforms(context, content.Value, new FileInfo[] {})
                    : new BundleResponse(content.Value, new FileInfo[] {});
        }

        static BundleContent GetContent(
            IDictionary<string, BundleContent> contents,
            IEnumerable<KeyValuePair<string, Type>> types,
            string cultureName,
            string scriptNamespace)
        {
            if (contents == null) throw new ArgumentNullException("contents");
            if (types == null) throw new ArgumentNullException("types");
            if (cultureName == null) throw new ArgumentNullException("cultureName");

            lock (LockObject)
            {
                if (contents.ContainsKey(cultureName))
                {
                    return contents[cultureName];
                }

                var contentValue = GetJS(types, cultureName, scriptNamespace);

                var content = contents.Values.FirstOrDefault(c => c.Value == contentValue)
                              ?? new BundleContent(contentValue);

                contents.Add(cultureName, new BundleContent(contentValue));

                return content;
            }
        }

        static Type GetValidType<T>()
        {
            var type = typeof (T);
            if (!type.GetProperties().Any(p => p.Name.Equals("Culture")))
                throw new InvalidOperationException(
                    string.Format(
                        "{0} is not a valid resource type", type));

            return type;
        }

        static string GetJS(
            IEnumerable<KeyValuePair<string, Type>> types,
            string cultureName,
            string scriptNamespace)
        {
            if (types == null) throw new ArgumentNullException("types");
            if (cultureName == null) throw new ArgumentNullException("cultureName");

            var content = new StringBuilder();

            string ns;
            if (string.IsNullOrWhiteSpace(scriptNamespace))
                ns = "window";

            else
            {
                ns = string.Format("window.{0}", scriptNamespace);
                content.AppendFormat("{0}={0}||{{}};\n", ns);
            }

            foreach (var kv in types)
            {
                var type = kv.Value;
                var name = kv.Key;

                type.GetProperty("Culture")
                    .SetValue(null, CultureInfo.GetCultureInfo(cultureName));

                var json = new JObject();
                content.AppendFormat("{0}.{1} = ", ns, name);
                foreach (var properties in type.GetProperties()
                                               .Where(p => p.PropertyType == typeof (string)))
                {
                    var value = (string) properties.GetValue(null);
                    json.Add(properties.Name, value);
                }
                content.Append(json.ToString(Formatting.Indented));

                content.Append(";\n");
            }

            return content.ToString();
        }

        static string GetRequestParameter(BundleContext context, string name)
        {
            var url = context.HttpContext.Request.Url;

            if (url == null)
                throw new NullReferenceException("Request Url must not be null");

            return HttpUtility
                .ParseQueryString(url.Query)
                .Get(name);
        }
    }
}