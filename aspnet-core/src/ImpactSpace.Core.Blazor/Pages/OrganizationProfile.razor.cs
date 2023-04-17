using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using ImpactSpace.Core.Blobs;
using ImpactSpace.Core.Organizations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ImpactSpace.Core.Blazor.Pages;

public partial class OrganizationProfile
{
    protected OrganizationProfileCreateUpdateDto OrganizationProfileDto = new();
    private Validations OrganizationProfileValidations;
    [Inject] protected NavigationManager NavigationManager { get; set; }
    
    private string _getLogoImage;

    protected override async Task OnInitializedAsync()
    {
        OrganizationProfileDto = ObjectMapper.Map<OrganizationProfileDto, OrganizationProfileCreateUpdateDto>(await OrganizationProfileAppService.GetAsync());
        // Load the logo and set it to the file edit component.
    }

    private async Task SubmitAsync()
    {
        if (!await OrganizationProfileValidations.ValidateAll())
        {
            return;
        }

        //await OrganizationProfileAppService.UpdateAsync(OrganizationProfileDto);
        
        /*if (_organizationProfileDto.Logo != null)
        {
            await OrganizationProfileAppService.SaveLogoAsync(_organizationProfileDto.Id, _organizationProfileDto.Logo.Content);
        }*/

        // Save the social media links and any other additional fields.

        NavigationManager.NavigateTo("/");
    }

    /*private void UpdateLogo(FileChangedEventArgs e)
    {
        _organizationProfileDto.Logo = e.File;
    }*/
}