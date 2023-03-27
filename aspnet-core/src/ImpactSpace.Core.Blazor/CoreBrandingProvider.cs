using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ImpactSpace.Core.Blazor;

[Dependency(ReplaceServices = true)]
public class CoreBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Impact Space";
}
