using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Skills;

public class SkillGroupDto : EntityDto<Guid>
{
    public string Name { get; set; }
    
    public string Description { get; set; }

    public List<SkillDto> Skills { get; set; } = new();
}