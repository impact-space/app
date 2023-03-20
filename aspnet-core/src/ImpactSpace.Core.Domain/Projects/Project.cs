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
    /// Gets or sets the organization that owns this project.
    /// </summary>
    public virtual Organization Organization { get; private set; }

    /// <summary>
    /// Gets or sets the ID of the tenant that owns this project.
    /// </summary>
    public virtual Guid? TenantId { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// This constructor is for deserialization / ORM purposes
    /// </summary>
    private Project()
    {
    }

    internal Project(Guid id, [NotNull] string name, [CanBeNull] string description, DateTime? startDate,
        DateTime? actualEndDate, [CanBeNull] string purpose, decimal? fundingAllocated, decimal? fundraisingTarget,
        decimal totalBudget, decimal remainingBudget, bool isFeatured, StatusType statusType, ProjectType projectType,
        [CanBeNull] string projectImageUrl,
        int progress, Guid tenantId, Guid? parentProjectId)
        : base(id)
    {
        ChangeName(name);
        ChangeDescription(description);
        ChangeStartDate(startDate);
        ChangeActualEndDate(actualEndDate);
        ChangePurpose(purpose);
        ChangeFundingAllocated(fundingAllocated);
        ChangeFundraisingTarget(fundraisingTarget);
        UpdateTotalBudget(totalBudget);
        UpdateRemainingBudget(remainingBudget);
        ChangeIsFeatured(isFeatured);
        ChangeStatusType(statusType);
        ChangeProjectType(projectType);
        ChangeProjectImageUrl(projectImageUrl);
        ChangeProgress(progress);
        ChangeTenantId(tenantId);

        Milestones = new HashSet<Milestone>();
        TeamMembers = new HashSet<OrganizationMember>();
        RequiredSkills = new HashSet<ProjectSkill>();
    }

    #endregion
    
    #region Methods
    
    /// <summary>
    /// Changes the project name. The name cannot be null or white space and must be at most <see cref="ProjectConsts.MaxNameLength"/> characters long.
    /// </summary>
    /// <param name="name">The name of the project.</param>
    /// <returns>The modified <see cref="Project"/> entity.</returns>
    public virtual Project ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }

    /// <summary>
    /// Changes the project description. The description must be at most <see cref="ProjectConsts.MaxDescriptionLength"/> characters long.
    /// </summary>
    /// <param name="description">The description of the project.</param>
    /// <returns>The modified <see cref="Project"/> entity.</returns>
    public virtual Project ChangeDescription([CanBeNull] string description)
    {
        SetDescription(description);
        return this;
    }

    /// <summary>
    /// Changes the project purpose. The purpose must be at most <see cref="ProjectConsts.MaxPurposeLength"/> characters long.
    /// </summary>
    /// <param name="purpose">The purpose of the project.</param>
    /// <returns>The modified <see cref="Project"/> entity.</returns>
    public virtual Project ChangePurpose([CanBeNull] string purpose)
    {
        SetPurpose(purpose);
        return this;
    }

    /// <summary>
    /// Changes the URL of the project image.
    /// </summary>
    /// <param name="projectImageUrl">The URL of the project image.</param>
    /// <returns>The modified <see cref="Project"/> entity.</returns>
    public virtual Project ChangeProjectImageUrl([CanBeNull] string projectImageUrl)
    {
        SetProjectImageUrl(projectImageUrl);
        return this;
    }

    /// <summary>
    /// Changes the progress of the project. The progress must be between 0 and 100 (inclusive).
    /// </summary>
    /// <param name="progress">The progress of the project.</param>
    /// <returns>The modified <see cref="Project"/> entity.</returns>
    public virtual Project ChangeProgress(int progress)
    {
        SetProgress(progress);
        return this;
    }

    /// <summary>
    /// Changes the start date of the project.
    /// </summary>
    /// <param name="startDate">The start date of the project.</param>
    /// <returns>The modified <see cref="Project"/> entity.</returns>
    public virtual Project ChangeStartDate(DateTime? startDate)
    {
        SetStartDate(startDate);
        return this;
    }

    /// <summary>
    /// Changes the actual end date of the project.
    /// </summary>
    /// <param name="actualEndDate">The actual end date of the project.</param>
    /// <returns>The modified <see cref="Project"/> entity.</returns>
    public virtual Project ChangeActualEndDate(DateTime? actualEndDate)
    {
        SetActualEndDate(actualEndDate);
        return this;
    }

    /// <summary>
    /// Changes the amount of funding allocated to the project.
    /// </summary>
    /// <param name="fundingAllocated">The amount of funding allocated to the project.</param>
    /// <returns>The modified <see cref="Project"/> entity.</returns>
    public virtual Project ChangeFundingAllocated(decimal? fundingAllocated)
    {
        SetFundingAllocated(fundingAllocated);
        return this;
    }

    /// <summary>
    /// Changes the fundraising target for the project.
    /// </summary>
    /// <param name="fundraisingTarget">The new fundraising target for the project.</param>
    /// <returns>The updated project instance.</returns>
    public virtual Project ChangeFundraisingTarget(decimal? fundraisingTarget)
    {
        SetFundraisingTarget(fundraisingTarget);
        return this;
    }

    /// <summary>
    /// Changes whether or not the project is featured.
    /// </summary>
    /// <param name="isFeatured">Whether or not the project is featured.</param>
    /// <returns>The updated project instance.</returns>
    public virtual Project ChangeIsFeatured(bool isFeatured)
    {
        SetIsFeatured(isFeatured);
        return this;
    }

    /// <summary>
    /// Changes the status type of the project.
    /// </summary>
    /// <param name="statusType">The new status type of the project.</param>
    /// <returns>The updated project instance.</returns>
    public virtual Project ChangeStatusType(StatusType statusType)
    {
        SetStatusType(statusType);
        return this;
    }

    /// <summary>
    /// Changes the project type of the project.
    /// </summary>
    /// <param name="projectType">The new project type of the project.</param>
    /// <returns>The updated project instance.</returns>
    public virtual Project ChangeProjectType(ProjectType projectType)
    {
        SetProjectType(projectType);
        return this;
    }

    /// <summary>
    /// Changes the tenant ID of the project.
    /// </summary>
    /// <param name="tenantId">The new tenant ID of the project.</param>
    /// <returns>The updated project instance.</returns>
    public virtual Project ChangeTenantId(Guid tenantId)
    {
        SetTenantId(tenantId);
        return this;
    }

    /// <summary>
    /// Sets the name of the project.
    /// </summary>
    /// <param name="name">The name of the project.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is empty or whitespace.</exception>
    private void SetName([NotNull] string name)
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
    private void SetDescription([CanBeNull] string description)
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
    private void SetPurpose([CanBeNull] string purpose)
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
    private void SetProjectImageUrl([CanBeNull] string projectImageUrl)
    {
        ProjectImageUrl = projectImageUrl;
    }

    /// <summary>
    /// Sets the progress of the project.
    /// </summary>
    /// <param name="progress">The progress of the project.</param>
    private void SetProgress(int progress)
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
    /// Sets the start date of the project.
    /// </summary>
    /// <param name="startDate">The new start date.</param>
    private void SetStartDate(DateTime? startDate)
    {
        StartDate = startDate;
        ValidateStartAndEndDate();
    }

    /// <summary>
    /// Sets the actual end date of the project.
    /// </summary>
    /// <param name="actualEndDate">The new actual end date.</param>
    private void SetActualEndDate(DateTime? actualEndDate)
    {
        ActualEndDate = actualEndDate;
        ValidateStartAndEndDate();
    }

    /// <summary>
    /// Sets the amount of funding allocated to the project.
    /// </summary>
    /// <param name="fundingAllocated">The new amount of funding allocated.</param>
    private void SetFundingAllocated(decimal? fundingAllocated)
    {
        FundingAllocated = fundingAllocated;
    }

    /// <summary>
    /// Sets the target amount of fundraising for the project.
    /// </summary>
    /// <param name="fundraisingTarget">The new target amount of fundraising.</param>
    private void SetFundraisingTarget(decimal? fundraisingTarget)
    {
        FundraisingTarget = fundraisingTarget;
    }

    /// <summary>
    /// Sets whether the project is featured or not.
    /// </summary>
    /// <param name="isFeatured">True if the project is featured, false otherwise.</param>
    private void SetIsFeatured(bool isFeatured)
    {
        IsFeatured = isFeatured;
    }

    /// <summary>
    /// Sets the status type of the project.
    /// </summary>
    /// <param name="statusType">The new status type.</param>
    private void SetStatusType(StatusType statusType)
    {
        StatusType = statusType;
    }

    /// <summary>
    /// Sets the type of the project.
    /// </summary>
    /// <param name="projectType">The new project type.</param>
    private void SetProjectType(ProjectType projectType)
    {
        ProjectType = projectType;
    }

    /// <summary>
    /// Sets the tenant ID of the project.
    /// </summary>
    /// <param name="tenantId">The new tenant ID.</param>
    private void SetTenantId(Guid tenantId)
    {
        TenantId = tenantId;
    }
    
    public virtual TimeSpan CalculateDuration()
    {
        DateTime endDate = ActualEndDate ?? DateTime.UtcNow; // Use the current date if the project is ongoing
        return endDate - StartDate.Value;
    }
    
    public virtual decimal CalculateBudgetUsedPercentage()
    {
        return ((TotalBudget - RemainingBudget) / TotalBudget) * 100;
    }
    
    private void ValidateStartAndEndDate()
    {
        if (StartDate.HasValue && ActualEndDate.HasValue && ActualEndDate.Value < StartDate.Value)
        {
            throw new ArgumentException("ActualEndDate cannot be earlier than StartDate.");
        }
    }

    #endregion
}