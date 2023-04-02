using System;
using ImpactSpace.Core.Projects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberObjective : Entity, IMultiTenant
{
    public Guid OrganizationMemberId { get; private set; }

    public virtual OrganizationMember OrganizationMember { get; private set; }
    
    public Guid ObjectiveId { get; private set; }

    public virtual Objective Objective { get; private set; }

    protected OrganizationMemberObjective()
    {
        
    }
    
    public override object[] GetKeys()
    {
        return new object[] { OrganizationMemberId, ObjectiveId };
    }

    public Guid? TenantId { get; }
}