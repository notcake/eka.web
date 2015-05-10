using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Eka.Web.MusicBrainz
{
    public class RecordingSearchResults : IEnumerable<RecordingSearchResult>
    {
        private readonly List<RecordingSearchResult> Results = new List<RecordingSearchResult>();

        internal RecordingSearchResults(RecordingSearch search, string queryUri)
        {
            Search = search;
            QueryUri = queryUri;

            var searchResults = new XmlDocument();
            searchResults.Load(QueryUri);

            var xmlns = new XmlNamespaceManager(searchResults.NameTable);
            xmlns.AddNamespace("_", searchResults.DocumentElement.NamespaceURI);
            xmlns.AddNamespace("ext", searchResults.DocumentElement.GetAttribute("xmlns:ext"));

            foreach (XmlNode result in searchResults.SelectNodes("//_:recording", xmlns))
            {
                Results.Add(new RecordingSearchResult(result, xmlns));
            }
        }

        public RecordingSearch Search { get; protected set; }
        public string QueryUri { get; protected set; }

        public IEnumerator<RecordingSearchResult> GetEnumerator() => Results.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Results.GetEnumerator();
    }
}