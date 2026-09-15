namespace VgAOI.Plugin;

public class VgAOIOptions
{
    public bool Enabled { get; set; } = false;
    public string Server { get;  set; } = string.Empty;
    public uint Port { get;  set; } = 0;
    public string UserID { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public int ReadTimeoutSeconds { get; set; } = 5;
    public string Database { get;  set; } = string.Empty;
}
