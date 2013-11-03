using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Eka.Web.MusicBrainz
{
    public class RecordingSearchResult
    {
        public string Id { get; protected set; }
        public string Title { get; protected set; }
        public string Artist { get; protected set; }
        public TimeSpan Duration { get; protected set; }

        private List<ReleaseResult> releases = new List<ReleaseResult>();

        internal RecordingSearchResult(XmlNode result, XmlNamespaceManager xmlns)
        {
            try
            {
                this.Id = result.SelectSingleNode("@id", xmlns).InnerText;
                this.Title = result.SelectSingleNode("_:title", xmlns).InnerText;
                this.Artist = result.SelectSingleNode("_:artist-credit/_:name-credit/_:artist/_:name", xmlns).InnerText;

                XmlNode lengthNode = result.SelectSingleNode("_:length", xmlns);
                if (lengthNode != null)
                {
                    this.Duration = new TimeSpan(0, 0, 0, 0, int.Parse(lengthNode.InnerText));
                }
                else
                {
                    this.Duration = new TimeSpan(0, 0, 0);
                }
            }
            catch { }
            try
            {
                XmlNodeList releases = result.SelectNodes("_:release-list/_:release", xmlns);
                foreach (XmlNode releaseNode in releases)
                {
                    this.releases.Add(new ReleaseResult(releaseNode, xmlns));
                }
            }
            catch { }
        }

        public IEnumerable<ReleaseResult> Releases
        {
            get
            {
                return this.releases;
            }
        }

        public override string ToString()
        {
            return this.Artist + " - " + this.Title;
        }
    }
}
