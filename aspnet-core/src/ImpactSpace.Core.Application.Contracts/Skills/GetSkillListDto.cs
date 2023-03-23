using System;
using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Skills;

public class GetSkillListDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
    
    public Guid? SkillGroupId { get; set; }
}