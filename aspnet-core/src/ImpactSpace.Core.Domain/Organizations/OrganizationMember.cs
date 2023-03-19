using System;
using System.Collections.Generic;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Projects;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

/// <summary>
/// Represents an organization member within the Impact Space app.
/// </summary>
public class OrganizationMember : AuditedAggregateRoot<Guid>, IMultiTenant
{
    
    /// <summary>
    /// Gets or sets the name of the organization member.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets or sets the email of the organization member.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Gets or sets the phone number of the organization member.
    /// </summary>
    public string Phone { get; private set; }

    /// <summary>
    /// Gets or sets the list of project actions associated with the organization member.
    /// </summary>
    public List<ProjectAction> ProjectActions { get; private set; } = new();

    /// <summary>
    /// Gets or sets the list of projects associated with the organization member.
    /// </summary>
    public List<Project> Projects { get; private set; } = new();

    /// <summary>
    /// Gets or sets the Guid of the user account associated with the organization member.
    /// </summary>
    public Guid? UserId { get; private set; }
    
    /// <summary>
    /// Gets or sets the list of skills associated with the organization member.
    /// </summary>
    public List<OrganizationMemberSkill> Skills { get; private set; } = new();

    /// <summary>
    /// Gets or sets the Guid of the organization associated with the organization member.
    /// </summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>
    /// Gets or sets the organization associated with the organization member.
    /// </summary>
    public Organization Organization { get; private set; }

    /// <summary>
    /// Gets or sets the Guid of the tenant associated with the organization member.
    /// </summary>
    public Guid? TenantId { get; private set; }

    /// <summary>
    /// Creates a new instance of the <see cref="OrganizationMember"/> class.
    /// </summary>
    private OrganizationMember()
    {
        // This constructor is for deserialization / ORM purposes
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrganizationMember"/> class with the specified parameters.
    /// </summary>
    /// <param name="id">The Guid ID of the organization member.</param>
    /// <param name="name">The name of the organization member.</param>
    /// <param name="email">The email of the organization member.</param>
    /// <param name="phone">The phone number of the organization member.</param>
    /// <param name="organizationId">The Guid ID of the organization associated with the organization member.</param>
    /// <param name="tenantId">The Guid ID of the tenant associated with the organization member.</param>
    internal OrganizationMember(Guid id, [NotNull] string name, [NotNull] string email, string phone,
        Guid organizationId, Guid? tenantId)
        : base(id)
    {
        SetName(name);
        SetEmail(email);
        SetPhone(phone);
        OrganizationId = organizationId;
        TenantId = tenantId;
    }


    /// <summary>
    /// Changes the name of the organization member.
    /// </summary>
    /// <param name="name">The new name for the organization member.</param>
    /// <returns>The updated organization member.</returns>
    internal OrganizationMember ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }

    /// <summary>
    /// Changes the email address of the organization member.
    /// </summary>
    /// <param name="email">The new email address for the organization member.</param>
    /// <returns>The updated organization member.</returns>
    internal OrganizationMember ChangeEmail([NotNull] string email)
    {
        SetEmail(email);
        return this;
    }

    /// <summary>
    /// Changes the phone number of the organization member.
    /// </summary>
    /// <param name="phone">The new phone number for the organization member.</param>
    /// <returns>The updated organization member.</returns>
    internal OrganizationMember ChangePhone(string phone)
    {
        SetPhone(phone);
        return this;
    }

    /// <summary>
    /// Changes the organization that the member belongs to.
    /// </summary>
    /// <param name="organizationId">The ID of the new organization.</param>
    /// <returns>The updated organization member.</returns>
    internal OrganizationMember ChangeOrganization(Guid organizationId)
    {
        OrganizationId = organizationId;
        return this;
    }

    /// <summary>
    /// Adds a project action to the list of project actions associated with the organization member.
    /// </summary>
    /// <param name="projectAction">The project action to be added.</param>
    internal void AddProjectAction(ProjectAction projectAction)
    {
        ProjectActions.Add(Check.NotNull(projectAction, nameof(projectAction)));
    }

    /// <summary>
    /// Removes a project action from the list of project actions associated with the organization member.
    /// </summary>
    /// <param name="projectAction">The project action to be removed.</param>
    internal void RemoveProjectAction(ProjectAction projectAction)
    {
        ProjectActions.Remove(Check.NotNull(projectAction, nameof(projectAction)));
    }

    /// <summary>
    /// Adds a project to the list of projects associated with the organization member.
    /// </summary>
    /// <param name="project">The project to be added.</param>
    internal void AddProject(Project project)
    {
        Projects.Add(Check.NotNull(project, nameof(project)));
    }

    /// <summary>
    /// Removes a project from the list of projects associated with the organization member.
    /// </summary>
    /// <param name="project">The project to be removed.</param>
    internal void RemoveProject(Project project)
    {
        Projects.Remove(Check.NotNull(project, nameof(project)));
    }

    /// <summary>
    /// Adds a skill to the list of skills associated with the organization member.
    /// </summary>
    /// <param name="skill">The skill to be added.</param>
    internal void AddSkill(OrganizationMemberSkill skill)
    {
        Skills.Add(Check.NotNull(skill, nameof(skill)));
    }

    /// <summary>
    /// Removes a skill from the list of skills associated with the organization member.
    /// </summary>
    /// <param name="skill">The skill to be removed.</param>
    internal void RemoveSkill(OrganizationMemberSkill skill)
    {
        Skills.Remove(Check.NotNull(skill, nameof(skill)));
    }

    /// <summary>
    /// Sets the Name property for the organization member.
    /// </summary>
    /// <param name="name">The name of the organization member.</param>
    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: OrganizationMemberConsts.MaxNameLength
        );
    }

    /// <summary>
    /// Sets the Email property for the organization member.
    /// </summary>
    /// <param name="email">The email of the organization member.</param>
    private void SetEmail([NotNull] string email)
    {
        Email = Check.NotNullOrWhiteSpace(
            email,
            nameof(email),
            maxLength: CommonConsts.MaxEmailLength
        );
    }

    /// <summary>
    /// Sets the Phone property for the organization member.
    /// </summary>
    /// <param name="phone">The phone number of the organization member.</param>
    private void SetPhone(string phone)
    {
        Phone = Check.Length(
            phone,
            nameof(phone),
            maxLength: CommonConsts.MaxPhoneLength
        );
    }

    /// <summary>
    /// Sets the UserId property for the organization member.
    /// </summary>
    /// <param name="userId">The Guid of the user account associated with the organization member.</param>
    /// <returns>The updated organization member.</returns>
    private OrganizationMember SetUserId(Guid? userId)
    {
        UserId = userId;
        return this;
    }
}