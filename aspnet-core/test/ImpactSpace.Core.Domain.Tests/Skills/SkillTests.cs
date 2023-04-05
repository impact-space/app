using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Skills;

public class SkillTests: CoreDomainTestBase
{
    [Fact]
    public async Task Can_Create_Skill()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Programming";
        var skillGroupId = Guid.NewGuid();

        // Act
        var skill = new Skill(id, name, skillGroupId);

        // Assert
        skill.Id.ShouldBe(id);
        skill.Name.ShouldBe(name);
        skill.SkillGroupId.ShouldBe(skillGroupId);
        skill.OrganizationMemberSkills.ShouldNotBeNull();
        skill.ProjectSkills.ShouldNotBeNull();
    }

    [Fact]
    public async Task  Can_Change_Skill_Name()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Programming";
        var skillGroupId = Guid.NewGuid();
        var skill = new Skill(id, name, skillGroupId);

        // Act
        var newName = "Software Development";
        skill.ChangeName(newName);

        // Assert
        skill.Name.ShouldBe(newName);
    }

    [Fact]
    public async Task  Can_Change_Skill_Group()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Programming";
        var skillGroupId = Guid.NewGuid();
        var skill = new Skill(id, name, skillGroupId);

        // Act
        var newSkillGroupId = Guid.NewGuid();
        skill.ChangeSkillGroup(newSkillGroupId);

        // Assert
        skill.SkillGroupId.ShouldBe(newSkillGroupId);
    }
}