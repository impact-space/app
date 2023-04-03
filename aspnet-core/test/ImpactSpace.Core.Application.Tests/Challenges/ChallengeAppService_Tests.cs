using System;
using System.Linq;
using System.Threading.Tasks;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Projects;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace ImpactSpace.Core.Challenges;

public class ChallengeAppService_Tests : CoreApplicationTestBase
{
    private readonly IChallengeAppService _challengeAppService;

    public ChallengeAppService_Tests()
    {
        _challengeAppService = GetRequiredService<IChallengeAppService>();
    }
    
    [Fact]
    public async Task Should_Get_Challenge_ById()
    {
        var id = (await _challengeAppService.GetListAsync(new GetChallengeListDto())).Items.First().Id;

        var result = await _challengeAppService.GetAsync(id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
    }

    [Fact]
    public async Task Should_Not_Get_Challenge_With_Wrong_Id()
    {
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
        {
            await _challengeAppService.GetAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Get_All_Challenges_Without_Any_Filter()
    {
        var result = await _challengeAppService.GetListAsync(new GetChallengeListDto());

        result.TotalCount.ShouldBeGreaterThanOrEqualTo(1);
        result.Items.ShouldContain(x => x.Name == "Cultural");
    }

    [Fact]
    public async Task Should_Get_Filtered_Challenges()
    {
        var result = await _challengeAppService.GetListAsync(new GetChallengeListDto { Filter = "Cultural" });

        result.TotalCount.ShouldBe(1);
        result.Items.ShouldContain(x => x.Name == "Cultural");
    }

    [Fact]
    public async Task Should_Create_Challenge()
    {
        var result = await _challengeAppService.CreateAsync(new ChallengeCreateDto
        {
            Name = "Test Challenge"
        });

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("Test Challenge");
    }
    
    [Fact]
    public async Task Should_Not_Allow_To_Create_Challenge_With_Same_Name()
    {
        await _challengeAppService.CreateAsync(new ChallengeCreateDto
        {
            Name = "Test Challenge"
        });

        await Should.ThrowAsync<ChallengeAlreadyExistsException>(async () =>
        {
            await _challengeAppService.CreateAsync(new ChallengeCreateDto
            {
                Name = "Test Challenge"
            });
        });
    }

    [Fact]
    public async Task Should_Update_Challenge()
    {
        var result = await _challengeAppService.CreateAsync(new ChallengeCreateDto
        {
            Name = "Test Challenge"
        });

        await _challengeAppService.UpdateAsync(result.Id, new ChallengeUpdateDto
        {
            Name = "Test Challenge Updated"
        });

        var updatedResult = await _challengeAppService.GetAsync(result.Id);

        updatedResult.ShouldNotBeNull();
        updatedResult.Id.ShouldBe(result.Id);
        updatedResult.Name.ShouldBe("Test Challenge Updated");
    }
    
    [Fact]
    public async Task Should_Not_Allow_To_Update_Challenge_With_Same_Name()
    {
        var result = await _challengeAppService.CreateAsync(new ChallengeCreateDto
        {
            Name = "Test Challenge 1"
        });

        await Should.ThrowAsync<ChallengeAlreadyExistsException>(async () =>
        {
            await _challengeAppService.UpdateAsync(result.Id, new ChallengeUpdateDto
            {
                Name = "Cultural"
            });
        });
    }

    [Fact]
    public async Task Should_Delete_Challenge()
    {
        var result = await _challengeAppService.CreateAsync(new ChallengeCreateDto
        {
            Name = "Test Challenge"
        });

        await _challengeAppService.DeleteAsync(result.Id);

        await Should.ThrowAsync<EntityNotFoundException>(async () =>
        {
            await _challengeAppService.GetAsync(result.Id);
        });
    }
}