using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public virtual string Name { get; private set; }

    public virtual string Description { get; private set; }

    public virtual DateTime? StartDate { get; private set; }

    public virtual DateTime? ActualEndDate { get; private set; }

    public virtual string Purpose { get; private set; }

    public virtual decimal? FundingAllocated { get; private set; }

    public virtual decimal? FundraisingTarget { get; private set; }

    public virtual decimal TotalBudget { get; private set; }

    public virtual decimal RemainingBudget { get; private set; }

    public virtual bool IsFeatured { get; private set; }

    public virtual StatusType StatusType { get; private set; }
    
    public Guid ProjectCategoryId { get; private set; }

    public virtual ProjectCategory ProjectCategory { get; private set; }

    public Guid ProjectOwnerId { get; private set; }
    
    public virtual OrganizationMember ProjectOwner { get; private set; }

    public virtual ProjectType ProjectType { get; private set; }

    public virtual string ProjectImageUrl { get; private set; }

    public virtual int Progress { get; private set; }

    public virtual ICollection<Milestone> Milestones { get; private set; }

    public virtual ICollection<OrganizationMember> TeamMembers { get; private set; }

    public virtual ICollection<ProjectSkill> RequiredSkills { get; private set; }
    
    public virtual ICollection<ProjectTag> ProjectTags { get; private set; }
    
    public Guid OrganizationId { get; private set; }
    
    public virtual Organization Organization { get; private set; }

    public virtual Guid? TenantId { get; private set; }
    public ICollection<ProjectChallenge> ProjectChallenges { get; private set; }

    #endregion

    #region Constructors
    
    protected Project()
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

        Milestones = new Collection<Milestone>();
        TeamMembers = new Collection<OrganizationMember>();
        RequiredSkills = new Collection<ProjectSkill>();
        ProjectTags = new Collection<ProjectTag>();
        ProjectChallenges = new Collection<ProjectChallenge>();
    }

    #endregion
    
    #region Methods
    
   public virtual Project ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }
    public virtual Project ChangeDescription([CanBeNull] string description)
    {
        SetDescription(description);
        return this;
    }

 
    public virtual Project ChangePurpose([CanBeNull] string purpose)
    {
        SetPurpose(purpose);
        return this;
    }
    
    public virtual Project ChangeProjectImageUrl([CanBeNull] string projectImageUrl)
    {
        SetProjectImageUrl(projectImageUrl);
        return this;
    }

    public virtual Project ChangeProgress(int progress)
    {
        SetProgress(progress);
        return this;
    }

    public virtual Project ChangeStartDate(DateTime? startDate)
    {
        SetStartDate(startDate);
        return this;
    }

    public virtual Project ChangeActualEndDate(DateTime? actualEndDate)
    {
        SetActualEndDate(actualEndDate);
        return this;
    }

    public virtual Project ChangeFundingAllocated(decimal? fundingAllocated)
    {
        SetFundingAllocated(fundingAllocated);
        return this;
    }

    public virtual Project ChangeFundraisingTarget(decimal? fundraisingTarget)
    {
        SetFundraisingTarget(fundraisingTarget);
        return this;
    }

    public virtual Project ChangeIsFeatured(bool isFeatured)
    {
        SetIsFeatured(isFeatured);
        return this;
    }

    public virtual Project ChangeStatusType(StatusType statusType)
    {
        SetStatusType(statusType);
        return this;
    }

    public virtual Project ChangeProjectType(ProjectType projectType)
    {
        SetProjectType(projectType);
        return this;
    }

    public virtual Project ChangeTenantId(Guid tenantId)
    {
        SetTenantId(tenantId);
        return this;
    }

    private void SetName([NotNull] string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > ProjectConstants.MaxNameLength)
        {
            throw new BusinessException(
                code: "Project.Name.Invalid",
                message: $"The name must be between 1 and {ProjectConstants.MaxNameLength} characters long."
            );
        }

        Name = name;
    }

    private void SetDescription([CanBeNull] string description)
    {
        Description = Check.Length(
            description,
            nameof(description),
            maxLength: ProjectConstants.MaxDescriptionLength
        );
    }

    private void SetPurpose([CanBeNull] string purpose)
    {
        Purpose = Check.Length(
            purpose,
            nameof(purpose),
            maxLength: ProjectConstants.MaxPurposeLength
        );
    }

    private void SetProjectImageUrl([CanBeNull] string projectImageUrl)
    {
        ProjectImageUrl = projectImageUrl;
    }

    private void SetProgress(int progress)
    {
        Progress = Check.Range(
            progress,
            nameof(progress),
            0, // minimum percent
            100 // maximum percent
        );
    }

    public virtual void AddMilestone(Milestone milestone)
    {
        Check.NotNull(milestone, nameof(milestone));
        Milestones.Add(milestone);
    }

    public virtual void RemoveMilestone(Milestone milestone)
    {
        Check.NotNull(milestone, nameof(milestone));
        Milestones.Remove(milestone);
    }

    public virtual void AddTeamMember(OrganizationMember teamMember)
    {
        Check.NotNull(teamMember, nameof(teamMember));
        TeamMembers.Add(teamMember);
    }

    public virtual void RemoveTeamMember(OrganizationMember teamMember)
    {
        Check.NotNull(teamMember, nameof(teamMember));
        TeamMembers.Remove(teamMember);
    }

    public virtual void AddRequiredSkill(ProjectSkill requiredSkill)
    {
        Check.NotNull(requiredSkill, nameof(requiredSkill));
        RequiredSkills.Add(requiredSkill);
    }

    public virtual void RemoveRequiredSkill(ProjectSkill requiredSkill)
    {
        Check.NotNull(requiredSkill, nameof(requiredSkill));
        RequiredSkills.Remove(requiredSkill);
    }
    
    public virtual void UpdateTotalBudget(decimal totalBudget)
    {
        if (totalBudget < 0)
        {
            throw new ArgumentException("TotalBudget cannot be negative.", nameof(totalBudget));
        }

        TotalBudget = totalBudget;
    }

    public virtual void UpdateRemainingBudget(decimal remainingBudget)
    {
        if (remainingBudget < 0)
        {
            throw new ArgumentException("RemainingBudget cannot be negative.", nameof(remainingBudget));
        }

        RemainingBudget = remainingBudget;
    }

    private void SetStartDate(DateTime? startDate)
    {
        StartDate = startDate;
        ValidateStartAndEndDate();
    }

    private void SetActualEndDate(DateTime? actualEndDate)
    {
        ActualEndDate = actualEndDate;
        ValidateStartAndEndDate();
    }

    private void SetFundingAllocated(decimal? fundingAllocated)
    {
        FundingAllocated = fundingAllocated;
    }

    private void SetFundraisingTarget(decimal? fundraisingTarget)
    {
        FundraisingTarget = fundraisingTarget;
    }

    private void SetIsFeatured(bool isFeatured)
    {
        IsFeatured = isFeatured;
    }

    private void SetStatusType(StatusType statusType)
    {
        StatusType = statusType;
    }

    private void SetProjectType(ProjectType projectType)
    {
        ProjectType = projectType;
    }

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