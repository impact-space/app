using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Skills;

public class UpdateSkillGroupDto
{
    [Required]
    [StringLength(SkillGroupConstants.MaxNameLength)]
    public string Name { get; set; }
    
    [StringLength(SkillGroupConstants.MaxDescriptionLength)]
    public string Description { get; set; }
}