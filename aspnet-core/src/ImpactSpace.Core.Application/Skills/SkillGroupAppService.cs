using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Skills;

[Authorize(CorePermissions.SkillGroups.Default)]
public class SkillGroupAppService : ApplicationService, ISkillGroupAppService
{
    private readonly ISkillGroupRepository _skillGroupRepository;
    private readonly SkillGroupManager _skillGroupManager;
    
    public SkillGroupAppService(ISkillGroupRepository skillGroupRepository, SkillGroupManager skillGroupManager)
    {
        _skillGroupRepository = skillGroupRepository;
        _skillGroupManager = skillGroupManager;
    }
    
    [Authorize(CorePermissions.SkillGroups.Create)]
    public async Task<SkillGroupDto> CreateAsync(CreateSkillGroupDto input)
    {
        var skillGroup = await _skillGroupManager.CreateAsync(input.Name, input.Description);
        
        await _skillGroupRepository.InsertAsync(skillGroup);
        
        return ObjectMapper.Map<SkillGroup, SkillGroupDto>(skillGroup);
    }
    
    [Authorize(CorePermissions.SkillGroups.Edit)]
    public async Task UpdateAsync(Guid id, UpdateSkillGroupDto input)
    {
        var skillGroup = await _skillGroupRepository.GetAsync(id);

        if (skillGroup.Name != input.Name)
        {
            await _skillGroupManager.ChangeNameAsync(skillGroup, input.Name);    
        }
        
        if (skillGroup.Description != input.Description)
        {
            _skillGroupManager.ChangeDescription(skillGroup, input.Description);
        }

        await _skillGroupRepository.UpdateAsync(skillGroup);
    }
    
    [Authorize(CorePermissions.SkillGroups.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _skillGroupManager.DeleteAsync(id);
    }
    
    public async Task<SkillGroupDto> GetAsync(Guid id)
    {
        var skillGroup = await _skillGroupRepository.GetAsync(id);
        
        return ObjectMapper.Map<SkillGroup, SkillGroupDto>(skillGroup);
    }
    
    public async Task<PagedResultDto<SkillGroupDto>> GetListAsync(GetSkillGroupListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(SkillGroup.Name);
        }
        
        var skillGroups = await _skillGroupRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter,
            input.IncludeSkills);
        
        var totalCount = input.Filter == null
            ? await _skillGroupRepository.CountAsync()
            : await _skillGroupRepository.CountAsync(
                skillGroup => skillGroup.Name.Contains(input.Filter));
        
        return new PagedResultDto<SkillGroupDto>(
            totalCount,
            ObjectMapper.Map<List<SkillGroup>, List<SkillGroupDto>>(skillGroups)
        );
    }
}