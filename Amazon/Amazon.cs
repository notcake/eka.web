using System.Net;
using System.Text.RegularExpressions;

namespace Eka.Web.Amazon
{
    public class Amazon
    {
        public Amazon(string uri)
        {
            Success = false;
            Uri = uri;

            // Get raw data
            try
            {
                Data = new WebClient().DownloadString(Uri);
            }
            catch (WebException)
            {
                return;
            }

            //Match match = new Regex("=\"btAsinTitle\">[^>]+>(.+)</span>", RegexOptions.IgnoreCase).Match(this.Data);
            //Match match = new Regex("\"title\":\"([^\"]+)\",", RegexOptions.IgnoreCase).Match(this.Data);
            var match =
                new Regex("<.*?meta.*?name=\"title\".*?content=\"([^\"]+)\"", RegexOptions.IgnoreCase).Match(Data);

            if (match.Success)
            {
                Title = WebUtility.HtmlDecode(match.Groups[1].ToString());
                Title = Regex.Replace(Title, "Amazon(\\.com|\\.de)?[ :-]+", "");
                Success = true;
            }
            else
            {
                return;
            }

            // Disassemble URI

            match =
                new Regex("https?://(www\\.)?amazon\\.(com|de|co\\.uk)([^ ]+)?/[dg]p/(product/)?([a-zA-Z0-9]+)",
                    RegexOptions.IgnoreCase).Match(Data);
            if (match.Success)
            {
                Tld = match.Groups[2].ToString();
                Id = match.Groups[5].ToString();
            }
            else
            {
                return;
            }

            // Assemble listing uri / Download listings

            ListingUri = "http://www.amazon." + Tld + "/gp/offer-listing/" + Id;

            try
            {
                var web = new WebClient();
                web.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:30.0) Gecko/20100101 Firefox/30.0");
                Data = web.DownloadString(ListingUri);
            }
            catch (WebException e)
            {
                Price = e.Message;
                return;
            }

            match = new Regex("<li id=\"olpTabNew\".+?href.+?>(.+?)</a></li>", RegexOptions.IgnoreCase).Match(Data);

            if (match.Success)
            {
                Price = match.Groups[1].ToString();
                Price = Regex.Replace(Price, "<.*?>", string.Empty);
                //this.Price = Double.Parse(match.Groups[1].ToString(), );
            }
            else
            {
            }
        }

        public bool Success { get; protected set; }
        public string Data { get; protected set; }
        public string Uri { get; protected set; }
        public string ListingUri { get; protected set; }
        public string Tld { get; protected set; }
        public string Id { get; protected set; }
        public string Title { get; protected set; }
        public string Price { get; protected set; }
    }
}