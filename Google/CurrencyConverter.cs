using System.Net;
using System.Text.RegularExpressions;

namespace Eka.Web.Google
{
    public class CurrencyConverter
    {
        private static readonly Regex NumericRegex = new Regex("\",rhs: \"([0-9\\s]+(\\.[0-9]*)?)");

        public static double Convert(double amount, string sourceCurrency, string destinationCurrency)
        {
            var uri = "https://www.google.com/ig/calculator?hl=en&q=" + amount + sourceCurrency + "=?" +
                      destinationCurrency;

            var json = new WebClient().DownloadString(uri);

            var match = NumericRegex.Match(json);

            return double.Parse(match.Groups[1].ToString().Replace("\u00A0", ""));
        }
    }
}