namespace AppsettingsUpdater;

internal interface IConfigurationFileReplacer
{
    string Replace(string json, Dictionary<string, string?> data);
}
