using Microsoft.Extensions.Logging;

namespace AppsettingsUpdater;

internal class Downloader : IDownloader
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<Downloader> _logger;

    public Downloader(IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory)
    {
        _httpClientFactory = httpClientFactory;
        _logger = loggerFactory.CreateLogger<Downloader>();
    }

    public async Task DownloadFile(string resourceUrl, string saveAsFilePath)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var responseStream = await httpClient.GetStreamAsync(resourceUrl);
            using (FileStream fileStream = new FileStream(saveAsFilePath, FileMode.Create))
            {
                await responseStream.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
