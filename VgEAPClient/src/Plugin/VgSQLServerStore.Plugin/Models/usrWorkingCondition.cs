using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgSQLServerStore.Plugin.Models;

[Table("usrWorkingCondition")]
public class usrWorkingCondition
{
    [Key]
    public string? sInnerID { get; set; }
    public DateOnly? dtDate { get; set; }
    public string? sTime { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sActProgram { get; set; }
    public string? sNeeded { get; set; }
    public string? sStartTime { get; set; }
    public string? sEndTime { get; set; }
    public string? sWorkTime { get; set; }
    public string? sWaitTime { get; set; }

}
