using System;
using ImpactSpace.Core.Challenges;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects;

public class ProjectChallenge: Entity, IMultiTenant, IHasConcurrencyStamp
{
    public virtual Guid ProjectId { get; private set; }
    public virtual Project Project { get; private set; }
    public virtual Guid ChallengeId { get; private set; }
    public virtual Challenge Challenge { get; private set; }
    
    public Guid? TenantId { get; set; }
    
    public string ConcurrencyStamp { get; set; }
    
    public ProjectChallenge()
    {
        
    }
    
    public ProjectChallenge(Guid projectId, Guid challengeId)
    {
        ProjectId = projectId;
        ChallengeId = challengeId;
    }

    public override object[] GetKeys()
    {
        return new object[] { ProjectId, ChallengeId };
    }
}