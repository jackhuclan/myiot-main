using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgMySqlStore.Plugin.Models;

[Table("EAP_TASK_DATA")]
public class EAP_TASK_DATA
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 计数
    /// </summary>
    public string Product { get; set; }
    public string CONTAINER { get; set; }
    public decimal COUNT { get; set; }
    public string? PNLlist { get; set; }

}
