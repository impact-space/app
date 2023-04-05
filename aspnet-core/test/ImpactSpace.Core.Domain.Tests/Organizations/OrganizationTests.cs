using Shouldly;
using System;
using Xunit;

namespace ImpactSpace.Core.Organizations
{
    public class OrganizationTests
    {
        [Fact]
        public void Create_Organization_Should_Set_Name_And_TenantId()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var name = "Test Organization";
            var tenantId = Guid.NewGuid();

            // Act
            var organization = new Organization(organizationId, name, tenantId);

            // Assert
            organization.Name.ShouldBe(name);
            organization.TenantId.ShouldBe(tenantId);
        }

        [Fact]
        public void Change_Name_Should_Update_Name()
        {
            // Arrange
            var organization = new Organization(Guid.NewGuid(), "Test Organization", Guid.NewGuid());
            var newName = "New Test Organization Name";

            // Act
            organization.ChangeName(newName);

            // Assert
            organization.Name.ShouldBe(newName);
        }

        [Fact]
        public void Change_Description_Should_Update_Description()
        {
            // Arrange
            var organization = new Organization(Guid.NewGuid(), "Test Organization", Guid.NewGuid());
            var newDescription = "New test description.";

            // Act
            organization.ChangeDescription(newDescription);

            // Assert
            organization.Description.ShouldBe(newDescription);
        }

        [Fact]
        public void Set_Name_Should_Throw_Exception_When_Null_Or_Whitespace()
        {
            // Arrange
            var organization = new Organization(Guid.NewGuid(), "Test Organization", Guid.NewGuid());

            // Act & Assert
            
            // ReSharper disable once AssignNullToNotNullAttribute
            Should.Throw<ArgumentException>(() => organization.ChangeName(null));
            Should.Throw<ArgumentException>(() => organization.ChangeName(string.Empty));
            Should.Throw<ArgumentException>(() => organization.ChangeName("   "));
        }

        [Fact]
        public void Set_Name_Should_Throw_Exception_When_Exceed_Max_Length()
        {
            // Arrange
            var organization = new Organization(Guid.NewGuid(), "Test Organization", Guid.NewGuid());
            var tooLongName = new string('a', OrganizationConstants.MaxNameLength + 1);

            // Act & Assert
            Should.Throw<ArgumentException>(() => organization.ChangeName(tooLongName));
        }

        [Fact]
        public void Set_Description_Should_Set_Null_When_Null()
        {
            // Arrange
            var organization = new Organization(Guid.NewGuid(), "Test Organization", Guid.NewGuid());

            // Act
            organization.ChangeDescription(null);

            // Assert
            organization.Description.ShouldBeNull();
        }

        [Fact]
        public void Set_Description_Should_Throw_Exception_When_Exceed_Max_Length()
        {
            // Arrange
            var organization = new Organization(Guid.NewGuid(), "Test Organization", Guid.NewGuid());
            var tooLongDescription = new string('a', OrganizationConstants.MaxDescriptionLength + 1);

            // Act & Assert
            Should.Throw<ArgumentException>(() => organization.ChangeDescription(tooLongDescription));
        }
    }
}