using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Xunit;

namespace ImpactSpace.Core.Skills;

public class SkillGroupAppService_Tests : CoreApplicationTestBase
{
    private readonly ISkillGroupAppService _skillGroupAppService;

    public SkillGroupAppService_Tests()
    {
        _skillGroupAppService = GetRequiredService<ISkillGroupAppService>();
    }
    
    [Fact]
    public async Task Should_Get_SkillGroup_ById()
    {
        var id = (await _skillGroupAppService.GetListAsync(new GetSkillGroupListDto())).Items.First().Id;

        var result = await _skillGroupAppService.GetAsync(id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
    }

    [Fact]
    public async Task Should_Not_Get_SkillGroup_With_Wrong_Id()
    {
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
        {
            await _skillGroupAppService.GetAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Get_All_SkillGroups_Without_Any_Filter()
    {
        var result = await _skillGroupAppService.GetListAsync(new GetSkillGroupListDto());

        result.TotalCount.ShouldBeGreaterThanOrEqualTo(4);
        result.Items.ShouldContain(x => x.Name == "Activism");
        result.Items.ShouldContain(x => x.Name == "Communication");
        result.Items.ShouldContain(x => x.Name == "Consulting");
        result.Items.ShouldContain(x => x.Name == "Creative");
    }

    [Fact]
    public async Task Should_Get_Filtered_SkillGroups()
    {
        var result = await _skillGroupAppService.GetListAsync(new GetSkillGroupListDto { Filter = "Communication" });

        result.TotalCount.ShouldBe(1);
        result.Items.ShouldContain(x => x.Name == "Communication");
    }

    [Fact]
    public async Task Should_Get_SkillGroups_With_Skills()
    {
        var result = await _skillGroupAppService.GetListAsync(new GetSkillGroupListDto { IncludeSkills = true });

        result.TotalCount.ShouldBeGreaterThanOrEqualTo(4);
        result.Items.ShouldContain(x => x.Name == "Activism" && x.Skills.Count > 0);
    }

    [Fact]
    public async Task Should_Create_SkillGroup()
    {
        var result = await _skillGroupAppService.CreateAsync(new CreateSkillGroupDto
        {
            Name = "Test Skill Group",
            Description = "Test Skill Group Description"
        });

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("Test Skill Group");
        result.Description.ShouldBe("Test Skill Group Description");
    }

    [Fact]
    public async Task Should_Not_Allow_To_Create_SkillGroup_With_Same_Name()
    {
        await Should.ThrowAsync<SkillGroupAlreadyExistsException>(async () =>
        {
            await _skillGroupAppService.CreateAsync(new CreateSkillGroupDto
            {
                Name = "Activism",
                Description = "Test Skill Group Description"
            });
        });
    }

    [Fact]
    public async Task Should_Update_SkillGroup()
    {
        var result = await _skillGroupAppService.CreateAsync(new CreateSkillGroupDto
        {
            Name = "Test Skill Group",
            Description = "Test Skill Group Description"
        });

        await _skillGroupAppService.UpdateAsync(result.Id, new UpdateSkillGroupDto
        {
            Name = "Test Skill Group Updated",
            Description = "Test Skill Group Description Updated"
        });

        var updatedResult = await _skillGroupAppService.GetAsync(result.Id);

        updatedResult.ShouldNotBeNull();
        updatedResult.Id.ShouldBe(result.Id);
        updatedResult.Name.ShouldBe("Test Skill Group Updated");
        updatedResult.Description.ShouldBe("Test Skill Group Description Updated");
    }

    [Fact]
    public async Task Should_Not_Allow_To_Update_SkillGroup_With_Same_Name()
    {
        var result = await _skillGroupAppService.CreateAsync(new CreateSkillGroupDto
        {
            Name = "Test Skill Group",
            Description = "Test Skill Group Description"
        });

        await Should.ThrowAsync<SkillGroupAlreadyExistsException>(async () =>
        {
            await _skillGroupAppService.UpdateAsync(result.Id, new UpdateSkillGroupDto
            {
                Name = "Activism",
                Description = "Test Skill Group Description Updated"
            });
        });
    }

    [Fact]
    public async Task Should_Delete_SkillGroup()
    {
        var result = await _skillGroupAppService.CreateAsync(new CreateSkillGroupDto
        {
            Name = "Test Skill Group",
            Description = "Test Skill Group Description"
        });

        await _skillGroupAppService.DeleteAsync(result.Id);

        await Should.ThrowAsync<EntityNotFoundException>(async () =>
        {
            await _skillGroupAppService.GetAsync(result.Id);
        });
    }

    [Fact]
    public async Task Should_Not_Allow_To_Delete_SkillGroup_With_Skills()
    {
        await Should.ThrowAsync<SkillGroupHasSkillsException>(async () =>
        {
            var existingSkillGroups = await _skillGroupAppService.GetListAsync(new GetSkillGroupListDto());
            var skillGroup = existingSkillGroups.Items.FirstOrDefault();
            if (skillGroup != null)
            {
                await _skillGroupAppService.DeleteAsync(skillGroup.Id);
            }
        });
    }
}