using Volo.Abp;

namespace ImpactSpace.Core.Skills;

public class SkillAlreadyExistsException: BusinessException
{
    public SkillAlreadyExistsException(string name)
        : base(CoreDomainErrorCodes.SkillAlreadyExists)
    {
        WithData("name", name);
    }
}