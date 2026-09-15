namespace VegaIot.External.AgvEntity.STD;

public class MaterialData
{
    /// <summary>
    /// 真实搬运任务单号
    /// </summary>
    public string? realTaskCode { get; set; }

    /// <summary>
    /// 运输工序 下工序
    /// </summary>
    public string? carryPro { get; set; }

    /// <summary>
    /// 批次组号(母lot)
    /// </summary>
    public string? materialGroupCode { get; set; }

    /// <summary>
    /// 批次号
    /// </summary>
    public string materialLot { get; set; }

    /// <summary>
    /// 物料号
    /// </summary>
    public string? materialCode { get; set; }

    /// <summary>
    /// 载具类型Q8(大车)，Q6(小车)
    /// </summary>
    public string? carrierTyp { get; set; }

    /// <summary>
    /// 批次系统数量
    /// </summary>
    public string? sysLotNum { get; set; }

    /// <summary>
    /// 批次本托盘数量
    /// </summary>
    public string podLotNum { get; set; }

    /// <summary>
    /// 批次状态（只能传OK/NG）
    /// </summary>
    public string? lotStatus { get; set; }

    /// <summary>
    /// 是否包含报废板子（0-否 1–是）
    /// </summary>
    public string? includeScrap { get; set; }

    /// <summary>
    /// 托盘状态：0-空托盘 1-正常物料 2-异常物料
    /// </summary>
    public string? podStatus { get; set; }

    public MaterialData()
    {
        realTaskCode = string.Empty;

        carryPro = string.Empty;

        materialGroupCode = string.Empty;

        materialLot = string.Empty;

        materialCode = string.Empty;

        carrierTyp = string.Empty;

        sysLotNum = string.Empty;

        podLotNum = string.Empty;

        lotStatus = string.Empty;

        includeScrap = string.Empty;

        podStatus = string.Empty;
    }
}
