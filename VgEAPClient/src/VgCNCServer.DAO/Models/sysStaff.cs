using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("sysStaff")]
public class sysStaff
{
    [Key]
    public string? sInnerID { get; set; }

    public string? sLoginName { get; set; }
    public string? sEName { get; set; }
    public string? sCName { get; set; }
    public string? sPWD { get; set; }
    public string? sRole { get; set; }
    public string? sAddTime { get; set; }
    public string? sAddUser { get; set; }
    public string? sMemo { get; set; }
    public int? iStatus { get; set; }
}
