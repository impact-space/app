using System;
using System.Text;
using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Projects;

public class TagTests
    {
        [Fact]
        public void Can_Create_Tag()
        {
            // Arrange & Act
            var tag = new Tag(
                Guid.NewGuid(),
                "Test Tag"
            );

            // Assert
            tag.Id.ShouldNotBe(Guid.Empty);
            tag.Name.ShouldBe("Test Tag");
            tag.ProjectTags.ShouldNotBeNull();
            tag.ProjectTags.ShouldBeEmpty();
            tag.TenantId.ShouldBeNull();
        }

        [Fact]
        public void Cannot_Create_Tag_Without_Name()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                var tag = new Tag(
                    Guid.NewGuid(),
                    null
                );
            });
        }

        [Fact]
        public void Cannot_Create_Tag_With_Empty_Name()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                var tag = new Tag(
                    Guid.NewGuid(),
                    ""
                );
            });
        }

        [Fact]
        public void Cannot_Create_Tag_With_Long_Name()
        {
            // Arrange
            var maxLength = TagConstants.MaxNameLength;
            var longName = new StringBuilder().Insert(0, "a", maxLength + 1).ToString();

            // Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                var tag = new Tag(
                    Guid.NewGuid(),
                    longName
                );
            });

        }

        [Fact]
        public void Can_Change_Tag_Name()
        {
            // Arrange
            var tag = new Tag(
                Guid.NewGuid(),
                "Old Tag Name"
            );

            // Act
            tag.ChangeName("New Tag Name");

            // Assert
            tag.Name.ShouldBe("New Tag Name");
        }
    }