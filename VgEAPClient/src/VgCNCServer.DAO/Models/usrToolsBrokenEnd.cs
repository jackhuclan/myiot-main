using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrToolsBrokenEnd")]
public class usrToolsBrokenEnd
{
    [Key]
    public string? sInnerID { get; set; }

    public DateOnly dtDate { get; set; }
    public string? sTime { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sActProgram { get; set; }
    public string? sBrokens { get; set; }
    public string? sSpindle { get; set; }
    public string? sToolID { get; set; }
    public string? sToolDIA { get; set; }
    public string? sHoleID { get; set; }
    public string? sX { get; set; }
    public string? sY { get; set; }
    public string? sProgramBlock { get; set; }
    public string? sProgramStep { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }
}
