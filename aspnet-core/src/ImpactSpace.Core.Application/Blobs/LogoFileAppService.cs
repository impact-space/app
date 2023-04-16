using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace ImpactSpace.Core.Blobs;

public class LogoFileAppService: ApplicationService, ILogoFileAppService
{
    private readonly IBlobContainer<LogoFileContainer> _fileContainer;

    public LogoFileAppService(IBlobContainer<LogoFileContainer> fileContainer)
    {
        _fileContainer = fileContainer;
    }

    public async Task SaveLogoBlobAsync(SaveLogoBlobInputDto input)
    {
        await _fileContainer.SaveAsync(input.Name, input.Content, true);
    }

    public async Task<LogoBlobDto> GetLogoBlobAsync(GetLogoBlobRequestDto input)
    {
        var blob = await _fileContainer.GetAllBytesAsync(input.Name);
        return new LogoBlobDto { Name = input.Name, Content = blob };
    }
}