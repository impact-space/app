using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Skills;

public class SkillGroupManager : DomainService
{
    private readonly ISkillGroupRepository _skillGroupRepository;

    public SkillGroupManager(ISkillGroupRepository skillGroupRepository)
    {
        _skillGroupRepository = skillGroupRepository;
    }

    public async Task<SkillGroup> CreateAsync(
        [NotNull] string name,
        [CanBeNull] string description = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

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
            throw new SkillAlreadyExistsException(newName);
        }

        skillGroup.ChangeName(newName);
    }
}