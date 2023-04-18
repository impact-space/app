using System;
using System.Threading.Tasks;
using ImpactSpace.Core.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ImpactSpace.Core.Organizations;

public class EfCoreOrganizationProfileRepository : EfCoreRepository<CoreDbContext, OrganizationProfile, Guid>, IOrganizationProfileRepository
{
    public EfCoreOrganizationProfileRepository(IDbContextProvider<CoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<OrganizationProfile> GetByOrganizationIdAsync(Guid organizationId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(organizationProfile => organizationProfile.OrganizationId == organizationId);
    }
}