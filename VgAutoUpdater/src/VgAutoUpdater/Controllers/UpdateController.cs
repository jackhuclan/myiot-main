using Microsoft.AspNetCore.Mvc;
using VgAutoUpdater.Core;
using VgAutoUpdater.Core.Models;

namespace VgAutoUpdater.Controllers;

[ApiController]
[Route("[controller]")]
public class UpdateController : ControllerBase
{
    private readonly IUpdater _updater;
    private readonly ILogger<UpdateController> _logger;

    public UpdateController(IUpdater updater,
        ILogger<UpdateController> logger)
    {
        _updater = updater;
        _logger = logger;
    }

    [HttpGet("CheckPackages", Name = "CheckPackages")]
    public Task<UpdateResponse> Check(string appName)
    {
        _logger.LogInformation(appName);
        return _updater.CheckPackages(appName);
    }

    [HttpGet("ExistPackages", Name = "ExistPackages")]
    public Task<List<App>> Exist(string appName)
    {
        return Task.FromResult(_updater.ExistPackages(appName));
    }

    [HttpGet("DownloadPackages", Name = "DownloadPackages")]
    public Task<UpdateResponse> Download(string appName, string version)
    {
        return _updater.DownloadPackages(appName, version);
    }

    [HttpPost("InstallPackages", Name = "InstallPackages")]
    public Task<UpdateResponse> Install(string appName, string version,Dictionary<string,object> extraParams)
    {
        return _updater.InstallPackages(appName, version, extraParams);
    }
}
