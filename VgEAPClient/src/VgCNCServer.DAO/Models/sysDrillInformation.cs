using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("sysDrillInformation")]
public class sysDrillInformation
{
    [Key]
    public string? sInnerID { get; set; }

    /// <summary>
    /// 设备编号
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 稼动率
    /// </summary>
    public string? sDuty { get; set; }

    /// <summary>
    /// 机台状态
    /// </summary>
    public string? sWorkMode { get; set; }

    /// <summary>
    /// 钻孔进度
    /// </summary>
    public string? sPersent { get; set; }

    /// <summary>
    /// 已钻孔数
    /// </summary>
    public string? sDrilled { get; set; }

    /// <summary>
    /// 总共孔数
    /// </summary>
    public string? sNeeded { get; set; }

    /// <summary>
    /// 钻带文件
    /// </summary>
    public string? sActProgram { get; set; }

    /// <summary>
    /// 参数程序
    /// </summary>
    public string? sDiaFileName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? sRegistrationDate { get; set; }

    /// <summary>
    /// 钻机状态
    /// </summary>
    public string? sStatus { get; set; }

    /// <summary>
    /// 当前时间
    /// </summary>
    public DateTime? dtDateTime { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
