using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Common;

public class ValidationHelperTests : CoreDomainTestBase
{
    [Theory]
    [InlineData("https://www.google.com", true)]
    [InlineData("invalid_url", false)]
    public void IsValidUrl_ReturnsExpectedResult(string url, bool expected)
    {
        // Act
        var result = ValidationHelper.IsValidUrl(url);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("", false)]
    [InlineData("invalid_email", false)]
    public void IsValidEmail_ReturnsExpectedResult(string email, bool expected)
    {
        // Act
        var result = ValidationHelper.IsValidEmail(email);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("250-555-0199", PhoneCountryCode.Canada, true)]
    [InlineData("(650) 555-0199", PhoneCountryCode.UnitedStates, true)]
    [InlineData("242-327-6284", PhoneCountryCode.Bahamas, true)]
    [InlineData("+49 30 1234567", PhoneCountryCode.Germany, true)]
    [InlineData("", PhoneCountryCode.Canada, false)]
    [InlineData("invalid_phone_number", PhoneCountryCode.Canada, false)]
    public void IsValidPhoneNumber_ValidPhoneNumber_ReturnsExpectedResult(string phoneNumber, PhoneCountryCode countryCode, bool expected)
    {
        // Act
        var result = ValidationHelper.IsValidPhoneNumber(phoneNumber, countryCode);

        // Assert
        result.ShouldBe(expected);
    }
}