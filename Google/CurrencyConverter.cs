using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Eka.Web.Google
{
    public class CurrencyConverter
    {
        private static Regex NumericRegex = new Regex("\",rhs: \"([0-9\\s]+(\\.[0-9]*)?)");

        public static double Convert(double amount, string sourceCurrency, string destinationCurrency)
        {
            string uri = "https://www.google.com/ig/calculator?hl=en&q=" + amount.ToString() + sourceCurrency + "=?" + destinationCurrency;

            string json = new WebClient().DownloadString(uri);

            Match match = CurrencyConverter.NumericRegex.Match(json);

            return double.Parse(match.Groups[1].ToString().Replace("\u00A0", ""));
        }
    }
}
