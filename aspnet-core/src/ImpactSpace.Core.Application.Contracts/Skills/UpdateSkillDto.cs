using System;
using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Skills;

public class UpdateSkillDto
{
    [Required]
    [StringLength(SkillConstants.MaxNameLength)]
    public string Name { get; set; }
    
    public Guid SkillGroupId { get; set; }
    
}