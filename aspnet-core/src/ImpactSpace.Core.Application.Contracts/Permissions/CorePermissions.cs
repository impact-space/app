﻿namespace ImpactSpace.Core.Permissions;

public static class CorePermissions
{
    public const string GlobalTypeGroupName = "GlobalTypes";

    public const string OrganizationGroupName = "Organizations";
    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    
    public static class GlobalTypes
    {
        public static class Skills
        {
            public const string Manage = GlobalTypeGroupName + ".Skills";
            public const string Create = Manage + ".Create";
            public const string Edit = Manage + ".Edit";
            public const string Delete = Manage + ".Delete";
        }
    
        public static class SkillGroups
        {
            public const string Manage = GlobalTypeGroupName + ".SkillGroups";
            public const string Create = Manage + ".Create";
            public const string Edit = Manage + ".Edit";
            public const string Delete = Manage + ".Delete";
        }
        
        public static class Challenges
        {
            public const string Manage = GlobalTypeGroupName + ".Challenges";
            public const string Create = Manage + ".Create";
            public const string Edit = Manage + ".Edit";
            public const string Delete = Manage + ".Delete";
        }
    }

    public static class OrganizationManagement
    {
        public const string Default = OrganizationGroupName + ".OrganizationManagement";
        public const string Organization = Default + ".Organization";
        public const string Profile = Default + ".Profile";
        public const string Members = Default + ".Members";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
