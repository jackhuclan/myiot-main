namespace VegaIot.External.XianjinIot.Models;

/// <summary>
/// 用于MES下发AGV行走到指定位置
/// </summary>
internal class AgvWalkLocationPayload
{
    /// <summary>
    /// 请求头
    /// </summary>
    public HeaderEntity header { get; set; } = new();
    /// <summary>
    /// 配置集合
    /// </summary>
    public WalkLocationBody body { get; set; } = new();
}

internal class WalkLocationBody
{
    /// <summary>
    /// 任务编号
    /// </summary>
    public string taskCode { get; set; } = string.Empty;
    /// <summary>
    /// 位置信息
    /// </summary>
    public string coordinate { get; set; } = string.Empty;
}
