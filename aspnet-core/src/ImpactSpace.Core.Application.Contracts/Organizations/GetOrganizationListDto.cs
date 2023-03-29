using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Organizations;

public class GetOrganizationListDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}