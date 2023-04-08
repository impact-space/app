using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Setup;

public interface ISetupAppService : IApplicationService
{
    Task<SetupDto> SetupAsync(SetupDto input);
}