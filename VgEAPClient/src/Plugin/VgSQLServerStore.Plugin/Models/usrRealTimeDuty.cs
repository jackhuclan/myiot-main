using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgSQLServerStore.Plugin.Models;

[Table("usrRealTimeDuty")]
public class usrRealTimeDuty
{
    [Key]
    public string? sInnerID { get; set; }
    public string? sZ { get; set; }
    public DateOnly? dtDate { get; set; }
    public string? sTime { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sDuty { get; set; }
    public string? sWorkTime { get; set; }
    public string? sWaitTime { get; set; }
    public string? sStopTime { get; set; }
    public string? sTotalTime { get; set; }
    public string? sHits { get; set; }
    public string? sRoutPath { get; set; }
    public string? sChangePanels { get; set; }
    public string? sChangeDrills { get; set; }
    public string? sRegistrationDate { get; set; }
    public string? sShift { get; set; }
}
