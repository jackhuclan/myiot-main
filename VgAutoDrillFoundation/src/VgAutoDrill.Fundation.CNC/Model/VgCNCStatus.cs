namespace VgAutoDrill.Fundation.CNC.Model;

public class VgCNCStatus
{
    /// <summary>
    /// 开始打板使用的时间
    /// </summary>
    public int RunTime { get; set; }
    /// <summary>
    /// 开始打板后当前孔数
    /// </summary>
    public int FinHole { get; set; }

    /// <summary>
    /// 轴状态
    /// </summary>
    public string SpindleStatus { get; set; }
    /// <summary>
    /// 原始轴状态
    /// </summary>
    public string SpindleStatusOriginalData { get; set; }
    /// <summary>
    /// 机器状态
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// 当前程序
    /// </summary>

    public string ProgramName { get; set; }

    /// <summary>
    /// 完成率
    /// </summary>
    public int FinPer { get; set; }
}
