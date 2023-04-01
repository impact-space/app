using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Skills;

public class SkillDto: EntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }
    
    public Guid SkillGroupId { get; set; }
    

    public string ConcurrencyStamp { get; set; }
}