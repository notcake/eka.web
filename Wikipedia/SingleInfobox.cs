using System;
using System.Xml;

namespace Eka.Web.Wikipedia
{
    public class SingleInfobox
    {
        internal SingleInfobox(XmlNode infoboxTable)
        {
            Name = infoboxTable.SelectSingleNode("tr/th").InnerText;

            foreach (XmlNode row in infoboxTable.SelectNodes("tr"))
            {
                var header = row.SelectSingleNode("th");
                var data = row.SelectSingleNode("td");
                if (header == null)
                {
                    continue;
                }
                if (data == null)
                {
                    continue;
                }

                if (header.InnerText == "Released")
                {
                    var releaseDateString = data.InnerText;
                    if (releaseDateString.IndexOf('(') >= 0)
                    {
                        releaseDateString = releaseDateString.Substring(0, releaseDateString.IndexOf('('));
                    }
                    if (releaseDateString.IndexOf('\n') >= 0)
                    {
                        releaseDateString = releaseDateString.Substring(0, releaseDateString.IndexOf('\n'));
                    }

                    if (releaseDateString.Length == 4)
                    {
                        releaseDateString = releaseDateString + "/01/01";
                    }

                    DateTime releaseDate;
                    HasReleaseDate = DateTime.TryParse(releaseDateString, out releaseDate);
                    ReleaseDate = releaseDate;
                }
                else if (header.InnerText == "Genre")
                {
                    Genre = data.InnerText;
                }
            }
        }

        public string Name { get; protected set; }
        public bool HasReleaseDate { get; protected set; }
        public DateTime ReleaseDate { get; protected set; }
        public string Genre { get; protected set; }
    }
}