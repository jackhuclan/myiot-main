namespace VgAutoDrill.Fundation.CNC;

public class BParameter
{
    public int LineNum { get; set; }
    public BParameter(int line)
    {
        LineNum = line;
    }
    public string Use { get; set; } = string.Empty;
    public string MinD { get; set; } = string.Empty;
    public string MaxD { get; set; } = string.Empty;
    public string Roughn { get; set; } = "0.013";
    public string InfeedFirst { get; set; } = "100.0";
    public string InfeedFast { get; set; } = "100.0";
    public string InfeedSlow { get; set; } = "100.0";
    public string ShiftOrthog { get; set; } = string.Empty;
    public string ShiftLong { get; set; } = string.Empty;

    public string ChangeData(string data, string formart = "0.000")
    {
        if (!string.IsNullOrWhiteSpace(data))
        {
            var d = double.Parse(data);
            return d.ToString(formart);
        }
        return data;
    }

    public string GetInchData(string data, string formart = "0.00000")
    {
        if (!string.IsNullOrWhiteSpace(data))
        {
            var d = Math.Round(double.Parse(data) * 0.0393701, 5);
            return d.ToString(formart);
        }
        return data;
    }
    public override string ToString()
    {
        return $"B{LineNum},{Use},{ChangeData(MinD)},{ChangeData(MaxD)},{ChangeData(ShiftOrthog)},{ChangeData(Roughn)},{ChangeData(InfeedFirst)},{ChangeData(InfeedFast)},{ChangeData(InfeedSlow)},{ChangeData(ShiftLong)},{GetInchData(Roughn)},{GetInchData(ShiftOrthog)},{GetInchData(ShiftLong)}";
    }
}
