using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrDuty")]
public class usrDuty
{
    [Key]
    public string? sInnerID { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly dtDate { get; set; }

    /// <summary>
    /// 机台编号
    /// </summary>
    public string? sEquipmentID { get; set; }

    /// <summary>
    /// 白班稼动率
    /// </summary>
    public string? sDutyShiftA { get; set; }

    /// <summary>
    /// 夜班稼动率
    /// </summary>
    public string? sDutyShiftB { get; set; }

    /// <summary>
    /// 中班稼动率
    /// </summary>
    public string? sDutyShiftC { get; set; }

    [NotMapped]
    public string? sEqpType { get; set; }

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
