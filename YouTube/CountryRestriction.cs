using System.Collections.Generic;

namespace Eka.Web.YouTube
{
    public class CountryRestriction : PlaybackRestriction
    {
        public static CountryRestriction Default = new CountryRestriction(Relationship.Deny, new string[] {});
        public HashSet<string> countryCodes = new HashSet<string>();

        public CountryRestriction(Relationship relationship, IEnumerable<string> countryCodes)
            : base(relationship)
        {
            Type = "country";
            foreach (var countryCode in countryCodes)
            {
                this.countryCodes.Add(countryCode);
            }
        }

        public IEnumerable<string> CountryCodes => countryCodes;

        public bool IsCountryAllowed(string countryCode)
        {
            var containsCountry = countryCodes.Contains(countryCode);
            if (Relationship == Relationship.Deny)
            {
                containsCountry = !containsCountry;
            }
            return containsCountry;
        }
    }
}