using System;

namespace Eka.Web.MusicBrainz
{
    public class RecordingSearch
    {
        public RecordingSearch()
        {
            SearchString = "";
        }

        public RecordingSearch(string searchString)
        {
            SearchString = searchString;
        }

        public string Artist { get; set; }
        public string SearchString { get; set; }

        public RecordingSearchResults GetResults()
        {
            var searchUri = "http://www.musicbrainz.org/ws/2/recording?query=";
            if (Artist != null)
            {
                searchUri += "artist:\"" + Uri.EscapeDataString(Artist.Replace("\"", "")) + "\" AND ";
            }
            searchUri += "\"" + Uri.EscapeDataString(SearchString.Replace("\"", "")) + "\"";
            searchUri += "&limit=100";

            return new RecordingSearchResults(this, searchUri);
        }
    }
}