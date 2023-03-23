namespace ImpactSpace.Core.Permissions;

public static class CorePermissions
{
    public const string GroupName = "Core";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    public static class Skills
    {
        public const string Default = GroupName + ".Skills";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    
    public static class SkillGroups
    {
        public const string Default = GroupName + ".SkillGroups";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
