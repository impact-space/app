using System;
using ImpactSpace.Core.Projects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using Action = ImpactSpace.Core.Projects.Action;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberAction : Entity, IMultiTenant
{
    public Guid OrganizationMemberId { get; private set; }

    public virtual OrganizationMember OrganizationMember { get; private set; }
    
    public Guid ActionId { get; private set; }

    public virtual Action Action { get; private set; }

    protected OrganizationMemberAction()
    {
        
    }
    
    public override object[] GetKeys()
    {
        return new object[] { OrganizationMemberId, ActionId };
    }

    public Guid? TenantId { get; }
}