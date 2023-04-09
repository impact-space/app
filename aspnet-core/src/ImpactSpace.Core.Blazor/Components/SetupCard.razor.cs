using System;
using System.Threading.Tasks;
using Blazorise;
using ImpactSpace.Core.Setup;
using Microsoft.AspNetCore.Components;

namespace ImpactSpace.Core.Blazor.Components;

public partial class SetupCard
{
    private SetupDto _setupDto = new();
    private Modal SetupModal { get; set; }
    private Validations SetupValidationsRef { get; set; }
    
    [Parameter] public EventCallback OnSetupCompleted { get; set; }

    private async Task SubmitSetupAsync()
    {
        try
        {
            if (!await SetupValidationsRef.ValidateAll())
            {
                return;
            }
            
            await SetupAppService.SetupAsync(_setupDto);
            
            await CloseSetupModalAsync();
            
            await OnSetupCompleted.InvokeAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task CloseSetupModalAsync()
    {
        await SetupModal.Hide();
        _setupDto = new();
        await SetupValidationsRef.ClearAll();
    }
}