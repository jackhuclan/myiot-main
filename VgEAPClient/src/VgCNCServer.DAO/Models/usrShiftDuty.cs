using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrShiftDuty")]
public class usrShiftDuty
{
    [Key]
    public string? sInnerID { get; set; }

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
    /// 机台编号
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 稼动率
    /// </summary>
    public string? sDuty { get; set; }

    /// <summary>
    /// 轴数
    /// </summary>
    public string? sT { get; set; }

    /// <summary>
    /// 加工时间
    /// </summary>
    public string? sWorkTime { get; set; }

    /// <summary>
    /// 等待时间
    /// </summary>
    public string? sWaitTime { get; set; }

    /// <summary>
    /// 停机时间
    /// </summary>
    public string? sStopTime { get; set; }

    /// <summary>
    /// 总时间
    /// </summary>
    public string? sTotalTime { get; set; }

    /// <summary>
    /// 孔数
    /// </summary>
    public string? sHits { get; set; }

    /// <summary>
    /// 班次总孔数
    /// </summary>
    public string? sShiftHits { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? sTools { get; set; }

    /// <summary>
    /// 断刀
    /// </summary>
    public string? sBrokens { get; set; }

    /// <summary>
    /// 换板次数
    /// </summary>
    public string? sChangePanels { get; set; }

    /// <summary>
    /// 换料次数
    /// </summary>
    public string? sChangeDrills { get; set; }

    /// <summary>
    /// 换料时间
    /// </summary>
    public string? sChangeDrillsTime { get; set; }

    //public string? sRegistrationDate { get; set; }

    /// <summary>
    /// 所属班次
    /// </summary>
    public string? sShift { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
