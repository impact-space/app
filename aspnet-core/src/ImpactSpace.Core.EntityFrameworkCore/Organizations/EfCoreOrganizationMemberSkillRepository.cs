using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImpactSpace.Core.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ImpactSpace.Core.Organizations;

public class EfCoreOrganizationMemberSkillRepository : EfCoreRepository<CoreDbContext, OrganizationMemberSkill>, IOrganizationMemberSkillRepository
{
    public EfCoreOrganizationMemberSkillRepository(IDbContextProvider<CoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<OrganizationMemberSkill>> GetListWithSkillsAsync(Guid memberId)
    {
        var dbSet = await GetDbSetAsync();

        var query = dbSet
            .Where(x => x.OrganizationMemberId == memberId)
            .Include(x => x.Skill);

        return await query.ToListAsync();
    }
}