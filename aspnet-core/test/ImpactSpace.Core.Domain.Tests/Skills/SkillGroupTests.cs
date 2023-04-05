using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Skills
{
    public class SkillGroupTests
    {
        [Fact]
        public Task Should_Create_SkillGroup_With_Correct_Name()
        {
            // Arrange
            var name = "Test Skill Group";

            // Act
            var skillGroup = new SkillGroup(Guid.NewGuid(), name);

            // Assert
            skillGroup.Name.ShouldBe(name);
            return Task.CompletedTask;
        }

        [Fact]
        public Task Should_Not_Create_SkillGroup_With_Null_Name()
        {
            // Arrange
            string name = null;

            // Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                // ReSharper disable once AssignNullToNotNullAttribute
                new SkillGroup(Guid.NewGuid(), name);
            });
            return Task.CompletedTask;
        }

        [Fact]
        public Task Should_Not_Create_SkillGroup_With_Whitespace_Name()
        {
            // Arrange
            var name = " ";

            // Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new SkillGroup(Guid.NewGuid(), name);
            });
            return Task.CompletedTask;
        }

        [Fact]
        public Task Should_Create_SkillGroup_With_Correct_Description()
        {
            // Arrange
            var name = "Test Skill Group";
            var description = "Test Description";

            // Act
            var skillGroup = new SkillGroup(Guid.NewGuid(), name, description);

            // Assert
            skillGroup.Description.ShouldBe(description);
            return Task.CompletedTask;
        }

        [Fact]
        public Task Should_Add_Skill_To_SkillGroup()
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "Test Skill Group");
            var skill = new Skill(Guid.NewGuid(), "Test Skill", skillGroup.Id);

            // Act
            skillGroup.AddSkill(skill);

            // Assert
            skillGroup.Skills.ShouldContain(skill);
            return Task.CompletedTask;
        }

        [Fact]
        public Task Should_Not_Add_Null_Skill_To_SkillGroup()
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "Test Skill Group");
            Skill skill = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                skillGroup.AddSkill(skill);
            });
            return Task.CompletedTask;
        }

        [Fact]
        public Task Should_Remove_Skill_From_SkillGroup()
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "Test Skill Group");
            var skill = new Skill(Guid.NewGuid(), "Test Skill", skillGroup.Id);
            skillGroup.AddSkill(skill);

            // Act
            skillGroup.RemoveSkill(skill);

            // Assert
            skillGroup.Skills.ShouldNotContain(skill);
            return Task.CompletedTask;
        }

        [Fact]
        public Task Should_Not_Remove_Null_Skill_From_SkillGroup()
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "Test Skill Group");
            Skill skill = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                skillGroup.RemoveSkill(skill);
            });
            return Task.CompletedTask;
        }
    }
}