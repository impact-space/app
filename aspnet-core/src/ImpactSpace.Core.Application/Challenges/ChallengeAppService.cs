using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core.Challenges;

[Authorize(CorePermissions.GlobalTypes.Challenges.Default)]
public class ChallengeAppService : CoreAppService,
    IChallengeAppService
{
    private readonly IChallengeRepository _challengeRepository;
    private readonly ChallengeManager _challengeManager;

    public ChallengeAppService(
        IChallengeRepository challengeRepository, 
        ChallengeManager challengeManager)
    {
        _challengeRepository = challengeRepository;
        _challengeManager = challengeManager;
    }

    public async Task<ChallengeDto> GetAsync(Guid id)
    {
        var challenge = await _challengeRepository.GetAsync(id);
        return ObjectMapper.Map<Challenge, ChallengeDto>(challenge);
    }

    public async Task<PagedResultDto<ChallengeDto>> GetListAsync(GetChallengeListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Challenge.Name);
        }

        var challenges = await _challengeRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
            ? await _challengeRepository.CountAsync()
            : await _challengeRepository.CountAsync(challenge => challenge.Name.Contains(input.Filter));

        return new PagedResultDto<ChallengeDto>(
            totalCount,
            ObjectMapper.Map<List<Challenge>, List<ChallengeDto>>(challenges)
        );
    }

    [Authorize(CorePermissions.GlobalTypes.Challenges.Create)]
    public async Task<ChallengeDto> CreateAsync(ChallengeCreateDto input)
    {
        var challenge = await _challengeManager.CreateAsync(input.Name);

        await _challengeRepository.InsertAsync(challenge);

        return ObjectMapper.Map<Challenge, ChallengeDto>(challenge);
    }

    [Authorize(CorePermissions.GlobalTypes.Challenges.Edit)]
    public async Task<ChallengeDto> UpdateAsync(Guid id, ChallengeUpdateDto input)
    {
        var challenge = await _challengeRepository.GetAsync(id);

        if (challenge.Name != input.Name)
        {
            await _challengeManager.ChangeNameAsync(challenge, input.Name);
        }

        await _challengeRepository.UpdateAsync(challenge);

        return ObjectMapper.Map<Challenge, ChallengeDto>(challenge);
    }
    
    [Authorize(CorePermissions.GlobalTypes.Challenges.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _challengeManager.DeleteAsync(id);
    }
}