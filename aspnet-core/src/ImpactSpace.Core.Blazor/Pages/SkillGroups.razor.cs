using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.Skills;
using ImpactSpace.Core.Permissions;
using ImpactSpace.Core.Localization;
using Blazorise;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class SkillGroups
{
    private string FilterText { get; set; } = string.Empty;

    protected PageToolbar Toolbar { get; } = new();

    protected List<TableColumn> SkillGroupManagementTableColumns => TableColumns.Get<SkillGroups>();

    public SkillGroups()
    {
        LocalizationResource = typeof(CoreResource);
        CreatePolicyName = CorePermissions.GlobalTypes.SkillGroups.Create;
        UpdatePolicyName = CorePermissions.GlobalTypes.SkillGroups.Edit;
        DeletePolicyName = CorePermissions.GlobalTypes.SkillGroups.Delete;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:SkillManagement"].Value));
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:SkillGroups"].Value));
        return base.SetBreadcrumbItemsAsync();
    }

    protected override Task UpdateGetListInputAsync()
    {
        GetListInput.Filter = FilterText;
        return base.UpdateGetListInputAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<SkillGroups>()
            .AddRange(new EntityAction[]
            {
                new EntityAction
                {
                    Text = L["Edit"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) => { await OpenEditModalAsync(data.As<SkillGroupDto>()); }
                },
                new EntityAction
                {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteEntityAsync(data.As<SkillGroupDto>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<SkillGroupDto>())
                }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        SkillGroupManagementTableColumns
            .AddRange(new[]
            {
                new TableColumn
                {
                    Title = L["Actions"],
                    Actions = EntityActions.Get<SkillGroups>(),
                },
                new TableColumn
                {
                    Title = L["Name"],
                    Sortable = true,
                    Data = nameof(SkillGroupDto.Name),
                },
                new TableColumn
                {
                    Title = L["Description"],
                    Sortable = true,
                    Data = nameof(SkillGroupDto.Description),
                }
            });

        return base.SetTableColumnsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewSkillGroup"], OpenCreateModalAsync,
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
}