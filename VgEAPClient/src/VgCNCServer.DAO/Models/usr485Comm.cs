using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usr485Comm")]
public class usr485Comm
{
    [Key]
    public string? sInnerID { get; set; }

    public DateOnly dtDate { get; set; }
    public string? sTime { get; set; }
    public string? sEquipmentID { get; set; }
    public string? sPressureValue { get; set; }
}
