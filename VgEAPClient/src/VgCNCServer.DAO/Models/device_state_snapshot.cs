using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;


[Table("device_state_snapshot")]
public class device_state_snapshot
{
    [Key]
    public Guid id { get; set; } = Guid.Empty;

    public string device_name { get; set; } = "";

    public DateTime create_time { get; set; } = DateTime.Now;

    public string work_order_id { get; set; } = "";



    public string dia_file { get; set; } = "";
    public string atp_file { get; set; } = "";
    public string program_file { get; set; } = "";

    public string pallet_code { get; set; } = "";//出自用户需求文档. 尚未使用

    public DateTime start_run_time { get; set; } = DateTime.MinValue;

    public DateTime end_run_time { get; set; } = DateTime.MinValue;

    public string cnc_status { get; set; } = "";
    public string case_code { get; set; } = "";
    public string case_msg { get; set; } = "";

    public string tool_type { get; set; } = "";
    public double tool_dia { get; set; } = 0;
    public int tool_pod_num { get; set; } = 0;


    public int enable_spindle_count { get; set; } = 0;
    public string enable_spindle_mask { get; set; } = "";
    public long hole_count { get; set; } = 0;
    public double rou_meters { get; set; } = 0;

    public double progress_pcnt { get; set; } = 0;

    //注意 ：这里只记录来自电表的读数。但是 电表有时会被重置 ！统计的时候要包容。
    public double ammeter_used_kwh { get; set; } = 0;

}
