using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Eka.Web.Xml;

namespace Eka.Web.Vimeo
{
    public class VideoInfo
    {
        public ulong VideoId { get; protected set; }
        public bool Exists { get; protected set; }

        public string InfoUri { get; protected set; }

        public string Title { get; protected set; }
        public TimeSpan Duration { get; protected set; }

        public string Description { get; protected set; }

        public ulong UserId { get; protected set; }
        public string UserName { get; protected set; }

        public uint Width { get; protected set; }
        public uint Height { get; protected set; }

        public VideoInfo(ulong videoId)
        {
            this.VideoId = videoId;
            this.InfoUri = "https://vimeo.com/api/v2/video/" + this.VideoId.ToString() + ".xml";

            XmlDocument videoMetadata = XmlLoader.Load(this.InfoUri);

            this.Title = videoMetadata.SelectSingleNode("videos/video/title").InnerText;
            this.Duration = new TimeSpan(0, 0, int.Parse(videoMetadata.SelectSingleNode("videos/video/duration").InnerText));

            this.Description = videoMetadata.SelectSingleNode("videos/video/description").InnerText;

            this.UserId = ulong.Parse(videoMetadata.SelectSingleNode("videos/video/user_id").InnerText);
            this.UserName = videoMetadata.SelectSingleNode("videos/video/user_name").InnerText;

            this.Width = uint.Parse(videoMetadata.SelectSingleNode("videos/video/width").InnerText);
            this.Height = uint.Parse(videoMetadata.SelectSingleNode("videos/video/height").InnerText);
        }
    }
}
