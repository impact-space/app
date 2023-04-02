using Volo.Abp;

namespace ImpactSpace.Core.Projects;

public class ProjectAlreadyExistsException : BusinessException
{
    public ProjectAlreadyExistsException(string name)
        : base(CoreDomainErrorCodes.ProjectAlreadyExists)
    {
        WithData("name", name);
    }
}