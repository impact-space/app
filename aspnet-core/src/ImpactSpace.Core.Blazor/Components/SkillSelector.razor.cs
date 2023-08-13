using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using ImpactSpace.Core.Skills;
using Microsoft.AspNetCore.Components;

namespace ImpactSpace.Core.Blazor.Components;

public partial class SkillSelector : IValidationInput
{
    [Inject]
    public ISkillAppService SkillAppService { get; set; }
    
    [Parameter]
    public EventCallback<SkillDto> OnSkillSelected { get; set; }
    
    [Parameter]
    public SkillDto SelectedSkill { get; set; } = new SkillDto { Name = string.Empty };
    
    [Parameter]
    public Guid Value { get; set; } = Guid.Empty;
    
    [Parameter]
    public EventCallback<Guid> ValueChanged { get; set; }

    public object ValidationValue => Value;

    [Parameter]
    public bool Disabled { get; set; }
    
    private List<SkillDto> MatchingSkills { get; set; } = new();
    
    private bool IsLoading { get; set; }
    
    private string ErrorMessage { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task OnSelectedSkill(SkillDto skill)
    {
        if (skill != null)
        {
            SelectedSkill = skill;
            Value = skill.Id;
            await ValueChanged.InvokeAsync(Value);
            await OnSkillSelected.InvokeAsync(skill);
            MatchingSkills = new(); // Clear the list as we've selected a skill
        }
    }

    private async Task OnSearchChanged(string searchTerm)
    {
        try
        {
            IsLoading = true;
            MatchingSkills = await SkillAppService.SearchSkillsAsync(searchTerm);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }
}