using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ImpactSpace.Core.Organizations;

public class EfCoreOrganizationRepository : EfCoreRepository<CoreDbContext, Organization, Guid>, IOrganizationRepository
{
    public EfCoreOrganizationRepository(IDbContextProvider<CoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<Organization> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(organization => organization.Name == name);
    }

    public async Task<List<Organization>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                organization => organization.Name.Contains(filter)
            )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<Organization> FindByTenantIdAsync(Guid tenantId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.SingleOrDefaultAsync(organization => organization.TenantId == tenantId);
    }
}
