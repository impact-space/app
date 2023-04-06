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
/// Represents an objective, a unit of work in a project, containing objective actions and associated with a milestone.
/// </summary>
public class Objective : AuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// Gets the name of the objective.
    /// </summary>
    public virtual string Name { get; private set; }

    /// <summary>
    /// Gets or sets the description of the objective.
    /// </summary>
    public virtual string Description { get; private set; }

    /// <summary>
    /// Gets or sets the due date of the objective, if any.
    /// </summary>
    public virtual DateTime? DueDate { get; private set; }

    /// <summary>
    /// Gets or sets the completion date of the objective, if any.
    /// </summary>
    public virtual DateTime? CompletedDate { get; private set; }

    /// <summary>
    /// Gets or sets the budget allocated to the objective.
    /// </summary>
    public virtual decimal Budget { get; private set; }

    /// <summary>
    /// Gets or sets the ID of the milestone to which the objective belongs.
    /// </summary>
    public virtual Guid MilestoneId { get; private set; }

    /// <summary>
    /// Gets or sets the milestone to which the objective belongs.
    /// </summary>
    public virtual Milestone Milestone { get; private set; }

    /// <summary>
    /// Gets the actions associated with this objective.
    /// </summary>
    public virtual ICollection<Action> Actions { get; private set; }

    /// <summary>
    /// Gets or sets the status type of the objective.
    /// </summary>
    public StatusType StatusType { get; private set; }

    /// <summary>
    /// Gets or sets the tenant ID for multi-tenancy support.
    /// </summary>
    public Guid? TenantId { get; private set; }

    /// <summary>
    /// This constructor is for deserialization / ORM purposes.
    /// </summary>
    protected Objective()
    {
    }

    /// <summary>
    /// Creates a new instance of the Objective class with the specified ID, name, and milestone ID.
    /// </summary>
    /// <param name="id">The ID of the objective.</param>
    /// <param name="name">The name of the objective.</param>
    /// <param name="milestoneId">The ID of the milestone to which the objective belongs.</param>
    internal Objective(Guid id, [NotNull] string name, Guid milestoneId)
        : base(id)
    {
        SetName(name);
        MilestoneId = milestoneId;

        Actions = new Collection<Action>();
    }
    
    /// <summary>
    /// Changes the name of the objective.
    /// </summary>
    /// <param name="name">The new name of the objective.</param>
    /// <returns>The objective object.</returns>
    public Objective SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: ObjectiveConstants.MaxNameLength);
        return this;
    }
    
    /// <summary>
    /// Sets the description of the objective.
    /// </summary>
    /// <param name="description">The new description of the objective.</param>
    /// <returns>The objective object.</returns>
    public Objective SetDescription(string description)
    {
        Description = Check.Length(
            description,
            nameof(description),
            maxLength: ObjectiveConstants.MaxDescriptionLength
        );
        return this;
    }

    /// <summary>
    /// Sets the due date of the objective.
    /// </summary>
    /// <param name="dueDate">The new due date of the objective.</param>
    /// <returns>The objective object.</returns>
    public Objective SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
        return this;
    }

    /// <summary>
    /// Sets the completion date of the objective.
    /// </summary>
    /// <param name="completedDate">The new completion date of the objective.</param>
    /// <returns>The objective object.</returns>
    public Objective SetCompletedDate(DateTime? completedDate)
    {
        CompletedDate = completedDate;
        return this;
    }

    /// <summary>
    /// Sets the budget allocated to the objective.
    /// </summary>
    /// <param name="budget">The new budget for the objective.</param>
    /// <returns>The objective object.</returns>
    public Objective SetBudget(decimal budget)
    {
        Budget = budget;
        return this;
    }

    /// <summary>
    /// Sets the status type of the objective.
    /// </summary>
    /// <param name="statusType">The new status type for the objective.</param>
    /// <returns>The objective object.</returns>
    public Objective SetStatusType(StatusType statusType)
    {
        StatusType = statusType;
        return this;
    }

    public void AddAction(Action action)
    {
        Check.NotNull(action, nameof(action));

        Actions.Add(action);
    }

    public void RemoveAction(Action action)
    {
        Check.NotNull(action, nameof(action));

        Actions.Remove(action);
    }
}