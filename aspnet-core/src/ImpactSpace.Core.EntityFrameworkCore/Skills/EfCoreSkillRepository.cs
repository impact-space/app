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

public class EfCoreSkillRepository
    : EfCoreRepository<CoreDbContext, Skill, Guid>,
        ISkillRepository
{
    public EfCoreSkillRepository(
        IDbContextProvider<CoreDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async Task<Skill> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(skill => skill.Name == name);
    }

    public async Task<List<Skill>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                skill => skill.Name.Contains(filter)
            )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<List<Skill>> GetSkillsBySkillGroupAsync(Guid skillGroupId)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .Where(skill => skill.SkillGroupId == skillGroupId)
            .ToListAsync();
    }

    public async Task<List<Skill>> GetSkillsByOrganizationMemberAsync(Guid organizationMemberId)
    {
        var dbSet = await GetDbSetAsync();

        // Assuming there is a navigation property in the Skill entity called OrganizationMemberSkills
        return await dbSet
            .Where(skill => skill.OrganizationMemberSkills.Any(oms => oms.OrganizationMemberId == organizationMemberId))
            .ToListAsync();
    }

    public async Task<List<Skill>> GetSkillsByProjectAsync(Guid projectId)
    {
        var dbSet = await GetDbSetAsync();

        // Assuming there is a navigation property in the Skill entity called ProjectSkills
        return await dbSet
            .Where(skill => skill.ProjectSkills.Any(ps => ps.ProjectId == projectId))
            .ToListAsync();
    }
}