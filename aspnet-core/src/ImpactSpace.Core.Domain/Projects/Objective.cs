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
    /// Represents an objective within a quest.
    /// </summary>
    public class Objective : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        // Properties
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public StatusType StatusType { get; private set; }
        
        public DateTime? DueDate { get; private set; }
        
        public PriorityLevel PriorityLevel { get; private set; }
        
        public Guid QuestId { get; private set; }
        
        public virtual Quest Quest { get; private set; }
        
        public decimal Budget { get; private set; }
        
        public DateTime? CompletedDate { get; private set; }
        
        public int EstimatedEffort { get; private set; }
        
        public virtual ICollection<OrganizationMemberObjective> OrganizationMemberActivities { get; private set; }
        
        public Guid? TenantId { get; private set; }

        // Constructors
        protected Objective()
        {
            // This constructor is for deserialization / ORM purposes
        }

        public Objective(Guid id, [NotNull] string name, string description, StatusType statusType, DateTime? dueDate,
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

            OrganizationMemberActivities = new Collection<OrganizationMemberObjective>();
        }

        // Methods
        public void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ActivityConstants.MaxNameLength
            );
        }

        public void SetDescription([CanBeNull] string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ActivityConstants.MaxDescriptionLength
            );
        }
    }
}
