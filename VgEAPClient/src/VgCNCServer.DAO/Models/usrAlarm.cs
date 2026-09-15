using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrAlarm")]
public class usrAlarm
{
    [Key]
    public string? sInnerID { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly dtDate { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public string? sStartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public string? sEndTime { get; set; }

    /// <summary>
    /// 机台编号
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 报警指令
    /// </summary>
    public string? sAlarmID { get; set; }

    /// <summary>
    /// 报警说明
    /// </summary>
    public string? sAlarmDesc { get; set; }

    /// <summary>
    /// 钻带文件
    /// </summary>
    public string? sActProgram { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
