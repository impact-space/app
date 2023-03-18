using Volo.Abp.Modularity;

namespace ImpactSpace.Core;

[DependsOn(
    typeof(CoreApplicationModule),
    typeof(CoreDomainTestModule)
    )]
public class CoreApplicationTestModule : AbpModule
{

}
