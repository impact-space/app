using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImpactSpace.Core.Common;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects;

/// <summary>
/// Represents a milestone within a project.
/// </summary>
public class Milestone : AuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// Gets the name of the milestone.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>s
    /// Gets the description of the milestone.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets the start date of the milestone.
    /// </summary>
    public DateTime? StartDate { get; private set; }

    /// <summary>
    /// Gets the completed date of the milestone.
    /// </summary>
    public DateTime? CompletedDate { get; private set; }

    /// <summary>
    /// Gets the deadline of the milestone.
    /// </summary>
    public DateTime? Deadline { get; private set; }

    /// <summary>
    /// Gets the budget allocated for the milestone.
    /// </summary>
    public decimal Budget { get; private set; }

    /// <summary>
    /// Gets the total votes for the milestone.
    /// </summary>
    public int TotalVotes { get; private set; }
    
    /// <summary>
    /// Gets the identifier for the milestone's project
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// Gets the project the milestone belongs to.
    /// </summary>
    public virtual Project Project { get; private set; }

    /// <summary>
    /// Gets the objectives associated with the milestone.
    /// </summary>
    public virtual ICollection<Objective> Objectives { get; private set; }

    /// <summary>
    /// Gets the priority level of the milestone.
    /// </summary>
    public PriorityLevel PriorityLevel { get; private set; }

    /// <summary>
    /// Gets the status type of the milestone.
    /// </summary>
    public StatusType StatusType { get; private set; }

    /// <summary>
    /// Gets the tenant ID associated with the milestone.
    /// </summary>
    public Guid? TenantId { get; private set; }

    /// <summary>
    /// This constructor is for deserialization / ORM purposes.
    /// </summary>
    protected Milestone()
    {
    }

    /// <summary>
    /// Creates a new instance of the Milestone class with the specified ID, name, description, and other properties.
    /// </summary>
    /// <param name="id">The ID of the milestone.</param>
    /// <param name="name">The name of the milestone.</param>
    /// <param name="description">The description of the milestone.</param>
    /// <param name="priorityLevel">The priority level of the milestone.</param>
    /// <param name="statusType">The status type of the milestone.</param>
    public Milestone(Guid id, [NotNull] string name, string description, PriorityLevel priorityLevel, StatusType statusType)
        : base(id)
    {
        SetName(name);
        SetDescription(description);
        PriorityLevel = priorityLevel;
        StatusType = statusType;

        Objectives = new Collection<Objective>();
    }
    
    /// <summary>
    /// Changes the milestone's name.
    /// </summary>
    /// <param name="name">The new name of the milestone.</param>
    /// <returns>The updated milestone.</returns>
    public Milestone ChangeName(string name)
    {
        SetName(name);
        return this;
    }

    /// <summary>
    /// Changes the milestone's description.
    /// </summary>
    /// <param name="description">The new description of the milestone.</param>
    /// <returns>The updated milestone.</returns>
    public Milestone ChangeDescription(string description)
    {
        SetDescription(description);
        return this;
    }

    /// <summary>
    /// Changes the milestone's priority level.
    /// </summary>
    /// <param name="priorityLevel">The new priority level of the milestone.</param>
    /// <returns>The updated milestone.</returns>
    public Milestone ChangePriorityLevel(PriorityLevel priorityLevel)
    {
        PriorityLevel = priorityLevel;
        return this;
    }

    /// <summary>
    /// Changes the milestone's status type.
    /// </summary>
    /// <param name="statusType">The new status type of the milestone.</param>
    /// <returns>The updated milestone.</returns>
    public Milestone ChangeStatusType(StatusType statusType)
    {
        StatusType = statusType;
        return this;
    }
    
    /// <summary>
    /// Changes the milestone's start date.
    /// </summary>
    /// <param name="startDate">The new start date of the milestone.</param>
    /// <returns>The updated milestone.</returns>
    /// <exception cref="BusinessException">Thrown when the specified start date is later than the deadline or completed date.</exception>
    public Milestone ChangeStartDate(DateTime? startDate)
    {
        if (startDate.HasValue && Deadline.HasValue && startDate.Value > Deadline.Value)
        {
            throw new BusinessException("Start date cannot be later than the deadline.");
        }

        if (startDate.HasValue && CompletedDate.HasValue && startDate.Value > CompletedDate.Value)
        {
            throw new BusinessException("Start date cannot be later than the completed date.");
        }

        StartDate = startDate;
        return this;
    }
    
    /// <summary>
    /// Changes the milestone's budget.
    /// </summary>
    /// <param name="budget">The new budget for the milestone.</param>
    /// <returns>The updated milestone.</returns>
    /// <exception cref="BusinessException">Thrown when the specified budget is negative.</exception>
    public Milestone ChangeBudget(decimal budget)
    {
        if (budget < 0)
        {
            throw new BusinessException("Budget cannot be negative.");
        }

        Budget = budget;
        return this;
    }

    /// <summary>
    /// Changes the milestone's completed date.
    /// </summary>
    /// <param name="completedDate">The new completed date of the milestone.</param>
    /// <returns>The updated milestone.</returns>
    public Milestone ChangeCompletedDate(DateTime? completedDate)
    {
        SetCompletedDate(completedDate);
        return this;
    }

    /// <summary>
    /// Changes the milestone's deadline.
    /// </summary>
    /// <param name="deadline">The new deadline of the milestone.</param>
    /// <returns>The updated milestone.</returns>
    public Milestone ChangeDeadline(DateTime? deadline)
    {
        SetDeadline(deadline);
        return this;
    }
    
    public Milestone ChangeTotalVotes(int totalVotes)
    {
        SetTotalVotes(totalVotes);
        return this;
    }

    public Milestone UpVote()
    {
        TotalVotes++;
        return this;
    }

    public Milestone DownVote()
    {
        if (TotalVotes == 0)
        {
            throw new BusinessException("Total votes cannot be negative.");
        }

        TotalVotes--;
        return this;
    }

    /// <summary>
    /// Sets the Milestone's name to the specified value.
    /// </summary>
    /// <param name="name">The new name for the Milestone</param>
    /// <exception cref="ArgumentException">Thrown when the specified name is null, whitespace, or exceeds the maximum length allowed.</exception>
    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: MilestoneConstants.MaxNameLength
        );
    }

    /// <summary>
    /// Sets the Milestone's description to the specified value.
    /// </summary>
    /// <param name="description">The new description for the Milestone</param>
    /// <exception cref="ArgumentException">Thrown when the specified description exceeds the maximum length allowed.</exception>
    private void SetDescription(string description)
    {
        if (description != null)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: MilestoneConstants.MaxDescriptionLength
            );
        }
    }
    
    private void SetTotalVotes(int totalVotes)
    {
        if (totalVotes < 0)
        {
            throw new BusinessException("Total votes cannot be negative.");
        }

        TotalVotes = totalVotes;
    }
    
    private void SetCompletedDate(DateTime? completedDate)
    {
        if (completedDate.HasValue && StartDate.HasValue && completedDate.Value < StartDate.Value)
        {
            throw new BusinessException("Completed date cannot be earlier than the start date.");
        }

        CompletedDate = completedDate;
    }

    private void SetDeadline(DateTime? deadline)
    {
        if (deadline.HasValue && StartDate.HasValue && deadline.Value < StartDate.Value)
        {
            throw new BusinessException("Deadline cannot be earlier than the start date.");
        }

        Deadline = deadline;
    }
}