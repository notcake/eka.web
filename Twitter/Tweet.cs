using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Eka.Web.Xml;

namespace Eka.Web.Twitter
{
    public class Tweet
    {
        public ulong Id { get; protected set; }
        public string ApiUri { get; protected set; }

        public string User { get; protected set; }
        public string Text { get; protected set; }
        public DateTime Date { get; protected set; }

        public bool Valid { get; protected set; }

        public Tweet(ulong id)
        {
            this.Id = id;

            this.ApiUri = "https://api.twitter.com/1/statuses/oembed.xml?id=" + this.Id.ToString();

            XmlDocument document = null;

            try
            {
                document = XmlLoader.Load(this.ApiUri);
            }
            catch (WebException)
            {
                return;
            }

            XmlNode htmlNode = document.SelectSingleNode("oembed/html");
            if (htmlNode == null) { return; }

            string html = htmlNode.InnerText;
            html = html.Replace("<script async", "<script ");

            XmlDocument messageDocument = XmlLoader.LoadHtmlFragment(html);

            XmlNamespaceManager xmlns = new XmlNamespaceManager(messageDocument.NameTable);
            xmlns.AddNamespace("_", messageDocument.DocumentElement.NamespaceURI);

            XmlNode blockquoteNode = messageDocument.SelectSingleNode("_:html/_:blockquote", xmlns);
            XmlNode messageNode = messageDocument.SelectSingleNode("_:html/_:blockquote/_:p", xmlns);
            XmlNode dateNode = messageDocument.SelectSingleNode("_:html/_:blockquote/_:a", xmlns);

            if (messageNode == null) { return; }
            if (dateNode == null) { return; }

            this.Text = messageNode.InnerText;
            this.Date = DateTime.Parse(dateNode.InnerText);

            Match match = new Regex("\\(@([a-zA-Z0-9_]+)\\)\\s*([a-zA-Z0-9, ]+)\\s*$", RegexOptions.IgnoreCase).Match(blockquoteNode.InnerText);

            if (!match.Success) { return; }

            this.Valid = true;

            this.User = match.Groups[1].ToString();
        }
    }
}
