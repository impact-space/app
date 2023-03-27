namespace ImpactSpace.Core.Permissions;

public static class CorePermissions
{
    public const string GlobalTypeGroupName = "GlobalTypes";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    
    public static class GlobalTypes
    {
        public const string Default = GlobalTypeGroupName + ".Default";
        public const string Manage = GlobalTypeGroupName + ".Manage";
        
        public static class Skills
        {
            public const string Default = GlobalTypeGroupName + ".Skills";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    
        public static class SkillGroups
        {
            public const string Default = GlobalTypeGroupName + ".SkillGroups";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
    

}
