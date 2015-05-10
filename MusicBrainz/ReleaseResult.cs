using System;
using System.Xml;

namespace Eka.Web.MusicBrainz
{
    public class ReleaseResult
    {
        internal ReleaseResult(XmlNode releaseNode, XmlNamespaceManager xmlns)
        {
            try
            {
                Title = releaseNode.SelectSingleNode("_:title", xmlns).InnerText;

                var typeNode = releaseNode.SelectSingleNode("_:release-group/_:primary-type", xmlns);
                if (typeNode != null)
                {
                    Type = typeNode.InnerText;
                }

                var secondaryTypeNode =
                    releaseNode.SelectSingleNode("_:release-group/_:secondary-type-list/_:secondary-type", xmlns);
                if (secondaryTypeNode != null)
                {
                    Type = secondaryTypeNode.InnerText;
                }

                var dateNode = releaseNode.SelectSingleNode("_:date", xmlns);
                if (dateNode != null)
                {
                    var dateString = dateNode.InnerText;
                    if (dateString.Length == 4)
                    {
                        dateString = dateString + "/01/01";
                    }

                    DateTime date;
                    HasDate = DateTime.TryParse(dateString, out date);
                    Date = date;
                }
            }
            catch
            {
            }
        }

        public string Title { get; protected set; }
        public string Type { get; protected set; }
        public DateTime Date { get; protected set; }
        public bool HasDate { get; protected set; }

        public override string ToString() => Title + " (" + Type + ")";
    }
}