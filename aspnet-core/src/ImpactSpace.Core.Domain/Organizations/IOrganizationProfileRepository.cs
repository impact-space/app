using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Organizations;

public interface IOrganizationProfileRepository : IRepository<OrganizationProfile, Guid>
{
    Task<OrganizationProfile> GetByOrganizationIdAsync(Guid organizationId);
    
    Task<bool> ExistsForTenantAsync(Guid tenantId);
}