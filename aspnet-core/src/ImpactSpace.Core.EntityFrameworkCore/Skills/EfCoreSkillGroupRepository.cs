using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ImpactSpace.Core.Skills;

public class EfCoreSkillGroupRepository
    : EfCoreRepository<CoreDbContext, SkillGroup, Guid>,
        ISkillGroupRepository
{
    public EfCoreSkillGroupRepository(
        IDbContextProvider<CoreDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async Task<SkillGroup> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(skillGroup => skillGroup.Name == name);
    }

    public async Task<List<SkillGroup>> GetListAsync(
        int skipCount, 
        int maxResultCount, 
        string sorting, 
        string filter = null,
        bool includeSkills = false)
    {
        var dbSet = await GetDbSetAsync();
        
        return await dbSet
            .IncludeIf(includeSkills, skillGroup => skillGroup.Skills)
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                skillGroup => skillGroup.Name.Contains(filter)
            )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<SkillGroup> GetSkillGroupBySkillAsync(Guid skillId)
    {
        var dbSet = await GetDbSetAsync();

        // Find the SkillGroup that contains the specified Skill
        return await dbSet
            .Include(skillGroup => skillGroup.Skills)
            .FirstOrDefaultAsync(skillGroup => skillGroup.Skills.Any(skill => skill.Id == skillId));
    }
}