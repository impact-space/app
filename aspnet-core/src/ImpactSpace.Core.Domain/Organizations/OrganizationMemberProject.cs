using System;
using ImpactSpace.Core.Projects;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberProject : Entity, IMultiTenant
{
    public Guid OrganizationMemberId { get; protected set; }
    
    public virtual OrganizationMember OrganizationMember { get; protected set; }
    
    public Guid ProjectId { get; protected set; }
    
    public virtual Project Project { get; protected set; }

    public Guid? TenantId { get; set; }
    
    protected OrganizationMemberProject()
    {
        
    }
    
    internal OrganizationMemberProject(
        Guid organizationMemberId, 
        Guid projectId)
    {
        OrganizationMemberId = organizationMemberId;
        ProjectId = projectId;
    }
    
    public override object[] GetKeys()
    {
        return new object[] { OrganizationMemberId, ProjectId };
    }
}