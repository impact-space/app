using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Skills;

public interface ISkillAppService : IApplicationService
{
    Task<SkillDto> GetAsync(Guid id);
    Task<ListResultDto<SkillDto>> GetListAsync(GetSkillListDto input);
    Task<SkillDto> CreateAsync(CreateSkillDto input);
    Task UpdateAsync(Guid id, UpdateSkillDto input);
    Task DeleteAsync(Guid id);
}