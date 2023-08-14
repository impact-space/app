using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Skills;

public interface ISkillAppService : ICrudAppService<SkillDto, Guid, GetSkillListDto, SkillCreateDto, SkillUpdateDto>
{
    Task<List<SkillDto>> SearchSkillsAsync(string searchTerm, int maxResultCount = 10);
}