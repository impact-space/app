using System;
using System.Collections.Generic;
using ImpactSpace.Core.Common;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects;

/// <summary>
/// Represents a project quest, a unit of work in a project, containing project actions and associated with a milestone.
/// </summary>
public class Quest : AuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// Gets the name of the quest.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets or sets the description of the quest.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets or sets the due date of the quest, if any.
    /// </summary>
    public DateTime? DueDate { get; private set; }

    /// <summary>
    /// Gets or sets the completion date of the quest, if any.
    /// </summary>
    public DateTime? CompletedDate { get; private set; }

    /// <summary>
    /// Gets or sets the budget allocated to the quest.
    /// </summary>
    public decimal Budget { get; private set; }

    /// <summary>
    /// Gets or sets the ID of the milestone to which the quest belongs.
    /// </summary>
    public Guid MilestoneId { get; private set; }

    /// <summary>
    /// Gets or sets the milestone to which the quest belongs.
    /// </summary>
    public Milestone Milestone { get; private set; }

    /// <summary>
    /// Gets the project actions associated with this quest.
    /// </summary>
    public List<ProjectAction> ProjectActions { get; private set; } = new();

    /// <summary>
    /// Gets or sets the status type of the quest.
    /// </summary>
    public StatusType StatusType { get; private set; }

    /// <summary>
    /// Gets or sets the tenant ID for multi-tenancy support.
    /// </summary>
    public Guid? TenantId { get; private set; }

    /// <summary>
    /// This constructor is for deserialization / ORM purposes.
    /// </summary>
    protected Quest()
    {
    }

    /// <summary>
    /// Creates a new instance of the ProjectQuest class with the specified ID, name, and milestone ID.
    /// </summary>
    /// <param name="id">The ID of the quest.</param>
    /// <param name="name">The name of the quest.</param>
    /// <param name="milestoneId">The ID of the milestone to which the quest belongs.</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support.</param>
    internal Quest(Guid id, [NotNull] string name, Guid milestoneId, Guid? tenantId)
        : base(id)
    {
        SetName(name);
        MilestoneId = milestoneId;
        TenantId = tenantId;
    }
    
    /// <summary>
    /// Changes the name of the quest.
    /// </summary>
    /// <param name="name">The new name of the quest.</param>
    /// <returns>The quest object.</returns>
    public Quest SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: QuestConsts.MaxNameLength);
        return this;
    }
    
    /// <summary>
    /// Sets the description of the quest.
    /// </summary>
    /// <param name="description">The new description of the quest.</param>
    /// <returns>The quest object.</returns>
    public Quest SetDescription(string description)
    {
        Description = Check.Length(
            description,
            nameof(description),
            maxLength: QuestConsts.MaxDescriptionLength
        );
        return this;
    }

    /// <summary>
    /// Sets the due date of the quest.
    /// </summary>
    /// <param name="dueDate">The new due date of the quest.</param>
    /// <returns>The quest object.</returns>
    public Quest SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
        return this;
    }

    /// <summary>
    /// Sets the completion date of the quest.
    /// </summary>
    /// <param name="completedDate">The new completion date of the quest.</param>
    /// <returns>The quest object.</returns>
    public Quest SetCompletedDate(DateTime? completedDate)
    {
        CompletedDate = completedDate;
        return this;
    }

    /// <summary>
    /// Sets the budget allocated to the quest.
    /// </summary>
    /// <param name="budget">The new budget for the quest.</param>
    /// <returns>The quest object.</returns>
    public Quest SetBudget(decimal budget)
    {
        Budget = budget;
        return this;
    }

    /// <summary>
    /// Sets the status type of the quest.
    /// </summary>
    /// <param name="statusType">The new status type for the quest.</param>
    /// <returns>The quest object.</returns>
    public Quest SetStatusType(StatusType statusType)
    {
        StatusType = statusType;
        return this;
    }

    /// <summary>
    /// Adds a project action to the quest.
    /// </summary>
    /// <param name="projectAction">The project action to be added.</param>
    public void AddProjectAction(ProjectAction projectAction)
    {
        if (projectAction == null)
        {
            throw new ArgumentNullException(nameof(projectAction));
        }

        ProjectActions.Add(projectAction);
    }

    /// <summary>
    /// Removes a project action from the quest.
    /// </summary>
    /// <param name="projectAction">The project action to be removed.</param>
    public void RemoveProjectAction(ProjectAction projectAction)
    {
        if (projectAction == null)
        {
            throw new ArgumentNullException(nameof(projectAction));
        }

        ProjectActions.Remove(projectAction);
    }
}