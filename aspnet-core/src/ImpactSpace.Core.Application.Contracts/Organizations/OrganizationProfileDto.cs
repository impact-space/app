using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ImpactSpace.Core.Common;

namespace ImpactSpace.Core.Organizations;

public class OrganizationProfileDto
{
    public Guid OrganizationId { get; set; }
    
    [StringLength(OrganizationProfileConstants.MaxMissionStatementLength)]
    public string MissionStatement { get; set; }
    
    [Url]
    [StringLength(CommonConstants.MaxWebsiteLength)]
    public string Website { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    [Url]
    [StringLength(OrganizationProfileConstants.MaxLogoUrlLength)]
    public string LogoUrl { get; set; }
    
    public List<SocialMediaLinkDto> SocialMediaLinks { get; set; }
}