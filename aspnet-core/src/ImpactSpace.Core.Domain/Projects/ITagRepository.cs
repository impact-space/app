using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Projects;

public interface ITagRepository : IRepository<Tag, Guid>
{
    Task<Tag> FindByNameAsync(string name);
    
    Task<List<Tag>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
    
    Task<List<Tag>> GetTagsByProjectAsync(Guid projectId);
}