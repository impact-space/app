using System;
using ImpactSpace.Core.Common;
using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberSkillDto : EntityDto
{
    public Guid OrganizationMemberId { get; set; }

    public Guid SkillId { get; set; }

    public ProficiencyLevel ProficiencyLevel { get; set; } = ProficiencyLevel.Beginner;
}