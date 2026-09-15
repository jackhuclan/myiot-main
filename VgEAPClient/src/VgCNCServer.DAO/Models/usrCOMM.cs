using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrCOMM")]
public class usrCOMM
{
    [Key]
    public string? sInnerID { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly dtDate { get; set; }

    /// <summary>
    /// 输入时间
    /// </summary>
    public string? sTime { get; set; }

    /// <summary>
    /// 机台编号
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 指令代码
    /// </summary>
    public string? sCOMM { get; set; }

    /// <summary>
    /// 钻带文件
    /// </summary>
    public string? sActProgram { get; set; }

    /// <summary>
    /// 事件代码
    /// </summary>
    public string? sEventCode { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
