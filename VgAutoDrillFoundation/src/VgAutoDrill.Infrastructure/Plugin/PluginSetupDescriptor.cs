using System.Reflection;

namespace VgAutoDrill.Infrastructure.Plugin;

public class PluginSetupDescriptor
{
    public PluginSetupDescriptor(string pluginFilePath,
        string pluginName,
        Assembly pluginAssembly,
        Type pluginSetupType,
        PluginEntry pluginOptions)
    {
        PluginFilePath = pluginFilePath;
        PluginName = pluginName;
        PluginAssembly = pluginAssembly;
        PluginSetupType = pluginSetupType;
        PluginOptions = pluginOptions;
    }

    public string PluginFilePath { get; }
    public string PluginName { get; }
    public Assembly PluginAssembly { get; }
    public Type PluginSetupType { get; }
    public PluginEntry PluginOptions { get; }

    public IPluginSetupItem? Create()
    {
        var setupInstance = Activator.CreateInstance(PluginSetupType, this) as IPluginSetupItem;
        return setupInstance;
    }
}
