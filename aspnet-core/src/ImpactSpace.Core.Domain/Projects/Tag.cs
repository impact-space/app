using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects;

public class Tag : Entity<Guid>, IMultiTenant
{
    public string Name { get; private set; }

    public Guid? TenantId { get; }
    
    public virtual ICollection<ProjectTag> ProjectTags { get; private set; }

    public Tag()
    {
        
    }
    
    public Tag(
        Guid id, 
        [NotNull] string name) : base(id)
    {
        SetName(name);
        
        ProjectTags = new Collection<ProjectTag>();
    }
    
    internal Tag ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }
    
    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name, 
            nameof(name), 
            maxLength: TagConstants.MaxNameLength
        );
        Name = name;
    }
    
}