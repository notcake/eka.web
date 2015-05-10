using System;
using System.Net;
using System.Xml;
using Eka.Web.Xml;

namespace Eka.Web.Wikipedia
{
    public class Page
    {
        public Page(string title)
        {
            PageUri = "https://en.wikipedia.org/wiki/" + Uri.EscapeDataString(title.Replace(" ", "_"));

            XmlDocument = null;

            try
            {
                XmlDocument = XmlLoader.Load(PageUri);
            }
            catch (WebException)
            {
                Exists = false;
                return;
            }

            Exists = true;

            // Infoboxes
            var infoboxTables = XmlDocument.SelectNodes("//table[contains(@class,'infobox')]");
            foreach (XmlNode infoboxTable in infoboxTables)
            {
                var header = infoboxTable.SelectSingleNode("tr/th");
                if (header == null)
                {
                    continue;
                }

                var classAttribute = header.SelectSingleNode("@class");
                if (classAttribute != null)
                {
                    var infoboxType = classAttribute.InnerText;
                    if (infoboxType == "summary album")
                    {
                        AlbumInfobox = new AlbumInfobox(infoboxTable);
                    }
                }

                if (infoboxTable.SelectSingleNode("tr/th/a[contains(@href,'Single')]") != null)
                {
                    SingleInfobox = new SingleInfobox(infoboxTable);
                }
            }
        }

        public bool Exists { get; protected set; }
        public string PageUri { get; protected set; }
        public XmlDocument XmlDocument { get; protected set; }
        public AlbumInfobox AlbumInfobox { get; protected set; }
        public SingleInfobox SingleInfobox { get; protected set; }
    }
}