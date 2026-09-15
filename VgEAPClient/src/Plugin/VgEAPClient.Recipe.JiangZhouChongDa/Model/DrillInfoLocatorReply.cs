namespace VgEAPClient.Recipe.JiangZhouChongDa.Model;

public class DrillInfoLocatorReply
{
    public string Code { get; set; } = "0";

    public string Msg { get; set; } = string.Empty;

    public HttpResultData Data { get; set; } = new HttpResultData();
}

public class HttpResultData
{
    public List<string> dia { get; set; } = new List<string>();
    public List<string> drl { get; set; } = new List<string>();
}
