// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace Vegalot.External.XianjinIot.Agv;

internal class ReportInfo
{
    private readonly ILogger<ReportInfo> _logger;

    public ReportInfo(ILogger<ReportInfo> logger)
    {
        _logger = logger;
    }

    public void CodeReaderReceive(int spindleNum, string codeReaderContent)
    {
        try
        {
            _logger.LogWarning($"{DateTime.Now.ToShortTimeString()} ReportInfo  读码器获取的是{spindleNum}轴  内容{codeReaderContent}");
        }
        catch (Exception)
        {
        }
        finally
        {
            _logger.LogWarning($"{DateTime.Now.ToShortTimeString()} ReportInfo  读码器 结束 ");
        }
    }
}
