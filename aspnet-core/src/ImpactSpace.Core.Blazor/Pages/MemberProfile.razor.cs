using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using ImpactSpace.Core.Challenges;
using ImpactSpace.Core.Localization;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Permissions;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class MemberProfile

{
    private IReadOnlyList<ChallengeDto> AvailableChallenges { get; set; } = Array.Empty<ChallengeDto>();

    [Parameter] public Guid MemberId { get; set; }
    private OrganizationMemberDto Member { get; set; } = new();
    
    protected Modal EditMemberModal;
    
    protected Modal EditChallengesModal;

    protected PageToolbar Toolbar { get; } = new();
    
    protected List<TableColumn> MemberProfileTableColumns => TableColumns.Get<MemberProfile>();

    public MemberProfile()
    {
        LocalizationResource = typeof(CoreResource);
        CreatePolicyName = CorePermissions.OrganizationManagement.Create;
        UpdatePolicyName = CorePermissions.OrganizationManagement.Edit;
        DeletePolicyName = CorePermissions.OrganizationManagement.Delete;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:OrganizationManagement"].Value));
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:MemberProfile"].Value));
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(Member.Name));
        return base.SetBreadcrumbItemsAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        await FetchDataAsync();

        await base.OnInitializedAsync();
    }

    private async Task FetchMemberAsync()
    {
        try
        {
            Member = await OrganizationMemberAppService.GetDetailsAsync(MemberId);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task FetchChallengesAsync()
    {
        try
        {
            var input = new GetChallengeListDto
            {
                Sorting = "Name",
                MaxResultCount = 1000
            };
        
            AvailableChallenges = (await ChallengeAppService.GetListAsync(input)).Items;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<OrganizationMemberDto>()
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
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteEntityAsync(data.As<OrganizationMemberDto>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<OrganizationMemberDto>())
                }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["EditMember"], OpenEditInformationModal,
            IconName.Add,
            requiredPolicyName: UpdatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    private async Task FetchDataAsync()
    {
        await FetchMemberAsync();
        await FetchChallengesAsync();
    }
    
    private void CloseEditMemberModal()
    {
        EditMemberModal.Hide();
    }

    private async Task OpenEditInformationModal()
    {
        try
        {
            if (EditValidationsRef != null)
            {
                await EditValidationsRef.ClearAll();
            }

            await CheckUpdatePolicyAsync();

            var entityDto = await OrganizationMemberAppService.GetAsync(MemberId);

            EditingEntityId = entityDto.Id;
            EditingEntity = MapToEditingEntity(entityDto);
            
            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (EditMemberModal != null)
                {
                    await EditMemberModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task UpdateMemberAsync()
    {
        try
        {
            var validate = true;
            if (EditValidationsRef != null)
            {
                validate = await EditValidationsRef.ValidateAll();
            }
            if (validate)
            {
                await OnUpdatingEntityAsync();

                await CheckUpdatePolicyAsync();
                var updateInput = MapToUpdateInput(EditingEntity);
                await AppService.UpdateAsync(EditingEntityId, updateInput);

                await OnUpdatedEntityAsync();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
        
        // Add the logic to update the member information and close the modal
        CloseEditMemberModal();
    }
}