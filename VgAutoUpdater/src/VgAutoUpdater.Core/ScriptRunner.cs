using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace VgAutoUpdater.Core;

public class ScriptRunner
{
    private readonly ILogger<ScriptRunner> _logger;
    public ScriptRunner(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ScriptRunner>();
    }

    public async Task<bool> RunCommand(string cmdText, string currentPath, Dictionary<string, object> extraParams, CancellationToken cancellationToken = default)
    {
        try
        {
            cmdText = PreProcess(cmdText.Trim(), extraParams);
            var processInfo = new ProcessStartInfo();
            processInfo.UseShellExecute = true;
            processInfo.WorkingDirectory = currentPath;
            processInfo.FileName = @"C:\Windows\System32\cmd.exe";
            processInfo.Verb = "runas";
            processInfo.Arguments = "/c " + cmdText;
            processInfo.WindowStyle = ProcessWindowStyle.Hidden;
            var process = Process.Start(processInfo);
            if (process == null)
            {
                _logger.LogError("cmd.exe can't be found!");
                return false;
            }

            await process.WaitForExitAsync(cancellationToken);
            if (!process.HasExited)
            {
                process.Kill();
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return false;
    }

    /// <summary>
    /// 对command text进行预处理
    /// </summary>
    /// <param name="cmdText"></param>
    /// <param name="extraParams"></param>
    /// <returns></returns>
    private string PreProcess(string cmdText, Dictionary<string, object> extraParams)
    {
        if (!string.IsNullOrEmpty(cmdText) && extraParams.Count > 0)
        {
            foreach (var kv in extraParams)
            {
                cmdText = cmdText.Replace(string.Format("{{{0}}}", kv.Key), kv.Value.ToString());
            }
        }

        return cmdText;
    }
}
