using System;
using System.Collections.Generic;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Skills;
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
    // Properties
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
    
    public ProjectCategory ProjectCategory { get; private set; }
    
    public Guid ProjectOwnerId { get; private set; }
    
    public OrganizationMember ProjectOwner { get; private set; }
    
    public ProjectType ProjectType { get; private set; }
    
    public string ProjectImageUrl { get; private set; }
    
    public int Progress { get; private set; }
    
    public List<Milestone> Milestones { get; private set; } = new();
    
    public List<OrganizationMember> TeamMembers { get; private set; } = new();
    
    public List<ProjectSkill> RequiredSkills { get; private set; } = new();
    
    public Guid? TenantId { get; private set; }
    
    public Guid? ParentProjectId { get; private set; }
    
    public Project ParentProject { get; private set; }
    
    public int OrganizationId { get; private set; }
    
    public Organization Organization { get; private set; }
    
    public List<Project> SubProjects { get; private set; } = new();
    
    private Project()
    {
    }

    public Project(Guid id, [NotNull] string name, [CanBeNull] string description, DateTime? startDate,
        DateTime? actualEndDate, [CanBeNull] string purpose, decimal? fundingAllocated, decimal? fundraisingTarget,
        decimal totalBudget, decimal remainingBudget, bool isFeatured, StatusType statusType,
        Guid projectCategoryId, Guid projectOwnerId, ProjectType projectType, [CanBeNull] string projectImageUrl,
        int progress, Guid tenantId, Guid? parentProjectId, int organizationId)
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
        ProjectCategoryId = projectCategoryId;
        ProjectOwnerId = projectOwnerId;
        ProjectType = projectType;
        SetProjectImageUrl(projectImageUrl);
        SetProgress(progress);
        TenantId = tenantId;
        ParentProjectId = parentProjectId;
        OrganizationId = organizationId;
    }
    
    public void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: ProjectConsts.MaxNameLength
        );
    }
    
    public void SetDescription([CanBeNull] string description)
    {
        Description = Check.Length(
            description,
            nameof(description),
            maxLength: ProjectConsts.MaxDescriptionLength
        );
    }

    public void SetPurpose([CanBeNull] string purpose)
    {
        Purpose = Check.Length(
            purpose,
            nameof(purpose),
            maxLength: ProjectConsts.MaxPurposeLength
        );
    }

    public void SetProjectImageUrl([CanBeNull] string projectImageUrl)
    {
        ProjectImageUrl = projectImageUrl;
    }

    public void SetProgress(int progress)
    {
        Progress = Check.Range(
            progress,
            nameof(progress),
            0,
            100
        );
    }

    public void AddMilestone(Milestone milestone)
    {
        Check.NotNull(milestone, nameof(milestone));
        Milestones.Add(milestone);
    }

    public void RemoveMilestone(Milestone milestone)
    {
        Check.NotNull(milestone, nameof(milestone));
        Milestones.Remove(milestone);
    }

    public void AddTeamMember(OrganizationMember teamMember)
    {
        Check.NotNull(teamMember, nameof(teamMember));
        TeamMembers.Add(teamMember);
    }

    public void RemoveTeamMember(OrganizationMember teamMember)
    {
        Check.NotNull(teamMember, nameof(teamMember));
        TeamMembers.Remove(teamMember);
    }

    public void AddRequiredSkill(ProjectSkill requiredSkill)
    {
        Check.NotNull(requiredSkill, nameof(requiredSkill));
        RequiredSkills.Add(requiredSkill);
    }

    public void RemoveRequiredSkill(ProjectSkill requiredSkill)
    {
        Check.NotNull(requiredSkill, nameof(requiredSkill));
        RequiredSkills.Remove(requiredSkill);
    }

    public void AddSubProject(Project subProject)
    {
        Check.NotNull(subProject, nameof(subProject));
        SubProjects.Add(subProject);
    }

    public void RemoveSubProject(Project subProject)
    {
        Check.NotNull(subProject, nameof(subProject));
        SubProjects.Remove(subProject);
    }
}