using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Common;

public class PhoneNumberTests : CoreDomainTestBase
{
    [Fact]
    public void Should_Throw_Exception_When_Phone_Number_Is_Invalid()
    {
        // Arrange
        var countryCode = PhoneCountryCode.UnitedStates;
        var invalidNumber = "1234";
            
        // Act & Assert
        Should.Throw<ArgumentException>(() => new PhoneNumber(countryCode, invalidNumber));
    }

    [Fact]
    public void Should_Format_National_Number_When_Creating_New_Phone_Number()
    {
        // Arrange
        var countryCode = PhoneCountryCode.UnitedStates;
        var rawNumber = "2695555555";
        var expectedFormattedNumber = "(269) 555-5555";

        // Act
        var phoneNumber = new PhoneNumber(countryCode, rawNumber);

        // Assert
        phoneNumber.NationalNumber.ShouldBe(expectedFormattedNumber);
    }

    [Fact]
    public void Should_Be_Equal_If_Both_Country_Code_And_National_Number_Are_Equal()
    {
        // Arrange
        var countryCode = PhoneCountryCode.UnitedStates;
        var rawNumber = "2695555555";

        var phoneNumber1 = new PhoneNumber(countryCode, rawNumber);
        var phoneNumber2 = new PhoneNumber(countryCode, rawNumber);

        // Act
        bool areEqual = phoneNumber1.ValueEquals(phoneNumber2);

        // Assert
        areEqual.ShouldBeTrue();
    }
}