using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.Challenges;
using ImpactSpace.Core.Permissions;
using ImpactSpace.Core.Localization;
using Blazorise;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class Challenges
    {
        private string FilterText { get; set; } = string.Empty;

        protected PageToolbar Toolbar { get; } = new();

        protected List<TableColumn> ChallengeManagementTableColumns => TableColumns.Get<Challenges>();

        public Challenges()
        {
            LocalizationResource = typeof(CoreResource);
            CreatePolicyName = CorePermissions.GlobalTypes.Challenges.Create;
            UpdatePolicyName = CorePermissions.GlobalTypes.Challenges.Edit;
            DeletePolicyName = CorePermissions.GlobalTypes.Challenges.Delete;
        }

        protected override ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Administration"].Value));
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Challenges"].Value));
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
                .Get<Challenges>()
                .AddRange(new EntityAction[]
                {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => { await OpenEditModalAsync(data.As<ChallengeDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<ChallengeDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<ChallengeDto>())
                    }
                });

            return base.SetEntityActionsAsync();
        }

        protected override ValueTask SetTableColumnsAsync()
        {
            ChallengeManagementTableColumns
                .AddRange(new[]
                {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<Challenges>(),
                    },
                    new TableColumn
                    {
                        Title = L["Name"],
                        Sortable = true,
                        Data = nameof(ChallengeDto.Name),
                    },
                    // Add more columns for other ChallengeDto properties as needed
                });

            return base.SetTableColumnsAsync();
        }

        protected override ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["NewChallenge"], OpenCreateModalAsync,
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