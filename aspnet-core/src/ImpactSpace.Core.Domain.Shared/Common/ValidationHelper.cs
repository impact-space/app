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
    
    public static bool IsValidPhoneNumber(string phoneNumber, PhoneCountryCode countryCode)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        try
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, countryCode.ToString());

            return phoneNumberUtil.IsValidNumber(parsedPhoneNumber);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
}