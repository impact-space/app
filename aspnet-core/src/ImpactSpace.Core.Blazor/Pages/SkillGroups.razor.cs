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

public partial class SkillGroups
{
    private IReadOnlyList<SkillGroupDto> SkillGroupList { get; set; }

    private string FilterText { get; set; } = string.Empty;
    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }
    
    private bool CanCreateSkillGroup { get; set; }
    private bool CanEditSkillGroup { get; set; }
    private bool CanDeleteSkillGroup { get; set; }
    
    private SkillGroupCreateDto New { get; set; }
    
    private Guid EditingSkillGroupId { get; set; }
    private SkillGroupUpdateDto Editing { get; set; }
    
    private Modal CreateSkillGroupModal { get; set; }
    private Modal EditSkillGroupModal { get; set; }

    protected Validations CreateValidationsRef;
    
    protected Validations EditValidationsRef;
    
    public SkillGroups()
    {
        New = new SkillGroupCreateDto();
        Editing = new SkillGroupUpdateDto();
    }
    
    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
    }
    
    private async Task SetPermissionsAsync() 
    {
        CanCreateSkillGroup = await AuthorizationService.IsGrantedAsync(CorePermissions.GlobalTypes.SkillGroups.Create);
        CanEditSkillGroup = await AuthorizationService.IsGrantedAsync(CorePermissions.GlobalTypes.SkillGroups.Edit);
        CanDeleteSkillGroup = await AuthorizationService.IsGrantedAsync(CorePermissions.GlobalTypes.SkillGroups.Delete);
    }
    
    private async Task OnFilterTextChanged(string newFilterText)
    {
        FilterText = newFilterText;
        CurrentPage = 0;
        await GetSkillGroupsAsync();
    }
    
    private async Task GetSkillGroupsAsync()
    {
        var result = await SkillGroupAppService.GetListAsync(new GetSkillGroupListDto
        {
            SkipCount = CurrentPage * PageSize,
            MaxResultCount = PageSize,
            Sorting = CurrentSorting,
            Filter = FilterText
        });
        
        SkillGroupList = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<SkillGroupDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetSkillGroupsAsync();
        
        await InvokeAsync(StateHasChanged);
    }
    
    private void OpenCreateSkillGroupModal()
    {
        CreateValidationsRef.ClearAll();
        
        New = new SkillGroupCreateDto();
        CreateSkillGroupModal.Show();
    }
    
    private void CloseCreateSkillGroupModal()
    {
        CreateSkillGroupModal.Hide();
    }
    
    private void OpenEditSkillGroupModal(SkillGroupDto skillGroup)
    {
        EditValidationsRef.ClearAll();
        
        EditingSkillGroupId = skillGroup.Id;
        Editing = ObjectMapper.Map<SkillGroupDto, SkillGroupUpdateDto>(skillGroup);
        EditSkillGroupModal.Show();
    }
    
    private async Task DeleteSkillGroupAsync(SkillGroupDto skillGroup)
    {
        var confirmMessage = L["SkillGroupDeletionConfirmationMessage", skillGroup.Name];
        if (!await Message.Confirm(confirmMessage))
        {
            return;
        }
        
        await SkillGroupAppService.DeleteAsync(skillGroup.Id);
            
        await GetSkillGroupsAsync();
    }
    
    private void CloseEditSkillGroupModal()
    {
        EditSkillGroupModal.Hide();
    }
    
    private async Task CreateSkillGroupAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await SkillGroupAppService.CreateAsync(New);
            await GetSkillGroupsAsync();
            CloseCreateSkillGroupModal();
        }
    }
    
    private async Task UpdateSkillGroupAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await SkillGroupAppService.UpdateAsync(EditingSkillGroupId, Editing);
            await GetSkillGroupsAsync();
            CloseEditSkillGroupModal();
        }
    }
}