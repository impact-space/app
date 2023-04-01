using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Skills;

public interface ISkillGroupAppService : IApplicationService
{
    Task<SkillGroupDto> CreateAsync(SkillGroupCreateDto input);
    Task UpdateAsync(Guid id, SkillGroupUpdateDto input);
    Task DeleteAsync(Guid id);
    Task<SkillGroupDto> GetAsync(Guid id);
    Task<PagedResultDto<SkillGroupDto>> GetListAsync(GetSkillGroupListDto input);
}