using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Organizations;

[Authorize]
public class OrganizationAppService : CoreAppService, IOrganizationAppService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly OrganizationManager _organizationManager;

    public OrganizationAppService(
        IOrganizationRepository organizationRepository,
        OrganizationManager organizationManager)
    {
        _organizationRepository = organizationRepository;
        _organizationManager = organizationManager;
    }
    
    public async Task<OrganizationDto> GetAsync(Guid id)
    {
        var organization = await _organizationRepository.GetAsync(id);
        return ObjectMapper.Map<Organization, OrganizationDto>(organization);
    }

    public async Task<PagedResultDto<OrganizationDto>> GetListAsync(GetOrganizationListDto input)
    {
        if(input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Organization.Name);
        }
        
        var organizations = await _organizationRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );
        
        var totalCount = input.Filter == null
            ? await _organizationRepository.CountAsync()
            : await _organizationRepository.CountAsync(organization => organization.Name.Contains(input.Filter));
        
        return new PagedResultDto<OrganizationDto>( 
            totalCount,
            ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(organizations)
        );
    }
    
    public async Task<OrganizationDto> CreateAsync(CreateOrganizationDto input)
    {
        var tenantId = CurrentTenant.Id!.Value;
        
        var organization = await _organizationManager.CreateAsync(
            input.Name,
            tenantId,
            input.Description
        );

        return ObjectMapper.Map<Organization, OrganizationDto>(organization);
    }
    
    public async Task UpdateAsync(Guid id, UpdateOrganizationDto input)
    {
        await _organizationManager.UpdateAsync(id, input.Name, input.Description);
    }

    [Authorize(CorePermissions.Organizations.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _organizationManager.DeleteAsync(id);
    }

    public Task<bool> ExistsForTenantAsync(Guid tenantId)
    {
        return _organizationManager.ExistsForTenantAsync(tenantId);
    }
}