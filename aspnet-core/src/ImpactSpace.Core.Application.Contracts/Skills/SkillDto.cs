using System;
using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Skills;

public class SkillDto: EntityDto<Guid>
{
    public string Name { get; set; }
    
    public Guid SkillGroupId { get; set; }
    
    
    
}