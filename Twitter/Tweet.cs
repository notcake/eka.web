using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Eka.Web.Xml;

namespace Eka.Web.Twitter
{
    public class Tweet
    {
        public Tweet(ulong id)
        {
            Id = id;

            ApiUri = "https://api.twitter.com/1/statuses/oembed.xml?id=" + Id;

            XmlDocument document = null;

            try
            {
                document = XmlLoader.Load(ApiUri);
            }
            catch (WebException)
            {
                return;
            }

            var htmlNode = document.SelectSingleNode("oembed/html");
            if (htmlNode == null)
            {
                return;
            }

            var html = htmlNode.InnerText;
            html = html.Replace("<script async", "<script ");

            var messageDocument = XmlLoader.LoadHtmlFragment(html);

            var xmlns = new XmlNamespaceManager(messageDocument.NameTable);
            xmlns.AddNamespace("_", messageDocument.DocumentElement.NamespaceURI);

            var blockquoteNode = messageDocument.SelectSingleNode("_:html/_:blockquote", xmlns);
            var messageNode = messageDocument.SelectSingleNode("_:html/_:blockquote/_:p", xmlns);
            var dateNode = messageDocument.SelectSingleNode("_:html/_:blockquote/_:a", xmlns);

            if (messageNode == null)
            {
                return;
            }
            if (dateNode == null)
            {
                return;
            }

            Text = messageNode.InnerText;
            Date = DateTime.Parse(dateNode.InnerText);

            var match =
                new Regex("\\(@([a-zA-Z0-9_]+)\\)\\s*([a-zA-Z0-9, ]+)\\s*$", RegexOptions.IgnoreCase).Match(
                    blockquoteNode.InnerText);

            if (!match.Success)
            {
                return;
            }

            Valid = true;

            User = match.Groups[1].ToString();
        }

        public ulong Id { get; protected set; }
        public string ApiUri { get; protected set; }
        public string User { get; protected set; }
        public string Text { get; protected set; }
        public DateTime Date { get; protected set; }
        public bool Valid { get; protected set; }
    }
}