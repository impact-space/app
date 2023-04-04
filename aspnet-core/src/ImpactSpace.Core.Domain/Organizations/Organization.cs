using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ImpactSpace.Core.Projects;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

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
    public virtual ICollection<Project> Projects { get; private set; }
    
    /// <summary>
    /// A list of the members of the organization
    /// </summary>
    public virtual ICollection<OrganizationMember> OrganizationMembers { get; private set; }
    
    public virtual OrganizationProfile OrganizationProfile { get; private set; }

    /// <summary>
    /// Gets or sets the ID of the tenant to which the organization belongs.
    /// </summary>
    public Guid? TenantId { get; set; }
    
    public virtual Tenant Tenant { get; set; }


    /// <summary>
    /// Default constructor used for deserialization and ORM purposes.
    /// </summary>
    protected Organization()
    {
        
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Organization"/> class with the specified ID, name, and description.
    /// </summary>
    /// <param name="id">The ID of the organization.</param>
    /// <param name="name">The name of the organization.</param>
    /// <param name="tenantId">The tenant of the organization</param>
    /// <param name="description">The description of the organization.</param>
    internal Organization(
        Guid id, 
        [NotNull] string name, 
        Guid tenantId,
        [CanBeNull] string description = null)
        : base(id)
    {
        SetName(name);
        SetDescription(description);
        TenantId = tenantId;
        Projects = new Collection<Project>();
        OrganizationMembers = new Collection<OrganizationMember>();
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
    internal Organization ChangeDescription([CanBeNull] string description)
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
            maxLength: OrganizationConstants.MaxNameLength
        );
    }

    /// <summary>
    /// Sets the description of the organization.
    /// </summary>
    /// <param name="description">The new description of the organization.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the description exceeds the maximum length allowed.
    /// </exception>
    private void SetDescription([CanBeNull] string description)
    {
        Description = Check.Length(
            description,
            nameof(description),
            maxLength: OrganizationConstants.MaxDescriptionLength
        );
    }
    
    /// <summary>
    /// Adds a project to the list of projects owned by the organization.
    /// </summary>
    /// <param name="project">The project to be added.</param>
    internal void AddProject(Project project)
    {
        Check.NotNull(project, nameof(project));
        
        // check if a project with the same Id already exists
        if (Projects.Any(p => p.Id == project.Id))
        {
            throw new ArgumentException($"A project with the ID {project.Id} already exists.");
        }
    }

    /// <summary>
    /// Removes a project from the list of projects owned by the organization.
    /// </summary>
    /// <param name="project">The project to be removed.</param>
    internal void RemoveProject(Project project)
    {
        Projects.Remove(Check.NotNull(project, nameof(project)));
    }

    /// <summary>
    /// Adds an organization member to the list of members of the organization.
    /// </summary>
    /// <param name="organizationMember">The organization member to be added.</param>
    internal void AddOrganizationMember(OrganizationMember organizationMember)
    {
        Check.NotNull(organizationMember, nameof(organizationMember));
        
        // check if a member with the same Id already exists
        if (OrganizationMembers.Any(m => m.Id == organizationMember.Id))
        {
            throw new ArgumentException($"An organization member with the ID {organizationMember.Id} already exists.");
        }
        
        OrganizationMembers.Add(organizationMember);
    }

    /// <summary>
    /// Removes an organization member from the list of members of the organization.
    /// </summary>
    /// <param name="organizationMember">The organization member to be removed.</param>
    internal void RemoveOrganizationMember(OrganizationMember organizationMember)
    {
        OrganizationMembers.Remove(Check.NotNull(organizationMember, nameof(organizationMember)));
    }


}
