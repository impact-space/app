using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Organizations;

public interface IOrganizationAppService : IApplicationService
{
    Task<OrganizationDto> GetAsync(Guid id);
    
    Task<PagedResultDto<OrganizationDto>> GetListAsync(GetOrganizationListDto input);
    
    Task<OrganizationDto> CreateAsync(CreateOrganizationDto input);
    
    Task UpdateAsync(Guid id, UpdateOrganizationDto input);
    
    Task DeleteAsync(Guid id);
}