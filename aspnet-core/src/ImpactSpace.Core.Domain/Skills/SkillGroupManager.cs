using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Skills;

public sealed class SkillGroupManager : DomainService
{
    private readonly ISkillGroupRepository _skillGroupRepository;
    private readonly ISkillRepository _skillRepository;

    public SkillGroupManager(
        ISkillGroupRepository skillGroupRepository, 
        ISkillRepository skillRepository)
    {
        _skillGroupRepository = skillGroupRepository;
        _skillRepository = skillRepository;
    }

    public async Task<SkillGroup> CreateAsync(
        [NotNull] string name,
        [CanBeNull] string description = null)
    {
        Check.NotNullOrWhiteSpace(
            name, 
            nameof(name),
            SkillGroupConstants.MaxNameLength
        );

        var existingSkillGroup = await _skillGroupRepository.FindByNameAsync(name);

        if (existingSkillGroup != null)
        {
            throw new SkillGroupAlreadyExistsException(name);
        }

        return new SkillGroup(
            GuidGenerator.Create(),
            name,
            description
        );
    }
    
    public async Task ChangeNameAsync(
        [NotNull] SkillGroup skillGroup,
        [NotNull] string newName)
    {
        Check.NotNull(skillGroup, nameof(skillGroup));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        var existingSkillGroup = await _skillGroupRepository.FindByNameAsync(newName);
        if (existingSkillGroup != null && existingSkillGroup.Id != skillGroup.Id)
        {
            throw new SkillGroupAlreadyExistsException(newName);
        }

        skillGroup.ChangeName(newName);
    }
    
    public void ChangeDescription(
        [NotNull] SkillGroup skillGroup,
        [CanBeNull] string newDescription)
    {
        Check.NotNull(skillGroup, nameof(skillGroup));

        skillGroup.ChangeDescription(newDescription);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var skillGroup = await _skillGroupRepository.GetAsync(id);

        var skillCount = await _skillRepository.CountAsync(x => x.SkillGroupId == skillGroup.Id);
        
        if (skillCount > 0)
        {
            throw new SkillGroupHasSkillsException(skillGroup.Id, skillGroup.Name);
        }

        await _skillGroupRepository.DeleteAsync(skillGroup, true);
    }
}