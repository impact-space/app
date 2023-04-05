using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Common;

public class FormatHelperTests
{
    [Theory]
    [InlineData("6505550199", PhoneCountryCode.UnitedStates, "+16505550199")]
    [InlineData("6135550199", PhoneCountryCode.Canada, "+16135550199")]
    [InlineData("2423276284", PhoneCountryCode.Bahamas, "+12423276284")]
    [InlineData("6145550199", PhoneCountryCode.Australia, "+6145550199")]
    [InlineData("45550199", PhoneCountryCode.Brazil, "+5545550199")]
    [InlineData("86105550199", PhoneCountryCode.China, "+86105550199")]
    [InlineData("4515550199", PhoneCountryCode.Denmark, "+4515550199")]
    [InlineData("10111213", PhoneCountryCode.Finland, "+35810111213")]
    [InlineData("33123456789", PhoneCountryCode.France, "+33123456789")]
    [InlineData("123456789", PhoneCountryCode.Germany, "+49123456789")]
    [InlineData("911234567890", PhoneCountryCode.India, "+911234567890")]
    [InlineData("353212345678", PhoneCountryCode.Ireland, "+353212345678")]
    [InlineData("39123456789", PhoneCountryCode.Italy, "+39123456789")]
    [InlineData("8112345678", PhoneCountryCode.Japan, "+818112345678")]
    [InlineData("521234567890", PhoneCountryCode.Mexico, "+521234567890")]
    [InlineData("3123456789", PhoneCountryCode.Netherlands, "+313123456789")]
    [InlineData("6421123456", PhoneCountryCode.NewZealand, "+6421123456")]
    [InlineData("23412345678", PhoneCountryCode.Nigeria, "+23412345678")]
    [InlineData("123456789", PhoneCountryCode.Norway, "+47123456789")]
    [InlineData("34567890", PhoneCountryCode.Pakistan, "+9234567890")]
    [InlineData("63212345678", PhoneCountryCode.Philippines, "+63212345678")]
    [InlineData("211234567", PhoneCountryCode.Portugal, "+351211234567")]
    [InlineData("79123456789", PhoneCountryCode.Russia, "+79123456789")]
    [InlineData("27711234567", PhoneCountryCode.SouthAfrica, "+27711234567")]
    [InlineData("2212345678", PhoneCountryCode.Spain, "+342212345678")]
    [InlineData("098-012-345", PhoneCountryCode.Sweden, "+4698012345")]
    [InlineData("41446681800", PhoneCountryCode.Switzerland, "+41446681800")]
    [InlineData("21234567", PhoneCountryCode.Turkey, "+9021234567")]
    [InlineData("16135550199", PhoneCountryCode.UnitedStates, "+16135550199")]
    public void FormatPhoneNumber_FormatsNumberCorrectly(string phoneNumber, PhoneCountryCode countryCode,
        string expected)
    {
        // Act
        var result = FormatHelper.FormatPhoneNumber(phoneNumber, countryCode);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("6505550199", PhoneCountryCode.UnitedStates, "(650) 555-0199")]
    [InlineData("6135550199", PhoneCountryCode.Canada, "(613) 555-0199")]
    [InlineData("2423276284", PhoneCountryCode.Bahamas, "(242) 327-6284")]
    [InlineData("0420019999", PhoneCountryCode.Australia, "0420 019 999")]
    [InlineData("45550199", PhoneCountryCode.Brazil, "45550199")]
    public void FormatNationalPhoneNumber_FormatsNumberCorrectly(string phoneNumber, PhoneCountryCode countryCode, string expected)
    {
        // Act
        var result = FormatHelper.FormatNationalPhoneNumber(phoneNumber, countryCode);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("6505550199", PhoneCountryCode.UnitedStates, "+1 650-555-0199")]
    [InlineData("6135550199", PhoneCountryCode.Canada, "+1 613-555-0199")]
    [InlineData("4035550199", PhoneCountryCode.Canada, "+1 403-555-0199")]
    [InlineData("2423276284", PhoneCountryCode.Bahamas, "+1 242-327-6284")]
    [InlineData("2425550199", PhoneCountryCode.Bahamas, "+1 242-555-0199")]
    [InlineData("123456789", PhoneCountryCode.Germany, "+49 123456789")]
    public void FormatInternationalPhoneNumber_FormatsNumberCorrectly(string phoneNumber, PhoneCountryCode countryCode, string expected)
    {
        // Act
        var result = FormatHelper.FormatInternationalPhoneNumber(phoneNumber, countryCode);

        // Assert
        result.ShouldBe(expected);
    }

}