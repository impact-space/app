using Volo.Abp;

namespace ImpactSpace.Core.Skills;

public class SkillGroupAlreadyExistsException : BusinessException
{
    public SkillGroupAlreadyExistsException(string name)
        : base(CoreDomainErrorCodes.SkillGroupAlreadyExists)
    {
        WithData("name", name);
    }
}