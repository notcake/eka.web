using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Eka.Web.Xml;

namespace Eka.Web.Wikipedia
{
    public class Page
    {
        public bool Exists { get; protected set; }
        public string PageUri { get; protected set; }
        public XmlDocument XmlDocument { get; protected set; }

        public AlbumInfobox AlbumInfobox { get; protected set; }
        public SingleInfobox SingleInfobox { get; protected set; }

        public Page(string title)
        {
            this.PageUri = "https://en.wikipedia.org/wiki/" + Uri.EscapeDataString(title.Replace(" ", "_"));

            this.XmlDocument = null;

            try
            {
                this.XmlDocument = XmlLoader.Load(this.PageUri);
            }
            catch (WebException)
            {
                this.Exists = false;
                return;
            }

            this.Exists = true;

            // Infoboxes
            XmlNodeList infoboxTables = this.XmlDocument.SelectNodes("//table[contains(@class,'infobox')]");
            foreach (XmlNode infoboxTable in infoboxTables)
            {
                XmlNode header = infoboxTable.SelectSingleNode("tr/th");
                if (header == null) { continue; }

                XmlNode classAttribute = header.SelectSingleNode("@class");
                if (classAttribute != null)
                {
                    string infoboxType = classAttribute.InnerText;
                    if (infoboxType == "summary album")
                    {
                        this.AlbumInfobox = new AlbumInfobox(infoboxTable);
                    }
                }

                if (infoboxTable.SelectSingleNode("tr/th/a[contains(@href,'Single')]") != null)
                {
                    this.SingleInfobox = new SingleInfobox(infoboxTable);
                }
            }
        }
    }
}
