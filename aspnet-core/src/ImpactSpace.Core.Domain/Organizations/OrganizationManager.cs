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
    
    #region Crud Operations
    
    public async Task<Organization> CreateAsync(
        [NotNull] string name,
        Guid tenantId,
        [CanBeNull] string description = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existingOrganization = await _organizationRepository.FindByNameAsync(name);

        if (existingOrganization != null || await ExistsForTenantAsync(tenantId))
        {
            throw new OrganizationAlreadyExistsException(name);
        }

        await _tenantRepository.GetAsync(tenantId);
        
        var organization = new Organization(
            GuidGenerator.Create(),
            name,
            tenantId,
            description
        );

        return await _organizationRepository.InsertAsync(organization);
    }

    public async Task UpdateAsync(Guid id, string newName, string newDescription)
    {
        var organization = await _organizationRepository.GetAsync(id);

        if (organization.Name != newName)
        {
            await ChangeNameAsync(organization, newName);
        }

        organization.ChangeDescription(newDescription);

        await _organizationRepository.UpdateAsync(organization);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _organizationRepository.GetAsync(id);
        await _organizationRepository.DeleteAsync(id);
    }
    
    #endregion

    private async Task ChangeNameAsync(Organization organization, string newName)
    {
        Check.NotNull(organization, nameof(organization));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        await EnsureNameIsUniqueAsync(newName, organization.Id);
        organization.ChangeName(newName);
    }
    
    private async Task EnsureNameIsUniqueAsync(string newName, Guid? currentOrganizationId = null)
    {
        var existingOrganization = await _organizationRepository.FindByNameAsync(newName);
        if (existingOrganization != null && existingOrganization.Id != currentOrganizationId)
        {
            throw new OrganizationAlreadyExistsException(newName);
        }
    }
    
    public async Task<bool> ExistsForTenantAsync(Guid tenantId)
    {
        return await _organizationRepository.FindByTenantIdAsync(tenantId) != null;
    }
}