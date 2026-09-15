using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgAOI.Plugin.Models;

[Table("EAP_MATERIAL_DATA")]
public class EAP_MATERIAL_DATA
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 计数
    /// </summary>
    public string PRODUCT { get; set; }
    public string? CONTAINER { get; set; }
    public int? COUNT { get; set; }
    public string MATERIALCODE { get; set; }
    public string ITEMCODE { get; set; }
    public string ITEMDES { get; set; }
    public string? UOM { get; set; }
    public string ITEMVALUE { get; set; }
    public decimal GROUPNUM { get; set; }
    public DateTime CREATETIME { get; set; }

}
