using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eka.Web.Properties;

namespace Eka.Web.Xml
{
    class DtdCache
    {
        private Dictionary<string, byte[]> Cache = new Dictionary<string, byte[]>(StringComparer.InvariantCultureIgnoreCase);

        public DtdCache()
        {
            this.Register("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", Resources.xhtml1_transitional_dtd);
            this.Register("http://www.w3.org/TR/xhtml1/DTD/xhtml-lat1.ent", Resources.xhtml_lat1_ent);
            this.Register("http://www.w3.org/TR/xhtml1/DTD/xhtml-symbol.ent", Resources.xhtml_symbol_ent);
            this.Register("http://www.w3.org/TR/xhtml1/DTD/xhtml-special.ent", Resources.xhtml_special_ent);
        }

        public bool Contains(string uri)
        {
            return this.Cache.ContainsKey(uri);
        }

        public byte[] Get(string uri)
        {
            if (!this.Cache.ContainsKey(uri))
            {
                return null;
            }

            return this.Cache[uri];
        }

        private void Register(string uri, string data)
        {
            this.Cache.Add(uri, Encoding.UTF8.GetBytes(data));
        }

        private void Register(string uri, byte[] data)
        {
            this.Cache.Add(uri, data);
        }
    }
}
