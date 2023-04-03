using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Skills;

[Authorize]
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

    public async Task<PagedResultDto<SkillDto>> GetListAsync(GetSkillListDto input)
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
        
        var totalCount = input.Filter == null && input.SkillGroupId == null
            ? await _skillRepository.CountAsync()
            : await _skillRepository.CountAsync(skill => 
                (input.Filter == null || skill.Name.Contains(input.Filter)) && 
                (input.SkillGroupId == null || skill.SkillGroupId == input.SkillGroupId));
        
        return new PagedResultDto<SkillDto>( 
            totalCount,
            ObjectMapper.Map<List<Skill>, List<SkillDto>>(skills)
        );
    }

    [Authorize(CorePermissions.GlobalTypes.Skills.Create)]
    public async Task<SkillDto> CreateAsync(SkillCreateDto input)
    {
        var skill = await _skillManager.CreateAsync(
            input.Name,
            input.SkillGroupId
        );
        
        await _skillRepository.InsertAsync(skill);
        
        return ObjectMapper.Map<Skill, SkillDto>(skill);
    }

    [Authorize(CorePermissions.GlobalTypes.Skills.Edit)]
    public async Task<SkillDto> UpdateAsync(Guid id, SkillUpdateDto input)
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
        
        return ObjectMapper.Map<Skill, SkillDto>(skill);
    }

    [Authorize(CorePermissions.GlobalTypes.Skills.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _skillRepository.GetAsync(id);
        await _skillRepository.DeleteAsync(id);
    }
}