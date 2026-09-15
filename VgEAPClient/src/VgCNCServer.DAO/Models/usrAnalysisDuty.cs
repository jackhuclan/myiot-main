using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrAnalysisDuty")]
public class usrAnalysisDuty
{
    [Key]
    public string? sInnerID { get; set; }

    /// <summary>
    /// "机台编号"
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 方式
    /// </summary>
    public string? sZ { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly dtDate { get; set; }

    /// <summary>
    /// 采集时间
    /// </summary>
    public string? sTime { get; set; }

    /// <summary>
    /// 换板次数
    /// </summary>
    public string? sChangePanels { get; set; }

    /// <summary>
    /// 标准换板时间
    /// </summary>
    public string? sStandardChangePanelsTime { get; set; }

    /// <summary>
    /// 换料次数
    /// </summary>
    public string? sChangeDrills { get; set; }

    /// <summary>
    /// 标准换料时间
    /// </summary>
    public string? sStandardChangeDrillsTime { get; set; }

    /// <summary>
    /// 异常处理次数
    /// </summary>
    public string? sExceptionHandleCounts { get; set; }

    /// <summary>
    /// 异常处理时间
    /// </summary>
    public string? sExceptionHandleTime { get; set; }

    /// <summary>
    /// 保养时间
    /// </summary>
    public string? sMaintainTime { get; set; }

    /// <summary>
    /// 理论稼动率
    /// </summary>
    public string? sTheoryDuty { get; set; }

    /// <summary>
    /// 实际稼动率
    /// </summary>
    public string? sActualDuty { get; set; }

    /// <summary>
    /// 差异
    /// </summary>
    public string? sDifference { get; set; }

    /// <summary>
    /// 所属班次
    /// </summary>
    public string? sShift { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
