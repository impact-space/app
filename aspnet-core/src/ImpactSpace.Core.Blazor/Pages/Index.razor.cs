using System.Threading.Tasks;
namespace ImpactSpace.Core.Blazor.Pages;

public partial class Index
{
    public bool IsSetupWizardRequired { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        if (CurrentUser.IsAuthenticated && CurrentTenant.Id.HasValue)
        {
            if (!await OrganizationAppService.ExistsForTenantAsync(CurrentTenant.Id.Value))
            {
                IsSetupWizardRequired = true;  
            }
        }
    }
}
