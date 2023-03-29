using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Organizations;

public interface IOrganizationRepository : IRepository<Organization, Guid>
{
    Task<Organization> FindByNameAsync(string name);
    
    Task<List<Organization>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
    
    Task<Organization> FindByTenantIdAsync(Guid tenantId);
}