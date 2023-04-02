using Volo.Abp;

namespace ImpactSpace.Core.Projects;

public class TagAlreadyExistsException : BusinessException
{
    public TagAlreadyExistsException(string name)
        : base(CoreDomainErrorCodes.TagAlreadyExists)
    {
        WithData("name", name);
    }
}