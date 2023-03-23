using System;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Organizations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects;

/// <summary>
/// Represents a vote on a milestone by an organization member.
/// </summary>
public class MilestoneVote : AuditedEntity, IMultiTenant
{
    /// <summary>
    /// The ID of the milestone this vote belongs to.
    /// </summary>
    public Guid MilestoneId { get; private set; }
    
    /// <summary>
    /// The milestone of the vote
    /// </summary>
    public virtual Milestone Milestone { get; private set; }

    /// <summary>
    /// The ID of the organization member who cast the vote.
    /// </summary>
    public Guid OrganizationMemberId { get; private set; }

    /// <summary>
    /// The organization member who cast the vote.
    /// </summary>
    public virtual OrganizationMember OrganizationMember { get; private set; }

    /// <summary>
    /// The ID of the MilestoneVoteAggregate this vote belongs to.
    /// </summary>
    public Guid MilestoneVoteAggregateId { get; private set; }

    /// <summary>
    /// The MilestoneVoteAggregate this vote belongs to.
    /// </summary>
    public virtual MilestoneVoteAggregate MilestoneVoteAggregate { get; private set; }

    /// <summary>
    /// The type of vote (UpVote or DownVote).
    /// </summary>
    public VoteType VoteType { get; private set; }

    /// <summary>
    /// The Tenant ID for multi-tenancy support.
    /// </summary>
    public Guid? TenantId { get; private set; }

    /// <summary>
    /// Private constructor for deserialization / ORM purposes.
    /// </summary>
    protected MilestoneVote()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MilestoneVote"/> class.
    /// </summary>
    /// <param name="milestoneId">The ID of the milestone this vote belongs to.</param>
    /// <param name="organizationMemberId">The ID of the organization member who cast the vote.</param>
    /// <param name="milestoneVoteAggregateId">The ID of the MilestoneVoteAggregate this vote belongs to.</param>
    /// <param name="voteType">The type of vote (UpVote or DownVote).</param>
    /// <param name="tenantId">The Tenant ID for multi-tenancy support.</param>
    public MilestoneVote(Guid milestoneId, Guid organizationMemberId, Guid milestoneVoteAggregateId, VoteType voteType, Guid? tenantId)
    {
        MilestoneId = milestoneId;
        OrganizationMemberId = organizationMemberId;
        MilestoneVoteAggregateId = milestoneVoteAggregateId;
        VoteType = voteType;
        TenantId = tenantId;
    }

    /// <summary>
    /// Updates the vote type of the MilestoneVote.
    /// </summary>
    /// <param name="voteType">The new vote type (UpVote or DownVote).</param>
    /// <returns>Returns the updated MilestoneVote.</returns>
    public MilestoneVote ChangeVoteType(VoteType voteType)
    {
        VoteType = voteType;
        return this;
    }

    public override object[] GetKeys()
    {
        return new object[] { MilestoneId, OrganizationMemberId };
    }
}