namespace VegaIot.External.AgvEntity;

/// <summary>
/// STD取放料回调数据
/// </summary>
public class STDMaterialsRequestEntity
{
    /// <summary>
    /// 请求编号
    /// </summary>
    /// <returns></returns>
    public String? reqCode { get; set; }
    /// <summary>
    /// 任务ID
    /// </summary>
    /// <returns></returns>
    public String? taskId { get; set; }
    /// <summary>
    /// 请求时间
    /// </summary>
    public String? reqTime { get; set; }
    /// <summary>
    /// 位置
    /// </summary>
    public Int32 position { get; set; }
    /// <summary>
    /// 站点库位编码
    /// </summary>
    public String? positionCode { get; set; }
    /// <summary>
    /// 车辆名称
    /// </summary>
    public String? vehicleName { get; set; }
    /// <summary>
    /// 托盘号
    /// </summary>
    public String? trayNum { get; set; }
    /// <summary>
    /// 物料列表
    /// </summary>
    public List<MaterialInfo> materialList { get; set; }

}

public class MaterialInfo
{
    /// <summary>
    /// 批次号
    /// </summary>
    public String? lot { get; set; }
    /// <summary>
    /// 物料编码
    /// </summary>
    public String? code { get; set; }
    /// <summary>
    /// 批次系统数量
    /// </summary>
    public Int32 sysLotNum { get; set; }
    /// <summary>
    /// 本托盘数量
    /// </summary>
    public Int32 podLotNum { get; set; }
    /// <summary>
    /// 产品批次
    /// </summary>
    public String? productBatch { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public String? createTime { get; set; }
    /// <summary>
    /// 交期
    /// </summary>
    public String? deliveryTime { get; set; }
    /// <summary>
    /// 所在层级
    /// </summary>
    public Int32 levelVal { get; set; }
    /// <summary>
    /// 叠板PNL码列表
    /// </summary>
    public String? panels { get; set; }
    /// <summary>
    /// 铝片码
    /// </summary>
    public String? foldQrCode { get; set; }

    public string? pnlStatus { get; set; }

}
