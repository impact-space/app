using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Organizations;

public interface IOrganizationMemberRepository: IRepository<OrganizationMember, Guid>
{
    Task<List<OrganizationMember>> GetListAsync(
        Guid organizationId,
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
    
    Task<OrganizationMember> GetAsync(
        Guid id,
        bool includeDetails = true
    );
}