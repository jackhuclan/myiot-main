using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;
[Table("work_order")]
public class work_order
{
    [Key]
    //public string id { get; set; } = "";
    public Guid id { get; set; } = Guid.Empty;

    /// <summary>
    /// 设备名
    /// </summary>
    public string device_name { get; set; } = "";


    public DateTime create_time { get; set; } = DateTime.MinValue;
    public DateTime modify_time { get; set; } = DateTime.MinValue;
    public DateTime start_run_time { get; set; } = DateTime.MinValue;
    public DateTime end_run_time { get; set; } = DateTime.MinValue;


    public string end_run_note { get; set; } = "";

    public int ended { get; set; } = 0;


    /// <summary>
    /// 操作人员工号
    /// </summary>
    public string operator_code { get; set; } = "";
    /// <summary>
    /// 操作人员描述
    /// </summary>
    public string operator_desc { get; set; } = "";

    /// <summary>
    /// 订单代号
    /// </summary>
    public string order_code { get; set; } = "";
    /// <summary>
    /// 订单描述
    /// </summary>
    public string order_desc { get; set; } = "";

    /// <summary>
    /// 批次代码
    /// </summary>
    public string batch_code { get; set; } = "";


    /// <summary>
    /// 托盘编号
    /// </summary>
    public string pallet_code { get; set; } = "";
    /// <summary>
    /// 生料名
    /// </summary>
    public string input_item_name { get; set; } = "";
    /// <summary>
    /// 生料类型代码
    /// </summary>
    public string input_item_type_code { get; set; } = "";
    /// <summary>
    /// 生料备注
    /// </summary>
    public string input_item_note { get; set; } = "";


    public string dia_file { get; set; } = "";
    public string atp_file { get; set; } = "";
    public string program_file { get; set; } = "";


    public int is_deleted { get; set; } = 0;
    public DateTime delete_time { get; set; } = DateTime.MinValue;


    /// <summary>
    /// 启用主轴的数量
    /// </summary>
    public int enable_spindle_count { get; set; } = 0;
    /// <summary>
    /// EG :  111100
    /// </summary>
    public string enable_spindle_mask { get; set; } = "";

    /// <summary>
    /// 最后的加工进度值
    /// </summary>
    public double progress_pcnt { get; set; } = 0;

    public long alarm_count { get; set; } = 0;

    public long broke_tool_count { get; set; } = 0;

    public long hole_count { get; set; } = 0;
    public double rou_meters { get; set; } = 0;


    public double work_dur_min { get; set; } = 0;
    public double stop_dur_min { get; set; } = 0;
    public double wait_dur_min { get; set; } = 0;


    //下面三个字段只是以"hh:mm:ss"的格式显示上面的三个时长
    public string work_dur { get; set; } = "00:00:00";
    public string stop_dur { get; set; } = "00:00:00";
    public string wait_dur { get; set; } = "00:00:00";

}
