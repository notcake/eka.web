using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Eka.Web.Wikipedia
{
    public class SingleInfobox
    {
        public string Name { get; protected set; }
        public bool HasReleaseDate { get; protected set; }
        public DateTime ReleaseDate { get; protected set; }
        public string Genre { get; protected set; }

        internal SingleInfobox(XmlNode infoboxTable)
        {
            this.Name = infoboxTable.SelectSingleNode("tr/th").InnerText;

            foreach (XmlNode row in infoboxTable.SelectNodes("tr"))
            {
                XmlNode header = row.SelectSingleNode("th");
                XmlNode data = row.SelectSingleNode("td");
                if (header == null) { continue; }
                if (data == null) { continue; }

                if (header.InnerText == "Released")
                {
                    string releaseDateString = data.InnerText;
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
                    this.HasReleaseDate = DateTime.TryParse(releaseDateString, out releaseDate);
                    this.ReleaseDate = releaseDate;
                }
                else if (header.InnerText == "Genre")
                {
                    this.Genre = data.InnerText;
                }
            }
        }
    }
}
