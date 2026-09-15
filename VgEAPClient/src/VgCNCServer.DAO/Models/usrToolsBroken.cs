using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrToolsBroken")]
public class usrToolsBroken
{
    [Key]
    public string? sInnerID { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly dtDate { get; set; }

    /// <summary>
    /// 断刀时间
    /// </summary>
    public string? sTime { get; set; }

    /// <summary>
    /// 机台编号
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 钻带文件
    /// </summary>
    public string? sActProgram { get; set; }

    /// <summary>
    /// 断刀次数
    /// </summary>
    public string? sBrokens { get; set; }

    /// <summary>
    /// 断刀轴号
    /// </summary>
    public string? sSpindle { get; set; }

    /// <summary>
    /// 刀具号
    /// </summary>
    public string? sToolID { get; set; }

    /// <summary>
    /// 刀径
    /// </summary>
    public string? sToolDIA { get; set; }

    /// <summary>
    /// 孔号
    /// </summary>
    public string? sHoleID { get; set; }

    /// <summary>
    /// 断刀寿命
    /// </summary>
    public string? sBrokenLife { get; set; }

    /// <summary>
    /// X坐标
    /// </summary>
    public string? sX { get; set; }

    /// <summary>
    /// Y坐标
    /// </summary>
    public string? sY { get; set; }

    /// <summary>
    /// 程序块
    /// </summary>
    public string? sProgramBlock { get; set; }

    /// <summary>
    /// 程序阶级
    /// </summary>
    public string? sProgramStep { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
