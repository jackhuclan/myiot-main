using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgMySqlStore.Plugin.Models;

[Table("PC_DATA_COUNT")]
public class PC_DATA_COUNT
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 计数
    /// </summary>
    public int COUNT { get; set; }

    /// <summary>
    /// 时间:yy-mm-dd hh:mm:ss
    /// </summary>
    public DateTime UPDATETIME { get; set; }

}
