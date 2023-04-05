using PhoneNumbers;

namespace ImpactSpace.Core.Common;

public static class FormatHelper
{
    public static string FormatPhoneNumber(string phoneNumber, PhoneCountryCode countryCode)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, GetRegionCode(countryCode));

        return phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.E164);
    }

    public static string FormatNationalPhoneNumber(string phoneNumber, PhoneCountryCode countryCode)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, GetRegionCode(countryCode));

        return phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.NATIONAL);
    }

    public static string FormatInternationalPhoneNumber(string phoneNumber, PhoneCountryCode countryCode)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, GetRegionCode(countryCode));

        return phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.INTERNATIONAL);
    }
    
    private static string GetRegionCode(PhoneCountryCode countryCode)
    {
        return countryCode switch
        {
            PhoneCountryCode.Canada => "CA",
            PhoneCountryCode.Bahamas => "BS",
            _ => PhoneNumberUtil.GetInstance().GetRegionCodeForCountryCode((int)countryCode)
        };
    }
}