namespace VgAutoDrill.Infrastructure.Plugin;

public class PluginEntry
{
    public bool Enabled { get; set; } = false;
    /// <summary>
    /// 插件所在目录
    /// </summary>
    public string PluginDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
    /// <summary>
    /// 插件所在程序集文件名
    /// </summary>
    public string AssemblyFileName { get; set; } = "*.dll";
    /// <summary>
    /// 插件名称
    /// </summary>
    public string PluginName { get; set; } = string.Empty;
}
