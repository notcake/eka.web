using System.Net;
using Newtonsoft.Json;

namespace Eka.Web.Json
{
    internal static class JsonLoader
    {
        public static object Load(string uri)
        {
            var magic = JsonConvert.DeserializeObject<dynamic>(new WebClient().DownloadString(uri));

            return magic;
        }
    }
}