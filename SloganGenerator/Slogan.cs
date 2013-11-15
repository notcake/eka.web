using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Eka.Web.SloganGenerator
{
    public class Slogan
    {
        public string Id { get; protected set; }
        public bool Success { get; protected set; }

        public string SloganText { get; protected set; }

        public string Data { get; protected set; }

        public string Uri { get; protected set; }

        public Slogan(string id)
        {
            this.Success = false;
            this.Id = id;

            this.Uri = "http://thesurrealist.co.uk/slogan.cgi?word=" + this.Id;

            // Get raw data
            try
            {
                this.Data = new WebClient().DownloadString(this.Uri);
            }
            catch (WebException)
            {
                return;
            }

            Match match = new Regex("class=\"h1a\">(.+)</a></h1>", RegexOptions.IgnoreCase).Match(this.Data);

            if (match.Success)
            {
                this.SloganText = match.Groups[1].ToString();
                this.SloganText = Regex.Replace(this.SloganText, "<.*?>", string.Empty);
                this.SloganText = this.SloganText.Replace(" -- ", string.Empty);
                this.Success = true;
            }
        }
    }
}
