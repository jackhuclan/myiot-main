using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.Hik;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;
using QueryTaskStatusResponse = VegaIot.External.AgvEntity.Hik.QueryTaskStatusResponse;

namespace VegaIot.External.Hik;

internal class HikAgvTaskMonitor : BackgroundService
{
    private readonly ILogger<HikAgvTaskMonitor> _logger;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private readonly HikTransfer? _device;
    private readonly string _queryTaskStatusUrl = string.Empty;
    private readonly CentralReporter _transferCommon;
    private volatile bool _isBusy = false;

    public HikAgvTaskMonitor(ILoggerFactory loggerFactory,
        IHttpRequestInvoker httpRequestInvoker,
        IDeviceProvider deviceProvider,
        IObjectFactory objectFactory)
    {
        _logger = loggerFactory.CreateLogger<HikAgvTaskMonitor>();
        _httpRequestInvoker = httpRequestInvoker;
        _device = deviceProvider.Devices.FirstOrDefault() as HikTransfer;
        _queryTaskStatusUrl = _device?.DeviceDescriptor.Extra["QueryTaskStatus"].ToStr() ?? string.Empty;
        int monitorTaskStatusInterval = _device?.DeviceDescriptor.Extra["MonitorTaskStatusInterval"].ToInt() ?? 30;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(monitorTaskStatusInterval));
        _transferCommon = objectFactory.GetOrCreate<CentralReporter>(_device);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested
            && !_isBusy
            && await _timer.WaitForNextTickAsync())
        {
            try
            {
                if (_device == null)
                {
                    _logger.LogError("can't get HikTransfer from deviceProvider, please check configuration file!");
                    break;
                }

                _autoResetEvent.WaitOne();
                _isBusy = true;
                _logger.LogDebug($"begin to do HikAgvMonitor ExecuteAsync");

                //未完成的currentContext
                var currentContextOfNotCompleted = _device.Locations.Values.Where(x => !string.IsNullOrWhiteSpace(x.CurrentContext.HikTaskCode))
                    .Select(x => x.CurrentContext)
                    .ToArray();

                //未完成的taskCode
                var taskCodesOfNotCompleted = currentContextOfNotCompleted
                    .Select(x => x.HikTaskCode)
                    .ToArray();

                //查询任务状态
                if (taskCodesOfNotCompleted != null && taskCodesOfNotCompleted.Any())
                {
                    var sentTaskToHikAgvData = new QueryTaskStatusRequest
                    {
                        reqCode = this._device.DeviceId.Right(10) + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        taskCodes = taskCodesOfNotCompleted,
                        reqTime = "",
                    };

                    var taskStatusResponse = await _httpRequestInvoker.PostAsJsonAsync<QueryTaskStatusRequest, QueryTaskStatusResponse>(_queryTaskStatusUrl, sentTaskToHikAgvData);
                    if (taskStatusResponse != null
                        && taskStatusResponse.code == "0")
                    {
                        foreach (var task in taskStatusResponse.data)
                        {
                            var queryingHikTask = currentContextOfNotCompleted.FirstOrDefault(x => x.HikTaskCode == task.taskCode);
                            if (queryingHikTask == null) continue;

                            switch (task.taskStatus.ToInt())
                            {
                                case (int)HkTaskStatusKind.Running:
                                    //通知中控这个任务正在进行
                                    await _transferCommon.ReportHikTaskLogToCentral(queryingHikTask.TransferId.ToString(),
                                        $"海康AGV{task.agvCode}正在执行料仓任务中,料仓号：{queryingHikTask.SiloCode},任务号:{task.taskCode}...");
                                    break;

                                case (int)HkTaskStatusKind.End:  //海康任务结束但是中转位没收到数据，应该先设置数据，再上报任务结束状态
                                    //通知中控这个任务正常结束了
                                    await _transferCommon.ReportHikTaskLogToCentral(queryingHikTask.TransferId.ToString(),
                                        $"海康AGV{task.agvCode}执行料仓任务正常结束,料仓号：{queryingHikTask.SiloCode},任务号:{task.taskCode}");
                                    break;

                                case (int)HkTaskStatusKind.Break:
                                    //通知中控这个任务被中断了
                                    _logger.LogInformation($"begin to do HikAgvMonitor ExecuteAsync,HIK 任务中断");
                                    await _transferCommon.ReportHikTaskLogToCentral(queryingHikTask.TransferId.ToString(),
                                        $"海康AGV{task.agvCode}料仓任务中断,料仓号：{queryingHikTask.SiloCode},任务号:{task.taskCode}");
                                    //    await _transferCommon.ReportTaskStatus(ScheduledTaskStatus.Failed, queryingHikTask);
                                    break;

                                case (int)HkTaskStatusKind.Cancel:
                                    //通知中控这个任务被取消了
                                    await _transferCommon.ReportTaskStatus(ScheduledTaskStatus.Canceled, queryingHikTask);
                                    await _transferCommon.ReportHikTaskLogToCentral(queryingHikTask.TransferId.ToString(),
                                        $"海康AGV{task.agvCode}料仓任务被取消,料仓号：{queryingHikTask.SiloCode},任务号:{task.taskCode}");
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _isBusy = false;
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do HikAgvMonitor ExecuteAsync");
            }
        }
    }
}
