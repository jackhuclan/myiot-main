using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VegaIot.External.AgvEntity.STD;
using VegaIot.External.StdAgv.Handler;
using VegaIot.External.StdAgv.Handler.MointorAgvTask;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.StdAgv;

internal class StdAgvTaskMonitor : BackgroundService
{
    private StdAgvSchedulerOptions _stdAgvSchedulerOptions;
    private readonly ILogger<StdAgvTaskMonitor> _logger;
    private readonly StdAgvConfig _stdAgvConfig;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IDeviceManager _deviceHolder;
    private readonly ILocationManager _locationManager;
    private readonly IObjectFactory _objectFactory;
    private readonly ISysConfigManager _sysConfigManager;

    public StdAgvTaskMonitor(ITransferPlanManager transferPlanManager,
        StdAgvConfig stdAgvConfig,
        ILoggerFactory loggerFactory,
        IHttpRequestInvoker httpRequestInvoker,
        IOptions<StdAgvSchedulerOptions> options,
        IDeviceManager deviceHolder,
        ILocationManager locationManager,
        IObjectFactory objectFactory,
        ISysConfigManager sysConfigManager)
    {
        _stdAgvSchedulerOptions = options.Value;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_stdAgvSchedulerOptions.MonitorTaskStatusInterval));
        _logger = loggerFactory.CreateLogger<StdAgvTaskMonitor>();
        _transferPlanManager = transferPlanManager;
        _stdAgvConfig = stdAgvConfig;
        _httpRequestInvoker = httpRequestInvoker;
        _deviceHolder = deviceHolder;
        _locationManager = locationManager;
        _objectFactory = objectFactory;
        _sysConfigManager = sysConfigManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested
            && await _timer.WaitForNextTickAsync())
        {
            try
            {
                if (!CentralFlags.SystemPreloadCompleted)
                    continue;

                if (_stdAgvConfig.GetEnableStdAgvTaskMonitor() == false)
                    continue;

                _autoResetEvent.WaitOne();
                _logger.LogDebug($"begin to do StdAgvMonitor ExecuteAsync");

                int monitorCompleteTimeOut = 10;
                var confMonitorCompleteTimeOut = await _sysConfigManager.GetIntValue(MESConfigConstants.MONITOR_TASK_COMPLETE_TIMEOUT);
                if (confMonitorCompleteTimeOut > monitorCompleteTimeOut)
                {
                    monitorCompleteTimeOut = confMonitorCompleteTimeOut;
                }

                int monitorFailTimeOut = 10;
                var confMonitorFailTimeOut = await _sysConfigManager.GetIntValue(MESConfigConstants.MONITOR_TASK_FAIL_TIMEOUT);
                if (confMonitorFailTimeOut > monitorFailTimeOut)
                {
                    monitorFailTimeOut = confMonitorFailTimeOut;
                }

                //查询任务状态
                var jobList = _transferPlanManager.TransferJobs.Where(t => t.ScheduledTaskStatus != ScheduledTaskStatus.Completed).ToList();
                var keyList = jobList.Where(t => !string.IsNullOrEmpty(t.HikResponseKey)).Select(t => t.HikResponseKey!).ToArray();
                if (keyList != null)
                {
                    var sentTaskToStdAgvData = new QueryTaskStatusRequest
                    {
                        reqCode = "QTS" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        taskCodes = keyList,
                        reqTime = "",
                    };

                    var queryTaskStatusUrl = await _stdAgvConfig.GetQueryTaskStatusUrl();
                    var taskStatusResponse = await _httpRequestInvoker.PostAsJsonAsync<QueryTaskStatusRequest, QueryTaskStatusResponse>(queryTaskStatusUrl, sentTaskToStdAgvData);
                    if (taskStatusResponse != null
                        && taskStatusResponse.code == "0")
                    {
                        foreach (var task in taskStatusResponse.data)
                        {
                            var agvTask = jobList.FirstOrDefault(t => t.HikResponseKey == task.taskCode);
                            if (agvTask != null)
                            {
                                if (agvTask.StartDeviceId == null)
                                {
                                    _logger.LogWarning($"start device {agvTask.StartDeviceId} cannot be empty.");
                                    continue;
                                }

                                if (agvTask.EndDeviceId == null)
                                {
                                    _logger.LogWarning($"start device {agvTask.EndDeviceId} cannot be empty.");
                                    continue;
                                }

                                var startDevice = _deviceHolder.GetOnlineDevice(agvTask.StartDeviceId);
                                if (startDevice == null)
                                {
                                    _logger.LogWarning($"CCS(central contral system) deviceHolder Can't get startDevice[{agvTask.StartDeviceId}]");
                                    continue;
                                }

                                var endDevice = _deviceHolder.GetOnlineDevice(agvTask.EndDeviceId);
                                if (endDevice == null)
                                {
                                    _logger.LogWarning($"CCS(central contral system) deviceHolder Can't get endDevice[{agvTask.EndDeviceId}]");
                                    continue;
                                }

                                if (string.IsNullOrEmpty(agvTask.StartLocationCode)
                                    || !_locationManager.TryGetLocation(agvTask.StartLocationCode, out var startLocation)
                                    || startLocation == null)
                                {
                                    continue;
                                }

                                if (string.IsNullOrEmpty(agvTask.EndLocationCode)
                                || !_locationManager.TryGetLocation(agvTask.EndLocationCode, out var endLocation)
                                || endLocation == null)
                                {
                                    continue;
                                }

                                switch (task.taskStatus.ToInt())
                                {
                                    case (int)STDTaskStatusKind.Running:
                                        //通知代理端，任务进行中
                                        //更新调度记录界面--调度设备栏位
                                        if (agvTask.ScheduledTaskStatus == ScheduledTaskStatus.Created)
                                        {
                                            var runningHandler = _objectFactory.GetOrCreate<MointorTaskRunningHandler>();
                                            await runningHandler.Handle(agvTask, task);
                                        }
                                        break;

                                    case (int)STDTaskStatusKind.End:
                                        //通知代理端，任务执行成功
                                        ///如果任务超过设定时间，发现仍处于挂载状态，自动设置完成。
                                        if (DateTime.Now > agvTask.CreateTime.HasValue.ToDate().AddMinutes(monitorCompleteTimeOut)
                                            && agvTask.ScheduledTaskStatus != ScheduledTaskStatus.Canceled
                                            && agvTask.ScheduledTaskStatus != ScheduledTaskStatus.Failed)
                                        {
                                            var sucessHandler = _objectFactory.GetOrCreate<MointorTaskSucessHandler>();
                                            await sucessHandler.Handle(agvTask, startLocation, endLocation, startDevice, endDevice, task);
                                        }
                                        break;

                                    case (int)STDTaskStatusKind.Break:
                                        //通知代理端，任务执行失败
                                        ///如果任务超过设定时间，发现仍处于挂载状态，自动设置失败。
                                        if (DateTime.Now > agvTask.CreateTime.HasValue.ToDate().AddMinutes(monitorFailTimeOut)
                                             && agvTask.ScheduledTaskStatus != ScheduledTaskStatus.Canceled)
                                        {
                                            var failedHandler = _objectFactory.GetOrCreate<MointorTaskFailedHandler>();
                                            await failedHandler.Handle(agvTask, startLocation, endLocation, startDevice, endDevice, task);
                                        }
                                        break;

                                    case (int)STDTaskStatusKind.Cancel:
                                        if (agvTask.ScheduledTaskStatus != ScheduledTaskStatus.Failed)
                                        {
                                            //通知代理端，任务取消
                                            var cancelHandler = _objectFactory.GetOrCreate<MointorTaskCancelHandler>();
                                            await cancelHandler.Handle(agvTask, startLocation, endLocation, startDevice, endDevice, task);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

                //查询AGV状态
                var queryAgvStatusUrl = await _stdAgvConfig.GetQueryAgvStatusUrl();
                var agvStatusResponse = await _httpRequestInvoker.PostAsJsonAsync<QueryAgvStatusRequest, QueryAgvStatusResponse>(queryAgvStatusUrl,
                    new QueryAgvStatusRequest
                    {
                        reqCode = Guid.NewGuid().ToStr(),
                        mapCode = "BB"
                    });
                var success = agvStatusResponse != null && agvStatusResponse.code == "0";
                if (success)
                {
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do StdAgvMonitor ExecuteAsync");
            }
        }
    }
}
