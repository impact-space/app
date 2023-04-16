using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Common;

public class PhoneNumberDto
{
    public PhoneCountryCode CountryCode { get; set; }
    
    [StringLength(CommonConstants.MaxPhoneLength)]
    [Phone]
    public string NationalNumber { get; set; }
}