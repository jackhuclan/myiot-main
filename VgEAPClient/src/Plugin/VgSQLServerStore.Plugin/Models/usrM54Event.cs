using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgSQLServerStore.Plugin.Models;

[Table("usrM54Event")]
public class usrM54Event
{
    [Key]
    public string? sInnerID { get; set; }
    public DateOnly? dtDate { get; set; }
    public string? sTime { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sEventID { get; set; }
    public string? sEventDesc { get; set; }
    public string? sActProgram { get; set; }
}
