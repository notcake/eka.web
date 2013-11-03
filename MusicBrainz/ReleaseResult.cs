using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Eka.Web.MusicBrainz
{
    public class ReleaseResult
    {
        public string Title { get; protected set; }
        public string Type { get; protected set; }
        public DateTime Date { get; protected set; }
        public bool HasDate { get; protected set; }

        internal ReleaseResult(XmlNode releaseNode, XmlNamespaceManager xmlns)
        {
            try
            {
                this.Title = releaseNode.SelectSingleNode("_:title", xmlns).InnerText;

                XmlNode typeNode = releaseNode.SelectSingleNode("_:release-group/_:primary-type", xmlns);
                if (typeNode != null)
                {
                    this.Type = typeNode.InnerText;
                }

                XmlNode secondaryTypeNode = releaseNode.SelectSingleNode("_:release-group/_:secondary-type-list/_:secondary-type", xmlns);
                if (secondaryTypeNode != null)
                {
                    this.Type = secondaryTypeNode.InnerText;
                }

                XmlNode dateNode = releaseNode.SelectSingleNode("_:date", xmlns);
                if (dateNode != null)
                {
                    string dateString = dateNode.InnerText;
                    if (dateString.Length == 4)
                    {
                        dateString = dateString + "/01/01";
                    }

                    DateTime date;
                    this.HasDate = DateTime.TryParse(dateString, out date);
                    this.Date = date;
                }
            }
            catch { }
        }

        public override string ToString()
        {
            return this.Title + " (" + this.Type + ")";
        }
    }
}
