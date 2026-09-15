namespace VgAutoDrill.Fundation.Drill;

public class DrillFilePathOptions
{
    public DrillFilePathKind StoreType { get; set; } = DrillFilePathKind.Local;
    public string StorePath { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public int ReadTimeoutSeconds { get; set; } = 5;
    public string PrefixDiaPath { get; set; } = "";
    public string DiaFileExtension { get; set; } = ".dia";

}
