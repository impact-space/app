using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using ImpactSpace.Core.Blazor.Pages;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Localization;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Permissions;
using ImpactSpace.Core.Skills;
using JetBrains.Annotations;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Authorization;
using Volo.Abp.BlazoriseUI.Components;

namespace ImpactSpace.Core.Blazor.Components;

public partial class MemberSkillsList
{
    [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }
    
    [Parameter] public Guid MemberId { get; set; } = Guid.Empty;
    
    [Parameter]
    public ValidationStatus ValidationStatus { get; set; } = ValidationStatus.None;
    
    private IReadOnlyList<SkillDto> AvailableSkills { get; set; } = Array.Empty<SkillDto>();
    private IReadOnlyList<OrganizationMemberSkillDto> MemberSkills { get; set; } = Array.Empty<OrganizationMemberSkillDto>();
    
    protected Modal EditSkillsModal;
    
    protected Validations EditSkillsValidationsRef;
    
    protected TableColumnDictionary SkillsTableColumns { get; set; }
    
    protected List<TableColumn> MemberSkillsTableColumns => SkillsTableColumns.Get<MemberSkillsList>();

    protected DataGridEntityActionsColumn<OrganizationMemberSkillDto> SkillEntityActionsColumn;
    
    private EntityActionDictionary SkillEntityActions { get; set; }
    
    public OrganizationMemberSkillDto EditingSkillEntity { get; set; } = new();

    public Guid EditingEntitySkillId { get; set; }
    
    protected string CreatePolicyName { get; set; }
    protected string UpdatePolicyName { get; set; }
    protected string DeletePolicyName { get; set; }

    public bool HasCreatePermission { get; set; }
    public bool HasUpdatePermission { get; set; }
    public bool HasDeletePermission { get; set; }

    public int PageSize { get; set; } = 50;
    
    private SkillDto SelectedSkill { get; set; }

    public MemberSkillsList()
    {
        LocalizationResource = typeof(CoreResource);
        CreatePolicyName = CorePermissions.OrganizationManagement.Create;
        UpdatePolicyName = CorePermissions.OrganizationManagement.Edit;
        DeletePolicyName = CorePermissions.OrganizationManagement.Delete;
        
        SkillsTableColumns = new TableColumnDictionary();
        SkillEntityActions = new EntityActionDictionary();
    }
    
    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await FetchDataAsync();
        
        await SetSkillEntityActionsAsync();
        await SetSkillTableColumnsAsync();
        
        await base.OnInitializedAsync();
    }
    
    protected virtual async Task SetPermissionsAsync()
    {
        if (CreatePolicyName != null)
        {
            HasCreatePermission = await AuthorizationService.IsGrantedAsync(CreatePolicyName);
        }

        if (UpdatePolicyName != null)
        {
            HasUpdatePermission = await AuthorizationService.IsGrantedAsync(UpdatePolicyName);
        }

        if (DeletePolicyName != null)
        {
            HasDeletePermission = await AuthorizationService.IsGrantedAsync(DeletePolicyName);
        }
    }
    
    protected virtual async Task CheckCreatePolicyAsync()
    {
        await CheckPolicyAsync(CreatePolicyName);
    }

    protected virtual async Task CheckUpdatePolicyAsync()
    {
        await CheckPolicyAsync(UpdatePolicyName);
    }

    protected virtual async Task CheckDeletePolicyAsync()
    {
        await CheckPolicyAsync(DeletePolicyName);
    }

    /// <summary>
    /// Calls IAuthorizationService.CheckAsync for the given <paramref name="policyName"/>.
    /// Throws <see cref="AbpAuthorizationException"/> if given policy was not granted for the current user.
    ///
    /// Does nothing if <paramref name="policyName"/> is null or empty.
    /// </summary>
    /// <param name="policyName">A policy name to check</param>
    protected virtual async Task CheckPolicyAsync([CanBeNull] string policyName)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            return;
        }

        await AuthorizationService.CheckAsync(policyName);
    }
    
    private async Task FetchDataAsync()
    {
        await FetchSkillsAsync();
        await FetchMemberSkillsAsync();
    }
    
    private async Task FetchSkillsAsync()
    {
        try
        {
            AvailableSkills = (await SkillAppService.GetListAsync(new GetSkillListDto
            {
                Sorting = "Name",
                MaxResultCount = 1000
            })).Items;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task FetchMemberSkillsAsync()
    {
        try
        {
            MemberSkills = await OrganizationMemberAppService.GetSkillsAsync(MemberId);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    private ValueTask SetSkillTableColumnsAsync()
    {
        MemberSkillsTableColumns
            .AddRange(new[]
            {
                new TableColumn
                {
                    Title = L["Actions"],
                    Actions = SkillEntityActions.Get<MemberSkillsList>(),
                },
                new TableColumn
                {
                    Title = L["Skill"],
                    Sortable = true,
                    Data = nameof(OrganizationMemberSkillDto.SkillId),
                    ValueConverter = (value) => AvailableSkills.FirstOrDefault(x => x.Id == ((OrganizationMemberSkillDto)value).SkillId)?.Name
                },
                new TableColumn
                {
                    Title = L["ProficiencyLevel"],
                    Sortable = false,
                    Data = nameof(OrganizationMemberSkillDto.ProficiencyLevel)
                }
            });
        return ValueTask.CompletedTask;
    }
    
    private ValueTask SetSkillEntityActionsAsync()
    {
        SkillEntityActions
            .Get<MemberSkillsList>()
            .AddRange(new EntityAction[]
            {
                new EntityAction
                {
                    Text = L["Edit"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) => { await OpenEditSkillsModal(data.As<OrganizationMemberSkillDto>()); }
                },
                new EntityAction
                {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteSkillAsync(data.As<OrganizationMemberSkillDto>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<OrganizationMemberSkillDto>())
                }
            });

        return ValueTask.CompletedTask;
    }
    
    protected virtual string GetDeleteConfirmationMessage(OrganizationMemberSkillDto entity)
    {
        return UiLocalizer["ItemWillBeDeletedMessage"];
    }
    
    private async Task DeleteSkillAsync(OrganizationMemberSkillDto entity)
    {
        try
        {
            await OrganizationMemberAppService.RemoveSkillAsync(MemberId, entity.SkillId);
            await FetchDataAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    private async Task OpenEditSkillsModal(OrganizationMemberSkillDto entity)
    {
        try
        {
            if (EditSkillsValidationsRef != null)
            {
                await EditSkillsValidationsRef.ClearAll();
            }

            await CheckUpdatePolicyAsync();

            OrganizationMemberSkillDto entityDto;
            if (entity == null || entity.SkillId == Guid.Empty)
            {
                entityDto = new()
                {
                    OrganizationMemberId = MemberId, 
                    ProficiencyLevel = ProficiencyLevel.Beginner, 
                    SkillId = FindUnassignedSkillId()
                };
            }
            else
            {
                entityDto = await OrganizationMemberAppService.GetSkillAsync(MemberId, entity.SkillId);
            }

            EditingEntitySkillId = entity!.SkillId;
            EditingSkillEntity = entityDto;
            
            // Find the SkillDto for the EditingSkillEntity.SkillId
            SelectedSkill = AvailableSkills.FirstOrDefault(x => x.Id == EditingSkillEntity.SkillId);
            SelectedSkillName = SelectedSkill?.Name;  
            
            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (EditSkillsModal != null)
                {
                    await EditSkillsModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    public string SelectedSkillName { get; set; }

    private Guid FindUnassignedSkillId()
    {
        var assignedSkillIds = MemberSkills.Select(x => x.SkillId).ToList();
        var unassignedSkill = AvailableSkills.FirstOrDefault(x => !assignedSkillIds.Contains(x.Id));
        return unassignedSkill?.Id ?? Guid.Empty;
    }
    
    private Task CloseEditSkillsModal()
    {
        EditingSkillEntity = new OrganizationMemberSkillDto(); // Reset the DTO
        return InvokeAsync(EditSkillsModal.Hide);
    }

    private async Task UpdateSkillsAsync()
    {
        if (await EditSkillsValidationsRef.ValidateAll())
        {
            try
            {
                await OrganizationMemberAppService.AddOrEditSkillAsync(MemberId, EditingSkillEntity.SkillId, EditingSkillEntity.ProficiencyLevel);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
            
            await CloseEditSkillsModal();
            await FetchDataAsync(); // Update the member's data to reflect the changes
        }
    }
    
    private async Task OpenCreateSkillsModal()
    { 
        await OpenEditSkillsModal(null);
    }
    
    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<OrganizationMemberSkillDto> e)
    {
        CurrentSkillSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentSkillPage = e.Page;

        await FetchDataAsync();
        
        await InvokeAsync(StateHasChanged);
    }
    
    protected Modal AddSkillsModal;
    protected Validations AddSkillsValidationsRef;
    public OrganizationMemberSkillDto AddingSkillEntity { get; set; } = new();

    private async Task OpenAddSkillsModal()
    {
        if (AddSkillsValidationsRef != null)
        {
            await AddSkillsValidationsRef.ClearAll();
        }

        await CheckCreatePolicyAsync();

        AddingSkillEntity = new OrganizationMemberSkillDto
        {
            OrganizationMemberId = MemberId, 
            ProficiencyLevel = ProficiencyLevel.Beginner, 
            SkillId = FindUnassignedSkillId()
        };

        await InvokeAsync(async () =>
        {
            StateHasChanged();
            if (AddSkillsModal != null)
            {
                await AddSkillsModal.Show();
            }
        });
    }
    
    private Task CloseAddSkillsModal()
    {
        AddingSkillEntity = new OrganizationMemberSkillDto(); // Reset the DTO
        return InvokeAsync(AddSkillsModal.Hide);
    }

    private async Task AddSkillsAsync()
    {
        if (await AddSkillsValidationsRef.ValidateAll())
        {
            try
            {
                await OrganizationMemberAppService.AddOrEditSkillAsync(MemberId, AddingSkillEntity.SkillId, AddingSkillEntity.ProficiencyLevel);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        
            await CloseAddSkillsModal();
            await FetchDataAsync(); // Update the member's data to reflect the changes
        }
    }
    
    private async Task OnSkillSelected(SkillDto selectedSkill)
    {
        AddingSkillEntity.SkillId = selectedSkill.Id;

        await InvokeAsync(StateHasChanged);
    }

    protected int CurrentSkillPage = 1;

    protected string CurrentSkillSorting = "Name";

    protected int? TotalSkillCount;

    private void ValidateSkill(ValidatorEventArgs e)
    {
        e.Status = AddingSkillEntity.SkillId != Guid.Empty ? ValidationStatus.Success : ValidationStatus.Error;
    }
}