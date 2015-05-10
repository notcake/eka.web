using System;
using System.Net;
using System.Text;
using System.Xml;

namespace Eka.Web.Wolfram
{
    public class Wolfram
    {
        public Wolfram(string Query)
        {
            Success = false;
            this.Query = System.Uri.EscapeDataString(Query);

            Uri = "http://api.wolframalpha.com/v2/query?input=" + this.Query + "&appid=TY5Q8Y-UVE96HQA92";

            try
            {
                Data = new WebClient().DownloadString(Uri);
            }
            catch (WebException)
            {
                return;
            }
            Success = true;

            var doc = new XmlDocument();

            try
            {
                doc.LoadXml(Data);
            }
            catch (XmlException)
            {
            }

            var queryresult = doc.SelectSingleNode("//queryresult");
            if (queryresult == null)
            {
                return;
            }

            var success = Convert.ToBoolean(queryresult.Attributes["success"].Value);

            if (success)
            {
                var pods = queryresult.SelectNodes("//pod");
                if (pods.Count < 1 | pods[1] == null)
                {
                    Output = "got nothing, database down?";
                    return;
                }

                var bytes =
                    Encoding.Default.GetBytes(pods[0]["subpod"]["plaintext"].InnerText + "\n\n" +
                                              pods[1]["subpod"]["plaintext"].InnerText.Replace("|", "->"));

                Output = Encoding.UTF8.GetString(bytes);
            }
            else
            {
                var rest = queryresult.SelectNodes("*");
                if (rest[0].Name == "didyoumeans")
                {
                    Output = "Did You mean? " + rest[0].InnerText + "?";
                }
                else if (rest[0].Name == "tips")
                {
                    Output = "Tip: " + rest[0].Attributes["text"].Value;
                }
            }
        }

        public bool Success { get; protected set; }
        public string Uri { get; protected set; }
        public string Query { get; protected set; }
        public string Data { get; protected set; }
        public string Output { get; protected set; }
    }
}