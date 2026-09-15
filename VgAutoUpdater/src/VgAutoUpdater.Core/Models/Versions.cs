namespace VgAutoUpdater.Core.Models;

public class Versions
{
    public UpdateMode Mode { get; set; }
    public bool Enable { get; set; } = true;
    public List<App> AppList { get; set; } = new List<App>();
    public List<string> CommonScripts { get; set; } = new();
}
