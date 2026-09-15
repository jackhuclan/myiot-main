using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgSQLServerStore.Plugin.Models;

[Table("usrDuty")]
public class usrDuty
{
    [Key]
    public string? sInnerID { get; set; }

    public DateOnly? dtDate { get; set; }

    public string? sEquipmentID { get; set; }

    public string? sDutyShiftA { get; set; }

    public string? sDutyShiftB { get; set; }

    public string? sDutyShiftC { get; set; }

    //public usrDuty(string InnerID, DateOnly doDate, string EquipmentID, string DutyShiftA, string DutyShiftB, string DutyShiftC)
    //{
    //    sInnerID = InnerID;
    //    dtDate = doDate;
    //    sEquipmentID = EquipmentID;
    //    sDutyShiftA = DutyShiftA;
    //    sDutyShiftB = DutyShiftB;
    //    sDutyShiftC = DutyShiftC;
    //}
}
