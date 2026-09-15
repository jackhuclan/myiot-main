namespace VgAutoDrill.Fundation.CNC;

public class TParameter
{
    public TParameter(int num)
    {
        ToolNum = num + "";
    }
    public string ToolNum { get; set; } = string.Empty;
    public string ToolDia { get; set; } = string.Empty;
    public string ToolSpeed { get; set; } = string.Empty;
    public string ToolFreed { get; set; } = string.Empty;
    public string ToolRetract { get; set; } = string.Empty;
    public string ToolWaitingtime { get; set; } = string.Empty;
    public string ToolZAddend { get; set; } = string.Empty;
    public string ToolLife { get; set; } = string.Empty;
    public string ToolCurrentLife { get; set; } = string.Empty;
    public List<int> Position { get; set; } = new List<int>();
    public List<ToolLifeInfo> ToolMagazines { get; set; } = new List<ToolLifeInfo>();


    public override string ToString()
    {
        return $"T{ToolNum}D{ToolDia}ES{ToolSpeed}F{ToolFreed}R{ToolRetract}A{ToolWaitingtime}Z{ToolZAddend}N{ToolLife}B{ToolCurrentLife}CPUVWXG\"-v+d+l+r-s-c+q+o-t-k-p\"\r\nT{ToolNum}Q,,t1M,,,Y,,,,,,,\r\nM{string.Join("M", Position)}";
    }

}
