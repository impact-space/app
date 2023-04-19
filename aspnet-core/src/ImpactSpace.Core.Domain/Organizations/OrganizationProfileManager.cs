using System;
using System.Threading.Tasks;
using ImpactSpace.Core.Common;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Organizations;

public sealed class OrganizationProfileManager : DomainService
{
    private readonly IOrganizationProfileRepository _organizationProfileRepository;
    private readonly IBlobContainer<OrganizationProfileLogoContainer> _blobContainer;
    private readonly IConfiguration _configuration;

    public OrganizationProfileManager(
        IOrganizationProfileRepository organizationProfileRepository, 
        IBlobContainer<OrganizationProfileLogoContainer> blobContainer, 
        IConfiguration configuration)
    {
        _organizationProfileRepository = organizationProfileRepository;
        _blobContainer = blobContainer;
        _configuration = configuration;
    }

    public async Task<OrganizationProfile> CreateAsync(
        Guid organizationId, 
        [CanBeNull] string missionStatement, 
        [CanBeNull] string website,
        [CanBeNull] string phoneNumber,
        [CanBeNull] string email, 
        [CanBeNull] string logoBase64)
    {
        string logoUrl = null;
        
        if (!logoBase64.IsNullOrEmpty())
        {
            logoUrl = await UploadLogoAsync(organizationId, logoBase64);
        }
        
        var organizationProfile = new OrganizationProfile(
            GuidGenerator.Create(),
            organizationId,
            missionStatement,
            website,
            phoneNumber,
            email,
            logoUrl
        );
        
        organizationProfile.ChangePhoneNumber(phoneNumber);

        return await _organizationProfileRepository.InsertAsync(organizationProfile);
    }
    
    public async Task UpdateAsync(
        Guid organizationId, 
        [CanBeNull] string missionStatement, 
        [CanBeNull] string website,
        [CanBeNull] string phoneNumber, 
        [CanBeNull] string email, 
        [CanBeNull] string logoBase64)
    {
        var organizationProfile = await _organizationProfileRepository.GetByOrganizationIdAsync(organizationId);

        organizationProfile.ChangeMissionStatement(missionStatement);
        organizationProfile.ChangeWebsite(website);
        organizationProfile.ChangePhoneNumber(phoneNumber);
        organizationProfile.ChangeEmail(email);

        if (!string.IsNullOrEmpty(logoBase64))
        {
            var newLogoUrl = await UploadLogoAsync(organizationId, logoBase64);
            organizationProfile.ChangeLogoUrl(newLogoUrl);
        }
        else
        {
            await DeleteLogoAsync(organizationId);
            organizationProfile.ChangeLogoUrl(null);
        }

        await _organizationProfileRepository.UpdateAsync(organizationProfile);
    }

    private async Task<string> UploadLogoAsync(Guid organizationId, string logoBase64)
    {
        var bytes = Convert.FromBase64String(logoBase64);
        var fileName = $"{organizationId}";

        await _blobContainer.SaveAsync(fileName, bytes, true);
        
        return GenerateLogoUrl(fileName);
    }
    
    private async Task DeleteLogoAsync(Guid organizationId)
    {
        var fileName = $"{organizationId}";
        await _blobContainer.DeleteAsync(fileName);
    }
    
    private string GenerateLogoUrl(string fileName)
    {
        var minioEndpoint = _configuration["Minio:EndPoint"]; 
        var bucketName = OrganizationProfileLogoContainer.GetContainerName(); 
        return $"{minioEndpoint}/{bucketName}/{fileName}";
    }

    public async Task<bool> ExistsForTenantAsync(Guid tenantId)
    {
        return await _organizationProfileRepository.ExistsForTenantAsync(tenantId);
    }
}