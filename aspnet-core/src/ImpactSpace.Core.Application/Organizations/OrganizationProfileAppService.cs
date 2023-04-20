using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;

namespace ImpactSpace.Core.Organizations;

public class OrganizationProfileAppService : ApplicationService, IOrganizationProfileAppService
{
    private readonly OrganizationProfileManager _organizationProfileManager;
    private readonly IOrganizationProfileRepository _organizationProfileRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public OrganizationProfileAppService(
        OrganizationProfileManager organizationProfileManager, 
        IOrganizationProfileRepository organizationProfileRepository, 
        IOrganizationRepository organizationRepository)
    {
        _organizationProfileManager = organizationProfileManager;
        _organizationProfileRepository = organizationProfileRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task<OrganizationProfileDto> GetAsync()
    {
        var tenantId = CurrentTenant.Id;
        
        if(tenantId == null)
        {
            throw new UserFriendlyException("TenantId is null");
        }

        var organization = await _organizationRepository.FindByTenantIdAsync(tenantId.Value);
        
        if(organization == null)
        {
            throw new UserFriendlyException("Organization is null");
        }

        return await GetAsync(organization.Id);
    }

    public async Task<OrganizationProfileDto> GetAsync(Guid organizationId)
    {
        var organizationProfile = await _organizationProfileRepository.GetByOrganizationIdAsync(organizationId);
            
        return ObjectMapper.Map<OrganizationProfile, OrganizationProfileDto>(organizationProfile);
    }

    public async Task<Guid> CreateAsync(OrganizationProfileCreateUpdateDto input)
    {
        var organizationProfile = await _organizationProfileManager.CreateAsync(
            input.OrganizationId,
            input.MissionStatement,
            input.Website,
            input.PhoneNumber,
            input.Email,
            input.Logo
        );

        return organizationProfile.Id;
    }

    public async Task UpdateAsync(Guid organizationId, OrganizationProfileCreateUpdateDto input)
    {
       
        // Get the existing organization profile
        await _organizationProfileManager.UpdateAsync(
            organizationId,
            input.MissionStatement,
            input.Website,
            input.PhoneNumber,
            input.Email,
            input.Logo
        );
    }

    public Task<bool> ExistsForTenantAsync(Guid tenantId)
    {
        return _organizationProfileManager.ExistsForTenantAsync(tenantId);
    }
    
    private async Task<string> GetLogoBase64(IRemoteStreamContent logoStreamContent)
    {
        if (logoStreamContent == null)
        {
            return null;
        }

        await using var logoStream = logoStreamContent.GetStream();
        byte[] logoBytes = new byte[logoStream.Length];
        await logoStream.ReadAsync(logoBytes, 0, (int)logoStream.Length);
        return Convert.ToBase64String(logoBytes);
    }
}