namespace VgDeviceGateway.Devices.Common.Agv;

public class AgvPageEntity
{
    public AgvPageEntity()
    {
        AgvId = string.Empty;
        AgvName = string.Empty;
        MaterialCode = string.Empty;
        LoadMaterialType = 0;
        StartLayer = 0;
        LoadLayerCount = 0;
        PanelWidth = 0;
        PanelLength = 0;
        PinOffset = 0;
        PanelPcs = 0;
    }

    public string AgvId { get; set; }
    public string AgvName { get; set; }

    /// <summary>
    /// 料仓
    /// </summary>
    public string SiloCode { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; }

    /// <summary>
    /// 物料类别
    /// 0 空，1生料，2熟料
    /// </summary>
    public int LoadMaterialType { get; set; }

    /// <summary>
    /// 开始加载的层数
    /// </summary>
    public int StartLayer { get; set; }

    /// <summary>
    /// 加载的层数
    /// </summary>
    public int LoadLayerCount { get; set; }

    public float PanelWidth { get; set; }

    public float PanelLength { get; set; }

    public float PinOffset { get; set; }

    public int PanelPcs { get; set; }
}
