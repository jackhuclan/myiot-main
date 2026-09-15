using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgMySqlStore.Plugin.Models;

[Table("EAP_SIGNAL_STATUS")]
public class EAP_SIGNAL_STATUS
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 计数
    /// </summary>
    public string SIGNALCODE { get; set; }
    public string SIGNALDES { get; set; }
    public string SIGNALVALUE { get; set; }
    /// <summary>
    /// 时间:yy-mm-dd hh:mm:ss
    /// </summary>
    public DateTime CREATETIME { get; set; }

}
