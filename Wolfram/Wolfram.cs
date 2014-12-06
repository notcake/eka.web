using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml;

namespace Eka.Web.Wolfram
{
    public class Wolfram
    {
        public bool Success { get; protected set; }

        public string Uri { get; protected set; }
        public string Query { get; protected set; }
        public string Data { get; protected set; }
        public string Output { get; protected set; }

        public Wolfram(string Query)
        {
            this.Success = false;
            this.Query = System.Uri.EscapeDataString(Query);

            this.Uri = "http://api.wolframalpha.com/v2/query?input=" + this.Query + "&appid=TY5Q8Y-UVE96HQA92";

            try
            {
                this.Data = new WebClient().DownloadString(this.Uri);
            }
            catch (WebException)
            {
                return;
            }
            this.Success = true;

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(this.Data);
            }
            catch (XmlException) { }

            XmlNode queryresult = doc.SelectSingleNode("//queryresult");
            if (queryresult == null) { return; }
            
            bool success = Convert.ToBoolean(queryresult.Attributes["success"].Value);

            if (success)
            {
                XmlNodeList pods = queryresult.SelectNodes("//pod");
                if (pods.Count < 1 | pods[1] == null) { this.Output = "got nothing, database down?"; return; }

                byte[] bytes = Encoding.Default.GetBytes(pods[0]["subpod"]["plaintext"].InnerText +"\n\n"+ pods[1]["subpod"]["plaintext"].InnerText.Replace("|","->"));

                this.Output = Encoding.UTF8.GetString(bytes);

            }
            else
            {
                XmlNodeList rest = queryresult.SelectNodes("*");
                if (rest[0].Name == "didyoumeans")
                {
                    this.Output = "Did You mean? "+rest[0].InnerText+"?";
                }
                else if (rest[0].Name == "tips")
                {
                    this.Output = "Tip: " + rest[0].Attributes["text"].Value;
                }
                            
            }
        }

    }
}
