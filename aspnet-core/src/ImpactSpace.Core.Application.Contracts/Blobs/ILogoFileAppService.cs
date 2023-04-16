using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Blobs;

public interface ILogoFileAppService : IApplicationService
{
    Task SaveLogoBlobAsync(SaveLogoBlobInputDto input);
    Task<LogoBlobDto> GetLogoBlobAsync(GetLogoBlobRequestDto input);
}