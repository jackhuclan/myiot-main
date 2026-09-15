using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgAOI.Plugin.Models;

[Table("PC_DATA_STATUS")]
public class PC_DATA_STATUS
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 计数
    /// </summary>
    public string STATUSCODE { get; set; }
    public string STATUSDES { get; set; }
    public string STATUSVALUE { get; set; }

    /// <summary>
    /// 时间:yy-mm-dd hh:mm:ss
    /// </summary>
    public DateTime CREATETIME { get; set; }

}
