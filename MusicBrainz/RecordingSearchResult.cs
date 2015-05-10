using System;
using System.Collections.Generic;
using System.Xml;

namespace Eka.Web.MusicBrainz
{
    public class RecordingSearchResult
    {
        private readonly List<ReleaseResult> releases = new List<ReleaseResult>();

        internal RecordingSearchResult(XmlNode result, XmlNamespaceManager xmlns)
        {
            try
            {
                Id = result.SelectSingleNode("@id", xmlns).InnerText;
                Title = result.SelectSingleNode("_:title", xmlns).InnerText;
                Artist = result.SelectSingleNode("_:artist-credit/_:name-credit/_:artist/_:name", xmlns).InnerText;

                var lengthNode = result.SelectSingleNode("_:length", xmlns);
                if (lengthNode != null)
                {
                    Duration = new TimeSpan(0, 0, 0, 0, int.Parse(lengthNode.InnerText));
                }
                else
                {
                    Duration = new TimeSpan(0, 0, 0);
                }
            }
            catch
            {
            }
            try
            {
                var releases = result.SelectNodes("_:release-list/_:release", xmlns);
                foreach (XmlNode releaseNode in releases)
                {
                    this.releases.Add(new ReleaseResult(releaseNode, xmlns));
                }
            }
            catch
            {
            }
        }

        public string Id { get; protected set; }
        public string Title { get; protected set; }
        public string Artist { get; protected set; }
        public TimeSpan Duration { get; protected set; }

        public IEnumerable<ReleaseResult> Releases => releases;

        public override string ToString() => Artist + " - " + Title;
    }
}