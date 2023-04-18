using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using ImpactSpace.Core.Blobs;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Organizations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class OrganizationProfile
{
    protected OrganizationProfileCreateUpdateDto OrganizationProfileDto = new();
    protected Validations OrganizationProfileValidations;
    [Inject] protected NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var organizationProfileDto = await OrganizationProfileAppService.GetAsync();
        
        OrganizationProfileDto = organizationProfileDto != null 
            ? ObjectMapper.Map<OrganizationProfileDto, OrganizationProfileCreateUpdateDto>(organizationProfileDto) 
            : new OrganizationProfileCreateUpdateDto();
        
        OrganizationProfileDto.SocialMediaLinks = organizationProfileDto?.SocialMediaLinks ?? new List<SocialMediaLinkDto>();
    }

    private async Task SubmitAsync()
    {
        if (!await OrganizationProfileValidations.ValidateAll())
        {
            return;
        }

        try
        {
            await OrganizationProfileAppService.UpdateAsync(OrganizationProfileDto.OrganizationId, OrganizationProfileDto);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
        
    }

    private async Task OnLogoChanged(FileChangedEventArgs e)
    {
        var file = e.Files.FirstOrDefault();
        if (file != null)
        {
            using var streamReader = new MemoryStream();
            await file.WriteToStreamAsync(streamReader);
            OrganizationProfileDto.LogoBase64 = Convert.ToBase64String(streamReader.ToArray());
        }
        else
        {
            OrganizationProfileDto.LogoBase64 = null;
        }
    }
    
    private SocialMediaLinkDto GetSocialMediaLink(SocialMediaPlatform platform)
    {
        var link = OrganizationProfileDto.SocialMediaLinks.FirstOrDefault(x => x.Platform == platform);
        if (link == null)
        {
            link = new SocialMediaLinkDto { Platform = platform, Url = null };
            OrganizationProfileDto.SocialMediaLinks.Add(link);
        }
        return link;
    }
}