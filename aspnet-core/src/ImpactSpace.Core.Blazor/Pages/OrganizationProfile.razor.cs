using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Organizations;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Notifications;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class OrganizationProfile
{
    protected OrganizationProfileCreateUpdateDto OrganizationProfileDto = new();
    
    protected Validations OrganizationProfileValidations;
    
    protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new(2);
    
    protected bool IsNew { get; set; }
    
    protected PageToolbar Toolbar { get; } = new();

    [Inject] protected NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var organizationProfileDto = await OrganizationProfileAppService.GetAsync();

        if (organizationProfileDto != null)
        {
            OrganizationProfileDto =
                ObjectMapper.Map<OrganizationProfileDto, OrganizationProfileCreateUpdateDto>(organizationProfileDto);

            IsNew = false;
        }
        else
        {
            OrganizationProfileDto = new OrganizationProfileCreateUpdateDto();

            var organizationDto = await OrganizationAppService.GetForTenantAsync(CurrentTenant.Id!.Value);
            
            OrganizationProfileDto.OrganizationId = organizationDto.Id;

            IsNew = true;
        }

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
        await OrganizationProfileValidations.ClearAll();
        if (!await OrganizationProfileValidations.ValidateAll())
        {
            await Notify.Error(L["ValidationFailed"]);
            return;
        }
        
        try
        {
            if (IsNew)
            {
                await OrganizationProfileAppService.CreateAsync(OrganizationProfileDto);
                IsNew = false;
            }
            else
            {
                await OrganizationProfileAppService.UpdateAsync(OrganizationProfileDto.OrganizationId, OrganizationProfileDto);    
            }
            await Notify.Success(L["SavedSuccessfully"]);
            await OrganizationProfileValidations.ClearAll();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
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

    private async Task OnLogoChanged(FileChangedEventArgs e)
    {
        try
        {
            var file = e.Files.FirstOrDefault();
            if (file != null)
            {
                using var stream = new MemoryStream();
                await file.WriteToStreamAsync(stream);
                stream.Position = 0; // Reset the position of the stream

                byte[] fileBytes = stream.ToArray();
                OrganizationProfileDto.Logo = Convert.ToBase64String(fileBytes);
            }
            else
            {
                OrganizationProfileDto.Logo = null;
            }
        }
        catch ( Exception exc )
        {
            Console.WriteLine( exc.Message );
        }
        finally
        {
            StateHasChanged();
        }
    }
}