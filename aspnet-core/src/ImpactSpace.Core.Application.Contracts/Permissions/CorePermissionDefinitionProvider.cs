using ImpactSpace.Core.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Permissions;

public class CorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var globalTypesGroup = context.AddGroup(CorePermissions.GlobalTypeGroupName, L("Permission:GlobalTypes"));

        var skillsGroup = globalTypesGroup.AddPermission(CorePermissions.GlobalTypes.Skills.Manage, L("Permission:GlobalTypes.Skills"), MultiTenancySides.Host);
        skillsGroup.AddChild(CorePermissions.GlobalTypes.Skills.Create, L("Permission:GlobalTypes.Skills.Create"), MultiTenancySides.Host);
        skillsGroup.AddChild(CorePermissions.GlobalTypes.Skills.Edit, L("Permission:GlobalTypes.Skills.Edit"), MultiTenancySides.Host);
        skillsGroup.AddChild(CorePermissions.GlobalTypes.Skills.Delete, L("Permission:GlobalTypes.Skills.Delete"), MultiTenancySides.Host);
        
        var skillGroupsGroup = globalTypesGroup.AddPermission(CorePermissions.GlobalTypes.SkillGroups.Manage, L("Permission:GlobalTypes.SkillGroups"), MultiTenancySides.Host);
        skillGroupsGroup.AddChild(CorePermissions.GlobalTypes.SkillGroups.Create, L("Permission:GlobalTypes.SkillGroups.Create"), MultiTenancySides.Host);
        skillGroupsGroup.AddChild(CorePermissions.GlobalTypes.SkillGroups.Edit, L("Permission:GlobalTypes.SkillGroups.Edit"), MultiTenancySides.Host);
        skillGroupsGroup.AddChild(CorePermissions.GlobalTypes.SkillGroups.Delete, L("Permission:GlobalTypes.SkillGroups.Delete"), MultiTenancySides.Host);
        
        var challengesGroup = globalTypesGroup.AddPermission(CorePermissions.GlobalTypes.Challenges.Manage, L("Permission:GlobalTypes.Challenges"), MultiTenancySides.Host);
        challengesGroup.AddChild(CorePermissions.GlobalTypes.Challenges.Create, L("Permission:GlobalTypes.Challenges.Create"), MultiTenancySides.Host);
        challengesGroup.AddChild(CorePermissions.GlobalTypes.Challenges.Edit, L("Permission:GlobalTypes.Challenges.Edit"), MultiTenancySides.Host);
        challengesGroup.AddChild(CorePermissions.GlobalTypes.Challenges.Delete, L("Permission:GlobalTypes.Challenges.Delete"), MultiTenancySides.Host);
    
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CoreResource>(name);
    }
}
