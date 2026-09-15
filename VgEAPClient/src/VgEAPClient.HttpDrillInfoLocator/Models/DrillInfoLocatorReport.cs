namespace VgEAPClient.HttpDrillInfoLocator.Models;

public class DrillInfoLocatorReport
{
    public string wipCode { get; set; } = string.Empty;
    public string workMac { get; set; } = string.Empty;

    //public override string ToString()
    //{
    //    return JsonSerializer.Serialize(this, new JsonSerializerOptions
    //    {
    //        WriteIndented = true,
    //        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    //    });
    //}
}
