using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Challenges;

public class ChallengeCreateDto
{
    [Required]
    [StringLength(ChallengeConstants.MaxNameLength)]
    public string Name { get; set; }
}