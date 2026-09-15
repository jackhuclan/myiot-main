namespace VgEAPClient.HttpDrillInfoLocator.Models;

public class DrillInfoLocatorReply
{
    public int code { get; set; } = 0;

    public string msg { get; set; } = string.Empty;

    public Data data { get; set; } = new Data();
}

public class Data
{
    public List<string> dia { get; set; } = new List<string>();
    public List<string> drl { get; set; } = new List<string>();
}
