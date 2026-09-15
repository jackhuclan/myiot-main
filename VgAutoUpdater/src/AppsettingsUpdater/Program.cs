using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;

namespace AppsettingsUpdater;

/// <summary>
/// this program update arbitrary json file like
/// appsettings.json,appsettings.Development.json,appsettings.Production.json
/// </summary>
internal class Program
{
    /// <summary>
    /// .\\AppsettingsUpdater.exe --file=.app\\appsettings.Development.json --section=MqttServerOptions:Host --value=192.168.102.115
    /// </summary>
    /// <param name="args"></param>
    internal static async Task Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddSingleton<IDownloader, Downloader>()
            .AddSingleton<IConfigurationFileReplacer, JsonConfigurationFileReplacer>()
            .AddHttpClient()
            .AddLogging(log => { log.AddNLog(); })
            .BuildServiceProvider();

        var conf = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

        var file = conf["file"] ?? string.Empty;
        var section = conf["section"] ?? string.Empty;
        var value = conf["value"] ?? string.Empty;
        var source = conf["source"] ?? string.Empty;
        var values = conf["values"] ?? string.Empty;

        var downloader = services.GetRequiredService<IDownloader>();
        var replacer = services.GetRequiredService<IConfigurationFileReplacer>();

        //case 1: .\\AppsettingsUpdater.exe --file=.\\app\\appsettings.Development.json --section=MqttServerOptions:Host --value=192.168.102.115
        if (!string.IsNullOrWhiteSpace(file)
        && !string.IsNullOrWhiteSpace(section)
        && !string.IsNullOrWhiteSpace(value)
        && string.IsNullOrEmpty(source)
        && string.IsNullOrEmpty(values))
        {
            var data = new Dictionary<string, string?>();
            data.Add(section, value);

            var replacedJson = replacer.Replace(File.ReadAllText(file), data);
            File.WriteAllText(file, replacedJson);
        }

        //case 2: .\\AppsettingsUpdater.exe --file=.\\app\\appsettings.Development.json --source=http://192.168.102.253:5258/conf/agv/appsettings.Development.json
        if (!string.IsNullOrWhiteSpace(file)
            && !string.IsNullOrWhiteSpace(source)
            && string.IsNullOrEmpty(value)
            && string.IsNullOrEmpty(section)
            && string.IsNullOrEmpty(values))
        {
            File.Delete(file);
            await downloader.DownloadFile(source, file);
        }

        //case 3: .\\AppsettingsUpdater.exe --file=.\\app\\appsettings.Development.json --source=http://192.168.102.253:5258/conf/agv/appsettings.Development.json --values=http://192.168.102.253:5258/conf/drill/values.json  --placeholder1=value1  --placeholder2=value2
        if (!string.IsNullOrWhiteSpace(file)
            && !string.IsNullOrWhiteSpace(source)
            && !string.IsNullOrWhiteSpace(values)
            && string.IsNullOrEmpty(value)
            && string.IsNullOrEmpty(section))
        {
            File.Delete(file);

            var valuesFilePath = Path.Combine(Path.GetDirectoryName(file)!, Path.GetFileName(values));
            await downloader.DownloadFile(source, file);
            await downloader.DownloadFile(values, valuesFilePath);

            using var valueStream = new FileStream(valuesFilePath, FileMode.Open);

            var placeholderData = ExtractPlaceholders(args, conf);
            var valuesData = JsonConfigurationFileReader.ParseToDictionary(valueStream);
            foreach (var item in valuesData)
            {
                if (placeholderData.ContainsKey(item.Key))
                    valuesData[item.Key] = placeholderData[item.Key];
            }

            var replacedJson = replacer.Replace(File.ReadAllText(file), valuesData);
            File.WriteAllText(file, replacedJson);
        }

        //case 4: .\\AppsettingsUpdater.exe --file=.\\app\\appsettings.Development.json -source=http://192.168.102.253:5258/conf/agv/appsettings.Development.json --placeholder1=value1  --placeholder2=value2
        if (!string.IsNullOrWhiteSpace(file)
            && !string.IsNullOrWhiteSpace(source)
            && string.IsNullOrEmpty(value)
            && string.IsNullOrEmpty(section)
            && string.IsNullOrEmpty(values)
            && args.Length > 2)
        {
            File.Delete(file);

            await downloader.DownloadFile(source, file);

            var data = ExtractPlaceholders(args, conf);
            var replacedJson = replacer.Replace(File.ReadAllText(file), data);
            File.WriteAllText(file, replacedJson);
        }
    }

    private static Dictionary<string, string?> ExtractPlaceholders(string[] args, IConfigurationRoot conf)
    {
        var data = new Dictionary<string, string?>();
        var argNames = args.Select(x => x.Split("=")[0].Replace("--", "").Trim()).ToList();
        var excludeArgs = new List<string>() { "file", "section", "value", "values", "source" };
        foreach (var item in argNames.Where(x => !excludeArgs.Contains(x)))
        {
            data.Add(item, conf[item]);
        }

        return data;
    }
}
