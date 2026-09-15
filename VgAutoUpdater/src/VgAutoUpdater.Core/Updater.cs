using System.IO.Compression;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoUpdater.Core.Configuration;
using VgAutoUpdater.Core.Models;

namespace VgAutoUpdater.Core;

public class Updater : IUpdater
{
    private readonly VgAutoUpdaterOptions _vgAutoUpdaterOptions;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IScriptRunnerFactory _scriptRunnerFactory;
    private readonly ILogger<Updater> _logger;
    private Versions? _versions;
    private List<App> _localPackages = new();
    private List<App> _availablePackages = new();
    private const string VERSION_FILE_NAME = "versions.json";

    public Updater(IOptions<VgAutoUpdaterOptions> options,
        IHttpClientFactory httpClientFactory,
        IScriptRunnerFactory scriptRunnerFactory,
        ILoggerFactory loggerFactory)
    {
        _vgAutoUpdaterOptions = options.Value;
        _httpClientFactory = httpClientFactory;
        _scriptRunnerFactory = scriptRunnerFactory;
        _logger = loggerFactory.CreateLogger<Updater>();
        _versions = new Versions();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var versionPath = Path.Combine(_vgAutoUpdaterOptions.UpdatesLocation, VERSION_FILE_NAME);
            if (File.Exists(versionPath))
            {
                _versions = JsonSerializer.Deserialize<Versions>(File.ReadAllText(versionPath));
                if (_versions == null) return Task.CompletedTask;
                _localPackages = _versions.AppList;

                foreach (var app in _versions.AppList)
                {
                    RefreshAppInfoViaLocalFile(app);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public List<App> ExistPackages(string appName)
    {
        if (_localPackages == null) return new List<App>();
        return _localPackages.Where(x => x.AppName == appName && x.Downloaded).ToList();
    }

    /// <inheritdoc/>
    public async Task<UpdateResponse> CheckPackages(string appName)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var json = await httpClient.GetStringAsync(_vgAutoUpdaterOptions.VersionUrl + "/" + VERSION_FILE_NAME);
        if (string.IsNullOrEmpty(json))
        {
            var errorMessage = "please configure correct VersionUrl in VgAutoUpdaterOptions section!";
            _logger.LogError(errorMessage);
            return UpdateResponse.Fail(errorMessage);
        }

        try
        {
            _versions = JsonSerializer.Deserialize<Versions>(json);
            if (_versions == null || !_versions.Enable)
            {
                var errorMessage = "can't deserialize version.json";
                _logger.LogError(errorMessage);
                return UpdateResponse.Fail(errorMessage);
            }

            if (!Directory.Exists(_vgAutoUpdaterOptions.UpdatesLocation))
            {
                Directory.CreateDirectory(_vgAutoUpdaterOptions.UpdatesLocation);
            }

            File.WriteAllText(Path.Combine(_vgAutoUpdaterOptions.UpdatesLocation, VERSION_FILE_NAME), json);

            foreach (var app in _versions.AppList)
            {
                if (_localPackages.Contains(app)) _localPackages.Remove(app);
                RefreshAppInfoViaLocalFile(app);
                _localPackages.Add(app);
            }

            //download common scripts for preparations
            foreach (var script in _versions.CommonScripts)
            {
                var resourceUrl = _vgAutoUpdaterOptions.VersionUrl + "/" + script;
                var saveAsFilePath = Path.Combine(_vgAutoUpdaterOptions.UpdatesLocation, Path.GetFileName(script));
                await DownloadFile(resourceUrl, saveAsFilePath);

                //如果是zip包解压到当前文件夹
                if (File.Exists(saveAsFilePath) && Path.GetExtension(saveAsFilePath) == ".zip")
                {
                    var zipDir = Path.Combine(_vgAutoUpdaterOptions.UpdatesLocation, Path.GetFileNameWithoutExtension(script));
                    if (Directory.Exists(zipDir)) Directory.Delete(zipDir, true);
                    ZipFile.ExtractToDirectory(saveAsFilePath, zipDir);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return UpdateResponse.Fail(ex.Message);
        }

        _availablePackages = _versions.Enable ? _localPackages.Where(x => x.AppName == appName && x.EnableUpdate).ToList() : new List<App>();
        return _availablePackages.Count > 0 ? UpdateResponse.Ok(_availablePackages) : UpdateResponse.Fail("no updates are found!");
    }

    /// <inheritdoc/>
    public async Task<UpdateResponse> DownloadPackages(string appName, string version, bool force = false)
    {
        if (_versions == null || _availablePackages.Count == 0) return UpdateResponse.Fail("no updates are found!");
        var targetApp = _availablePackages.FirstOrDefault(x => x.AppName == appName && x.AppVersion.ToString() == version);
        if (targetApp == null) return UpdateResponse.Fail("no updates are configured in " + VERSION_FILE_NAME);

        var saveDir = Path.Combine(_vgAutoUpdaterOptions.UpdatesLocation, targetApp.AppName);
        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
        var zipFilePath = Path.Combine(_vgAutoUpdaterOptions.UpdatesLocation, targetApp.AppName, targetApp.ZipName);
        if (force)
        {
            if (File.Exists(zipFilePath)) File.Delete(zipFilePath);
        }
        else
        {
            if (File.Exists(zipFilePath)) return UpdateResponse.Ok("already exists");
        }

        try
        {
            var zipResourceUrl = _vgAutoUpdaterOptions.VersionUrl + "/" + targetApp.AppName + "/" + targetApp.ZipName;
            await DownloadFile(zipResourceUrl, zipFilePath);
            targetApp.Downloaded = true;

            return UpdateResponse.Ok(zipFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return UpdateResponse.Fail(ex.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<UpdateResponse> InstallPackages(string appName, string version, Dictionary<string, object> extraParams, bool force = false)
    {
        if (_versions == null || _availablePackages.Count == 0) return UpdateResponse.Fail("no updates are found!");
        var targetApp = _availablePackages.FirstOrDefault(x => x.AppName == appName && x.AppVersion.ToString() == version);
        if (targetApp == null) return UpdateResponse.Fail("no updates are configured in " + VERSION_FILE_NAME);
        var downPackageResult = await DownloadPackages(appName, version);
        if (downPackageResult.Code != "200")
        {
            return downPackageResult;
        }
        var saveDir = Path.Combine(_vgAutoUpdaterOptions.UpdatesLocation, targetApp.AppName);
        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
        var zipFilePath = Path.Combine(saveDir, targetApp.ZipName);
        if (!File.Exists(zipFilePath)) return UpdateResponse.Fail("please download " + targetApp.ZipName + " at first!");
        saveDir = Path.Combine(saveDir, Path.GetFileNameWithoutExtension(targetApp.ZipName));

        try
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30000));

            foreach (var script in targetApp.PreScripts)
            {
                await ExecuteScript(script, _vgAutoUpdaterOptions.UpdatesLocation, extraParams, cancellationTokenSource.Token);
            }

            //解压并覆盖同文件
            using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
            {
                archive.ExtractToDirectory(saveDir, true);
            }

            // ZipFile.ExtractToDirectory(zipFilePath, saveDir);
            //execute Common script
            foreach (var script in targetApp.PostScripts)
            {
                await ExecuteScript(script, _vgAutoUpdaterOptions.UpdatesLocation, extraParams, cancellationTokenSource.Token);
            }

            targetApp.Installed = true;
            targetApp.InstalledLocation = saveDir;
            UpdateVersionsJson(_versions);

            return UpdateResponse.Ok(zipFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return UpdateResponse.Fail(ex.Message);
        }
    }

    private void RefreshAppInfoViaLocalFile(App app)
    {
        var zipFullPath = Path.Combine(_vgAutoUpdaterOptions.UpdatesLocation, app.AppName, app.ZipName);
        app.Downloaded = File.Exists(zipFullPath);
        app.Installed = Directory.Exists(Path.Combine(Path.GetDirectoryName(zipFullPath), Path.GetFileNameWithoutExtension(app.ZipName)));
    }

    private async Task DownloadFile(string resourceUrl, string saveAsFilePath)
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

    private async Task<UpdateResponse> ExecuteScript(string script, string rootPath, Dictionary<string, object> extraParams, CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Run(async () =>
             {
                 var runner = _scriptRunnerFactory.Create();
                 await runner.RunCommand(script, rootPath, extraParams, cancellationToken);
             });

            return UpdateResponse.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return UpdateResponse.Fail(ex.Message);
        }
    }

    private void UpdateVersionsJson(Versions versions)
    {
        try
        {
            string json = JsonSerializer.Serialize(versions, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(Path.Combine(_vgAutoUpdaterOptions.UpdatesLocation, VERSION_FILE_NAME), json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
