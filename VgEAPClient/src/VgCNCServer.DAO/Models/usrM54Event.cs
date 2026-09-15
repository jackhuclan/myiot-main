using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrM54Event")]
public class usrM54Event
{
    [Key]
    public string? sInnerID { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly dtDate { get; set; }

    /// <summary>
    /// 事件时间
    /// </summary>
    public string? sTime { get; set; }

    /// <summary>
    /// 机台编号
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 事件代码
    /// </summary>
    public string? sEventID { get; set; }

    /// <summary>
    /// 日志描述
    /// </summary>
    public string? sEventDesc { get; set; }

    /// <summary>
    /// 钻带文件
    /// </summary>
    public string? sActProgram { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
