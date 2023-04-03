using ImpactSpace.Core.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ImpactSpace.Core.Permissions;

public class CorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CorePermissions.GlobalTypeGroupName, L("Permission:GlobalTypes"));
        //Define your own permissions here. Example:
        myGroup.AddPermission(CorePermissions.GlobalTypes.Manage, L("Permission:GlobalTypes.Manage"));
        
        
        
        var skillsGroup = myGroup.AddPermission(CorePermissions.GlobalTypes.Skills.Default, L("Permission:GlobalTypes.Skills"));
        skillsGroup.AddChild(CorePermissions.GlobalTypes.Skills.Create, L("Permission:GlobalTypes.Skills.Create"));
        skillsGroup.AddChild(CorePermissions.GlobalTypes.Skills.Edit, L("Permission:GlobalTypes.Skills.Edit"));
        skillsGroup.AddChild(CorePermissions.GlobalTypes.Skills.Delete, L("Permission:GlobalTypes.Skills.Delete"));
        
        var skillGroupsGroup = myGroup.AddPermission(CorePermissions.GlobalTypes.SkillGroups.Default, L("Permission:GlobalTypes.SkillGroups"));
        skillGroupsGroup.AddChild(CorePermissions.GlobalTypes.SkillGroups.Create, L("Permission:GlobalTypes.SkillGroups.Create"));
        skillGroupsGroup.AddChild(CorePermissions.GlobalTypes.SkillGroups.Edit, L("Permission:GlobalTypes.SkillGroups.Edit"));
        skillGroupsGroup.AddChild(CorePermissions.GlobalTypes.SkillGroups.Delete, L("Permission:GlobalTypes.SkillGroups.Delete"));
        
        var challengesGroup = myGroup.AddPermission(CorePermissions.GlobalTypes.Challenges.Default, L("Permission:GlobalTypes.Challenges"));
        challengesGroup.AddChild(CorePermissions.GlobalTypes.Challenges.Create, L("Permission:GlobalTypes.Challenges.Create"));
        challengesGroup.AddChild(CorePermissions.GlobalTypes.Challenges.Edit, L("Permission:GlobalTypes.Challenges.Edit"));
        challengesGroup.AddChild(CorePermissions.GlobalTypes.Challenges.Delete, L("Permission:GlobalTypes.Challenges.Delete"));
    
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CoreResource>(name);
    }
}
