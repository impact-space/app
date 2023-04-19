using System.Threading.Tasks;
namespace ImpactSpace.Core.Blazor.Pages;

public partial class Index
{
    public bool IsOrganizationSetupRequired { get; set; }
    public bool IsOrganizationProfileSetupRequired { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        if (CurrentUser.IsAuthenticated && CurrentTenant.Id.HasValue)
        {
            IsOrganizationSetupRequired = await CheckIfSetupIsRequiredAsync();
            IsOrganizationProfileSetupRequired = await CheckIfProfileSetupIsRequiredAsync();
        }
    }
    
    private async Task OnSetupCompleted()
    {
        IsOrganizationSetupRequired = await CheckIfSetupIsRequiredAsync();
        IsOrganizationProfileSetupRequired = await CheckIfProfileSetupIsRequiredAsync();
    }
    
    private async Task<bool> CheckIfSetupIsRequiredAsync()
    {
        if (CurrentUser.IsAuthenticated && CurrentTenant.Id.HasValue)
        {
            return !await OrganizationAppService.ExistsForTenantAsync(CurrentTenant.Id.Value);
        }
        return false;
    }
    
    private async Task<bool> CheckIfProfileSetupIsRequiredAsync()
    {
        if (CurrentUser.IsAuthenticated && CurrentTenant.Id.HasValue)
        {
            return !await OrganizationProfileAppService.ExistsForTenantAsync(CurrentTenant.Id.Value);
        }
        return false;
    }
}
