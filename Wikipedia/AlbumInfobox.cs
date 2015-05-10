using System;
using System.Xml;

namespace Eka.Web.Wikipedia
{
    public class AlbumInfobox
    {
        internal AlbumInfobox(XmlNode infoboxTable)
        {
            Name = infoboxTable.SelectSingleNode("tr/th").InnerText;

            var releaseDateString = infoboxTable.SelectSingleNode("tr/td[contains(@class,'published')]").InnerText;
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
            Genre = infoboxTable.SelectSingleNode("tr/td[contains(@class,'category')]").InnerText;
        }

        public string Name { get; protected set; }
        public bool HasReleaseDate { get; protected set; }
        public DateTime ReleaseDate { get; protected set; }
        public string Genre { get; protected set; }
    }
}