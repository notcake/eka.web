using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Eka.Web.Xml;

namespace Eka.Web.YouTube
{
    public class VideoInfo
    {
        public string VideoId { get; protected set; }
        public string InfoUri { get; protected set; }

        public string Title { get; protected set; }
        public TimeSpan Duration { get; protected set; }

        public string Description { get; protected set; }

        public CountryRestriction CountryRestriction { get; protected set; }

        public VideoInfo(string videoId)
        {
            this.VideoId = videoId;
            this.InfoUri = "https://gdata.youtube.com/feeds/api/videos/" + this.VideoId + "?v=2";
            this.CountryRestriction = CountryRestriction.Default;

            XmlDocument videoMetadata = XmlLoader.Load(this.InfoUri);

            XmlNamespaceManager xmlns = new XmlNamespaceManager(videoMetadata.NameTable);
            xmlns.AddNamespace("_", videoMetadata.DocumentElement.NamespaceURI);
            xmlns.AddNamespace("yt", videoMetadata.DocumentElement.GetAttribute("xmlns:yt"));
            xmlns.AddNamespace("media", videoMetadata.DocumentElement.GetAttribute("xmlns:media"));

            this.Title = videoMetadata.SelectSingleNode("_:entry/_:title", xmlns).InnerText;
            this.Duration = new TimeSpan(0, 0, int.Parse(videoMetadata.SelectSingleNode("_:entry/media:group/yt:duration/@seconds", xmlns).InnerText));

            this.Description = videoMetadata.SelectSingleNode("_:entry/media:group/media:description", xmlns).InnerText;

            XmlNode countryRestrictionNode = videoMetadata.SelectSingleNode("_:entry/media:group/media:restriction[@type='country']", xmlns);
            if (countryRestrictionNode != null)
            {
                XmlNode relationshipAttribute = countryRestrictionNode.Attributes.GetNamedItem("relationship");
                Relationship relationship = Relationship.Allow;
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

                this.CountryRestriction = new CountryRestriction(relationship, countryRestrictionNode.InnerText.Split(' '));
            }
        }
    }
}
