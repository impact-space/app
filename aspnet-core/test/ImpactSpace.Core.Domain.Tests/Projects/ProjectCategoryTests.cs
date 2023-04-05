using System;
using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Projects;

public class ProjectCategoryTests
{
    [Fact]
    public void Should_Create_ProjectCategory()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Project Category";

        // Act
        var projectCategory = new ProjectCategory(id, name);

        // Assert
        projectCategory.Id.ShouldBe(id);
        projectCategory.Name.ShouldBe(name);
        projectCategory.Projects.ShouldNotBeNull();
        projectCategory.Projects.ShouldBeEmpty();
    }

    [Fact]
    public void Should_Change_ProjectCategory_Name()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Project Category";
        var new_name = "New Test Project Category";
        var projectCategory = new ProjectCategory(id, name);

        // Act
        projectCategory.ChangeName(new_name);

        // Assert
        projectCategory.Name.ShouldBe(new_name);
    }

    [Fact]
    public void Should_Not_Create_ProjectCategory_With_Empty_Name()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "";

        // Act and Assert
        Assert.Throws<ArgumentException>(() => new ProjectCategory(id, name));
    }

    [Fact]
    public void Should_Not_Create_ProjectCategory_With_Long_Name()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = new string('A', ProjectCategoryConsts.MaxNameLength + 1);

        // Act and Assert
        Assert.Throws<ArgumentException>(() => new ProjectCategory(id, name));
    }
}