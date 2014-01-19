using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Eka.Web.PlotGenerator
{
    public class Plot
    {
        public bool Success { get; protected set; }

        public string PlotText { get; protected set; }

        public string Uri { get; protected set; }

        public Plot()
        {
            this.Success = false;
            string Data;

            this.Uri = "https://words.bighugelabs.com/plot.php";

            // Get raw data
            try
            {
                Data = new WebClient().DownloadString(this.Uri);
            }
            catch (WebException)
            {
                return;
            }

            Match match = new Regex("<ul class=\"loglines\"><li>(.+?)</li>", RegexOptions.IgnoreCase).Match(Data);

            if (match.Success)
            {
                this.PlotText = match.Groups[1].ToString();
                this.Success = true;
            }
        }
    }
}
