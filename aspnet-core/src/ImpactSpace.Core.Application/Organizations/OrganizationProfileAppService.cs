using System;
using System.IO;
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
    private readonly IBlobContainer _logoBlobContainer;
    public OrganizationProfileAppService(
        OrganizationProfileManager organizationProfileManager, 
        IOrganizationProfileRepository organizationProfileRepository, 
        IOrganizationRepository organizationRepository,
        IBlobContainerFactory blobContainerFactory)
    {
        _organizationProfileManager = organizationProfileManager;
        _organizationProfileRepository = organizationProfileRepository;
        _organizationRepository = organizationRepository;
        _logoBlobContainer = blobContainerFactory.Create<OrganizationProfileLogoContainer>();
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
            
        var organizationProfileDto = ObjectMapper.Map<OrganizationProfile, OrganizationProfileDto>(organizationProfile);
        
        var logoStream = await _logoBlobContainer.GetOrNullAsync(organizationProfile.OrganizationId.ToString());

        if (logoStream == null)
        {
            return organizationProfileDto;
        }

        logoStream.Seek(0, SeekOrigin.Begin);
        var logoBytes = await logoStream.GetAllBytesAsync();
        organizationProfileDto.Logo = Convert.ToBase64String(logoBytes);

        return organizationProfileDto;
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