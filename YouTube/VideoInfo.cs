using System;
using System.Xml;
using Eka.Web.Xml;

namespace Eka.Web.YouTube
{
    public class VideoInfo
    {
        public VideoInfo(string videoId)
        {
            VideoId = videoId;
            InfoUri = "https://gdata.youtube.com/feeds/api/videos/" + VideoId + "?v=2";
            CountryRestriction = CountryRestriction.Default;

            var videoMetadata = XmlLoader.Load(InfoUri);

            var xmlns = new XmlNamespaceManager(videoMetadata.NameTable);
            xmlns.AddNamespace("_", videoMetadata.DocumentElement.NamespaceURI);
            xmlns.AddNamespace("yt", videoMetadata.DocumentElement.GetAttribute("xmlns:yt"));
            xmlns.AddNamespace("media", videoMetadata.DocumentElement.GetAttribute("xmlns:media"));

            Title = videoMetadata.SelectSingleNode("_:entry/_:title", xmlns).InnerText;
            Duration = new TimeSpan(0, 0,
                int.Parse(videoMetadata.SelectSingleNode("_:entry/media:group/yt:duration/@seconds", xmlns).InnerText));

            Description = videoMetadata.SelectSingleNode("_:entry/media:group/media:description", xmlns).InnerText;

            var countryRestrictionNode =
                videoMetadata.SelectSingleNode("_:entry/media:group/media:restriction[@type='country']", xmlns);
            if (countryRestrictionNode != null)
            {
                var relationshipAttribute = countryRestrictionNode.Attributes.GetNamedItem("relationship");
                var relationship = Relationship.Allow;
                if (relationshipAttribute != null)
                {
                    if (relationshipAttribute.InnerText == "allow")
                    {
                        relationship = Relationship.Allow;
                    }
                    else if (relationshipAttribute.InnerText == "deny")
                    {
                        relationship = Relationship.Deny;
                    }
                }

                CountryRestriction = new CountryRestriction(relationship, countryRestrictionNode.InnerText.Split(' '));
            }
        }

        public string VideoId { get; protected set; }
        public string InfoUri { get; protected set; }
        public string Title { get; protected set; }
        public TimeSpan Duration { get; protected set; }
        public string Description { get; protected set; }
        public CountryRestriction CountryRestriction { get; protected set; }
    }
}