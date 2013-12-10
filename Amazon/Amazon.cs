using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Eka.Web.Amazon
{
    public class Amazon
    {
        public bool Success { get; protected set; }

        public string Data { get; protected set; }

        public string Uri { get; protected set; }
        public string ListingUri { get; protected set; }

        public string Tld { get; protected set; }
        public string Id { get; protected set; }

        public string Title { get; protected set; }
        public string Price { get; protected set; }

        public Amazon(string uri)
        {
            this.Success = false;
            this.Uri = uri;

            // Get raw data
            try
            {
                this.Data = new WebClient().DownloadString(this.Uri);
            }
            catch (WebException)
            {
                return;
            }

            //Match match = new Regex("=\"btAsinTitle\">[^>]+>(.+)</span>", RegexOptions.IgnoreCase).Match(this.Data);
            Match match = new Regex("\"title\":\"([^\"]+)\",", RegexOptions.IgnoreCase).Match(this.Data);

            if (match.Success)
            {
                this.Title = match.Groups[1].ToString();
                this.Success = true;
            }
            else
            {
                return;
            }

            // Disassemble URI

            match = new Regex("https?://(www\\.)?amazon\\.(com|de|co\\.uk)([^ ]+)?/[dg]p/(product/)?([a-zA-Z0-9]+)", RegexOptions.IgnoreCase).Match(this.Data);
            if (match.Success)
            {
                this.Tld = match.Groups[2].ToString();
                this.Id = match.Groups[5].ToString();
            }
            else
            {
                return;
            }

            // Assemble listing uri / Download listings

            this.ListingUri = "http://www.amazon." + this.Tld + "/gp/offer-listing/" + this.Id;

            try
            {
                WebClient web = new WebClient();
                web.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0");
                this.Data = web.DownloadString(this.ListingUri);
            }
            catch (WebException e)
            {
                this.Price = e.Message;
                return;
            }

            match = new Regex("<li id=\"olpTabNew\".+?href.+?>(.+?)</a></li>", RegexOptions.IgnoreCase).Match(this.Data);

            if (match.Success)
            {
                this.Price = match.Groups[1].ToString();
                this.Price = Regex.Replace(this.Price, "<.*?>", string.Empty);
                //this.Price = Double.Parse(match.Groups[1].ToString(), );
            }
            else
            {
                return;
            }
        }
    }
}
