using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Skills;

public interface ISkillAppService : ICrudAppService<SkillDto, Guid, GetSkillListDto, SkillCreateDto, SkillUpdateDto>
{
    
}