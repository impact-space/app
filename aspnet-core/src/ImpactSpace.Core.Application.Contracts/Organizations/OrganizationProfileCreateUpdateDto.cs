using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ImpactSpace.Core.Common;
using PhoneNumbers;

namespace ImpactSpace.Core.Organizations;

public class OrganizationProfileCreateUpdateDto
{
    [Required]
    public Guid OrganizationId { get; set; }

    public string MissionStatement { get; set; }

    [Url]
    public string Website { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string LogoBase64 { get; set; }
    
    public List<SocialMediaLinkDto> SocialMediaLinks { get; set; }

    public OrganizationProfileCreateUpdateDto()
    {
        SocialMediaLinks = new List<SocialMediaLinkDto>();
    }
}