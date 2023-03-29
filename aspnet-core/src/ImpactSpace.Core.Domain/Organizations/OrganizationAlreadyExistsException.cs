using Volo.Abp;

namespace ImpactSpace.Core.Organizations;

public class OrganizationAlreadyExistsException : BusinessException
{
    public OrganizationAlreadyExistsException(string name)
        : base(CoreDomainErrorCodes.OrganizationAlreadyExists)
    {
        WithData("name", name);
    }
}