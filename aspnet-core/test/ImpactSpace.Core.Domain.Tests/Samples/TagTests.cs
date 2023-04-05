using System;
using System.Linq;
using ImpactSpace.Core.Projects;
using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Samples;

public class TagTests : CoreDomainTestBase
{
    [Fact]
    public void Should_Create_Tag()
    {
        // Arrange
        var tagName = "Test Tag";
        var tagId = Guid.NewGuid();

        // Act
        var tag = new Tag(tagId, tagName);

        // Assert
        tag.Id.ShouldBe(tagId);
        tag.Name.ShouldBe(tagName);
        tag.ProjectTags.ShouldNotBeNull();
        tag.ProjectTags.ShouldBeEmpty();
    }

    [Fact]
    public void Should_Change_Tag_Name()
    {
        // Arrange
        var tag = new Tag(Guid.NewGuid(), "Test Tag");
        var newName = "New Tag Name";

        // Act
        tag.ChangeName(newName);

        // Assert
        tag.Name.ShouldBe(newName);
    }

    [Fact]
    public void Should_Not_Allow_Empty_Tag_Name()
    {
        // Arrange
        var tag = new Tag(Guid.NewGuid(), "Test Tag");
        var emptyName = "";

        // Act & Assert
        Should.Throw<ArgumentException>(() => tag.ChangeName(emptyName))
            .Message.ShouldBe("name can not be null, empty or white space! (Parameter 'name')");
    }

    [Fact]
    public void Should_Not_Allow_Tag_Name_Over_Max_Length()
    {
        // Arrange
        var tag = new Tag(Guid.NewGuid(), "Test Tag");
        var longName = string.Join("", Enumerable.Repeat("a", TagConstants.MaxNameLength + 1));

        // Act & Assert
        Should.Throw<ArgumentException>(() => tag.ChangeName(longName))
            .Message.ShouldBe($"name length must be equal to or lower than {TagConstants.MaxNameLength}! (Parameter 'name')");
    }
}