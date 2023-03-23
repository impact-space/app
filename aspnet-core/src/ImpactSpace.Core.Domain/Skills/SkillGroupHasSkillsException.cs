using System;
using Volo.Abp;

namespace ImpactSpace.Core.Skills;

public class SkillGroupHasSkillsException : BusinessException
{
    public SkillGroupHasSkillsException(Guid skillGroupId, string name): base(CoreDomainErrorCodes.SkillGroupHasSkills)
    {
        WithData("skillGroupId", skillGroupId);
        WithData("name", name);
    }
}