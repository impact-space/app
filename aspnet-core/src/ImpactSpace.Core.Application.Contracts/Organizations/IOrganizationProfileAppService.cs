using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Organizations;

public interface IOrganizationProfileAppService : IApplicationService
{
    Task<OrganizationProfileDto> GetAsync();
    
    Task<OrganizationProfileDto> GetAsync(Guid organizationId);
    
    Task<Guid> CreateAsync(OrganizationProfileCreateUpdateDto input);
    
    Task UpdateAsync(Guid organizationId, OrganizationProfileCreateUpdateDto input);
    
    Task<bool> ExistsForTenantAsync(Guid currentTenantId);
}