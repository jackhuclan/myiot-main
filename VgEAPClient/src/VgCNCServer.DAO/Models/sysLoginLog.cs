using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("sysLoginLog")]
public class sysLoginLog
{
    [Key]
    public string? sInnerID { get; set; }

    /// <summary>
    /// 设备编号
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 登录时间
    /// </summary>
    public string? sLoginTime { get; set; }

    /// <summary>
    /// 授权码
    /// </summary>
    public string? sAuthorizationCode { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? sMemo { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
