using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Skills;

public interface ISkillGroupRepository : IRepository<SkillGroup, Guid>
{
    Task<SkillGroup> FindByNameAsync(string name);

    Task<List<SkillGroup>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
    
    Task<List<SkillGroup>> GetListWithSkillsAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
    
    Task<SkillGroup> GetSkillGroupBySkillAsync(Guid skillId);
}