using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ImpactSpace.Core.Projects;

public class EfCoreTagRepository
    : EfCoreRepository<CoreDbContext, Tag, Guid>, 
        ITagRepository        
{
    public EfCoreTagRepository(
        IDbContextProvider<CoreDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async Task<Tag> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(tag => tag.Name == name);
    }

    public async Task<List<Tag>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                tag => tag.Name.Contains(filter)
            )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<List<Tag>> GetTagsByProjectAsync(Guid projectId)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .Include(tag => tag.ProjectTags)
            .Where(tag => tag.ProjectTags.Any(projectTag => projectTag.ProjectId == projectId))
            .ToListAsync();
    }
}