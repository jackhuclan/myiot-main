namespace VegaIot.External.AgvEntity.STD;

/// <summary>
/// 请求 生成斯坦德移动任务
/// </summary>
public class StdMoveRequest
{
    /// <summary>
    /// 每个请求都要一个唯一编号， 同一个请求重复提交， 使用同一编号
    /// </summary>
    public string? ReqCode { get; private set; } = Guid.NewGuid().ToString();

    public string? ReqTime { get; private set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    /// <summary>
    /// 1：指定库位，2：指定区域
    /// 其他非法值
    /// </summary>
    public int TargetType { get; set; } = -1;

    public string? TargetPosCode { get; set; } = string.Empty;

    /// <summary>
    /// 传入agv 的ID， 如AGV01
    /// </summary>
    public string VehicleId { get; set; } = string.Empty;

    /// <summary>
    /// 组任务编号
    /// </summary>
    public string? GroupId { get; set; } = string.Empty;
}

public class StdCompleteGroupRequest
{
    public string? GroupId { get; set; } = string.Empty;
}
