using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

/// <summary>
/// 本表主要用于记录各种‘余额’;
/// </summary>
[Table("device_odometer_history")]
public class device_odometer_history
{
    [Key]
    public Guid id { get; set; } = Guid.Empty;
    public string device_name { get; set; } = "";
    public DateTime create_time { get; set; } = DateTime.Now;

    public DateTime modify_time { get; set; } = DateTime.Now;


    //机台的累计耗电量
    public double used_kwh_total { get; set; } = 0;
    //机台的累计钻孔数
    public long hole_count_total { get; set; } = 0;
    //机台的累计成型米数
    public double rou_meters_total { get; set; } = 0;
    //机台的累计加工时长（分钟）
    public double work_dur_min_total { get; set; } = 0;

    //主轴累计工作时长（分钟）
    public double s1_work_dur_min_total { get; set; } = 0;
    public double s2_work_dur_min_total { get; set; } = 0;
    public double s3_work_dur_min_total { get; set; } = 0;
    public double s4_work_dur_min_total { get; set; } = 0;
    public double s5_work_dur_min_total { get; set; } = 0;
    public double s6_work_dur_min_total { get; set; } = 0;


    public double s7_work_dur_min_total { get; set; } = 0;
    public double s8_work_dur_min_total { get; set; } = 0;
    public double s9_work_dur_min_total { get; set; } = 0;
    public double s10_work_dur_min_total { get; set; } = 0;
    public double s11_work_dur_min_total { get; set; } = 0;
    public double s12_work_dur_min_total { get; set; } = 0;
}
