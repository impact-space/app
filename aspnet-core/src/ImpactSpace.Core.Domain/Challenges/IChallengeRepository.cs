using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Challenges;

public interface IChallengeRepository: IRepository<Challenge, Guid>
{
    Task<Challenge> FindByNameAsync(string name);
    
    Task<List<Challenge>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
}
