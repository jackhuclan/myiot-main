using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;


[Table("device_maintenance")]
public class device_maintenance
{

    [Key]
    public Guid id { get; set; } = Guid.Empty;
    public string device_name { get; set; } = "";
    public DateTime create_time { get; set; } = DateTime.Now;

    //执行人
    public string executor { get; set; } = "";

    //保养项目名称（由用户在配置文件中自己不同保养项目的名称）
    public string job_item_name { get; set; } = "";

    //保养摘要描述
    public string job_brief { get; set; } = "";

    //备注
    public string note { get; set; } = "";

    //本行的作废标记 : 1表示已经作废（但是不会删除行）； 0表示有效;
    public int is_deleted { get; set; } = 0;

    //本行的作废时间
    public DateTime delete_time { get; set; } = DateTime.MinValue;
}
