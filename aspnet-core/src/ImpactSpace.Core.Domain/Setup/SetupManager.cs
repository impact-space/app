using System;
using System.Threading.Tasks;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Organizations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Setup;

public class SetupManager : DomainService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IRepository<OrganizationMember, Guid> _organizationMemberRepository;
    private readonly IDataFilter _dataFilter;
    
    public SetupManager(
        IOrganizationRepository organizationRepository,
        IRepository<OrganizationMember, Guid> organizationMemberRepository, 
        IDataFilter dataFilter)
    {
        _organizationRepository = organizationRepository;
        _organizationMemberRepository = organizationMemberRepository;
        _dataFilter = dataFilter;
    }

    public async Task<Organization> CreateOrganizationAsync(string name, string description, Guid tenantId)
    {
        if (!CurrentTenant.Id.HasValue)
        {
            throw new TenantNotAvailableException();
        }

        using (_dataFilter.Disable<IMultiTenant>())
        {
            var existingOrganization = await _organizationRepository.FindByNameAsync(name);
            
            if (existingOrganization != null)
            {
                throw new OrganizationAlreadyExistsException(name);
            }
        }

        var organizationExists = await _organizationRepository.CountAsync() > 0;

        if (organizationExists)
        {
            throw new OrganizationAlreadyExistsException(name);
        }

        var organization = new Organization(
            id: GuidGenerator.Create(),
            name: name,
            tenantId: tenantId,
            description: description
        );
        
        await _organizationRepository.InsertAsync(organization);
        
        return organization;
    }

    public async Task<OrganizationMember> CreateOrganizationMemberAsync(string name, string email, Guid organizationId)
    {
        var organizationMember = new OrganizationMember(
            id: GuidGenerator.Create(),
            name: name,
            email: email,
            phone: null,
            organizationId: organizationId
        );
        
        await _organizationMemberRepository.InsertAsync(organizationMember);
        
        return organizationMember;
    }
}