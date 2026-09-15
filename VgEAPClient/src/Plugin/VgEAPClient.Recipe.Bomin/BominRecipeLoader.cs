using FluentFTP;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.OpenAPI;
using VgEAPClient.Common;
using VgEAPClient.Common.CNC;

namespace VgEAPClient.Recipe.Bomin;

internal class BominRecipeLoader : IWorkOrderRecipeLoader
{
    private readonly ILogger<BominRecipeLoader> _logger;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly EAPClientOptions _eAPClientOptions;

    public BominRecipeLoader(IOptions<EAPClientOptions> options,
        ILogger<BominRecipeLoader> logger,
        IHttpRequestInvoker httpRequestInvoker)
    {
        _eAPClientOptions = options.Value;
        _logger = logger;
        _httpRequestInvoker = httpRequestInvoker;
    }

    private string LoadFileFromFtp(FtpClient ftp, string filePath)
    {
        _logger.LogDebug($"ftp 要搜索的文件 {filePath}");
        if (!ftp.FileExists(filePath))
        {
            _logger.LogError($"ftp   未找到文件 {filePath}");
            throw new Exception($"ftp   未找到文件 {filePath}");
        }

        string localFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recipes", NormalizePath(filePath.TrimStart('/')));
        var directoryPath = Path.GetDirectoryName(localFilePath)!;

        _logger.LogInformation($"ftp文件目录 {directoryPath}");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        _logger.LogInformation($"ftp下载到本地地址 {localFilePath}");
        ftp.DownloadFile(localFilePath, filePath, FtpLocalExists.Overwrite);
        _logger.LogInformation($"ftp下载文件成功");
        return NormalizePath(localFilePath);
    }

    private static string NormalizePath(string path)
    {
        // 替换所有非标准分隔符为当前系统的默认分隔符
        string normalizedPath = path.Replace('/', Path.DirectorySeparatorChar);
        return normalizedPath;
    }

    public async Task<List<WorkOrderRecipe>> LoadRecipe(WorkOrderRecipeRequest workOrderRecipeRequest)
    {
        var list = new List<WorkOrderRecipe>();
        if (string.IsNullOrWhiteSpace(_eAPClientOptions.RecipeSearchPath)
            || !_eAPClientOptions.RecipeSearchPath.StartsWith("http"))
            return list;

        try
        {
            var reportBody = new GetDrillRecipesManualReqest
            {
                Lot = workOrderRecipeRequest.ItemCode,
                MachineCode = workOrderRecipeRequest.EquipmentId,
                ProcessCode = ""
            };

            string strPostURL = _eAPClientOptions.RecipeSearchPath + "GetDrillRecipesManual";
            var drillInfoLocatorReply = await _httpRequestInvoker.PostAsJsonAsync<GetDrillRecipesManualReqest, GetDrillRecipesManualResponse>(strPostURL, reportBody) ?? new GetDrillRecipesManualResponse();
            _logger.LogInformation($"PostURL = {strPostURL}");
            _logger.LogInformation(reportBody.ToJson());
            _logger.LogInformation(drillInfoLocatorReply.ToJson());

            if (string.IsNullOrWhiteSpace(drillInfoLocatorReply.data.FtpBasicUrl))
            {
                _logger.LogError($"FtpBasicUrl为空");
                return list;
            }
            if (string.IsNullOrWhiteSpace(drillInfoLocatorReply.data.FtpAccount))
            {
                _logger.LogError($"FtpAccount为空");
                return list;
            }
            if (string.IsNullOrWhiteSpace(drillInfoLocatorReply.data.FtpPassword))
            {
                _logger.LogError($"FtpPassword为空");
                return list;
            }

            using (FtpClient ftp = new FtpClient(drillInfoLocatorReply.data.FtpBasicUrl, drillInfoLocatorReply.data.FtpAccount, drillInfoLocatorReply.data.FtpPassword))
            {
                ftp.Connect();

                foreach (var dia in drillInfoLocatorReply.data.dia)
                {
                    list.Add(new WorkOrderRecipe
                    {
                        tagPath = LoadFileFromFtp(ftp, dia.Trim().Replace(drillInfoLocatorReply.data.FtpBasicUrl, "")),
                        tagCode = reportBody.Lot,
                        tagType = LoadFileType.DIA
                    });
                }
                foreach (var drl in drillInfoLocatorReply.data.drl)
                {
                    list.Add(new WorkOrderRecipe
                    {
                        tagPath = LoadFileFromFtp(ftp, drl.Trim().Replace(drillInfoLocatorReply.data.FtpBasicUrl, "")),
                        tagCode = reportBody.Lot,
                        tagType = LoadFileType.PROGRAM
                    });
                }
                foreach (var pin in drillInfoLocatorReply.data.pin)
                {
                    list.Add(new WorkOrderRecipe
                    {
                        tagPath = LoadFileFromFtp(ftp, pin.Trim().Replace(drillInfoLocatorReply.data.FtpBasicUrl, "")),
                        tagCode = reportBody.Lot,
                        tagType = LoadFileType.PIN
                    });
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return list;
    }
}
