using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects
{
    /// <summary>
    /// Represents the aggregated voting information for a milestone.
    /// </summary>
    public class MilestoneVoteAggregate : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Gets the associated Milestone ID.
        /// </summary>
        public Guid MilestoneId { get; private set; }
        
        public Milestone Milestone { get; private set; }

        /// <summary>
        /// Gets the total vote score.
        /// </summary>
        public int VoteScore { get; private set; }

        /// <summary>
        /// Gets the list of individual milestone votes.
        /// </summary>
        public List<MilestoneVote> Votes { get; private set; } = new();

        /// <summary>
        /// Gets the Tenant ID for multi-tenancy support.
        /// </summary>
        public Guid? TenantId { get; private set; }

        /// <summary>
        /// Private constructor for deserialization / ORM purposes.
        /// </summary>
        protected MilestoneVoteAggregate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MilestoneVoteAggregate"/> class.
        /// </summary>
        /// <param name="id">The ID of the aggregate.</param>
        /// <param name="milestoneId">The ID of the associated milestone.</param>
        /// <param name="tenantId">The tenant ID for multi-tenancy support.</param>
        public MilestoneVoteAggregate(Guid id, Guid milestoneId, Guid? tenantId)
            : base(id)
        {
            MilestoneId = milestoneId;
            TenantId = tenantId;
            VoteScore = 0;
        }

        /// <summary>
        /// Adds a vote to the aggregate and updates the vote score.
        /// </summary>
        /// <param name="vote">The vote to add.</param>
        public void AddVote([NotNull] MilestoneVote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException(nameof(vote));
            }

            Votes.Add(vote);
            UpdateVoteScore();
        }

        /// <summary>
        /// Removes a vote from the aggregate and updates the vote score.
        /// </summary>
        /// <param name="vote">The vote to remove.</param>
        public void RemoveVote([NotNull] MilestoneVote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException(nameof(vote));
            }

            Votes.Remove(vote);
            UpdateVoteScore();
        }

        /// <summary>
        /// Updates the vote score based on the current list of votes.
        /// </summary>
        private void UpdateVoteScore()
        {
            VoteScore = 0;

            foreach (var vote in Votes)
            {
                VoteScore += (int)vote.VoteType;
            }
        }
    }
}
