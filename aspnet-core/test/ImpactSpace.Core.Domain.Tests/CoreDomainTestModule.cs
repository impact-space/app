using ImpactSpace.Core.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ImpactSpace.Core;

[DependsOn(
    typeof(CoreEntityFrameworkCoreTestModule)
    )]
public class CoreDomainTestModule : AbpModule
{

}
