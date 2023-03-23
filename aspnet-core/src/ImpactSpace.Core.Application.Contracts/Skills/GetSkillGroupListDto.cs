using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Skills;

public class GetSkillGroupListDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
    
    public bool IncludeSkills { get; set; }
}