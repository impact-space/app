using System;
using System.Collections.Generic;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Organizations;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects;

/// <summary>
/// Represents a project within the Impact Space app.
/// </summary>
public class Project : AuditedAggregateRoot<Guid>, IMultiTenant
{
    #region Properties
    
    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public virtual string Name { get; private set; }
    
    /// <summary>
    /// Gets or sets the description of the project.
    /// </summary>
    public virtual string Description { get; private set; }

    /// <summary>
    /// Gets or sets the start date of the project.
    /// </summary>
    public virtual DateTime? StartDate { get; private set; }

    /// <summary>
    /// Gets or sets the actual end date of the project.
    /// </summary>
    public virtual DateTime? ActualEndDate { get; private set; }

    /// <summary>
    /// Gets or sets the purpose of the project.
    /// </summary>
    public virtual string Purpose { get; private set; }

    /// <summary>
    /// Gets or sets the amount of funding allocated to the project.
    /// </summary>
    public virtual decimal? FundingAllocated { get; private set; }
    
    /// <summary>
    /// Gets or sets the target fundraising amount of the project.
    /// </summary>
    public virtual decimal? FundraisingTarget { get; private set; }

    /// <summary>
    /// Gets or sets the total budget of the project.
    /// </summary>
    public virtual decimal TotalBudget { get; private set; }

    /// <summary>
    /// Gets or sets the remaining budget of the project.
    /// </summary>
    public virtual decimal RemainingBudget { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the project is featured.
    /// </summary>
    public virtual bool IsFeatured { get; private set; }

    /// <summary>
    /// Gets or sets the status of the project.
    /// </summary>
    public virtual StatusType StatusType { get; private set; }

    /// <summary>
    /// Gets or sets the project category of the project.
    /// </summary>
    public virtual ProjectCategory ProjectCategory { get; private set; }

    /// <summary>
    /// Gets or sets the project owner of the project.
    /// </summary>
    public virtual OrganizationMember ProjectOwner { get; private set; }

    /// <summary>
    /// Gets or sets the type of the project.
    /// </summary>
    public virtual ProjectType ProjectType { get; private set; }

    /// <summary>
    /// Gets or sets the URL of the project image.
    /// </summary>
    public virtual string ProjectImageUrl { get; private set; }

    /// <summary>
    /// Gets or sets the progress of the project.
    /// </summary>
    public virtual int Progress { get; private set; }

    /// <summary>
    /// Gets or sets the list of milestones of the project.
    /// </summary>
    public virtual ICollection<Milestone> Milestones { get; private set; }

    /// <summary>
    /// Gets or sets the list of team members of the project.
    /// </summary>
    public virtual ICollection<OrganizationMember> TeamMembers { get; private set; }
    
    /// <summary>
    /// The required skills for the project.
    /// </summary>
    public virtual ICollection<ProjectSkill> RequiredSkills { get; private set; }
    
    /// <summary>
    /// Gets or sets the ID of the tenant that owns this project.
    /// </summary>
    public virtual Guid? TenantId { get; private set; }

    /// <summary>
    /// Gets or sets the ID of the parent project, if this is a sub-project.
    /// </summary>
    public virtual Guid? ParentProjectId { get; private set; }

    /// <summary>
    /// Gets or sets the parent project, if this is a sub-project.
    /// </summary>
    public virtual Project ParentProject { get; private set; }

    /// <summary>
    /// Gets or sets the organization that owns this project.
    /// </summary>
    public virtual Organization Organization { get; private set; }

    /// <summary>
    /// Gets or sets the list of sub-projects under this project.
    /// </summary>
    public virtual ICollection<Project> SubProjects { get; private set; }
    
    #endregion

    #region Constructors

    private Project()
    {
    }

    internal Project(Guid id, [NotNull] string name, [CanBeNull] string description, DateTime? startDate,
        DateTime? actualEndDate, [CanBeNull] string purpose, decimal? fundingAllocated, decimal? fundraisingTarget,
        decimal totalBudget, decimal remainingBudget, bool isFeatured, StatusType statusType, ProjectType projectType, [CanBeNull] string projectImageUrl,
        int progress, Guid tenantId, Guid? parentProjectId)
        : base(id)
    {
        SetName(name);
        SetDescription(description);
        StartDate = startDate;
        ActualEndDate = actualEndDate;
        SetPurpose(purpose);
        FundingAllocated = fundingAllocated;
        FundraisingTarget = fundraisingTarget;
        TotalBudget = totalBudget;
        RemainingBudget = remainingBudget;
        IsFeatured = isFeatured;
        StatusType = statusType;
        ProjectType = projectType;
        SetProjectImageUrl(projectImageUrl);
        SetProgress(progress);
        TenantId = tenantId;
        ParentProjectId = parentProjectId;
        
        Milestones = new HashSet<Milestone>();
        TeamMembers = new HashSet<OrganizationMember>();
        RequiredSkills = new HashSet<ProjectSkill>();
        SubProjects = new HashSet<Project>();
    }

    #endregion
    
    /// <summary>
    /// Sets the name of the project.
    /// </summary>
    /// <param name="name">The name of the project.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is empty or whitespace.</exception>
    public virtual void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: ProjectConsts.MaxNameLength
        );
    }
    
    /// <summary>
    /// Sets the description of the project.
    /// </summary>
    /// <param name="description">The description of the project.</param>
    public virtual void SetDescription([CanBeNull] string description)
    {
        Description = Check.Length(
            description,
            nameof(description),
            maxLength: ProjectConsts.MaxDescriptionLength
        );
    }

    /// <summary>
    /// Sets the purpose of the project.
    /// </summary>
    /// <param name="purpose">The purpose of the project.</param>
    public virtual void SetPurpose([CanBeNull] string purpose)
    {
        Purpose = Check.Length(
            purpose,
            nameof(purpose),
            maxLength: ProjectConsts.MaxPurposeLength
        );
    }

    /// <summary>
    /// Sets the project image URL.
    /// </summary>
    /// <param name="projectImageUrl">The project image URL to be set.</param>
    public virtual void SetProjectImageUrl([CanBeNull] string projectImageUrl)
    {
        ProjectImageUrl = projectImageUrl;
    }

    /// <summary>
    /// Sets the progress of the project.
    /// </summary>
    /// <param name="progress">The progress of the project.</param>
    public virtual void SetProgress(int progress)
    {
        Progress = Check.Range(
            progress,
            nameof(progress),
            0, // minimum percent
            100 // maximum percent
        );
    }

    /// <summary>
    /// Adds a milestone to the project.
    /// </summary>
    /// <param name="milestone">The milestone to be added.</param>
    public virtual void AddMilestone(Milestone milestone)
    {
        Check.NotNull(milestone, nameof(milestone));
        Milestones.Add(milestone);
    }

    /// <summary>
    /// Removes a milestone from the project.
    /// </summary>
    /// <param name="milestone">The milestone to be removed.</param>
    public virtual void RemoveMilestone(Milestone milestone)
    {
        Check.NotNull(milestone, nameof(milestone));
        Milestones.Remove(milestone);
    }

    /// <summary>
    /// Adds a team member to the project.
    /// </summary>
    /// <param name="teamMember">The team member to be added.</param>
    public virtual void AddTeamMember(OrganizationMember teamMember)
    {
        Check.NotNull(teamMember, nameof(teamMember));
        TeamMembers.Add(teamMember);
    }

    /// <summary>
    /// Removes a team member from the project.
    /// </summary>
    /// <param name="teamMember">The team member to be removed.</param>
    public virtual void RemoveTeamMember(OrganizationMember teamMember)
    {
        Check.NotNull(teamMember, nameof(teamMember));
        TeamMembers.Remove(teamMember);
    }

    /// <summary>
    /// Adds a required skill to the project.
    /// </summary>
    /// <param name="requiredSkill">The required skill to be added.</param>
    public virtual void AddRequiredSkill(ProjectSkill requiredSkill)
    {
        Check.NotNull(requiredSkill, nameof(requiredSkill));
        RequiredSkills.Add(requiredSkill);
    }

    /// <summary>
    /// Removes a required skill from the project.
    /// </summary>
    /// <param name="requiredSkill">The required skill to be removed.</param>
    public virtual void RemoveRequiredSkill(ProjectSkill requiredSkill)
    {
        Check.NotNull(requiredSkill, nameof(requiredSkill));
        RequiredSkills.Remove(requiredSkill);
    }

    /// <summary>
    /// Adds a sub-project to the project.
    /// </summary>
    /// <param name="subProject">The sub-project to be added.</param>
    public virtual void AddSubProject(Project subProject)
    {
        Check.NotNull(subProject, nameof(subProject));
        SubProjects.Add(subProject);
        subProject.SetParentProject(this);
    }

    /// <summary>
    /// Removes a sub-project from the project.
    /// </summary>
    /// <param name="subProject">The sub-project to be removed.</param>
    public virtual void RemoveSubProject(Project subProject)
    {
        Check.NotNull(subProject, nameof(subProject));
        SubProjects.Remove(subProject);
        subProject.SetParentProject(null);
    }
    
    /// <summary>
    /// Updates the total budget of the project.
    /// </summary>
    /// <param name="totalBudget">The new total budget for the project.</param>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="totalBudget"/> is negative.</exception>
    public virtual void UpdateTotalBudget(decimal totalBudget)
    {
        if (totalBudget < 0)
        {
            throw new ArgumentException("TotalBudget cannot be negative.", nameof(totalBudget));
        }

        TotalBudget = totalBudget;
    }

    /// <summary>
    /// Updates the remaining budget of the project.
    /// </summary>
    /// <param name="remainingBudget">The new remaining budget for the project.</param>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="remainingBudget"/> is negative.</exception>
    public virtual void UpdateRemainingBudget(decimal remainingBudget)
    {
        if (remainingBudget < 0)
        {
            throw new ArgumentException("RemainingBudget cannot be negative.", nameof(remainingBudget));
        }

        RemainingBudget = remainingBudget;
    }
    
    /// <summary>
    /// Sets the parent project of this project.
    /// </summary>
    /// <param name="parentProject">The parent project to be set. Pass null to unset the parent project.</param>
    public virtual void SetParentProject(Project parentProject)
    {
        ParentProject = parentProject;
        ParentProjectId = parentProject?.Id;
    }
}