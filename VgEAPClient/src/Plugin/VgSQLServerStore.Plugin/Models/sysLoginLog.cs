using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgSQLServerStore.Plugin.Models;

[Table("sysLoginLog")]
public class sysLoginLog
{
    [Key]
    public string? sInnerID { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sLoginTime { get; set; }
    public string? sAuthorizationCode { get; set; }
    public string? sMemo { get; set; }
}
