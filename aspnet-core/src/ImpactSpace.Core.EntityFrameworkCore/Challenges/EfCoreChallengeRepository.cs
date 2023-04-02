using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ImpactSpace.Core.Challenges;

public class EfCoreChallengeRepository : EfCoreRepository<CoreDbContext, Challenge, Guid>, IChallengeRepository
{
    public EfCoreChallengeRepository(IDbContextProvider<CoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<Challenge> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(challenge => challenge.Name == name);
    }

    public async Task<List<Challenge>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                challenge => challenge.Name.Contains(filter)
            )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }
}