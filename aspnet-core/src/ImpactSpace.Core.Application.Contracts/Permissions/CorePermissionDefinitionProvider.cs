using ImpactSpace.Core.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ImpactSpace.Core.Permissions;

public class CorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CorePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(CorePermissions.MyPermission1, L("Permission:MyPermission1"));
        
        var skillsGroup = myGroup.AddPermission(CorePermissions.Skills.Default, L("Permission:Skills"));
        skillsGroup.AddChild(CorePermissions.Skills.Create, L("Permission:Skills.Create"));
        skillsGroup.AddChild(CorePermissions.Skills.Edit, L("Permission:Skills.Edit"));
        skillsGroup.AddChild(CorePermissions.Skills.Delete, L("Permission:Skills.Delete"));
        
        var skillGroupsGroup = myGroup.AddPermission(CorePermissions.SkillGroups.Default, L("Permission:SkillGroups"));
        skillGroupsGroup.AddChild(CorePermissions.SkillGroups.Create, L("Permission:SkillGroups.Create"));
        skillGroupsGroup.AddChild(CorePermissions.SkillGroups.Edit, L("Permission:SkillGroups.Edit"));
        skillGroupsGroup.AddChild(CorePermissions.SkillGroups.Delete, L("Permission:SkillGroups.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CoreResource>(name);
    }
}
