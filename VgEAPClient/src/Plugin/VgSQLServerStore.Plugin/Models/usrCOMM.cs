using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgSQLServerStore.Plugin.Models;

[Table("usrCOMM")]
public class usrCOMM
{
    [Key]
    public string? sInnerID { get; set; }
    public DateOnly? dtDate { get; set; }
    public string? sTime { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sCOMM { get; set; }
    public string? sActProgram { get; set; }
    public string? sEventCode { get; set; }

}
