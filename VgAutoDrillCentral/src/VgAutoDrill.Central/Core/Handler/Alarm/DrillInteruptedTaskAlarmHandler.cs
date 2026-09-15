using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure.Clickhouse;

namespace VgAutoDrill.Central.Core.Handler.Alarm;

/// <summary>
/// 钻机中断任务告警处理
/// </summary>
internal class DrillInteruptedTaskAlarmHandler : AbstractAlarmHandler
{
    private readonly IDeviceManager _deviceHolder;
    private readonly ILogger<DrillInteruptedTaskAlarmHandler> _logger;

    public DrillInteruptedTaskAlarmHandler(IDeviceManager deviceHolder,
        IClickHouseConnector clickHouseConnector,
        ILoggerFactory loggerFactory) : base(clickHouseConnector, loggerFactory)
    {
        _deviceHolder = deviceHolder;
        _logger = loggerFactory.CreateLogger<DrillInteruptedTaskAlarmHandler>();

    }

    public override Task HandleDeviceAlarmReportRequest(DeviceAlarmReportRequest handledRequest) => base.HandleDeviceAlarmReportRequest(handledRequest);
}
