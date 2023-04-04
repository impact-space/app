using System;
using ImpactSpace.Core.Projects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberProject : Entity, IMultiTenant
{
    public Guid OrganizationMemberId { get; private set; }
    
    public virtual OrganizationMember OrganizationMember { get; private set; }
    
    public Guid ProjectId { get; private set; }
    
    public virtual Project Project { get; private set; }

    public Guid? TenantId { get; }
    
    protected OrganizationMemberProject()
    {
        
    }
    
    public override object[] GetKeys()
    {
        return new object[] { OrganizationMemberId, ProjectId };
    }
}