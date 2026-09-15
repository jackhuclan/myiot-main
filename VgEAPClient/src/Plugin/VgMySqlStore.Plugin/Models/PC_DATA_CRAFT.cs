using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgMySqlStore.Plugin.Models;

[Table("PC_DATA_CRAFT")]
public class PC_DATA_CRAFT
{
    [Key]

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    /// <summary>
    /// 批次号
    /// </summary>
    public string? CONTAINER { get; set; }    //批次号
    /// <summary>
    /// 板ID
    /// </summary>
    public string PNLIDORSETID { get; set; }
    /// <summary>
    /// 采集项目代码
    /// </summary>
    public string ITEMCODE { get; set; }
    /// <summary>
    /// 采集项目描述
    /// </summary>
    public string ITEMDES { get; set; }
    /// <summary>
    /// 单位
    /// </summary>
    public string? UOM { get; set; }
    /// <summary>
    /// 采集值
    /// </summary>
    public string ITEMVALUE { get; set; }
    /// <summary>
    /// 合格标志
    /// </summary>
    public string ISOK { get; set; }
    /// <summary>
    /// 组号：设备对每一次数据采集数据产生一个唯一组号
    /// </summary>
    public decimal GROUPNUM { get; set; }
    /// <summary>
    /// 时间:yy-mm-dd hh:mm:ss
    /// </summary>
    public DateTime CREATETIME { get; set; }

}
