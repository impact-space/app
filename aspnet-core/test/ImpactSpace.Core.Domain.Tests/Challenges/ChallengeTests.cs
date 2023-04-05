using System;
using System.Collections.ObjectModel;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Projects;
using Xunit;

namespace ImpactSpace.Core.Challenges;

public class ChallengeTests : CoreDomainTestBase
{
    [Fact]
    public void Should_Create_Challenge()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Challenge";

        // Act
        var challenge = new Challenge(id, name);

        // Assert
        Assert.Equal(id, challenge.Id);
        Assert.Equal(name, challenge.Name);
        Assert.NotNull(challenge.ProjectChallenges);
        Assert.IsType<Collection<ProjectChallenge>>(challenge.ProjectChallenges);
        Assert.NotNull(challenge.OrganizationMemberChallenges);
        Assert.IsType<Collection<OrganizationMemberChallenge>>(challenge.OrganizationMemberChallenges);
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Null()
    {
        // Arrange
        var id = Guid.NewGuid();
        string name = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Challenge(id, name));
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Too_Long()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = new string('A', ChallengeConstants.MaxNameLength + 1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Challenge(id, name));
    }

    [Fact]
    public void Should_Change_Challenge_Name()
    {
        // Arrange
        var id = Guid.NewGuid();
        var oldName = "Old Name";
        var newName = "New Name";
        var challenge = new Challenge(id, oldName);

        // Act
        challenge.ChangeName(newName);

        // Assert
        Assert.Equal(newName, challenge.Name);
    }
}