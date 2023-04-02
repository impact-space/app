using System;
using ImpactSpace.Core.Challenges;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberChallenge : Entity, IMultiTenant
{
    public virtual Guid OrganizationMemberId { get; private set; }
    public virtual OrganizationMember OrganizationMember { get; private set; }
    
    public virtual Guid ChallengeId { get; private set; }
    public virtual Challenge Challenge { get; private set; }
    
    public Guid? TenantId { get; set; }

    protected OrganizationMemberChallenge()
    {
    }

    public OrganizationMemberChallenge(Guid organizationMemberId, Guid challengeId)
    {
        OrganizationMemberId = organizationMemberId;
        ChallengeId = challengeId;
    }

    public override object[] GetKeys()
    {
        return new object[] { OrganizationMemberId, ChallengeId };
    }
}