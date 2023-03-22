using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Skills;

public class SkillManager : DomainService
{
    private readonly ISkillRepository _skillRepository;
    private readonly ISkillGroupRepository _skillGroupRepository;

    public SkillManager(ISkillRepository skillRepository, ISkillGroupRepository skillGroupRepository)
    {
        _skillRepository = skillRepository;
        _skillGroupRepository = skillGroupRepository;
    }

    public async Task<Skill> CreateAsync(
        [NotNull] string name,
        Guid skillGroupId)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existingSkill = await _skillRepository.FindByNameAsync(name);

        if (existingSkill != null)
        {
            throw new SkillAlreadyExistsException(name);
        }

        var existingSkillGroup = await _skillGroupRepository.GetAsync(skillGroupId);

        if (existingSkillGroup == null)
        {
            throw new SkillGroupNotFoundException(skillGroupId);
        }

        return new Skill(
            GuidGenerator.Create(),
            name, 
            skillGroupId
        );
    }
    
    public async Task ChangeNameAsync(
        [NotNull] Skill skill,
        [NotNull] string newName)
    {
        Check.NotNull(skill, nameof(skill));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        var existingSkill = await _skillRepository.FindByNameAsync(newName);
        if (existingSkill != null && existingSkill.Id != skill.Id)
        {
            throw new SkillAlreadyExistsException(newName);
        }

        skill.ChangeName(newName);
    }

    public async Task ChangeSkillGroupAsync(
        [NotNull] Skill skill,
        Guid skillGroupId)
    {
        Check.NotNull(skill, nameof(skill));
        
        var existingSkillGroup = await _skillGroupRepository.GetAsync(skillGroupId);

        if (existingSkillGroup == null)
        {
            throw new SkillGroupNotFoundException(skillGroupId);
        }

        skill.ChangeSkillGroup(skillGroupId);
    }
}