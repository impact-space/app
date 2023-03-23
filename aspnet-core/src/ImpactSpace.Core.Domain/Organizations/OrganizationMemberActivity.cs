using System;
using ImpactSpace.Core.Projects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberActivity : Entity, IMultiTenant
{
    public Guid OrganizationMemberId { get; private set; }

    public virtual OrganizationMember OrganizationMember { get; private set; }
    
    public Guid ActivityId { get; private set; }

    public virtual Activity Activity { get; private set; }

    protected OrganizationMemberActivity()
    {
        
    }
    
    public override object[] GetKeys()
    {
        return new object[] { OrganizationMemberId, ActivityId };
    }

    public Guid? TenantId { get; }
}