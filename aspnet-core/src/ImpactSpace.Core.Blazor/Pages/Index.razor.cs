using System.Threading.Tasks;
namespace ImpactSpace.Core.Blazor.Pages;

public partial class Index
{
    public bool IsOrganizationSetupRequired { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        if (CurrentUser.IsAuthenticated && CurrentTenant.Id.HasValue)
        {
            IsOrganizationSetupRequired = await CheckIfSetupIsRequiredAsync();
        }
    }
    
    private async Task OnSetupCompleted()
    {
        // Refresh the dashboard or update the IsSetupRequired property as needed.
        // For example, you can call a method to check if the setup is still required:
        IsOrganizationSetupRequired = await CheckIfSetupIsRequiredAsync();
    }
    
    private async Task<bool> CheckIfSetupIsRequiredAsync()
    {
        if (CurrentUser.IsAuthenticated && CurrentTenant.Id.HasValue)
        {
            return !await OrganizationAppService.ExistsForTenantAsync(CurrentTenant.Id.Value);
        }
        return false;
    }

}
