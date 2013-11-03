using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Eka.Web.Pastebin
{
    public class Paste
    {
        public string Id { get; protected set; }
        public bool Exists { get; protected set; }

        public string User { get; protected set; }
        public DateTime Date { get; protected set; }
        public string Language { get; protected set; }

        public string Data { get; protected set; }

        public string Uri { get; protected set; }
        public string RawUri { get; protected set; }

        public Paste(string id)
        {
            this.Id = id;

            this.Uri = "http://pastebin.com/" + this.Id;
            this.RawUri = "http://pastebin.com/raw.php?i=" + this.Id;

            // Get raw data
            try
            {
                this.Data = new WebClient().DownloadString(this.RawUri);
            }
            catch (WebException)
            {
                return;
            }
            this.Exists = true;

            // Get metadata
            XmlDocument document = new XmlDocument();
            document.XmlResolver = null;
            try
            {
                document.Load(this.Uri);
            }
            catch (WebException) { }
            catch (XmlException) { }

            XmlNode metadataContainerNode = document.SelectSingleNode("//div[contains(@class,'paste_box_line2')]");
            if (metadataContainerNode == null) { return; }
            string metadata = metadataContainerNode.InnerText;
            XmlNode dateNode = metadataContainerNode.SelectSingleNode("span/@title");

            Match match = new Regex("By: +(.*?) +on +.*? +\\| +syntax: +(.*?) +\\| +size: +(.*?) +\\| +hits: +([0-9,]+) +\\| +expires: +(.*?)", RegexOptions.IgnoreCase).Match(metadata);

            if (dateNode != null)
            {
                this.Date = DateTime.Parse(dateNode.InnerText);
            }

            if (match.Success)
            {
                this.User = match.Groups[1].ToString();
                this.Language = match.Groups[2].ToString();
            }
        }
    }
}
