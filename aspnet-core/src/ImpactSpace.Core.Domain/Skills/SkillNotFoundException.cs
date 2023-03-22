using System;
using Volo.Abp;

namespace ImpactSpace.Core.Skills;

public class SkillNotFoundException : BusinessException
{
    public SkillNotFoundException(Guid id)
        : base(CoreDomainErrorCodes.SkillNotFound)
    {
        WithData("id", id);
    }
}