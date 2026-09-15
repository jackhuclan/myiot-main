using System.Reflection;

namespace VgAutoDrill.Infrastructure.Plugin;

internal class PluginSetupSelector
{
    private static HashSet<PluginSetupDescriptor> _pluginSetupDescriptors = new();

    public static List<IPluginSetupItem> LoadFrom(PluginSetupOptions pluginOptions)
    {
        PopulatePluginSetupItems(pluginOptions);
        var pluginSetupItems = new List<IPluginSetupItem>();

        foreach (var setupItemDescriptor in _pluginSetupDescriptors)
        {
            var pluginSetupItem = setupItemDescriptor.Create();
            if (pluginSetupItem != null)
            {
                pluginSetupItems.Add(pluginSetupItem);
            }
        }

        return pluginSetupItems;
    }

    private static void PopulatePluginSetupItems(PluginSetupOptions pluginOptions)
    {
        if (!pluginOptions.Any()) return;
        _pluginSetupDescriptors.Clear();

        var disabledPluginNames = pluginOptions.Where(x => !x.Enabled)
            .Select(x => x.PluginName)
            .Distinct()
            .ToList();

        foreach (var assemblyEntry in pluginOptions.Where(x => x.Enabled))
        {
            var absolutePath = assemblyEntry.PluginDirectory;
            if (!Path.IsPathRooted(absolutePath))
                absolutePath = Path.Combine(absolutePath, AppDomain.CurrentDomain.BaseDirectory);

            var pluginPath = Path.Combine(absolutePath, assemblyEntry.AssemblyFileName);

            if (!File.Exists(pluginPath)) continue;
            var assembly = Assembly.LoadFrom(Path.GetFullPath(pluginPath));

            Predicate<Type> isPluginSetupItem = t => t.IsAssignableTo(typeof(IPluginSetupItem)) && !t.IsAbstract;

            var pluginSetupItemTypeList = assembly.GetTypes()
                .Where(t => isPluginSetupItem(t) && assemblyEntry.PluginName == t.Name && !disabledPluginNames.Contains(t.Name))
                .ToList();

            foreach (var pluginSetupItemType in pluginSetupItemTypeList)
            {
                _pluginSetupDescriptors.Add(new PluginSetupDescriptor(
                    pluginPath,
                    assemblyEntry.PluginName,
                    assembly,
                    pluginSetupItemType,
                    assemblyEntry
                    ));
            }
        }
    }
}
