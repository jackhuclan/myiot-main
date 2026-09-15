using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgAOI.Plugin.Models;

[Table("PC_DATA_EMP")]
public class PC_DATA_EMP
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 计数
    /// </summary>
    public string EMPNO { get; set; }
    public string EMPNAME { get; set; }
    public string EMPCODE { get; set; }
    public string EMPDES { get; set; }
    public DateTime EMPTIME { get; set; }

    /// <summary>
    /// 时间:yy-mm-dd hh:mm:ss
    /// </summary>
    public DateTime CREATETIME { get; set; }

}
