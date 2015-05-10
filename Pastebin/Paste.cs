using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace Eka.Web.Pastebin
{
    public class Paste
    {
        public Paste(string id)
        {
            Id = id;

            Uri = "http://pastebin.com/" + Id;
            RawUri = "http://pastebin.com/raw.php?i=" + Id;

            // Get raw data
            try
            {
                Data = new WebClient().DownloadString(RawUri);
            }
            catch (WebException)
            {
                return;
            }
            Exists = true;

            // Get metadata
            var document = new XmlDocument();
            document.XmlResolver = null;
            try
            {
                document.Load(Uri);
            }
            catch (WebException)
            {
            }
            catch (XmlException)
            {
            }

            var metadataContainerNode = document.SelectSingleNode("//div[contains(@class,'paste_box_line2')]");
            if (metadataContainerNode == null)
            {
                return;
            }
            var metadata = metadataContainerNode.InnerText;
            var dateNode = metadataContainerNode.SelectSingleNode("span/@title");

            var match =
                new Regex(
                    "By: +(.*?) +on +.*? +\\| +syntax: +(.*?) +\\| +size: +(.*?) +\\| +hits: +([0-9,]+) +\\| +expires: +(.*?)",
                    RegexOptions.IgnoreCase).Match(metadata);

            if (dateNode != null)
            {
                Date = DateTime.Parse(dateNode.InnerText);
            }

            if (match.Success)
            {
                User = match.Groups[1].ToString();
                Language = match.Groups[2].ToString();
            }
        }

        public string Id { get; protected set; }
        public bool Exists { get; protected set; }
        public string User { get; protected set; }
        public DateTime Date { get; protected set; }
        public string Language { get; protected set; }
        public string Data { get; protected set; }
        public string Uri { get; protected set; }
        public string RawUri { get; protected set; }
    }
}