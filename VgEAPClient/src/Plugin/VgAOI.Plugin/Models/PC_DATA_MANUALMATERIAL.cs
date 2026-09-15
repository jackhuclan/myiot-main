using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgAOI.Plugin.Models;

[Table("PC_DATA_MANUALMATERIAL")]
public class PC_DATA_MANUALMATERIAL
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 计数
    /// </summary>
    public string CONTAINER { get; set; }
    public string ITEMCODE { get; set; }
    public string ITEMDES { get; set; }
    public string UOM { get; set; }
    public string OLDITEMVALUE { get; set; }
    public string NEWITEMVALUE { get; set; }
    public string ISOK { get; set; }

    /// <summary>
    /// 时间:yy-mm-dd hh:mm:ss
    /// </summary>
    public DateTime CREATETIME { get; set; }

}
