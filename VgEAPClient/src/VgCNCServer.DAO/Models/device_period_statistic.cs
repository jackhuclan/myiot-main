using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;


[Table("device_period_statistic")]
public class device_period_statistic
{
    [Key]
    public Guid id { get; set; } = Guid.Empty;
    public string device_name { get; set; } = "";
    public DateTime create_time { get; set; } = DateTime.Now;
    public DateTime modify_time { get; set; } = DateTime.Now;

    /// <summary>
    /// 被统计的时间区间类型，视具体项目而定义，只要和 period_begin ，period_end 吻合即可。例如：早班，中班，晚班，每天（当前早班+对应的中班和晚班）。
    /// </summary>
    public string period_type { get; set; } = "";
    /// <summary>
    /// 描述
    /// </summary>
    public string period_desc { get; set; } = "";
    /// <summary>
    /// 被统计的时间区间的开始
    /// </summary>
    public DateTime period_begin { get; set; } = DateTime.MinValue;
    /// <summary>
    /// 被统计的时间区间的结束
    /// </summary>
    public DateTime period_end { get; set; } = DateTime.MinValue;

    /// <summary>
    /// 时段内消耗的电量
    /// </summary>
    public double used_kwh { get; set; } = 0;

    //注意 ：这里只记录来自电表的读数。但是 电表有时会被重置 ！统计的时候要包容。
    public double ammeter_used_kwh { get; set; } = 0;

    /// <summary>
    /// 稼动率（利用率）；
    /// </summary>
    public double utilization { get; set; } = 0;


    public long alarm_count { get; set; } = 0;
    /// <summary>
    /// 断刀次数
    /// </summary>
    public long broke_tool_count { get; set; } = 0;
    /// <summary>
    /// （钻机）钻孔数
    /// </summary>
    public long hole_count { get; set; } = 0;
    /// <summary>
    /// 锣机成型米数
    /// </summary>
    public double rou_meters { get; set; } = 0;



    /// <summary>
    /// 正在运行program,正常工作中
    /// </summary>
    public double work_dur_min { get; set; } = 0;
    /// <summary>
    /// 正在运行program,异常停止的时长；
    /// </summary>
    public double stop_dur_min { get; set; } = 0;
    /// <summary>
    /// 当前没有运行program的时长，单位min;
    /// </summary>
    public double wait_dur_min { get; set; } = 0;

    //下面几个值只是把时长换一种格式显示为"hh:mm:ss"
    public string work_dur { get; set; } = "";
    public string stop_dur { get; set; } = "";
    public string wait_dur { get; set; } = "";


    //轴的加工时长（分钟）
    public double s1_work_dur_min { get; set; } = 0;
    public double s2_work_dur_min { get; set; } = 0;
    public double s3_work_dur_min { get; set; } = 0;
    public double s4_work_dur_min { get; set; } = 0;
    public double s5_work_dur_min { get; set; } = 0;
    public double s6_work_dur_min { get; set; } = 0;

    public double s7_work_dur_min { get; set; } = 0;
    public double s8_work_dur_min { get; set; } = 0;
    public double s9_work_dur_min { get; set; } = 0;
    public double s10_work_dur_min { get; set; } = 0;
    public double s11_work_dur_min { get; set; } = 0;
    public double s12_work_dur_min { get; set; } = 0;



    //hh:mm:ss格式的时长
    public string s1_work_dur { get; set; } = "";
    public string s2_work_dur { get; set; } = "";
    public string s3_work_dur { get; set; } = "";
    public string s4_work_dur { get; set; } = "";
    public string s5_work_dur { get; set; } = "";
    public string s6_work_dur { get; set; } = "";

    public string s7_work_dur { get; set; } = "";
    public string s8_work_dur { get; set; } = "";
    public string s9_work_dur { get; set; } = "";
    public string s10_work_dur { get; set; } = "";
    public string s11_work_dur { get; set; } = "";
    public string s12_work_dur { get; set; } = "";

    public string GetKeyBrief()
    {
        return $"'{period_type}'.'{period_begin}'~'{period_end}'.";
    }
}

