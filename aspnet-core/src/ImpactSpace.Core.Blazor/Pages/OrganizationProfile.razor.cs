using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Organizations;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class OrganizationProfile
{
    protected OrganizationProfileCreateUpdateDto OrganizationProfileDto = new();
    
    protected Validations OrganizationProfileValidations;
    
    protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new(2);
    
    protected PageToolbar Toolbar { get; } = new();
    
    [Inject] protected NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var organizationProfileDto = await OrganizationProfileAppService.GetAsync();
        
        OrganizationProfileDto = organizationProfileDto != null 
            ? ObjectMapper.Map<OrganizationProfileDto, OrganizationProfileCreateUpdateDto>(organizationProfileDto) 
            : new OrganizationProfileCreateUpdateDto();
        
        OrganizationProfileDto.SocialMediaLinks = organizationProfileDto?.SocialMediaLinks ?? new List<SocialMediaLinkDto>();
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:OrganizationManagement"].Value));
        BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Profile"].Value));
        return ValueTask.CompletedTask;
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