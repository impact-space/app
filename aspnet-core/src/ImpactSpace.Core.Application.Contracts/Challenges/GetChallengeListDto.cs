using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Challenges;

public class GetChallengeListDto: PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}