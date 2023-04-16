using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Common;

public class SocialMediaLinkDto
{
    public SocialMediaPlatform Platform { get; set; }
    
    [Url]
    [StringLength(CommonConstants.MaxWebsiteLength)]
    public string Url { get; set; }
}