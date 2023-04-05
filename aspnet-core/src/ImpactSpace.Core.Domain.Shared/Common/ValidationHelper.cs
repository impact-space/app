using System;
using System.Net.Mail;
using PhoneNumbers;

namespace ImpactSpace.Core.Common;

public static class ValidationHelper
{
    public static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var mailAddress = new MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPhoneNumber(string phoneNumber, string regionCode)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        try
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, regionCode);
            return phoneNumberUtil.IsValidNumber(parsedPhoneNumber);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(string phoneNumber, PhoneCountryCode countryCode)
    {
        return countryCode switch
        {
            PhoneCountryCode.Canada => IsValidPhoneNumber(phoneNumber, "CA"),
            PhoneCountryCode.Bahamas => IsValidPhoneNumber(phoneNumber, "BS"),
            _ => IsValidPhoneNumber(phoneNumber,
                PhoneNumberUtil.GetInstance().GetRegionCodeForCountryCode((int)countryCode))
        };
    }
    

}