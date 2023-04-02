using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Represents an action within a objective.
    /// </summary>
    public class Action : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        // Properties
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public StatusType StatusType { get; private set; }
        
        public DateTime? DueDate { get; private set; }
        
        public PriorityLevel PriorityLevel { get; private set; }
        
        public Guid ObjectiveId { get; private set; }
        
        public virtual Objective Objective { get; private set; }
        
        public decimal Budget { get; private set; }
        
        public DateTime? CompletedDate { get; private set; }
        
        public int EstimatedEffort { get; private set; }
        
        public virtual ICollection<OrganizationMemberAction> OrganizationMemberActions { get; private set; }
        
        public Guid? TenantId { get; private set; }

        // Constructors
        protected Action()
        {
            // This constructor is for deserialization / ORM purposes
        }

        public Action(Guid id, [NotNull] string name, string description, StatusType statusType, DateTime? dueDate,
            PriorityLevel priorityLevel, Guid objectiveId, decimal budget, int estimatedEffort, Guid? tenantId)
            : base(id)
        {
            SetName(name);
            SetDescription(description);
            StatusType = statusType;
            DueDate = dueDate;
            PriorityLevel = priorityLevel;
            ObjectiveId = objectiveId;
            Budget = budget;
            EstimatedEffort = estimatedEffort;
            TenantId = tenantId;

            OrganizationMemberActions = new Collection<OrganizationMemberAction>();
        }

        // Methods
        public void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ActionConstants.MaxNameLength
            );
        }

        public void SetDescription([CanBeNull] string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ActionConstants.MaxDescriptionLength
            );
        }
    }
}
