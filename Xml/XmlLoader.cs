using System.Xml;

namespace Eka.Web.Xml
{
    internal static class XmlLoader
    {
        public static XmlDocument Load(string filename)
        {
            var document = new XmlDocument();
            document.XmlResolver = CachingXmlResolver.Default;
            document.Load(filename);

            return document;
        }

        public static XmlDocument LoadHtmlFragment(string html) => LoadXml(
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\" >\r\n" +
                "<html>" +
                html +
                "</html>"
                );

        public static XmlDocument LoadXml(string xml)
        {
            var document = new XmlDocument();
            document.XmlResolver = CachingXmlResolver.Default;
            document.LoadXml(xml);

            return document;
        }
    }
}