using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImpactSpace.Core.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ImpactSpace.Core.Organizations;

public class EfCoreOrganizationMemberRepository: EfCoreRepository<CoreDbContext, OrganizationMember, Guid>, IOrganizationMemberRepository
{
    public EfCoreOrganizationMemberRepository(IDbContextProvider<CoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<OrganizationMember>> GetListAsync(Guid organizationId)
    {
        return await DbSet
            .Where(om => om.OrganizationId == organizationId)
            .ToListAsync();
    }
}