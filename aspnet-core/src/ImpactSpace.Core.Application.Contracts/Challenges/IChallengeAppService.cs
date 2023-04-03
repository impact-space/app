using System;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Challenges;

public interface IChallengeAppService: ICrudAppService<
    ChallengeDto,
    Guid,
    GetChallengeListDto,
    ChallengeCreateDto,
    ChallengeUpdateDto>
{
}