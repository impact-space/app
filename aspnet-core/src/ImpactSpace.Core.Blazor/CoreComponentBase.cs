using ImpactSpace.Core.Localization;
using Volo.Abp.AspNetCore.Components;

namespace ImpactSpace.Core.Blazor;

public abstract class CoreComponentBase : AbpComponentBase
{
    protected CoreComponentBase()
    {
        LocalizationResource = typeof(CoreResource);
    }
}
