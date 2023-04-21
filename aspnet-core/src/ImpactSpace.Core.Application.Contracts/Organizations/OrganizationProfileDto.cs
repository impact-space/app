using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ImpactSpace.Core.Common;

namespace ImpactSpace.Core.Organizations;

public class OrganizationProfileDto
{
    public Guid OrganizationId { get; set; }
    
    public string MissionStatement { get; set; }
    
    public string Website { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Email { get; set; }
    
    public string Logo { get; set; }

    public List<SocialMediaLinkDto> SocialMediaLinks { get; set; }
}