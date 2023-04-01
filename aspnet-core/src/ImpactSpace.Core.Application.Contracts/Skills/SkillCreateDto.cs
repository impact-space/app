using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Skills;

public class SkillCreateDto : IHasConcurrencyStamp
{
    [Required]
    [StringLength(SkillConstants.MaxNameLength)]
    public string Name { get; set; }
    
    public Guid SkillGroupId { get; set; }
    
    public string ConcurrencyStamp { get; set; }
}