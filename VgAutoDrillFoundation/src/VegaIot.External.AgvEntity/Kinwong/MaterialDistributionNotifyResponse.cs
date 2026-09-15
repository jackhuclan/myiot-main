namespace VegaIot.External.AgvEntity.Kinwong;

/// <summary>
/// EAP下机配送物料信息通知 - 返回结果
/// </summary>
public class MaterialDistributionNotifyResponse
{
    /// <summary>
    /// 状态编号  0 – 成功
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 返回信息
    /// </summary>
    public string Message { get; set; }
}
