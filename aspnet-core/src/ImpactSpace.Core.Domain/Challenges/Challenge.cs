using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Projects;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Challenges;

public class Challenge : Entity<Guid>, IMultiTenant, IHasConcurrencyStamp
{
    public string Name { get; private set; }
    
    public virtual ICollection<ProjectChallenge> ProjectChallenges { get; private set; }
    
    public virtual ICollection<OrganizationMemberChallenge> OrganizationMemberChallenges { get; private set; }
    
    public Guid? TenantId { get; set; }
    
    public string ConcurrencyStamp { get; set; }

    protected Challenge()
    {
        
    }

    internal Challenge(Guid id, [NotNull] string name) : base(id)
    {
        SetName(name);
        ProjectChallenges = new Collection<ProjectChallenge>();
        OrganizationMemberChallenges = new Collection<OrganizationMemberChallenge>();
    }

    internal Challenge ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }

    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: ChallengeConstants.MaxNameLength
        );
    }
}