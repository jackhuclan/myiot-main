using VgAutoDrill.Fundation.CNC;

namespace VgAutoDrill.Fundation.Utils.CNC;

public class PeckInf
{
    public int ToolNum { get; set; }
    List<PeckParameter> peckParameters = new List<PeckParameter>();

    public PeckInf(int toolNum)
    {
        ToolNum = toolNum;
        peckParameters = Enumerable.Range(0, 10).Select(i => new PeckParameter(toolNum)).ToList();
    }

    public bool AddPeckParameter(PeckParameter peckParameter)
    {
        if (peckParameter == null) return false;
        if (peckParameter.SegmentNum <= 0 || peckParameter.SegmentNum > 10) return false;
        peckParameters.Add(peckParameter);
        return true;

    }

    public string PeckParameterFullString(PeckParameter peckParameter)
    {
        return $"P{ToolNum}{peckParameter.ToString()}";
    }

    IEnumerable<int> GetConsecutiveNumbers(int[] numbers)
    {
        int n = 10;
        return Enumerable.Range(1, n)
            .Select((num, index) => new { Number = num, Index = index })
            .Join(numbers, ideal => ideal.Number, real => real, (ideal, real) => ideal)
            .GroupBy(n => n.Number - n.Index)
            .Where(g => g.Count() > 1)
            .SelectMany(g => g.Select(n => n.Number));
    }

    public override string ToString()
    {
        if (peckParameters.Count == 0)
        {
            return $"P{ToolNum}S1I*JFR";
        }

        var consecutiveNumbers = GetConsecutiveNumbers(peckParameters.Select(x => x.SegmentNum).ToArray());

        if (consecutiveNumbers == null || !consecutiveNumbers.Any())
        {
            return $"P{ToolNum}S1I*JFR";
        }
        List<string> pInf = new List<string>();
        for (int i = 0; i < consecutiveNumbers.Count(); i++)
        {
            pInf.Add(PeckParameterFullString(peckParameters.First(x => x.SegmentNum == (i + 1))));
        }
        return string.Join("\r\n", pInf);
    }
}
