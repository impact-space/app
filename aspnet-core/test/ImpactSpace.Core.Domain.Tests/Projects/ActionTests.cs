using System;
using ImpactSpace.Core.Common;
using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Projects;

public class ActionTests
{
    [Fact]
    public void Should_Set_Name()
    {
        // Arrange
        var action = new Action(Guid.NewGuid(), "My Action", null, StatusType.Draft, null,
            PriorityLevel.Low, Guid.NewGuid(), 1000, 10);

        // Act
        action.SetName("New Name");

        // Assert
        action.Name.ShouldBe("New Name");
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Null()
    {
        // Arrange
        var action = new Action(Guid.NewGuid(), "My Action", null, StatusType.Draft, null,
            PriorityLevel.Low, Guid.NewGuid(), 1000, 10);

        // Act & Assert
        Should.Throw<ArgumentException>(() =>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            action.SetName(null);
        });
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Too_Long()
    {
        // Arrange
        var action = new Action(Guid.NewGuid(), "My Action", null, StatusType.Draft, null,
            PriorityLevel.Low, Guid.NewGuid(), 1000, 10);

        // Act & Assert
        Should.Throw<ArgumentException>(() => { action.SetName(new string('x', ActionConstants.MaxNameLength + 1)); });
    }

    [Fact]
    public void Should_Set_Description()
    {
        // Arrange
        var action = new Action(Guid.NewGuid(), "My Action", null, StatusType.Draft, null,
            PriorityLevel.Low, Guid.NewGuid(), 1000, 10);

        // Act
        action.SetDescription("New Description");

        // Assert
        action.Description.ShouldBe("New Description");
    }

    [Fact]
    public void Should_Set_Description_To_Null()
    {
        // Arrange
        var action = new Action(Guid.NewGuid(), "My Action", "Old Description", StatusType.Draft, null,
            PriorityLevel.Low, Guid.NewGuid(), 1000, 10);

        // Act
        action.SetDescription(null);

        // Assert
        action.Description.ShouldBeNull();
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Is_Too_Long()
    {
        // Arrange
        var action = new Action(Guid.NewGuid(), "My Action", null, StatusType.Draft, null,
            PriorityLevel.Low, Guid.NewGuid(), 1000, 10);

        // Act & Assert
        Should.Throw<ArgumentException>(() =>
        {
            action.SetDescription(new string('x', ActionConstants.MaxDescriptionLength + 1));
        });
    }
}