using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Xunit;

namespace ImpactSpace.Core.Projects
{
    public sealed class TagManagerTests : CoreDomainTestBase
    {
        private readonly TagManager _tagManager;
        private readonly ITagRepository _tagRepository;

        public TagManagerTests()
        {
            _tagRepository = GetRequiredService<ITagRepository>();
            _tagManager = GetRequiredService<TagManager>();
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Tag()
        {
            // Arrange
            var tagName = "MyTag";

            // Act
            var tag = await _tagManager.CreateAsync(tagName);

            // Assert
            tag.ShouldNotBeNull();
            tag.Name.ShouldBe(tagName);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_Exception_When_Tag_Already_Exists()
        {
            // Arrange
            var existingTag = new Tag(Guid.NewGuid(), "ExistingTag");
            await _tagRepository.InsertAsync(existingTag);

            // Act & Assert
            await Should.ThrowAsync<TagAlreadyExistsException>(async () =>
                await _tagManager.CreateAsync(existingTag.Name)
            );
        }

        [Fact]
        public async Task ChangeNameAsync_Should_Change_Tag_Name()
        {
            // Arrange
            var tagName = "MyTag";
            var tag = new Tag(Guid.NewGuid(), tagName);
            await _tagRepository.InsertAsync(tag);

            // Act
            var newTagName = "NewTag";
            await _tagManager.ChangeNameAsync(tag, newTagName);

            await _tagRepository.UpdateAsync(tag);

            // Assert
            var updatedTag = await _tagRepository.GetAsync(tag.Id);
            updatedTag.ShouldNotBeNull();
            updatedTag.Name.ShouldBe(newTagName);
        }

        [Fact]
        public async Task ChangeNameAsync_Should_Throw_Exception_When_Changing_To_Existing_Tag_Name()
        {
            // Arrange
            var existingTag = new Tag(Guid.NewGuid(), "ExistingTag");
            await _tagRepository.InsertAsync(existingTag);
            var tag = new Tag(Guid.NewGuid(), "MyTag");
            await _tagRepository.InsertAsync(tag);

            // Act & Assert
            await Should.ThrowAsync<TagAlreadyExistsException>(async () =>
                await _tagManager.ChangeNameAsync(tag, existingTag.Name)
            );
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Tag()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                // Arrange
                var tag = new Tag(Guid.NewGuid(), "MyTag");
                await _tagRepository.InsertAsync(tag, true);

                // Act
                await _tagManager.DeleteAsync(tag.Id);

                // Assert
                await Should.ThrowAsync<EntityNotFoundException>(async () =>
                    await _tagRepository.GetAsync(tag.Id)
                );
            });
        }
    }
}