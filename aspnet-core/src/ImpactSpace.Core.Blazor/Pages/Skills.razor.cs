using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImpactSpace.Core.Skills;
using ImpactSpace.Core.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class Skills
{
    private IReadOnlyList<SkillDto> SkillList { get; set; }
    private IReadOnlyList<SkillGroupDto> SkillGroupList { get; set; } = Array.Empty<SkillGroupDto>();
    
    private string FilterText { get; set; } = string.Empty;
    
    private Guid? SelectedSkillGroupId { get; set; }
    
    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }
    
    private bool CanCreateSkill { get; set; }
    private bool CanEditSkill { get; set; }
    private bool CanDeleteSkill { get; set; }
    
    private CreateSkillDto NewSkill { get; set; }
    
    private Guid EditingSkillId { get; set; }
    private UpdateSkillDto EditingSkill { get; set; }
    
    private Modal CreateSkillModal { get; set; }
    private Modal EditSkillModal { get; set; }

    protected Validations CreateValidationsRef;
    
    protected Validations EditValidationsRef;
    
    public Skills()
    {
        NewSkill = new CreateSkillDto();
        EditingSkill = new UpdateSkillDto();
    }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetPermissionsAsync();
        await GetSkillsAsync();
        await GetSkillGroupsAsync();
    }
    
    private async Task GetSkillGroupsAsync()
    {
        SkillGroupList = (await SkillGroupAppService.GetListAsync(new GetSkillGroupListDto
        {
            Sorting = "Name", 
            MaxResultCount = 1000
        })).Items;
    }
    
    private async Task SetPermissionsAsync() 
    {
        CanCreateSkill = await AuthorizationService.IsGrantedAsync(CorePermissions.Skills.Create);
        CanEditSkill = await AuthorizationService.IsGrantedAsync(CorePermissions.Skills.Edit);
        CanDeleteSkill = await AuthorizationService.IsGrantedAsync(CorePermissions.Skills.Delete);
    }
    
    private async Task OnFilterTextChanged(string newFilterText)
    {
        FilterText = newFilterText;
        CurrentPage = 0;
        await GetSkillsAsync();
    }
    
    private async Task OnSkillGroupFilterChanged(Guid? newSkillGroupId)
    {
        SelectedSkillGroupId = newSkillGroupId;
        CurrentPage = 0;
        await GetSkillsAsync();
    }
    
    private async Task GetSkillsAsync()
    {
        var result = await SkillAppService.GetListAsync(new GetSkillListDto
        {
            SkipCount = CurrentPage * PageSize,
            MaxResultCount = PageSize,
            Sorting = CurrentSorting,
            Filter = FilterText,
            SkillGroupId = SelectedSkillGroupId
        });
        
        SkillList = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<SkillDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetSkillsAsync();
        
        await InvokeAsync(StateHasChanged);
    }
    
    private void OpenCreateSkillModal()
    {
        CreateValidationsRef.ClearAll();
        
        NewSkill = new CreateSkillDto();
        CreateSkillModal.Show();
    }
    
    private void CloseCreateSkillModal()
    {
        CreateSkillModal.Hide();
    }
    
    private void OpenEditSkillModal(SkillDto Skill)
    {
        EditValidationsRef.ClearAll();
        
        EditingSkillId = Skill.Id;
        EditingSkill = ObjectMapper.Map<SkillDto, UpdateSkillDto>(Skill);
        EditSkillModal.Show();
    }
    
    private async Task DeleteSkillAsync(SkillDto Skill)
    {
        var confirmMessage = L["SkillDeletionConfirmationMessage", Skill.Name];
        if (!await Message.Confirm(confirmMessage))
        {
            return;
        }
        
        await SkillAppService.DeleteAsync(Skill.Id);
            
        await GetSkillsAsync();
    }
    
    private void CloseEditSkillModal()
    {
        EditSkillModal.Hide();
    }
    
    private async Task CreateSkillAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await SkillAppService.CreateAsync(NewSkill);
            await GetSkillsAsync();
            CloseCreateSkillModal();
        }
    }
    
    private async Task UpdateSkillAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await SkillAppService.UpdateAsync(EditingSkillId, EditingSkill);
            await GetSkillsAsync();
            CloseEditSkillModal();
        }
    }
}