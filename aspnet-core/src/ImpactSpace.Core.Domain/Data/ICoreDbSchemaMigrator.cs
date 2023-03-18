using System.Threading.Tasks;

namespace ImpactSpace.Core.Data;

public interface ICoreDbSchemaMigrator
{
    Task MigrateAsync();
}
