using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Organizations;

public interface IOrganizationMemberRepository: IRepository<OrganizationMember, Guid>
{
    Task<List<OrganizationMember>> GetListAsync(Guid organizationId);
}