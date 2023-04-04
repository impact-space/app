using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace ImpactSpace.Core.Common;

public class PhoneNumber : ValueObject
{
    public PhoneCountryCode CountryCode { get; private set;}
    public string NationalNumber { get; private set; }

    private PhoneNumber()
    {
        
    }

    public PhoneNumber(PhoneCountryCode countryCode, string number)
    {
        SetCountryCode(countryCode);
        SetNationalNumber(number);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return CountryCode;
        yield return NationalNumber;
    }

    private void SetCountryCode(PhoneCountryCode countryCode)
    {
        CountryCode = countryCode;
    }

    private void SetNationalNumber(string number)
    {
        if (!ValidationHelper.IsValidPhoneNumber(number, CountryCode))
        {
            throw new ArgumentException("The provided phone number is not valid.", nameof(number));
        }

        NationalNumber = FormatHelper.FormatNationalPhoneNumber(number, CountryCode);
    }
    
    public string ToE164Format()
    {
        return $"+{(int)CountryCode}{NationalNumber}";
    }
}