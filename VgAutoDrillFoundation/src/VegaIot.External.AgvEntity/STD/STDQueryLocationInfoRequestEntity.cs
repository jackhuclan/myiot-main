using System.Diagnostics.CodeAnalysis;

namespace VegaIot.External.AgvEntity.STD;

public class STDQueryLocationInfoRequestEntity : STDBaseEntity
{
    #region -- 属性 --
    /// <summary>
    /// 区域编码
    /// 必填
    /// </summary>
    [DisallowNull]
    public String areaCode { get; set; }
    /// <summary>
    /// 返回码
    /// 必填
    /// </summary>
    [DisallowNull]
    public String code { get; set; }
    /// <summary>
    /// 返回消息
    /// 必填
    /// </summary>
    [DisallowNull]
    public String msg { get; set; }
    /// <summary>
    /// 返回数据
    /// 必填
    /// </summary>
    [DisallowNull]
    public QueryLocationInfoData data { get; set; }

    #endregion
}

public class QueryLocationInfoData
{
    /// <summary>
    /// 区域编号
    /// </summary>
    public String areaCode { get; set; }
    /// <summary>
    /// 物料批次
    /// </summary>
    public String materialLot { get; set; }
    /// <summary>
    /// 货架编号
    /// 必填
    /// </summary>
    [DisallowNull]
    public String podCode { get; set; }
    /// <summary>
    /// 地码编号，唯一标识
    /// 必填
    /// </summary>
    [DisallowNull]
    public String mapDataCode { get; set; }
    /// <summary>
    /// 位置编号，地图位置的别名，能任意命名(字母+数字)，但要唯一，由RCS-2000 界面配置。
    /// 必填
    /// </summary>
    [DisallowNull]
    public String positionCode { get; set; }
}
