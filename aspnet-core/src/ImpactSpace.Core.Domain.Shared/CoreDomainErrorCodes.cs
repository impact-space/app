namespace ImpactSpace.Core;

public static class CoreDomainErrorCodes
{
    public const string SkillAlreadyExists = "Core:00001";
    public const string SkillGroupAlreadyExists = "Core:00002";
    public const string SkillGroupNotFound = "Core:00003";
    public const string SkillNotFound = "Core:00004";
    public const string SkillGroupHasSkills = "Core:00005";
    public const string OrganizationAlreadyExists = "Core:00006";
    public const string TagAlreadyExists = "Core:00007";
    public const string ProjectAlreadyExists = "Core:00008";
    public const string ChallengeAlreadyExists = "Core:00009";
    public const string TenantNotAvailable = "Core:00010";
}
