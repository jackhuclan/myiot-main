namespace VgAutoDrill.Fundation.CNC;

public class PeckParameter
{
    public PeckParameter(int segmentNum)
    {
        SegmentNum = segmentNum;
    }
    public int SegmentNum { get; set; }
    public string LoweringPlane { get; set; } = string.Empty;
    public string RetrationPlane { get; set; } = string.Empty;
    public string InfeedRate { get; set; } = string.Empty;
    public string RetractdRate { get; set; } = string.Empty;



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
        return $"S{SegmentNum}I{ChangeData(LoweringPlane)}*J{ChangeData(RetrationPlane)}F{ChangeData(InfeedRate)}R{ChangeData(RetractdRate)}";
    }
}
