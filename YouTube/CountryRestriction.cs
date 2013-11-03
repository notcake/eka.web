using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eka.Web.YouTube
{
    public class CountryRestriction : PlaybackRestriction
    {
        public HashSet<string> countryCodes = new HashSet<string>();

        public static CountryRestriction Default = new CountryRestriction(Relationship.Deny, new string[] { });

        public CountryRestriction(Relationship relationship, IEnumerable<string> countryCodes)
            : base(relationship)
        {
            this.Type = "country";
            foreach (string countryCode in countryCodes)
            {
                this.countryCodes.Add(countryCode);
            }
        }

        public IEnumerable<string> CountryCodes
        {
            get
            {
                return this.countryCodes;
            }
        }

        public bool IsCountryAllowed(string countryCode)
        {
            bool containsCountry = this.countryCodes.Contains(countryCode);
            if (this.Relationship == Relationship.Deny)
            {
                containsCountry = !containsCountry;
            }
            return containsCountry;
        }
    }
}
