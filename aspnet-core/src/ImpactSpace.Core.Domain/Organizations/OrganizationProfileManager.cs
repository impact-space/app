using System;
using System.Threading.Tasks;
using ImpactSpace.Core.Common;
using Microsoft.Extensions.Configuration;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Organizations;

public class OrganizationProfileManager : DomainService
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
        string missionStatement, 
        string website, 
        PhoneCountryCode phoneNumberCountryCode,
        string phoneNumber,
        string email, 
        string logoBase64)
    {
        var logoUrl = await UploadLogoAsync(organizationId, logoBase64);

        var organizationProfile = new OrganizationProfile(
            id: GuidGenerator.Create(),
            organizationId: organizationId,
            missionStatement: missionStatement,
            websiteUrl: website,
            phoneNumber: new PhoneNumber(phoneNumberCountryCode, phoneNumber),
            email: email,
            logoUrl: logoUrl
        );

        return await _organizationProfileRepository.InsertAsync(organizationProfile);
    }
    
    public async Task UpdateAsync(Guid organizationId, string missionStatement, string website, PhoneCountryCode phoneCountryCode, string nationalNumber, string email, string logoBase64)
    {
        var organizationProfile = await _organizationProfileRepository.GetByOrganizationIdAsync(organizationId);

        organizationProfile.ChangeMissionStatement(missionStatement);
        organizationProfile.ChangeWebsite(website);
        organizationProfile.ChangePhoneNumber(new PhoneNumber(phoneCountryCode, nationalNumber));
        organizationProfile.ChangeEmail(email);

        if (!string.IsNullOrEmpty(logoBase64))
        {
            var newLogoUrl = await UploadLogoAsync(organizationId, logoBase64);
            organizationProfile.ChangeLogoUrl(newLogoUrl);
        }

        await _organizationProfileRepository.UpdateAsync(organizationProfile);
    }

    private async Task<string> UploadLogoAsync(Guid organizationId, string logoBase64)
    {
        var bytes = Convert.FromBase64String(logoBase64);
        var fileName = $"{organizationId}.png";

        await _blobContainer.SaveAsync(fileName, bytes, true);
        
        return GenerateLogoUrl(fileName);
    }
    
    private string GenerateLogoUrl(string fileName)
    {
        var minioEndpoint = _configuration["Minio:EndPoint"]; // Get the Minio endpoint from the configuration
        var bucketName = OrganizationProfileLogoContainer.GetContainerName(); // Get the container name from the IBlobContainer instance
        return $"{minioEndpoint}/{bucketName}/{fileName}";
    }
}