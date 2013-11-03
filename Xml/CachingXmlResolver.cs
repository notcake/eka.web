using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Xml;

namespace Eka.Web.Xml
{
    public class CachingXmlResolver : XmlUrlResolver
    {
        public static XmlResolver Default = new CachingXmlResolver();

        private static DtdCache DtdCache = new DtdCache();
        private ICredentials credentials = null;

        public CachingXmlResolver()
        {
        }

        public new ICredentials Credentials
        {
            get
            {
                return this.credentials;
            }
            set
            {
                this.credentials = value;
                base.Credentials = value;
            }
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri == null)
            {
                throw new ArgumentNullException("absoluteUri");
            }

            if (absoluteUri.Scheme == "http" && (ofObjectToReturn == null || ofObjectToReturn == typeof(Stream)))
            {
                byte[] cachedResource = CachingXmlResolver.DtdCache.Get(absoluteUri.AbsoluteUri);
                if (cachedResource != null)
                {
                    return new MemoryStream(cachedResource);
                }
                else if (absoluteUri.Host.ToLower() == "www.w3.org")
                {
                    if (absoluteUri.LocalPath.Contains("-//"))
                    {
                        return null;
                    }
                    throw new FileNotFoundException("w3.org URI " + absoluteUri.AbsoluteUri + " was not found in the cache!");
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(absoluteUri);
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.0; rv:22.0) Gecko/20100101 Firefox/22.0";
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                if (this.Credentials != null)
                {
                    request.Credentials = this.Credentials;
                }
                WebResponse response = request.GetResponse();
                return response.GetResponseStream();
            }
            else
            {
                return base.GetEntity(absoluteUri, role, ofObjectToReturn);
            }
        }
    }
}
