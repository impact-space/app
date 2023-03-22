using System;
using Volo.Abp;

namespace ImpactSpace.Core.Skills;

public class SkillGroupNotFoundException : BusinessException
{
    public SkillGroupNotFoundException(Guid id)
        : base(CoreDomainErrorCodes.SkillGroupNotFound)
    {
        WithData("id", id);
    }
}