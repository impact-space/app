using Volo.Abp;

namespace ImpactSpace.Core.Challenges;

public class ChallengeAlreadyExistsException: BusinessException
{
    public ChallengeAlreadyExistsException(string name)
        : base(CoreDomainErrorCodes.ChallengeAlreadyExists)
    {
        WithData("name", name);
    }
}