using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public Guid OrganizationId { get; set; }

    public List<OrganizationMemberSkillDto> OrganizationMemberSkills { get; set; } = new();

}