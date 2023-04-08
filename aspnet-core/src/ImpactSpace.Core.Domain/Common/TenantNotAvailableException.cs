using Volo.Abp;

namespace ImpactSpace.Core.Common;

public class TenantNotAvailableException: BusinessException
{
    public TenantNotAvailableException()
        : base(CoreDomainErrorCodes.TenantNotAvailable)
    {
    }
}