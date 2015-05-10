using System.Net;
using System.Text.RegularExpressions;

namespace Eka.Web.PlotGenerator
{
    public class Plot
    {
        public Plot()
        {
            Success = false;
            string Data;

            Uri = "https://words.bighugelabs.com/plot.php";

            // Get raw data
            try
            {
                Data = new WebClient().DownloadString(Uri);
            }
            catch (WebException)
            {
                return;
            }

            var match = new Regex("<ul class=\"loglines\"><li>(.+?)</li>", RegexOptions.IgnoreCase).Match(Data);

            if (match.Success)
            {
                PlotText = match.Groups[1].ToString();
                Success = true;
            }
        }

        public bool Success { get; protected set; }
        public string PlotText { get; protected set; }
        public string Uri { get; protected set; }
    }
}