using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Eka.Web.Thesaurus
{
    public class Thesaurus
    {
        public List<string> Synonyms = new List<string>();

        public Thesaurus(string word)
        {
            Success = false;
            string Data;

            Uri = "https://words.bighugelabs.com/" + word;

            // Get raw data
            try
            {
                Data = new WebClient().DownloadString(Uri);
            }
            catch (WebException)
            {
                return;
            }

            var match =
                new Regex("<h3>noun</h3>.+?(<li><a href=\".+?\">.+?</a></li>)+?</ul>", RegexOptions.IgnoreCase).Match(
                    Data);

            if (match.Success)
            {
                var matches = Regex.Matches(match.Groups[0].ToString(), "<li><a href=\".+?\">(.+?)</a></li>");

                foreach (Match m in matches)
                {
                    Synonyms.Add(m.Groups[1].ToString());
                }

                Success = true;
            }

            match =
                new Regex("<h3>verb</h3>.+?(<li><a href=\".+?\">.+?</a></li>)+?</ul>", RegexOptions.IgnoreCase).Match(
                    Data);

            if (match.Success)
            {
                var matches = Regex.Matches(match.Groups[0].ToString(), "<li><a href=\".+?\">(.+?)</a></li>");

                foreach (Match m in matches)
                {
                    Synonyms.Add(m.Groups[1].ToString());
                }

                Success = true;
            }

            match =
                new Regex("<h3>adjective</h3>.+?(<li><a href=\".+?\">.+?</a></li>)+?</ul>", RegexOptions.IgnoreCase)
                    .Match(Data);

            if (match.Success)
            {
                var matches = Regex.Matches(match.Groups[0].ToString(), "<li><a href=\".+?\">(.+?)</a></li>");

                foreach (Match m in matches)
                {
                    Synonyms.Add(m.Groups[1].ToString());
                }

                Success = true;
            }
        }

        public bool Success { get; protected set; }
        public string Uri { get; protected set; }
    }
}