using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Xunit;

namespace ImpactSpace.Core.Skills;

public class SkillAppService_Tests : CoreApplicationTestBase
{
    private readonly ISkillAppService _skillAppService;
    private readonly ISkillGroupAppService _skillGroupAppService;

    public SkillAppService_Tests()
    {
        _skillAppService = GetRequiredService<ISkillAppService>();
        _skillGroupAppService = GetRequiredService<ISkillGroupAppService>();
    }
    
    [Fact]
    public async Task Should_Get_Skill_ById()
    {
        var id = (await _skillAppService.GetListAsync(new GetSkillListDto())).Items.First().Id;

        var result = await _skillAppService.GetAsync(id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
    }
    
    [Fact]
    public async Task Should_Not_Get_Skill_With_Wrong_Id()
    {
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
        {
            await _skillAppService.GetAsync(Guid.NewGuid());
        });
    }
    
    [Fact]
    public async Task Should_Get_All_Skills_Without_Any_Filter()
    {
        var result = await _skillAppService.GetListAsync(new GetSkillListDto());
      
        result.Items.Count.ShouldBeGreaterThanOrEqualTo(1);
        result.Items.ShouldContain(x => x.Name == ".NET");
    }
    
    [Fact]
    public async Task Should_Get_Filtered_Skills()
    {
        var result = await _skillAppService.GetListAsync(new GetSkillListDto { Filter = ".NE" });
      
        result.Items.Count.ShouldBe(1);
        
        result.Items.ShouldContain(x => x.Name == ".NET");
    }
    
    [Fact]
    public async Task Should_Get_Skills_With_SkillGroup_Without_Any_Filter()
    {
        var skillGroupId = (await _skillAppService.GetListAsync(new GetSkillListDto())).Items.First().SkillGroupId;
        var result = await _skillAppService.GetListAsync(new GetSkillListDto { SkillGroupId = skillGroupId });

        foreach (var skillDto in result.Items)
        {
            skillDto.SkillGroupId.ShouldBe(skillGroupId);
        }
    }
    
    [Fact]
    public async Task Should_Get_Filtered_Skills_With_SkillGroup()
    {
        var skillGroup = (await _skillGroupAppService.GetListAsync(new GetSkillGroupListDto { Filter = "Non" } )).Items.First();
        var result = await _skillAppService.GetListAsync(new GetSkillListDto { SkillGroupId = skillGroup.Id, Filter = "Activ" });
      
        result.Items.Count.ShouldBe(1);
        result.Items.ShouldContain(x => x.Name == "Activist");
    }

    [Fact]
    public async Task Should_Create_Skill()
    {
        var result = await _skillAppService.CreateAsync(new SkillCreateDto
        {
            Name = "Test Skill",
            SkillGroupId = (await _skillAppService.GetListAsync(new GetSkillListDto())).Items.First().SkillGroupId
        });
      
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Test Skill");
    }
    
    [Fact]
    public async Task Should_Not_Create_Skill_With_The_Same_Name()
    {
        var skill = (await _skillAppService.GetListAsync(new GetSkillListDto())).Items.First();
        
        await Should.ThrowAsync<SkillAlreadyExistsException>(async () =>
        {
            await _skillAppService.CreateAsync(new SkillCreateDto
            {
                Name = skill.Name,
                SkillGroupId = skill.SkillGroupId
            });
        });
    }
    
    [Fact]
    public async Task Should_Not_Create_Skill_With_Wrong_SkillGroupId()
    {
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
        {
            await _skillAppService.CreateAsync(new SkillCreateDto
            {
                Name = "Test Skill",
                SkillGroupId = Guid.NewGuid()
            });
        });
    }
    
    [Fact]
    public async Task Should_Update_Skill()
    {
        var skill = (await _skillAppService.GetListAsync(new GetSkillListDto())).Items.First(); 
        
        var skillGroup = (await _skillGroupAppService.GetListAsync(new GetSkillGroupListDto { Filter = "Non-Tech Skills" } )).Items.First();
        
        await _skillAppService.UpdateAsync(skill.Id, new SkillUpdateDto
        {
            Name = "Test Skill",
            SkillGroupId = skillGroup.Id
        });
        
        var result = await _skillAppService.GetAsync(skill.Id);
        
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Test Skill");
        result.SkillGroupId.ShouldBe(skillGroup.Id);
    }
    
    [Fact]
    public async Task Should_Not_Update_Skill_With_The_Same_Name()
    {
        var skill = (await _skillAppService.GetListAsync(new GetSkillListDto())).Items.First(); 
        
        var skillGroup = (await _skillGroupAppService.GetListAsync(new GetSkillGroupListDto { Filter = "Non-Tech Skills" } )).Items.First();
        
        await Should.ThrowAsync<SkillAlreadyExistsException>(async () =>
        {
            await _skillAppService.UpdateAsync(skill.Id, new SkillUpdateDto
            {
                Name = "Elected Official",
                SkillGroupId = skillGroup.Id
            });
        });
    }
    
    [Fact]
    public async Task Should_Not_Update_Skill_With_Wrong_SkillGroupId()
    {
        var skill = (await _skillAppService.GetListAsync(new GetSkillListDto())).Items.First(); 
        
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
        {
            await _skillAppService.UpdateAsync(skill.Id, new SkillUpdateDto
            {
                Name = "Test Skill",
                SkillGroupId = Guid.NewGuid()
            });
        });
    }
    
    [Fact]
    public async Task Should_Delete_Skill()
    {
        var skill = (await _skillAppService.GetListAsync(new GetSkillListDto())).Items.First(); 
        
        await _skillAppService.DeleteAsync(skill.Id);
        
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
        {
            await _skillAppService.GetAsync(skill.Id);
        });
    }

    [Fact]
    public async Task Should_Not_Delete_Skill_With_Wrong_Id()
    {
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
        {
            await _skillAppService.DeleteAsync(Guid.NewGuid());
        });
    }
}

