namespace VgAutoDrill.Fundation.CNC;

public class LParameter
{
    public LParameter(int num)
    {
        LineNum = num + "";
    }
    public string LineNum { get; set; } = string.Empty;
    public string[] ToolNum { get; set; } = new string[10];
    public override string ToString()
    {
        return $"L{LineNum},10,{string.Join(",", ToolNum)}";
    }
}

public class WParameter
{
    public WParameter(int num)
    {
        LineNum = num + "";
    }
    public string LineNum { get; set; } = string.Empty;
    public string[] ToolNum { get; set; } = new string[10];
    public override string ToString()
    {
        return $"W{LineNum},10,{string.Join(",", ToolNum)}";
    }
}


public class HParameter
{
    public HParameter(int num)
    {
        LineNum = num + "";
    }
    public string LineNum { get; set; } = string.Empty;
    public string[] ToolNum { get; set; } = new string[10];
    public override string ToString()
    {
        return $"H{LineNum},10,{string.Join(",", ToolNum)}";
    }
}
public class LParameterMetada
{
    public LParameterMetada(int index, string toolNum)
    {
        Index = index;
        ToolNum = toolNum;
    }
    public int Index { get; set; }
    public string ToolNum { get; set; } = string.Empty;

}
