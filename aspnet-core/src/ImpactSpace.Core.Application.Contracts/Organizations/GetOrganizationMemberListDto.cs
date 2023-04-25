using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Organizations;

public class GetOrganizationMemberListDto: PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}