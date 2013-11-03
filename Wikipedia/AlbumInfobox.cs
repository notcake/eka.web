using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Eka.Web.Wikipedia
{
    public class AlbumInfobox
    {
        public string Name { get; protected set; }
        public bool HasReleaseDate { get; protected set; }
        public DateTime ReleaseDate { get; protected set; }
        public string Genre { get; protected set; }

        internal AlbumInfobox(XmlNode infoboxTable)
        {
            this.Name = infoboxTable.SelectSingleNode("tr/th").InnerText;

            string releaseDateString = infoboxTable.SelectSingleNode("tr/td[contains(@class,'published')]").InnerText;
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
            this.Genre = infoboxTable.SelectSingleNode("tr/td[contains(@class,'category')]").InnerText;
        }
    }
}
