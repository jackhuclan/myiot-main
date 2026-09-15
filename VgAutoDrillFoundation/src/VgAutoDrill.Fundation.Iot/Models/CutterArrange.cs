namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 钻机刀盒换刀信息
/// </summary>
public class CutterArrange
{
    public string WipId { get; set; } = string.Empty;
    public List<CutterLifeDefine> LifeDefines { get; set; } = new List<CutterLifeDefine> { };
    public List<CutterArrangeDefine> Arranges { get; set; } = new List<CutterArrangeDefine> { };
    public List<BoxAlterDefine> Boxes { get; set; } = new List<BoxAlterDefine> { };
}

public class CutterLifeDefine
{
    public double PgmDiameter { get; set; }
    public int MoCount { get; set; }
    public int Life { get; set; }
}

public class CutterArrangeDefine
{
    public int BoxIndex { get; set; }
    public int Site { get; set; }
    public double PgmDiameter { get; set; }
    public int MoCount { get; set; }
    public int Life { get; set; }
}
public class BoxAlterDefine
{
    public int BoxIndex { get; set; }
    public string SpindleBoxCode_1 { get; set; } = string.Empty;
    public string SpindleBoxCode_2 { get; set; } = string.Empty;
    public string SpindleBoxCode_3 { get; set; } = string.Empty;
    public string SpindleBoxCode_4 { get; set; } = string.Empty;
    public string SpindleBoxCode_5 { get; set; } = string.Empty;
    public string SpindleBoxCode_6 { get; set; } = string.Empty;
}
/// <summary>
/// 刀具需求
/// 每个直径，需要钻孔的数量
/// </summary>
public class CutterRequirement
{
    public double PgmDiameter { get; set; }
    public int NeedDrillCount { get; set; }
}
