using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgSQLServerStore.Plugin.Models;

[Table("sysDrillInformation")]
public class sysDrillInformation
{
    [Key]
    public string? sInnerID { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sDuty { get; set; }
    public string? sWorkMode { get; set; }
    public string? sPersent { get; set; }
    public string? sDrilled { get; set; }
    public string? sNeeded { get; set; }
    public string? sActProgram { get; set; }
    public string? sDiaFileName { get; set; }
    public string? sRegistrationDate { get; set; }
    public string? sStatus { get; set; }
    public DateTime? dtDateTime { get; set; }
}
