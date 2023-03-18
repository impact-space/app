using ImpactSpace.Core.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ImpactSpace.Core.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class CoreController : AbpControllerBase
{
    protected CoreController()
    {
        LocalizationResource = typeof(CoreResource);
    }
}
