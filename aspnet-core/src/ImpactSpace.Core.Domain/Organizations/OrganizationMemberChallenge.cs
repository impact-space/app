using System;
using ImpactSpace.Core.Challenges;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberChallenge : Entity, IMultiTenant
{
    public Guid OrganizationMemberId { get; protected set; }
    public virtual OrganizationMember OrganizationMember { get; protected set; }
    
    public Guid ChallengeId { get; protected set; }
    public virtual Challenge Challenge { get; protected set; }
    
    public Guid? TenantId { get; set; }

    protected OrganizationMemberChallenge()
    {
        
    }
    
    internal OrganizationMemberChallenge(Guid organizationMemberId, Guid challengeId)
    {
        OrganizationMemberId = organizationMemberId;
        ChallengeId = challengeId;
    }

    public override object[] GetKeys()
    {
        return new object[] { OrganizationMemberId, ChallengeId };
    }
}