namespace VgEAPClient.Recipe.Bomin;

internal class GetDrillRecipesManualResponse
{
    public string checkType { get; set; } = string.Empty;
    public string checkCode { get; set; } = string.Empty;
    public string severName { get; set; } = string.Empty;
    public BominRecipeData data { get; set; } = new();
}

public class BominRecipeData
{
    public string Lot { get; set; } = string.Empty;
    public string ProcessCode { get; set; } = string.Empty;
    public string MachineCode { get; set; } = string.Empty;

    public List<string> dia { get; set; } = new List<string>();
    public List<string> drl { get; set; } = new List<string>();
    public List<string> pin { get; set; } = new List<string>();
    public string FtpBasicUrl { get; set; } = string.Empty;
    public string FtpAccount { get; set; } = string.Empty;
    public string FtpPassword { get; set; } = string.Empty;
    public bool Success { get; set; } = false;
    public string Content { get; set; } = string.Empty;
}
