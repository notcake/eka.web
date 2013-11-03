using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eka.Web.MusicBrainz
{
    public class RecordingSearch
    {
        public string Artist { get; set; }
        public string SearchString { get; set; }

        public RecordingSearch()
        {
            this.SearchString = "";
        }

        public RecordingSearch(string searchString)
        {
            this.SearchString = searchString;
        }

        public RecordingSearchResults GetResults()
        {
            string searchUri = "http://www.musicbrainz.org/ws/2/recording?query=";
            if (this.Artist != null)
            {
                searchUri += "artist:\"" + Uri.EscapeDataString(this.Artist.Replace("\"", "")) + "\" AND ";
            }
            searchUri += "\"" + Uri.EscapeDataString(this.SearchString.Replace("\"", "")) + "\"";
            searchUri += "&limit=100";

            return new RecordingSearchResults(this, searchUri);
        }
    }
}
