
namespace VgAutoUpdater.Core.Configuration;

public class VgAutoUpdaterOptions
{
    public string VersionUrl { get; set; } = string.Empty;
    public bool Enable { get; set; } = true;
    public int UpdateIntervalAtMinute { get; set; } = 10;
    /// <summary>
    /// 更新包存放位置
    /// </summary>
    public string UpdatesLocation { get; set; } = string.Empty;
    public UpdatesInstallationPolicy InstallationPolicy { get; set; } = UpdatesInstallationPolicy.AgentReady;
    public List<string> Apps { get; set; } = new List<string>();
}
