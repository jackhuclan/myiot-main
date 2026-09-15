using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgSQLServerStore.Plugin.Models;

[Table("usrAnalysisDuty")]
public class usrAnalysisDuty
{
    [Key]
    public string? sInnerID { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sZ { get; set; }
    public DateOnly? dtDate { get; set; }
    public string? sTime { get; set; }
    public string? sChangePanels { get; set; }
    public string? sStandardChangePanelsTime { get; set; }
    public string? sChangeDrills { get; set; }
    public string? sStandardChangeDrillsTime { get; set; }
    public string? sExceptionHandleCounts { get; set; }
    public string? sExceptionHandleTime { get; set; }
    public string? sMaintainTime { get; set; }
    public string? sTheoryDuty { get; set; }
    public string? sActualDuty { get; set; }
    public string? sDifference { get; set; }
    public string? sShift { get; set; }
}
