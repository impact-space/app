using System;
using System.Collections.Generic;
using ImpactSpace.Core.Organizations;
using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Common
{
    public class SocialMediaLinkTests
    {
        [Fact]
        public void Should_Create_SocialMediaLink()
        {
            var socialMediaLink = new SocialMediaLink(SocialMediaPlatform.Twitter, "https://twitter.com/Impact_Space");

            socialMediaLink.Platform.ShouldBe(SocialMediaPlatform.Twitter);
            socialMediaLink.Url.ShouldBe("https://twitter.com/Impact_Space");
        }

        [Fact]
        public void Should_Throw_Validation_Exception_For_Invalid_Url()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var socialMediaLink = new SocialMediaLink(SocialMediaPlatform.Facebook, "invalid url");
            });
        }

        [Theory]
        [MemberData(nameof(ValidUrls))]
        public void Should_Set_Valid_Url(string validUrl)
        {
            var socialMediaLink = new SocialMediaLink(SocialMediaPlatform.LinkedIn, validUrl);

            socialMediaLink.Platform.ShouldBe(SocialMediaPlatform.LinkedIn);
            socialMediaLink.Url.ShouldBe(validUrl);
        }

        public static IEnumerable<object[]> ValidUrls => new List<object[]>
        {
            new object[] { "https://www.linkedin.com/company/impact-space/" },
            new object[] { "https://www.facebook.com/ImpactSpaceHQ" },
            new object[] { "https://www.instagram.com/impact_space/" },
            new object[] { "https://www.youtube.com/channel/UCsPGkNvTJZ0h3qawbIjCr-w" },
        };
    }
}