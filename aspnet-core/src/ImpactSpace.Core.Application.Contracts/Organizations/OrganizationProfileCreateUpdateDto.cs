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

    public string Website { get; set; }

    public PhoneNumberDto PhoneNumber { get; set; }

    public string Email { get; set; }

    public string LogoBase64 { get; set; }
    
    public List<SocialMediaLinkDto> SocialMediaLinks { get; set; }

    public OrganizationProfileCreateUpdateDto()
    {
        SocialMediaLinks = new List<SocialMediaLinkDto>();
    }
}