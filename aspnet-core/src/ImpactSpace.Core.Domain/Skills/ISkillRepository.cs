using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Skills;

public interface ISkillRepository : IRepository<Skill, Guid>
{
    Task<Skill> FindByNameAsync(string name);

    Task<List<Skill>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null,
        Guid? skillGroupId = null
    );
    
    Task<List<Skill>> GetSkillsBySkillGroupAsync(Guid skillGroupId);
    
    Task<List<Skill>> GetSkillsByOrganizationMemberAsync(Guid organizationMemberId);
    
    Task<List<Skill>> GetSkillsByProjectAsync(Guid projectId);
}