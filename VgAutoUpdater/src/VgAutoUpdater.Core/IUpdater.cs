using Microsoft.Extensions.Hosting;
using VgAutoUpdater.Core.Models;

namespace VgAutoUpdater.Core;

public interface IUpdater : IHostedService
{
    /// <summary>
    /// 已经下载的安装包
    /// </summary>
    /// <param name="appName">应用程序名称</param>
    /// <returns></returns>
    List<App> ExistPackages(string appName);
    /// <summary>
    /// 检查是否有更新包
    /// </summary>
    /// <param name="appName">应用程序名称</param>
    /// <returns></returns>
    Task<UpdateResponse> CheckPackages(string appName);
    /// <summary>
    /// 下载更新包
    /// </summary>
    /// <param name="appName">应用程序名称</param>
    /// <param name="version">应用程序版本</param>
    /// <param name="force">强制重新下载更新</param>
    /// <returns></returns>
    Task<UpdateResponse> DownloadPackages(string appName, string version, bool force = false);
    /// <summary>
    /// 安装更新包
    /// </summary>
    /// <param name="appName">应用程序名称</param>
    /// <param name="version">应用程序版本</param>
    /// <param name="force">强制重新安装更新</param>
    /// <returns></returns>
    Task<UpdateResponse> InstallPackages(string appName, string version, Dictionary<string,object> extraParams, bool force = false);
}
