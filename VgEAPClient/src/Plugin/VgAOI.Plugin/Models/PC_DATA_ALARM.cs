using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgAOI.Plugin.Models;

[Table("PC_DATA_ALARM")]
public class PC_DATA_ALARM
{
    [Key]

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 报警代码
    /// </summary>
    public string ALARMCODE { get; set; }
    /// <summary>
    /// 报警描述
    /// </summary>
    public string ALARMDES { get; set; }
    /// <summary>
    /// 报警值(0/1)：0复位，1报警
    /// </summary>
    public string ALARMVALUE { get; set; }
    /// <summary>
    /// 时间:yy-mm-dd hh:mm:ss
    /// </summary>
    public DateTime CREATETIME { get; set; }
}
