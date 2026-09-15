using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgSQLServerStore.Plugin.Models;

[Table("usrAlarm")]
public class usrAlarm
{
    [Key]
    public string? sInnerID { get; set; }
    public DateOnly? dtDate { get; set; }
    public string? sStartTime { get; set; }
    public string? sEndTime { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sAlarmID { get; set; }
    public string? sAlarmDesc { get; set; }
    public string? sActProgram { get; set; }

}
