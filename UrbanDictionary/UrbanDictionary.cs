using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace Eka.Web.UrbanDictionary
{
/*** UrbanDictionary Example Response
    {
        "tags":[
            "vocaloid",
            "hatsune",
            "nyappy",
            "antic cafe",
            "ancafe",
            "an cafe",
            "antique cafe",
            "happy",
            "kanon",
            "bou"
        ],
        "result_type":"exact",
        "list":[
            {
                "defid":2986078,
                "word":"Miku",
                "author":"ifwbekfwe",
                "permalink":"http://miku.urbanup.com/2986078",
                "definition":"A miku is a goddess of a person: beautiful, intelligent, fun. The kind of person everyone wishes they could be.",
                "example":"My new girlfriend is awesome, she's such a miku.",
                "thumbs_up":220,
                "thumbs_down":98,
                "current_vote":""
            }
        ],
        "sounds":[]
    }
    */

    internal class UrbanDictionaryResponse
    {
        public IDictionary<string, string>[] list;
        public string result_type;
        public string[] tags;
    }


    public class UrbanDictionary
    {
        public UrbanDictionary(string id)
        {
            Success = false;
            Id = id;

            Uri = "http://api.urbandictionary.com/v0/define?term=" + Id;

            // Get raw data
            try
            {
                Data = new WebClient().DownloadString(Uri);
            }
            catch (WebException)
            {
                return;
            }

            var response = JsonConvert.DeserializeObject<UrbanDictionaryResponse>(Data);

            if (!response.list.Any()) return;
            Definition = response.list[0]["definition"];
            Example = response.list[0]["example"];
            Success = true;
        }

        public string Id { get; protected set; }
        public bool Success { get; protected set; }
        public string Definition { get; protected set; }
        public string Example { get; protected set; }
        public string Data { get; protected set; }
        public string Uri { get; protected set; }
    }
}