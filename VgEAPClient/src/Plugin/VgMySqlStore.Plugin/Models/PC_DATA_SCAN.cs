using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgMySqlStore.Plugin.Models;

[Table("PC_DATA_SCAN")]
public class PC_DATA_SCAN
{
    [Key]

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 批次号
    /// </summary>
    public string? CONTAINER { get; set; }    //批次号
    /// <summary>
    /// 码类型
    /// </summary>
    public string PARENTID { get; set; }
    /// <summary>
    /// 父级ID
    /// </summary>
    public string DRCTYPE { get; set; }
    /// <summary>
    /// 板码
    /// </summary>
    public string PNLIDORSETID { get; set; }
    /// <summary>
    /// 建立人
    /// </summary>
    public string CREATEUSER { get; set; }
    /// <summary>
    /// 时间:yy-mm-dd hh:mm:ss
    /// </summary>
    public DateTime CREATETIME { get; set; }

}
