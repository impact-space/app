using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Organizations;

public interface IOrganizationProfileAppService : IApplicationService
{
    Task<OrganizationProfileDto> GetAsync();
    Task UpdateAsync(UpdateOrganizationProfileDto organizationProfile);
}