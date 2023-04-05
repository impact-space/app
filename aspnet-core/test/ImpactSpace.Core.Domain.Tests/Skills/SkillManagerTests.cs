using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Xunit;

namespace ImpactSpace.Core.Skills;

public sealed class SkillManagerTests : CoreDomainTestBase
{
    private readonly SkillManager _skillManager;
    private readonly ISkillRepository _skillRepository;
    private readonly ISkillGroupRepository _skillGroupRepository;

    public SkillManagerTests()
    {
        _skillRepository = GetRequiredService<ISkillRepository>();
        _skillGroupRepository = GetRequiredService<ISkillGroupRepository>();
        _skillManager = GetRequiredService<SkillManager>();
    }

    [Fact]
    public async Task Should_Create_Skill()
    {
        // Arrange
        var name = "new skill";
        var skillGroup = new SkillGroup(Guid.NewGuid(), "new skill group");
        await _skillGroupRepository.InsertAsync(skillGroup, true);

        // Act
        var skill = await _skillManager.CreateAsync(name, skillGroup.Id);

        // Assert
        skill.ShouldNotBeNull();
        skill.Id.ShouldNotBe(Guid.Empty);
        skill.Name.ShouldBe(name);
        skill.SkillGroupId.ShouldBe(skillGroup.Id);
    }

    [Fact]
    public async Task Should_Not_Create_Skill_With_Null_Or_WhiteSpace_Name()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "new skill group");
            await _skillGroupRepository.InsertAsync(skillGroup, true);

            // Act & Assert
            await Should.ThrowAsync<ArgumentException>(
                // ReSharper disable once AssignNullToNotNullAttribute
                _skillManager.CreateAsync(null, skillGroup.Id)
            );
            await Should.ThrowAsync<ArgumentException>(
                _skillManager.CreateAsync("", skillGroup.Id)
            );
            await Should.ThrowAsync<ArgumentException>(
                _skillManager.CreateAsync("   ", skillGroup.Id)
            );    
        });
    }

    [Fact]
    public async Task Should_Not_Create_Skill_With_Duplicate_Name()
    {
        // Arrange
        var skillGroup = new SkillGroup(Guid.NewGuid(), "new skill group");
        await _skillGroupRepository.InsertAsync(skillGroup,true );
        var existingSkill = new Skill(Guid.NewGuid(), "existing skill", skillGroup.Id);
        await _skillRepository.InsertAsync(existingSkill, true);

        // Act & Assert
        await Should.ThrowAsync<SkillAlreadyExistsException>(
            _skillManager.CreateAsync(existingSkill.Name, skillGroup.Id)
        );
    }

    [Fact]
    public async Task Should_Change_Name_Of_Skill()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "new skill group");
            await _skillGroupRepository.InsertAsync(skillGroup, true);
        
            var skill = new Skill(Guid.NewGuid(), "old name", skillGroup.Id);
            await _skillRepository.InsertAsync(skill, true);
            var newName = "new name";

            // Act
            await _skillManager.ChangeNameAsync(skill, newName);

            await _skillRepository.UpdateAsync(skill, true);

            // Assert
            skill.Name.ShouldBe(newName);
            (await _skillRepository.FindAsync(skill.Id)).Name.ShouldBe(newName);
        });
    }

    [Fact]
    public async Task Should_Not_Change_Name_Of_Skill_With_Null_Or_WhiteSpace_NewName()
    {
        // Arrange
        var skill = new Skill(Guid.NewGuid(), "name", Guid.NewGuid());

        // Act & Assert
        await Should.ThrowAsync<ArgumentException>(
            // ReSharper disable once AssignNullToNotNullAttribute
            _skillManager.ChangeNameAsync(skill, null)
        );
        await Should.ThrowAsync<ArgumentException>(
            _skillManager.ChangeNameAsync(skill, "")
        );
        await Should.ThrowAsync<ArgumentException>(
            _skillManager.ChangeNameAsync(skill, "   ")
        );
    }

    [Fact]
    public async Task Should_Not_Change_Name_Of_Skill_With_Duplicate_NewName()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "new skill group");
            await _skillGroupRepository.InsertAsync(skillGroup, true);
        
            var skill1 = new Skill(Guid.NewGuid(), "skill 1", skillGroup.Id);
            var skill2 = new Skill(Guid.NewGuid(), "skill 2", skillGroup.Id);
            await _skillRepository.InsertAsync(skill1, true);
            await _skillRepository.InsertAsync(skill2, true);

            // Act & Assert
            await Should.ThrowAsync<SkillAlreadyExistsException>(
                _skillManager.ChangeNameAsync(skill1, skill2.Name)
            );    
        });
    }

    [Fact]
    public async Task Should_Change_SkillGroup_Of_Skill()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "new skill group");
            await _skillGroupRepository.InsertAsync(skillGroup, true);
        
            var skill = new Skill(Guid.NewGuid(), "name", skillGroup.Id);
            var newSkillGroupId = Guid.NewGuid();
            await _skillGroupRepository.InsertAsync(new SkillGroup(newSkillGroupId, "new group"), true);

            // Act
            await _skillManager.ChangeSkillGroupAsync(skill, newSkillGroupId);
            
            // Assert
            skill.SkillGroupId.ShouldBe(newSkillGroupId);
        });
    }

    [Fact]
    public async Task Should_Not_Change_SkillGroup_Of_Skill_With_Nonexistent_SkillGroup()
    {
        // Arrange
       
        var skill = new Skill(Guid.NewGuid(), "name", Guid.NewGuid());
        var nonexistentSkillGroupId = Guid.NewGuid();

        // Act & Assert
        await Should.ThrowAsync<EntityNotFoundException>(
            _skillManager.ChangeSkillGroupAsync(skill, nonexistentSkillGroupId)
        );
    }
}