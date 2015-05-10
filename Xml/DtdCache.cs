using System;
using System.Collections.Generic;
using System.Text;
using Eka.Web.Properties;

namespace Eka.Web.Xml
{
    internal class DtdCache
    {
        private readonly Dictionary<string, byte[]> Cache =
            new Dictionary<string, byte[]>(StringComparer.InvariantCultureIgnoreCase);

        public DtdCache()
        {
            Register("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", Resources.xhtml1_transitional_dtd);
            Register("http://www.w3.org/TR/xhtml1/DTD/xhtml-lat1.ent", Resources.xhtml_lat1_ent);
            Register("http://www.w3.org/TR/xhtml1/DTD/xhtml-symbol.ent", Resources.xhtml_symbol_ent);
            Register("http://www.w3.org/TR/xhtml1/DTD/xhtml-special.ent", Resources.xhtml_special_ent);
        }

        public bool Contains(string uri) => Cache.ContainsKey(uri);

        public byte[] Get(string uri)
        {
            if (!Cache.ContainsKey(uri))
            {
                return null;
            }

            return Cache[uri];
        }

        private void Register(string uri, string data)
        {
            Cache.Add(uri, Encoding.UTF8.GetBytes(data));
        }

        private void Register(string uri, byte[] data)
        {
            Cache.Add(uri, data);
        }
    }
}