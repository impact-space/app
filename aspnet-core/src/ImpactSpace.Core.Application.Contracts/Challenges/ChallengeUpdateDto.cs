using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Challenges;

public class ChallengeUpdateDto : IHasConcurrencyStamp
{
    [Required]
    [StringLength(ChallengeConstants.MaxNameLength)]
    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }
}