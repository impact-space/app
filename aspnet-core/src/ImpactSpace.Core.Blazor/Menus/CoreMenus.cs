namespace ImpactSpace.Core.Blazor.Menus;

public class CoreMenus
{
    private const string Prefix = "Core";
    public const string Home = Prefix + ".Home";
    
    //Add your menu items here...
    public const string GlobalTypes = Prefix + ".GlobalTypes";
    public const string SkillManagement = GlobalTypes + ".SkillManagement";
    public const string SkillGroups = GlobalTypes + ".SkillGroups";
    public const string Skills = GlobalTypes + ".Skills";
    public const string ChallengeManagement = GlobalTypes + ".ChallengeManagement";

    // Add Organization Management menu constants
    public const string OrganizationManagement = Prefix + ".OrganizationManagement";
    public const string Organization = OrganizationManagement + ".Organization";
    public const string Profile = OrganizationManagement + ".Profile";
    public const string Members = OrganizationManagement + ".Members";
}
