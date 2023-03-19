using System;
using System.Collections.Generic;
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
    /// Gets the end date of the milestone.
    /// </summary>
    public DateTime? EndDate { get; private set; }

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
    /// Gets the project ID the milestone belongs to.
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// Gets the project the milestone belongs to.
    /// </summary>
    public Project Project { get; private set; }

    /// <summary>
    /// Gets the quests associated with the milestone.
    /// </summary>
    public List<Quest> Quests { get; private set; } = new();

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
    private Milestone()
    {
    }

    /// <summary>
    /// Creates a new instance of the Milestone class with the specified ID, name, description, and other properties.
    /// </summary>
    /// <param name="id">The ID of the milestone.</param>
    /// <param name="name">The name of the milestone.</param>
    /// <param name="description">The description of the milestone.</param>
    /// <param name="projectId">The ID of the project the milestone belongs to.</param>
    /// <param name="priorityLevel">The priority level of the milestone.</param>
    /// <param name="statusType">The status type of the milestone.</param>
    /// <param name="tenantId">The tenant ID associated with the milestone.</param>
    public Milestone(Guid id, [NotNull] string name, string description,Guid projectId, PriorityLevel priorityLevel, StatusType statusType, Guid? tenantId)
        : base(id)
    {
        SetName(name);
        SetDescription(description);
        ProjectId = projectId;
        PriorityLevel = priorityLevel;
        StatusType = statusType;
        TenantId = tenantId;
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
            maxLength: MilestoneConsts.MaxNameLength
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
                maxLength: MilestoneConsts.MaxDescriptionLength
            );
        }
    }

    /// <summary>
    /// Adds a quest to the list of quests associated with the milestone.
    /// </summary>
    /// <param name="quest">The quest to add.</param>
    public void AddQuest(Quest quest)
    {
        Check.NotNull(quest, nameof(quest));
        Quests.Add(quest);
    }

    /// <summary>
    /// Removes a quest from the list of quests associated with the milestone.
    /// </summary>
    /// <param name="quest">The quest to remove.</param>
    public void RemoveQuest(Quest quest)
    {
        Check.NotNull(quest, nameof(quest));
        Quests.Remove(quest);
    }
}