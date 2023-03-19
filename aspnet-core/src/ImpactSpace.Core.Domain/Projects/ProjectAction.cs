using System;
using System.Collections.Generic;
using System.Linq;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Organizations;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects
{
    /// <summary>
    /// Represents a project action within a quest.
    /// </summary>
    public class ProjectAction : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        // Properties
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public StatusType StatusType { get; private set; }
        
        public DateTime? DueDate { get; private set; }
        
        public PriorityLevel PriorityLevel { get; private set; }
        
        public Guid QuestId { get; private set; }
        
        public Quest Quest { get; private set; }
        
        public decimal Budget { get; private set; }
        
        public DateTime? CompletedDate { get; private set; }
        
        public int EstimatedEffort { get; private set; }
        
        public List<OrganizationMember> TeamMembers { get; private set; } = new();
        
        public Guid? TenantId { get; private set; }

        // Constructors
        private ProjectAction()
        {
            // This constructor is for deserialization / ORM purposes
        }

        public ProjectAction(Guid id, [NotNull] string name, string description, StatusType statusType, DateTime? dueDate,
            PriorityLevel priorityLevel, Guid questId, decimal budget, int estimatedEffort, Guid? tenantId)
            : base(id)
        {
            SetName(name);
            SetDescription(description);
            StatusType = statusType;
            DueDate = dueDate;
            PriorityLevel = priorityLevel;
            QuestId = questId;
            Budget = budget;
            EstimatedEffort = estimatedEffort;
            TenantId = tenantId;
        }

        // Methods
        public void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProjectActionConsts.MaxNameLength
            );
        }

        public void SetDescription([CanBeNull] string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ProjectActionConsts.MaxDescriptionLength
            );
        }

        public void AddTeamMember(OrganizationMember teamMember)
        {
            if (TeamMembers.Any(t => t.Id == teamMember.Id))
            {
                throw new InvalidOperationException($"Team member with Id {teamMember.Id} is already added to the action.");
            }

            TeamMembers.Add(teamMember);
        }

        public void RemoveTeamMember(Guid teamMemberId)
        {
            var teamMember = TeamMembers.FirstOrDefault(t => t.Id == teamMemberId);
            if (teamMember == null)
            {
                throw new InvalidOperationException($"Team member with Id {teamMemberId} not found in the action.");
            }

            TeamMembers.Remove(teamMember);
        }
    }
}
