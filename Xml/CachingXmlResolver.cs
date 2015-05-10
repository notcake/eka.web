using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Xml;

namespace Eka.Web.Xml
{
    public class CachingXmlResolver : XmlUrlResolver
    {
        public static XmlResolver Default = new CachingXmlResolver();
        private static readonly DtdCache DtdCache = new DtdCache();
        private ICredentials credentials;

        public new ICredentials Credentials
        {
            get { return credentials; }
            set
            {
                credentials = value;
                base.Credentials = value;
            }
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri == null)
            {
                throw new ArgumentNullException(nameof(absoluteUri));
            }

            if (absoluteUri.Scheme == "http" && (ofObjectToReturn == null || ofObjectToReturn == typeof (Stream)))
            {
                var cachedResource = DtdCache.Get(absoluteUri.AbsoluteUri);
                if (cachedResource != null)
                {
                    return new MemoryStream(cachedResource);
                }
                if (absoluteUri.Host.ToLower() == "www.w3.org")
                {
                    if (absoluteUri.LocalPath.Contains("-//"))
                    {
                        return null;
                    }
                    throw new FileNotFoundException("w3.org URI " + absoluteUri.AbsoluteUri +
                                                    " was not found in the cache!");
                }

                var request = (HttpWebRequest) WebRequest.Create(absoluteUri);
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.0; rv:22.0) Gecko/20100101 Firefox/22.0";
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                if (Credentials != null)
                {
                    request.Credentials = Credentials;
                }
                var response = request.GetResponse();
                return response.GetResponseStream();
            }
            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }
    }
}