using System.Net;
using System.Text.RegularExpressions;

namespace Eka.Web.SloganGenerator
{
    public class Slogan
    {
        public Slogan(string id)
        {
            Success = false;
            Id = id;

            Uri = "http://thesurrealist.co.uk/slogan.cgi?word=" + Id;

            // Get raw data
            try
            {
                Data = new WebClient().DownloadString(Uri);
            }
            catch (WebException)
            {
                return;
            }

            var match = new Regex("class=\"h1a\">(.+)</a></h1>", RegexOptions.IgnoreCase).Match(Data);

            if (match.Success)
            {
                SloganText = match.Groups[1].ToString();
                SloganText = Regex.Replace(SloganText, "<.*?>", string.Empty);
                SloganText = SloganText.Replace(" -- ", string.Empty);
                Success = true;
            }
        }

        public string Id { get; protected set; }
        public bool Success { get; protected set; }
        public string SloganText { get; protected set; }
        public string Data { get; protected set; }
        public string Uri { get; protected set; }
    }
}