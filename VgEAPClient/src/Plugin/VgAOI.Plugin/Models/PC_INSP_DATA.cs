using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgAOI.Plugin.Models;

[Table("PC_INSP_DATA")]
public class PC_INSP_DATA
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 计数
    /// </summary>
    public string PRODUCT { get; set; }
    public string CONTAINER { get; set; }
    public string INSPITEMCODE { get; set; }
    public string INSPITEMDES { get; set; }
    public string PARENTID { get; set; }
    public string PNLIDORSETID { get; set; }
    public string MINVAL { get; set; }
    public string MAXVAL { get; set; }
    public string VAL { get; set; }
    public string UOM { get; set; }
    public string BAD_CAUSE_CODE { get; set; }
    public string PASSYN { get; set; }
    public string GROUPNUM { get; set; }

    /// <summary>
    /// 时间:yy-mm-dd hh:mm:ss
    /// </summary>
    public DateTime CREATETIME { get; set; }

}
