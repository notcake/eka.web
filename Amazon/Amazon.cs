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

        public string Title { get; protected set; }
        public string Price { get; protected set; }
        public string Currency { get; protected set; }

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
            }
            else
            {
                return;
            }

            //match = new Regex("<td><b class=\"priceLarge\">(.+)</b>", RegexOptions.IgnoreCase).Match(this.Data);
            match = new Regex("\"buyingPrice\":([0-9.]+)", RegexOptions.IgnoreCase).Match(this.Data);
            if (match.Success)
            {
                this.Price = match.Groups[1].ToString();
                //this.Price = Double.Parse(match.Groups[1].ToString(), );
            }
            else
            {
                return;
            }

            match = new Regex("\"currencyCode\":\"([a-zA-Z]+)\",", RegexOptions.IgnoreCase).Match(this.Data);
            if (match.Success)
            {
                this.Currency = match.Groups[1].ToString();
                this.Success = true;
            }
        }
    }
}
