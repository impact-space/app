using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ImpactSpace.Core.Common;
using Volo.Abp.Validation;

namespace ImpactSpace.Core.Organizations;

public class OrganizationProfileCreateUpdateDto
{
    [Required]
    public Guid OrganizationId { get; set; }

    [MaxLength(OrganizationProfileConstants.MaxMissionStatementLength)]
    public string MissionStatement { get; set; }
    
    [DynamicStringLength(typeof(CommonConstants), nameof(CommonConstants.MaxWebsiteLength))]
    [Url]
    public string Website { get; set; }
    
    [DynamicStringLength(typeof(CommonConstants), nameof(CommonConstants.MaxPhoneLength))]
    [Phone]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    [DynamicStringLength(typeof(CommonConstants), nameof(CommonConstants.MaxEmailLength))]
    public string Email { get; set; }

    public string Logo { get; set; }
    
    public List<SocialMediaLinkDto> SocialMediaLinks { get; set; }

    public OrganizationProfileCreateUpdateDto()
    {
        SocialMediaLinks = new List<SocialMediaLinkDto>();
    }
}