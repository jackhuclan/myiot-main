namespace VgAutoDrill.Fundation.CNC.Model;

public class VgCNCTools
{
    /// <summary>
    /// 当前刀号
    /// </summary>
    public string ToolNumber { get; set; }
    /// <summary>
    /// 当前刀直径
    /// </summary>
    public float ToolDiameter { get; set; }
    /// <summary>
    /// 预设寿命
    /// </summary>
    public float PresetToolLife { get; set; }
    /// <summary>
    /// 实际寿命
    /// </summary>

    public float RemainderToolLife { get; set; }

    /// <summary>
    /// 刀具编号T的配备刀具总数
    /// </summary>

    public int ToolCount { get; set; }
    /// <summary>
    /// 刀具编号T的已使用刀具总数
    /// </summary>

    public int ExecutedToolNum { get; set; }
    /// <summary>
    /// 当次循环仍需刀具编号T刀具数
    /// </summary>
    public int RequiredToolNum { get; set; }
    /// <summary>
    /// 刀具编号T的配备刀具总寿命
    /// </summary>
    public float ToolLifeCount { get; set; }
    /// <summary>
    /// 刀具编号T的配备刀具已使用寿命
    /// </summary>
    public float UsedLifeCount { get; set; }
    /// <summary>
    /// 当次循环仍需刀具编号T的寿命数
    /// </summary>
    public float RemainderLifeCount { get; set; }

    /// <summary>
    /// 刀具类型 （TTYP指令才能开启）
    /// </summary>
    public int ToolType { get; set; }

}
