using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sandbox.ResxScriptBundling.Portal.Bundling
{
    public class BundleContent
    {
        readonly string _hash;
        readonly string _value;

        public BundleContent(string value)
        {
            _value = value;
            _hash = GetHash(value);
        }

        public string Hash
        {
            get { return _hash; }
        }

        public string Value
        {
            get { return _value; }
        }

        static string GetHash(string content)
        {
            using (var hashAlgorithm = new SHA256Managed())
            {
                var hash = hashAlgorithm.
                    ComputeHash(Encoding.Unicode.GetBytes(content));

                return HttpServerUtility.UrlTokenEncode(hash);
            }
        }
    }
}