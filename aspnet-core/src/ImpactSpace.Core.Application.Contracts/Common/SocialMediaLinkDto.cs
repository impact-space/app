using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace ImpactSpace.Core.Common;

public class SocialMediaLinkDto
{
    public SocialMediaPlatform Platform { get; set; }
    
    [Url]
    [DynamicStringLength(typeof(CommonConstants), nameof(CommonConstants.MaxWebsiteLength))]
    public string Url { get; set; }
}