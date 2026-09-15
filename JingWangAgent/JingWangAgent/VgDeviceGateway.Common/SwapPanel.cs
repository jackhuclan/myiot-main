using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Common;

public class SwapPanel
{
    public SwapPanel()
    {
        AgvPosition = string.Empty;
        SpindleId = 0;
        PanelList = new List<Panel>();
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
    /// 对应Drill轴动作
    /// </summary>
    public InteractionSequence InteractionSequence { set; get; }

    /// <summary>
    /// 板信息
    /// </summary>
    public List<Panel> PanelList { set; get; }
}
