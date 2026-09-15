using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Common;

public class SwapTray
{
    public SwapTray()
    {
        AgvPosition = string.Empty;
        SpindleId = 0;
        TrayList = new List<CutterTray>();
        InteractionSequence = new InteractionSequence();
    }

    /// <summary>
    /// agv位置
    /// </summary>
    public string AgvPosition { set; get; }

    /// <summary>
    /// 对应Drill轴ID
    /// </summary>
    public int SpindleId { set; get; }

    /// <summary>
    /// 动作区域
    /// </summary>
    public int Region { set; get; }

    /// <summary>
    /// 刀盘的位置
    /// </summary>
    public string CutterTrayPosition { set; get; }

    /// <summary>
    /// 对应Drill轴动作
    /// </summary>
    public InteractionSequence InteractionSequence { set; get; }

    /// <summary>
    /// 刀盘列表
    /// </summary>
    public List<CutterTray> TrayList { set; get; }
}
