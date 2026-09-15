namespace VgAutoDrill.Central.Core.Calculator;

/// <summary>
/// 料仓转运任务策略
/// </summary>
public interface IAutoSiloTransferStrategy
{
    /// <summary>
    /// 重新安排本区域的料仓转运任务
    /// </summary>
    /// <returns></returns>
    void ReassignSiloTransferJob();

    /// <summary>
    /// 安排熟料转出料仓任务
    /// </summary>
    /// <returns></returns>
    Task StartDrilledTrackOutThread();

    /// <summary>
    ///安排熟料转入料仓任务
    /// </summary>
    /// <returns></returns>

    Task StartDrilledTrackInThread();

    /// <summary>
    /// 安排生料转出料仓任务
    /// </summary>
    /// <returns></returns>
    Task StartUndrilledTrackOutThread();

    /// <summary>
    /// 安排空仓转入料仓任务
    /// </summary>
    /// <returns></returns>
    Task StartEmptyBoxTrackInThread();

    /// <summary>
    /// 安排空仓转出料仓任务
    /// </summary>
    /// <returns></returns>
    Task StartEmptyBoxTrackOutThread();

    /// <summary>
    /// 安排首件转出料仓任务
    /// </summary>
    /// <returns></returns>
    Task StartFirstTrackOutThread();

    /// <summary>
    /// 安排生料转入料仓任务
    /// </summary>
    /// <returns></returns>
    Task StartRawTrackInThread();
}
