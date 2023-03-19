using System;
using System.Collections.Generic;
using ImpactSpace.Core.Projects;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

/// <summary>
/// Represents an organization that can own projects and have members with skills.
/// </summary>
public class Organization : AuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// The name of the organization.
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// The description of the organization.
    /// </summary>
    public string Description { get; private set; }
    
    /// <summary>
    /// The list of projects owned by the organization.
    /// </summary>
    public List<Project> Projects { get; private set; } = new();
    
    /// <summary>
    /// The ID of the tenant to which the organization belongs.
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Default constructor used for deserialization and ORM purposes.
    /// </summary>
    private Organization()
    {
        
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Organization"/> class with the specified ID, name, and description.
    /// </summary>
    /// <param name="id">The ID of the organization.</param>
    /// <param name="name">The name of the organization.</param>
    /// <param name="description">The description of the organization.</param>
    internal Organization(Guid id, [NotNull] string name, string description)
        : base(id)
    {
        SetName(name);
        SetDescription(description);
    }

    /// <summary>
    /// Changes the name of the organization.
    /// </summary>
    /// <param name="name">The new name of the organization.</param>
    /// <returns>The updated <see cref="Organization"/> instance.</returns>
    internal Organization ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }

    /// <summary>
    /// Changes the description of the organization.
    /// </summary>
    /// <param name="description">The new description of the organization.</param>
    /// <returns>The updated <see cref="Organization"/> instance.</returns>
    internal Organization ChangeDescription(string description)
    {
        SetDescription(description);
        return this;
    }

    /// <summary>
    /// Sets the name of the organization.
    /// </summary>
    /// <param name="name">The new name of the organization.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the provided name is null or whitespace.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the name exceeds the maximum length allowed.
    /// </exception>
    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: OrganizationConsts.MaxNameLength
        );
    }

    /// <summary>
    /// Sets the description of the organization.
    /// </summary>
    /// <param name="description">The new description of the organization.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the description exceeds the maximum length allowed.
    /// </exception>
    private void SetDescription(string description)
    {
        Description = Check.Length(
            description,
            nameof(description),
            maxLength: OrganizationConsts.MaxDescriptionLength
        );
    }
}
