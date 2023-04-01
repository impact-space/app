using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImpactSpace.Core.Skills;
using ImpactSpace.Core.Permissions;
using ImpactSpace.Core.Localization;
using Blazorise;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class Skills
{
    private IReadOnlyList<SkillGroupDto> SkillGroupList { get; set; } = Array.Empty<SkillGroupDto>();
    
    private string FilterText { get; set; } = string.Empty;
    
    private Guid? SelectedSkillGroupId { get; set; }
    protected PageToolbar Toolbar { get; } = new();

    protected List<TableColumn> SkillManagementTableColumns => TableColumns.Get<Skills>();

    public Skills()
    {
        LocalizationResource = typeof(CoreResource);
        CreatePolicyName = CorePermissions.GlobalTypes.Skills.Create;
        UpdatePolicyName = CorePermissions.GlobalTypes.Skills.Edit;
        DeletePolicyName = CorePermissions.GlobalTypes.Skills.Delete;
    }
    
    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:SkillManagement"].Value));
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Skills"].Value));
        return base.SetBreadcrumbItemsAsync();
    }
    
    private async Task GetSkillGroupsAsync()
    {
        SkillGroupList = (await SkillGroupAppService.GetListAsync(new GetSkillGroupListDto
        {
            Sorting = "Name", 
            MaxResultCount = 1000
        })).Items;
    }
    
    protected override async Task OnInitializedAsync()
    {
        await GetSkillGroupsAsync();
        await base.OnInitializedAsync();
    }

    protected override Task UpdateGetListInputAsync()
    {
        GetListInput.Filter = FilterText;
        GetListInput.SkillGroupId = SelectedSkillGroupId;
        return base.UpdateGetListInputAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<Skills>()
            .AddRange(new EntityAction[]
            {
                new EntityAction
                {
                    Text = L["Edit"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) => { await OpenEditModalAsync(data.As<SkillDto>()); }
                },
                new EntityAction
                {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteEntityAsync(data.As<SkillDto>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<SkillDto>())
                }
            });

        return base.SetEntityActionsAsync();
    }
    
    protected override ValueTask SetTableColumnsAsync()
    {
        SkillManagementTableColumns
            .AddRange(new[]
            {
                new TableColumn
                {
                    Title = L["Actions"],
                    Actions = EntityActions.Get<Skills>(),
                },
                new TableColumn
                {
                    Title = L["Skill"],
                    Sortable = true,
                    Data = nameof(SkillDto.Name),
                },
                new TableColumn
                {
                    Title = L["SkillGroup"],
                    Sortable = false,
                    Data = nameof(SkillDto.SkillGroupId),
                    ValueConverter = (value) => SkillGroupList.FirstOrDefault(x => x.Id == ((SkillDto)value).SkillGroupId)?.Name
                }
            });

        return base.SetTableColumnsAsync();
    }
    
    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewSkill"], OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }
    
    private async Task OnFilterTextChanged(string newFilterText)
    {
        FilterText = newFilterText;
        CurrentPage = 1;
        await GetEntitiesAsync();
    }
    
    private async Task OnSkillGroupFilterChanged(Guid? newSkillGroupId)
    {
        SelectedSkillGroupId = newSkillGroupId;
        CurrentPage = 1;
        await GetEntitiesAsync();
    }
}