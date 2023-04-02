using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects;

public class Tag : Entity<Guid>, IMultiTenant
{
    public string Name { get; set; }

    public Guid? TenantId { get; }

    public Tag()
    {
        
    }
    
    public Tag(Guid id, string name) : base(id)
    {
        Name = name;
    }
}