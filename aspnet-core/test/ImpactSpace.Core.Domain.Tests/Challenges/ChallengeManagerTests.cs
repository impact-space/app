using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ImpactSpace.Core.Challenges;

public class ChallengeManagerTests : CoreDomainTestBase
{
    private readonly ChallengeManager _challengeManager;
    private readonly IChallengeRepository _challengeRepository;

    public ChallengeManagerTests()
    {
        _challengeManager = GetRequiredService<ChallengeManager>();
        _challengeRepository = GetRequiredService<IChallengeRepository>();
    }

    [Fact]
    public async Task Should_Create_Challenge()
    {
        var challenge = await _challengeManager.CreateAsync("Test Challenge");
        challenge.ShouldNotBeNull();
        challenge.Id.ShouldNotBe(Guid.Empty);
        challenge.Name.ShouldBe("Test Challenge");
    }

    [Fact]
    public async Task Should_Not_Allow_To_Create_Challenge_With_Same_Name()
    {
        await _challengeManager.CreateAsync("Test Challenge 1");

        await Should.ThrowAsync<ChallengeAlreadyExistsException>(async () =>
        {
            await _challengeManager.CreateAsync("Test Challenge 1");
        });
    }

    [Fact]
    public async Task Should_Change_Challenge_Name()
    {
        var challenge = await _challengeManager.CreateAsync("Test Challenge 2");

        await _challengeManager.ChangeNameAsync(challenge, "Changed Test Challenge 2");
        await _challengeRepository.UpdateAsync(challenge);
        
        var updatedChallenge = await _challengeRepository.GetAsync(challenge.Id);
        updatedChallenge.Name.ShouldBe("Changed Test Challenge 2");
    }

    [Fact]
    public async Task Should_Not_Allow_To_Change_Challenge_Name_To_Same_Name()
    {
        var challenge1 = await _challengeManager.CreateAsync("Test Challenge 3");
        await _challengeManager.CreateAsync("Test Challenge 4");

        await Should.ThrowAsync<ChallengeAlreadyExistsException>(async () =>
        {
            await _challengeManager.ChangeNameAsync(challenge1, "Test Challenge 4");
        });
    }
}