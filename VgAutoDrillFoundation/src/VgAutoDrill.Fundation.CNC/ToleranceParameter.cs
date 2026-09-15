namespace VgAutoDrill.Fundation.CNC;

public class ToleranceParameter
{
    public int LineNum { get; set; }
    public ToleranceParameter(int line)
    {
        LineNum = line;
    }
    public string Dia { get; set; } = string.Empty;
    public string NegD { get; set; } = string.Empty;
    public string PosD { get; set; } = string.Empty;
    public string ShortLength { get; set; } = string.Empty;
    public string LongLength { get; set; } = string.Empty;
    public string WarnRunout { get; set; } = string.Empty;
    public string HaltRunout { get; set; } = string.Empty;
    public string Z1DiaAdj { get; set; } = string.Empty;
    public string Z2DiaAdj { get; set; } = string.Empty;
    public string Z3DiaAdj { get; set; } = string.Empty;
    public string Z4DiaAdj { get; set; } = string.Empty;
    public string Z5DiaAdj { get; set; } = string.Empty;
    public string Z6DiaAdj { get; set; } = string.Empty;


    public string ChangeData(string data, string formart = "0.000")
    {
        if (!string.IsNullOrWhiteSpace(data))
        {
            var d = double.Parse(data);
            return d.ToString(formart);
        }
        return data;
    }


    public override string ToString()
    {
        return $"Q{LineNum},{ChangeData(NegD)},{ChangeData(PosD)},{ChangeData(ShortLength)},{ChangeData(LongLength)},{ChangeData(WarnRunout)},{ChangeData(HaltRunout)},K,{ChangeData(Z1DiaAdj)},{ChangeData(Z2DiaAdj)},{ChangeData(Z3DiaAdj)},{ChangeData(Z4DiaAdj)},{ChangeData(Z5DiaAdj)},{ChangeData(Z6DiaAdj)}";
    }
}
