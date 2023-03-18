using ImpactSpace.Core.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ImpactSpace.Core.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(CoreEntityFrameworkCoreModule),
    typeof(CoreApplicationContractsModule)
    )]
public class CoreDbMigratorModule : AbpModule
{

}
