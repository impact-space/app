using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Skills;

[Authorize( CorePermissions.Skills.Default)]
public class SkillAppService : CoreAppService, ISkillAppService
{
    private readonly ISkillRepository _skillRepository;
    private readonly SkillManager _skillManager;

    public SkillAppService(ISkillRepository skillRepository, SkillManager skillManager)
    {
        _skillRepository = skillRepository;
        _skillManager = skillManager;
    }
    
    public async Task<SkillDto> GetAsync(Guid id)
    {
        var skill = await _skillRepository.GetAsync(id);
        return ObjectMapper.Map<Skill, SkillDto>(skill);
    }

    public async Task<ListResultDto<SkillDto>> GetListAsync(GetSkillListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Skill.Name);
        }
        
        var skills = await _skillRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter,
            input.SkillGroupId
        );
        
        var totalCount = input.Filter == null
            ? await _skillRepository.CountAsync()
            : await _skillRepository.CountAsync(skill => skill.Name.Contains(input.Filter));
        
        return new PagedResultDto<SkillDto>( 
            totalCount,
            ObjectMapper.Map<List<Skill>, List<SkillDto>>(skills)
        );
    }

    [Authorize(CorePermissions.Skills.Create)]
    public async Task<SkillDto> CreateAsync(CreateSkillDto input)
    {
        var skill = await _skillManager.CreateAsync(
            input.Name,
            input.SkillGroupId
        );
        
        await _skillRepository.InsertAsync(skill);
        
        return ObjectMapper.Map<Skill, SkillDto>(skill);
    }

    [Authorize(CorePermissions.Skills.Edit)]
    public async Task UpdateAsync(Guid id, UpdateSkillDto input)
    {
        var skill = await _skillRepository.GetAsync(id);
        
        if (skill.Name != input.Name)
        {
            await _skillManager.ChangeNameAsync(skill, input.Name);
        }
        
        if (skill.SkillGroupId != input.SkillGroupId)
        {
            await _skillManager.ChangeSkillGroupAsync(skill, input.SkillGroupId);
        }
        
        await _skillRepository.UpdateAsync(skill);
    }

    [Authorize(CorePermissions.Skills.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _skillRepository.GetAsync(id);
        await _skillRepository.DeleteAsync(id);
    }
}