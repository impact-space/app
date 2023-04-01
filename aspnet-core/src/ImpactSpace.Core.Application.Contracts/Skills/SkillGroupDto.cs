using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Skills;

public class SkillGroupDto : EntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }
    
    public string Description { get; set; }

    public List<SkillDto> Skills { get; set; } = new();
    
    public string ConcurrencyStamp { get; set; }
}