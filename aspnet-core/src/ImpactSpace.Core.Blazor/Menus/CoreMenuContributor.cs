using System.Threading.Tasks;
using Blazorise.Icons.FontAwesome;
using ImpactSpace.Core.Localization;
using ImpactSpace.Core.MultiTenancy;
using ImpactSpace.Core.Permissions;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace ImpactSpace.Core.Blazor.Menus;

public class CoreMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<CoreResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                CoreMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home",
                order: 0
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "SkillManagement",
                l["Menu:SkillManagement"],
                icon: "fas fa-bogs",
                requiredPermissionName: CorePermissions.Skills.Default
            ).AddItem(
                new ApplicationMenuItem(
                    "SkillGroups",
                    l["Menu:SkillGroups"],
                    url: "/skill-groups",
                    icon: "fas fa-layer-group",
                    requiredPermissionName: CorePermissions.SkillGroups.Default
                )
            ).AddItem(
                new ApplicationMenuItem(
                    "Skills",
                    l["Menu:Skills"],
                    url: "/skills",
                    icon: "fas fa-tasks",
                    requiredPermissionName: CorePermissions.Skills.Default
                )
            )
        );

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);
        
        

        return Task.CompletedTask;
    }
}
