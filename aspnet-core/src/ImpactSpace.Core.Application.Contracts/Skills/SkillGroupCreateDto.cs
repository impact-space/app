using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Skills;

public class SkillGroupCreateDto : IHasConcurrencyStamp
{
    
    [Required]
    [StringLength(SkillGroupConstants.MaxNameLength)]
    public string Name { get; set; }
    
    [StringLength(SkillGroupConstants.MaxDescriptionLength)]
    public string Description { get; set; }

    public string ConcurrencyStamp { get; set; }
}