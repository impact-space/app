using System;
using System.Threading.Tasks;
using ImpactSpace.Core.Common;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Setup;

public class SetupAppService : CoreAppService, ISetupAppService
{
    private readonly SetupManager _setupManager;
    private readonly ICurrentTenant _currentTenant;

    public SetupAppService(
        SetupManager setupManager,
        ICurrentTenant currentTenant)
    {
        _setupManager = setupManager;
        _currentTenant = currentTenant;
    }

    public async Task<SetupDto> SetupAsync(SetupDto input)
    {
        var tenantId = _currentTenant.Id;
        
        if (tenantId == null || tenantId == Guid.Empty)
        {
            throw new TenantNotAvailableException();
        }
        
        var organization = await _setupManager.CreateOrganizationAsync(
            input.OrganizationName,
            input.OrganizationDescription,
            tenantId.Value
        );

        await _setupManager.CreateOrganizationMemberAsync(
            input.MemberName,
            input.MemberEmail,
            organization.Id
        );

        // set up a blank organization profile
        await _setupManager.CreateOrganizationProfileAsync(
            organization.Id
        );

        return input;
    }
}