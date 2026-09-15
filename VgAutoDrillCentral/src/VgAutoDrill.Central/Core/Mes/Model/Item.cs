namespace VgAutoDrill.Central.Core.Mes;

public class Item
{
    /// <summary>
    /// 编码
    /// </summary>
    public string? Code { get; set; }
    /// <summary>
    /// 板料长度
    /// </summary>
    public float PanelLength { get; set; }
    /// <summary>
    /// 条码
    /// </summary>
    public string? IncodeNumber { get; set; }
    /// <summary>
    /// 叠板层数，
    /// 从单元定义表获取，如果未定义，默认为1
    /// </summary>
    public decimal? PanelCount { get; set; }

    /// <summary>
    /// 钻带文件路径
    /// </summary>
    public virtual string? DrillFilePath { get; set; }

    /// <summary>
    /// 板料宽度
    /// </summary>
    public float PanelWidth { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public virtual string? UnitOfMeasure { get; set; } = "Panel";

    /// <summary>
    /// 产品大类编码
    /// </summary>
    public virtual string? ProductCategoryCode { get; set; } = "P01";

    /// <summary>
    /// 物料1产品2
    /// Item Or Product
    /// </summary>
    public virtual int? ItemOrProduct { get; set; } = 2;
}
