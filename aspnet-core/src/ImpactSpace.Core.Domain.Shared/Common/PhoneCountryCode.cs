using System;
using System.ComponentModel;

namespace ImpactSpace.Core.Common
{
    public enum PhoneCountryCode
    {
        [Description("Australia")] Australia = 61,

        [Description("Brazil")] Brazil = 55,

        [Description("China")] China = 86,

        [Description("Denmark")] Denmark = 45,

        [Description("Finland")] Finland = 358,

        [Description("France")] France = 33,

        [Description("Germany")] Germany = 49,

        [Description("India")] India = 91,

        [Description("Ireland")] Ireland = 353,

        [Description("Italy")] Italy = 39,

        [Description("Japan")] Japan = 81,

        [Description("Mexico")] Mexico = 52,

        [Description("Netherlands")] Netherlands = 31,

        [Description("New Zealand")] NewZealand = 64,

        [Description("Nigeria")] Nigeria = 234,

        [Description("Norway")] Norway = 47,

        [Description("Pakistan")] Pakistan = 92,

        [Description("Philippines")] Philippines = 63,

        [Description("Portugal")] Portugal = 351,

        [Description("Russia")] Russia = 7,

        [Description("South Africa")] SouthAfrica = 27,

        [Description("South Korea")] SouthKorea = 82,

        [Description("Spain")] Spain = 34,

        [Description("Sweden")] Sweden = 46,

        [Description("Switzerland")] Switzerland = 41,

        [Description("Turkey")] Turkey = 90,

        [Description("United Kingdom")] UnitedKingdom = 44,

        [Description("United States")] UnitedStates = 1,

        [Description("Canada")] Canada = 1321,

        [Description("Bahamas")] Bahamas = 1224, // Bahamas shares the same country code as US and Canada
    }
}