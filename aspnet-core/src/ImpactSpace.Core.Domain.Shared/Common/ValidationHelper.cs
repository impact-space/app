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

            PhoneNumber parsedPhoneNumber;

            if (countryCode == PhoneCountryCode.Canada)
            {
                // Use the area code for Alberta, Canada
                parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, "CA");
            }
            else if (countryCode == PhoneCountryCode.Bahamas)
            {
                // Use the default region code for the United States (US) and Canada (CA) and the Bahamas (BS)
                parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, "BS");
            }
            else
            {
                // Use the region code corresponding to the given PhoneCountryCode enum value
                parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, 
                    phoneNumberUtil.GetRegionCodeForCountryCode((int)countryCode));
            }

            return phoneNumberUtil.IsValidNumber(parsedPhoneNumber);
        }
        catch (NumberParseException ex)
        {
            return false;
        }
    }
}