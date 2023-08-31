using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImpactSpace.Core.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ImpactSpace.Core.Organizations;

public class EfCoreOrganizationMemberRepository: EfCoreRepository<CoreDbContext, OrganizationMember, Guid>, IOrganizationMemberRepository
{
    public EfCoreOrganizationMemberRepository(IDbContextProvider<CoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<OrganizationMember>> GetListAsync(Guid organizationId, int skipCount, int maxResultCount, string sorting, string filter = null)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .Where(organizationMember => organizationMember.OrganizationId == organizationId)
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                organizationMember => organizationMember.Name.Contains(filter)
            )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async Task<OrganizationMember> GetAsync(Guid id, bool includeDetails = true)
    {
        var dbSet = await GetDbSetAsync();

        var query = dbSet
            .Where(organizationMember => organizationMember.Id == id)
            .IncludeIf(includeDetails,x => x.OrganizationMemberSkills)
            .IncludeIf(includeDetails,x => x.OrganizationMemberProjects)
            .IncludeIf(includeDetails,x => x.OrganizationMemberActions)
            .IncludeIf(includeDetails,x => x.OrganizationMemberChallenges);
        
        return await query.FirstOrDefaultAsync();
    }
}