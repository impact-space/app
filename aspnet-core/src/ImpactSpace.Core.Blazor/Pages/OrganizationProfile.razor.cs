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
    protected UpdateOrganizationProfileDto OrganizationProfileDto = new();
    private Validations OrganizationProfileValidations;
    [Inject] protected NavigationManager NavigationManager { get; set; }
    [Inject] protected ILogoFileAppService LogoFileAppService { get; set; }
    
    private readonly IList<string> _imageDataUrls = new List<string>();
    private string _getLogoImage;

    protected override async Task OnInitializedAsync()
    {
        OrganizationProfileDto = ObjectMapper.Map<OrganizationProfileDto, UpdateOrganizationProfileDto>(await OrganizationProfileAppService.GetAsync());
        // Load the logo and set it to the file edit component.
    }

    private async Task SubmitAsync()
    {
        if (!await OrganizationProfileValidations.ValidateAll())
        {
            return;
        }

        await OrganizationProfileAppService.UpdateAsync(OrganizationProfileDto);
        
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
    
    private async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        var maxAllowedFiles = 1;
        var format = "image/jpeg";

        foreach (var imageFile in e.GetMultipleFiles(maxAllowedFiles))
        {
            var resizedImageFile = await imageFile.RequestImageFileAsync(format, 100, 100);
            var buffer = new byte[resizedImageFile.Size];
            await resizedImageFile.OpenReadStream().ReadAsync(buffer);

            // await FileAppService.SaveBlobAsync(new SaveBlobInputDto { Name = imageFile.Name, Content = buffer });
            await LogoFileAppService.SaveLogoBlobAsync(new SaveLogoBlobInputDto { Name = "MyImageNme.jpeg", Content = buffer });

            var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
            _imageDataUrls.Add(imageDataUrl);
        }
    }

    private async Task GetImageFromDb()
    {
        var format = "image/jpeg";
        var blob = await LogoFileAppService.GetLogoBlobAsync(new GetLogoBlobRequestDto { Name = "MyImageNme.jpeg" });

        _getLogoImage = $"data:{format};base64,{Convert.ToBase64String(blob.Content)}";

    }
}