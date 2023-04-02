using System;
using System.Threading.Tasks;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Projects;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Challenges;

public class ChallengeManager : DomainService
{
    private readonly IChallengeRepository _challengeRepository;
    private readonly IRepository<OrganizationMemberChallenge> _organizationMemberChallengeRepository;
    private readonly IRepository<ProjectChallenge> _projectChallengeRepository;

    public ChallengeManager(
        IChallengeRepository challengeRepository, 
        IRepository<OrganizationMemberChallenge> organizationMemberChallengeRepository, 
        IRepository<ProjectChallenge> projectChallengeRepository)
    {
        _challengeRepository = challengeRepository;
        _organizationMemberChallengeRepository = organizationMemberChallengeRepository;
        _projectChallengeRepository = projectChallengeRepository;
    }
    
    public async Task<Challenge> CreateAsync([NotNull] string name)
    {
        Check.NotNullOrWhiteSpace(
            name, 
            nameof(name),
            ChallengeConstants.MaxNameLength
        );

        // Check if a Challenge with the given name already exists
        var existingChallenge = await _challengeRepository.FindByNameAsync(name);
        
        if (existingChallenge != null)
        {
            throw new ChallengeAlreadyExistsException(name);
        }

        var challenge = new Challenge(GuidGenerator.Create(), name);
        await _challengeRepository.InsertAsync(challenge);

        return challenge;
    }
    
    public async Task ChangeNameAsync(
        [NotNull] Challenge challenge,
        [NotNull] string newName)
    {
        Check.NotNull(challenge, nameof(challenge));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        var existingChallenge = await _challengeRepository.FindByNameAsync(newName);
        if (existingChallenge != null && existingChallenge.Id != challenge.Id)
        {
            throw new ChallengeAlreadyExistsException(newName);
        }

        challenge.ChangeName(newName);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var challenge = await _challengeRepository.GetAsync(id);
        
        await _projectChallengeRepository.DeleteAsync(x => x.ChallengeId == challenge.Id);
        await _organizationMemberChallengeRepository.DeleteAsync(x => x.ChallengeId == challenge.Id);
        await _challengeRepository.DeleteAsync(challenge);
    }
}