using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Eka.Web.Thesaurus
{
    public class Thesaurus
    {
        public bool Success { get; protected set; }

        public List<string> Synonyms = new List<string>();

        public string Uri { get; protected set; }

        public Thesaurus(string word)
        {
            this.Success = false;
            string Data;

            this.Uri = "https://words.bighugelabs.com/" + word;

            // Get raw data
            try
            {
                Data = new WebClient().DownloadString(this.Uri);
            }
            catch (WebException)
            {
                return;
            }

            Match match = new Regex("<h3>noun</h3>.+?(<li><a href=\".+?\">.+?</a></li>)+?</ul>", RegexOptions.IgnoreCase).Match(Data);

            if (match.Success)
            {
                MatchCollection matches = Regex.Matches(match.Groups[0].ToString(), "<li><a href=\".+?\">(.+?)</a></li>");

                foreach (Match m in matches)
                {
                    Synonyms.Add(m.Groups[1].ToString());
                }

                this.Success = true;
            }

            match = new Regex("<h3>verb</h3>.+?(<li><a href=\".+?\">.+?</a></li>)+?</ul>", RegexOptions.IgnoreCase).Match(Data);

            if (match.Success)
            {
                MatchCollection matches = Regex.Matches(match.Groups[0].ToString(), "<li><a href=\".+?\">(.+?)</a></li>");

                foreach (Match m in matches)
                {
                    Synonyms.Add(m.Groups[1].ToString());
                }

                this.Success = true;
            }
            
            match = new Regex("<h3>adjective</h3>.+?(<li><a href=\".+?\">.+?</a></li>)+?</ul>", RegexOptions.IgnoreCase).Match(Data);

            if (match.Success)
            {
                MatchCollection matches = Regex.Matches(match.Groups[0].ToString(), "<li><a href=\".+?\">(.+?)</a></li>");

                foreach (Match m in matches)
                {
                    Synonyms.Add(m.Groups[1].ToString());
                }

                this.Success = true;
            }
        }
    }
}
