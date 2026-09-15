namespace AppsettingsUpdater;

public interface IDownloader
{
    Task DownloadFile(string resourceUrl, string saveAsFilePath);
}
