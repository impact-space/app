using System;
using System.Collections.Generic;
using ImpactSpace.Core.Common;
using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Organizations;

public class UpdateOrganizationProfileDto : EntityDto<Guid>
{
    public string MissionStatement { get; set; }
    public string Website { get; set; }
    public PhoneNumberDto PhoneNumber { get; set; }
    public string Email { get; set; }
    //public FileDto Logo { get; set; }
    public List<SocialMediaLinkDto> SocialMediaLinks { get; set; }

    public UpdateOrganizationProfileDto()
    {
        SocialMediaLinks = new List<SocialMediaLinkDto>();
    }
}