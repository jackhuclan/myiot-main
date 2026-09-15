namespace VgAutoUpdater.Core.Configuration;

/// <summary>
/// 更新包安装策略
/// </summary>
[Flags]
public enum UpdatesInstallationPolicy
{
    /// <summary>
    /// install when updates is ready
    /// </summary>
    Immediate = 1,
    /// <summary>
    /// agent 准备好的时候，主动请求更新
    /// </summary>
    AgentReady = 2,
    /// <summary>
    /// 从中控强制更新
    /// </summary>
    CentralForce = 4,

    AgentReadyOrCentralForce = AgentReady | CentralForce,
}
