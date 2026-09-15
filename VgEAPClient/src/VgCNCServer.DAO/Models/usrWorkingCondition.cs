using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrWorkingCondition")]
public class usrWorkingCondition
{
    [Key]
    public string? sInnerID { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly dtDate { get; set; }

    /// <summary>
    /// 时间
    /// </summary>
    public string? sTime { get; set; }

    /// <summary>
    /// 机台编号
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 轴数
    /// </summary>
    public string? sT { get; set; }

    /// <summary>
    /// 钻带文件
    /// </summary>
    public string? sActProgram { get; set; }

    /// <summary>
    /// 加工孔数
    /// </summary>
    public string? sNeeded { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public string? sStartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public string? sEndTime { get; set; }

    /// <summary>
    /// 加工时间
    /// </summary>
    public string? sWorkTime { get; set; }

    /// <summary>
    /// 等待时间
    /// </summary>
    public string? sWaitTime { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
