using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImpactSpace.Core.Projects;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace ImpactSpace.Core.Organizations;

public class Organization : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public virtual ICollection<Project> Projects { get; }
    
    public virtual ICollection<OrganizationMember> OrganizationMembers { get; }
    
    public virtual OrganizationProfile OrganizationProfile { get; protected set; }

    public Guid? TenantId { get; set; }
    
    public virtual Tenant Tenant { get; set; }


    protected Organization()
    {
        
    }

    internal Organization(
        Guid id, 
        [NotNull] string name, 
        Guid tenantId,
        [CanBeNull] string description)
        : base(id)
    {
        SetName(name);
        SetDescription(description);
        TenantId = tenantId;
        Projects = new Collection<Project>();
        OrganizationMembers = new Collection<OrganizationMember>();
    }

    internal Organization ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }

    internal Organization ChangeDescription([CanBeNull] string description)
    {
        SetDescription(description);
        return this;
    }

    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: OrganizationConstants.MaxNameLength
        );
    }

    private void SetDescription([CanBeNull] string description)
    {
        Description = Check.Length(
            description,
            nameof(description),
            maxLength: OrganizationConstants.MaxDescriptionLength
        );
    }
}
