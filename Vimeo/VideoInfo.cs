using System;
using Eka.Web.Xml;

namespace Eka.Web.Vimeo
{
    public class VideoInfo
    {
        public VideoInfo(ulong videoId)
        {
            VideoId = videoId;
            InfoUri = "https://vimeo.com/api/v2/video/" + VideoId + ".xml";

            var videoMetadata = XmlLoader.Load(InfoUri);

            Title = videoMetadata.SelectSingleNode("videos/video/title").InnerText;
            Duration = new TimeSpan(0, 0, int.Parse(videoMetadata.SelectSingleNode("videos/video/duration").InnerText));

            Description = videoMetadata.SelectSingleNode("videos/video/description").InnerText;

            UserId = ulong.Parse(videoMetadata.SelectSingleNode("videos/video/user_id").InnerText);
            UserName = videoMetadata.SelectSingleNode("videos/video/user_name").InnerText;

            Width = uint.Parse(videoMetadata.SelectSingleNode("videos/video/width").InnerText);
            Height = uint.Parse(videoMetadata.SelectSingleNode("videos/video/height").InnerText);
        }

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
    }
}