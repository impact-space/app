using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using ImpactSpace.Core.Localization;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Permissions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class OrganizationMembers
{
    private string FilterText { get; set; } = string.Empty;

    protected PageToolbar Toolbar { get; } = new();

    protected List<TableColumn> OrganizationMemberManagementTableColumns => TableColumns.Get<OrganizationMembers>();

    public OrganizationMembers()
    {
        LocalizationResource = typeof(CoreResource);
        CreatePolicyName = CorePermissions.OrganizationManagement.Create;
        UpdatePolicyName = CorePermissions.OrganizationManagement.Edit;
        DeletePolicyName = CorePermissions.OrganizationManagement.Delete;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:OrganizationManagement"].Value));
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:OrganizationMembers"].Value));
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
            .Get<OrganizationMembers>()
            .AddRange(new EntityAction[]
            {
                new EntityAction
                {
                    Text = L["Edit"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) => { await OpenEditModalAsync(data.As<OrganizationMemberDto>()); }
                },
                new EntityAction
                {
                    Text = L["Profile"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = (data) =>
                    {
                        NavigationManager.NavigateTo($"/member-profile/{data.As<OrganizationMemberDto>().Id}");
                        return Task.CompletedTask;
                    }
                },
                new EntityAction
                {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteEntityAsync(data.As<OrganizationMemberDto>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<OrganizationMemberDto>())
                }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        OrganizationMemberManagementTableColumns
            .AddRange(new[]
            {
                new TableColumn
                {
                    Title = L["Actions"],
                    Actions = EntityActions.Get<OrganizationMembers>(),
                },
                new TableColumn
                {
                    Title = L["OrganizationMember"],
                    Sortable = true,
                    Data = nameof(OrganizationMemberDto.Name),
                },
                // Add more columns as needed
            });

        return base.SetTableColumnsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewMember"], OpenCreateModalAsync,
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