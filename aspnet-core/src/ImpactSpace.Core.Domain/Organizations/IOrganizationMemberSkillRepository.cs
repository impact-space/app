using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Organizations;

public interface IOrganizationMemberSkillRepository : IRepository<OrganizationMemberSkill> 
{
    Task<List<OrganizationMemberSkill>> GetListWithSkillsAsync(Guid memberId);
}