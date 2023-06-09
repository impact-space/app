using System;
using System.Collections;
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

    public string Name { get; private set; }

    public string Description { get; private set; }

    public DateTime? StartDate { get; private set; }

    public DateTime? ActualEndDate { get; private set; }

    public string Purpose { get; private set; }

    public decimal? FundingAllocated { get; private set; }

    public decimal? FundraisingTarget { get; private set; }

    public decimal TotalBudget { get; private set; }

    public decimal RemainingBudget { get; private set; }

    public bool IsFeatured { get; private set; }

    public StatusType StatusType { get; private set; }
    
    public Guid ProjectCategoryId { get; private set; }

    public virtual ProjectCategory ProjectCategory { get; private set; }

    public Guid OwnerId { get; private set; }
    
    public virtual OrganizationMember Owner { get; private set; }

    public string ProjectImageUrl { get; private set; }

    public int Progress { get; private set; }

    public virtual ICollection<Milestone> Milestones { get; private set; }

    public virtual ICollection<ProjectSkill> RequiredSkills { get; private set; }
    
    public virtual ICollection<ProjectTag> ProjectTags { get; private set; }
    
    public virtual ICollection<OrganizationMemberProject> OrganizationMemberProjects { get; private set; }
    
    public Guid OrganizationId { get; private set; }
    
    public virtual Organization Organization { get; private set; }

    public Guid? TenantId { get; set; }
    public ICollection<ProjectChallenge> ProjectChallenges { get; private set; }

    #endregion

    #region Constructors
    
    protected Project()
    {
    }

    internal Project(
        Guid id, 
        [NotNull] string name, 
        [CanBeNull] string description, 
        DateTime? startDate,
        DateTime? actualEndDate, 
        [CanBeNull] string purpose, 
        decimal? fundingAllocated, 
        decimal? fundraisingTarget,
        decimal totalBudget, 
        decimal remainingBudget, 
        bool isFeatured, 
        StatusType statusType,
        [CanBeNull] string projectImageUrl,
        int progress)
        : base(id)
    {
        SetName(name);
        SetDescription(description);
        SetStartDate(startDate);
        SetActualEndDate(actualEndDate);
        SetPurpose(purpose);
        SetFundingAllocated(fundingAllocated);
        SetFundraisingTarget(fundraisingTarget);
        UpdateTotalBudget(totalBudget);
        UpdateRemainingBudget(remainingBudget);
        SetIsFeatured(isFeatured);
        SetStatusType(statusType);
        SetProjectImageUrl(projectImageUrl);
        SetProgress(progress);

        Milestones = new Collection<Milestone>();
        OrganizationMemberProjects = new Collection<OrganizationMemberProject>();
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

    private void SetName([NotNull] string name)
    {
        Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            ProjectConstants.MaxNameLength);
        
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
    
    public void UpdateTotalBudget(decimal totalBudget)
    {
        Check.Range(totalBudget, nameof(totalBudget), 0);
        
        TotalBudget = totalBudget;
    }

    public void UpdateRemainingBudget(decimal remainingBudget)
    {
        Check.Range(remainingBudget, nameof(remainingBudget), 0);
        
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
        if (fundingAllocated != null)
        {
            Check.Range(fundingAllocated.Value, nameof(fundingAllocated), 0);
        }
        FundingAllocated = fundingAllocated;
    }

    private void SetFundraisingTarget(decimal? fundraisingTarget)
    {
        if (fundraisingTarget != null)
        {
            Check.Range(fundraisingTarget.Value, nameof(fundraisingTarget), 0);
        }
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

    private void ValidateStartAndEndDate()
    {
        if (StartDate.HasValue && ActualEndDate.HasValue && ActualEndDate.Value < StartDate.Value)
        {
            throw new ArgumentException("ActualEndDate cannot be earlier than StartDate.");
        }
    }

    #endregion
}