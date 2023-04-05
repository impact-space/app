using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Xunit;

namespace ImpactSpace.Core.Skills;

public sealed class SkillGroupManagerTests : CoreDomainTestBase
    {
        private readonly ISkillGroupRepository _skillGroupRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly SkillGroupManager _skillGroupManager;

        public SkillGroupManagerTests()
        {
            _skillGroupRepository = GetRequiredService<ISkillGroupRepository>();
            _skillRepository = GetRequiredService<ISkillRepository>();
            _skillGroupManager = GetRequiredService<SkillGroupManager>();
        }

        [Fact]
        public async Task Should_Create_New_Skill_Group()
        {
            // Arrange
            var name = "New Skill Group";

            // Act
            var skillGroup = await _skillGroupManager.CreateAsync(name);

            // Assert
            skillGroup.Id.ShouldNotBe(Guid.Empty);
            skillGroup.Name.ShouldBe(name);
            skillGroup.Description.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Not_Create_Existing_Skill_Group()
        {
            // Arrange
            var existingSkillGroup = new SkillGroup(Guid.NewGuid(), "Existing Skill Group");
            await _skillGroupRepository.InsertAsync(existingSkillGroup);

            // Act & Assert
            await Should.ThrowAsync<SkillGroupAlreadyExistsException>(async () =>
            {
                await _skillGroupManager.CreateAsync(existingSkillGroup.Name);
            });
        }

        [Fact]
        public async Task Should_Change_Skill_Group_Name()
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "Old Name");
            await _skillGroupRepository.InsertAsync(skillGroup);

            var newName = "New Name";

            // Act
            await _skillGroupManager.ChangeNameAsync(skillGroup, newName);

            await _skillGroupRepository.UpdateAsync(skillGroup, true);

            // Assert
            var changedSkillGroup = await _skillGroupRepository.FindAsync(skillGroup.Id);
            changedSkillGroup.Name.ShouldBe(newName);
        }

        [Fact]
        public async Task Should_Not_Change_Name_With_Existing_Name()
        {
            // Arrange
            var existingSkillGroup = new SkillGroup(Guid.NewGuid(), "Existing Skill Group");
            await _skillGroupRepository.InsertAsync(existingSkillGroup);

            var skillGroup = new SkillGroup(Guid.NewGuid(), "Old Name");
            await _skillGroupRepository.InsertAsync(skillGroup);

            // Act & Assert
            await Should.ThrowAsync<SkillGroupAlreadyExistsException>(async () =>
            {
                await _skillGroupManager.ChangeNameAsync(skillGroup, existingSkillGroup.Name);
            });
        }

        [Fact]
        public async Task Should_Change_Skill_Group_Description()
        {
            // Arrange
            var skillGroup = new SkillGroup(Guid.NewGuid(), "Name");
            var description = "Old Description";
            skillGroup.ChangeDescription(description);
            await _skillGroupRepository.InsertAsync(skillGroup);

            var newDescription = "New Description";

            // Act
            _skillGroupManager.ChangeDescription(skillGroup, newDescription);

            await _skillGroupRepository.UpdateAsync(skillGroup);

            // Assert
            var changedSkillGroup = _skillGroupRepository.GetAsync(skillGroup.Id).Result;
            changedSkillGroup.Description.ShouldBe(newDescription);
        }

        [Fact]
        public async Task Should_Delete_Skill_Group_Without_Skills()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                // Arrange
                var skillGroup = new SkillGroup(Guid.NewGuid(), "Name");
                await _skillGroupRepository.InsertAsync(skillGroup, true);

                // Act
                await _skillGroupManager.DeleteAsync(skillGroup.Id);

                // Assert
                await Should.ThrowAsync<EntityNotFoundException>(async () =>
                {
                    await _skillGroupRepository.GetAsync(skillGroup.Id);
                });
            });
        }

        [Fact]
        public async Task Should_Not_Delete_Skill_Group_With_Skills()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                // Arrange
                var skillGroup = new SkillGroup(Guid.NewGuid(), "Name");
                await _skillGroupRepository.InsertAsync(skillGroup, true);

                var skill = new Skill(Guid.NewGuid(), "Name", skillGroup.Id);
                await _skillRepository.InsertAsync(skill, true);

                // Act & Assert
                await Should.ThrowAsync<SkillGroupHasSkillsException>(async () =>
                {
                    await _skillGroupManager.DeleteAsync(skillGroup.Id);
                });    
            });
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Deleting_Non_Existent_Skill_Group()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act & Assert
            await Should.ThrowAsync<EntityNotFoundException>(async () =>
            {
                await _skillGroupManager.DeleteAsync(nonExistentId);
            });
        }
    }