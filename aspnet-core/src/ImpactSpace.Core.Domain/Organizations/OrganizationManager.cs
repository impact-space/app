using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.TenantManagement;

namespace ImpactSpace.Core.Organizations;

public sealed class OrganizationManager : DomainService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ITenantRepository _tenantRepository;

    public OrganizationManager(
        IOrganizationRepository organizationRepository,
        ITenantRepository tenantRepository)
    {
        _organizationRepository = organizationRepository;
        _tenantRepository = tenantRepository;
    }
    
    public async Task<bool> ExistsForTenantAsync(Guid tenantId)
    {
        return await _organizationRepository.FindByTenantIdAsync(tenantId) != null;
    }
    
    
    public async Task<Organization> CreateAsync(
        [NotNull] string name,
        Guid tenantId,
        [CanBeNull] string description = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existingOrganization = await _organizationRepository.FindByNameAsync(name);

        if (existingOrganization != null)
        {
            throw new OrganizationAlreadyExistsException(name);
        }

        await _tenantRepository.GetAsync(tenantId);

        return new Organization(
            GuidGenerator.Create(),
            name,
            tenantId,
            description
        );
    }

    public async Task ChangeNameAsync(
        [NotNull] Organization organization,
        [NotNull] string newName)
    {
        Check.NotNull(organization, nameof(organization));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        var existingOrganization = await _organizationRepository.FindByNameAsync(newName);
        if (existingOrganization != null && existingOrganization.Id != organization.Id)
        {
            throw new OrganizationAlreadyExistsException(newName);
        }

        organization.ChangeName(newName);
    }

    public void ChangeDescription([NotNull] Organization organization, [CanBeNull] string description)
    {
        Check.NotNull(organization, nameof(organization));
        organization.ChangeDescription(description);
    }
}