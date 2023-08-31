using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Components;
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

    [Parameter]
    public EventCallback<Guid> TextChanged { get; set; }

    public object ValidationValue => Value;

    [Parameter]
    public bool Disabled { get; set; }
    
    [Parameter]
    public List<Guid> ExcludeSkillIds { get; set; } = new();
    
    private List<SkillDto> MatchingSkills { get; set; } = new();
    
    private bool IsLoading { get; set; }
    
    private string ErrorMessage { get; set; }
    private Autocomplete<SkillDto,Guid> AutoCompleteRef { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task OnValueChanged(Guid skillId)
    {
        Value = skillId;
        await ValueChanged.InvokeAsync(Value);
    }

    private async Task OnTextChanged(string skillText)
    {
        await TextChanged.InvokeAsync(Value);
    }
    
    public void ResetSelection()
    {
        AutoCompleteRef.Clear();
    }

    private async Task OnSearchChanged(string searchTerm)
    {
        try
        {
            IsLoading = true;
            var allMatchingSkills = await SkillAppService.SearchSkillsAsync(searchTerm);
            MatchingSkills = allMatchingSkills.Where(skill => !ExcludeSkillIds.Contains(skill.Id)).ToList();        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private void Validator(ValidatorEventArgs obj)
    {
        obj.Status = Value != Guid.Empty ? ValidationStatus.Success : ValidationStatus.Error;
    }
}