using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects;

public class ProjectTag : Entity, IMultiTenant
{
    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; }
    
    public Guid TagId { get; set; }
    public virtual Tag Tag { get; set; }
    
    public ProjectTag()
    {
        
    }
    
    public ProjectTag(Guid projectId, Guid tagId)
    {
        ProjectId = projectId;
        TagId = tagId;
    }
    
    public override object[] GetKeys()
    {
        return new object[] {ProjectId, TagId};
    }

    public Guid? TenantId { get; }
}