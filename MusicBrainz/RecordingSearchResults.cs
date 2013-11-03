using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Eka.Web.MusicBrainz
{
    public class RecordingSearchResults : IEnumerable<RecordingSearchResult>
    {
        public RecordingSearch Search { get; protected set; }
        public string QueryUri { get; protected set; }

        private List<RecordingSearchResult> Results = new List<RecordingSearchResult>();

        internal RecordingSearchResults(RecordingSearch search, string queryUri)
        {
            this.Search = search;
            this.QueryUri = queryUri;

            XmlDocument searchResults = new XmlDocument();
            searchResults.Load(this.QueryUri);

            XmlNamespaceManager xmlns = new XmlNamespaceManager(searchResults.NameTable);
            xmlns.AddNamespace("_", searchResults.DocumentElement.NamespaceURI);
            xmlns.AddNamespace("ext", searchResults.DocumentElement.GetAttribute("xmlns:ext"));

            foreach (XmlNode result in searchResults.SelectNodes("//_:recording", xmlns))
            {
                this.Results.Add(new RecordingSearchResult(result, xmlns));
            }
        }

        public IEnumerator<RecordingSearchResult> GetEnumerator()
        {
            return this.Results.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Results.GetEnumerator();
        }
    }
}
