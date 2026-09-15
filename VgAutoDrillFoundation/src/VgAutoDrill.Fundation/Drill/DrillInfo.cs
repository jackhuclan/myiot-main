namespace VgAutoDrill.Fundation.Drill;

/// <summary>
/// 钻带信息
/// </summary>
public class DrillInfo
{
    /// <summary>
    /// 板料长度
    /// </summary>
    public float PanelLength { get; set; }
    /// <summary>
    /// 板料宽度
    /// </summary>
    public float PanelWidth { get; set; }
    /// <summary>
    /// 板料销钉距中心偏移量
    /// </summary>
    public float PinOffset { get; set; }
    /// <summary>
    /// 料号
    /// </summary>
    public string ItemCode { get; set; } = string.Empty;
    /// <summary>
    /// Lot二维码
    /// </summary>
    public string LotId { get; set; } = string.Empty;
    /// <summary>
    /// 钻带文件路径
    /// </summary>
    public string DrlPath { get; set; } = string.Empty;
    /// <summary>
    /// 钻带参数文件路径
    /// </summary>
    public string DiaPath { get; set; } = string.Empty;
    /// <summary>
    /// Atp文件路径
    /// </summary>
    public string AtpPath { get; set; } = string.Empty;
}
