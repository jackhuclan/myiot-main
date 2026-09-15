using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ScheduleHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob;

namespace VgAutoDrill.Admin.WebApi.ScheduleJobs
{
    /// <summary>
    /// 后台定时任务
    /// </summary>
    public class RegularDeleteDataJob : BackgroundService
    {
        private readonly ILogger<RegularDeleteDataJob> _logger;
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        private volatile bool _isBusy = false;
        private readonly PeriodicTimer _timer;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IScheduleService _scheduleService;
        private readonly IDeviceRecordsService _deviceRecordsService;
        private readonly ITransferJobLogService _tranferJobLogService;
        private readonly ITransportationTaskService _transportationTaskService;
        private readonly IScheduleLogService _scheduleLogService;
        private readonly IAlarmService _alarmService;
        private readonly IDeviceServiceInvocationService _deviceServiceInvocationService;

        public RegularDeleteDataJob(ILogger<RegularDeleteDataJob> logger,
            ITransferJobLogService tranferJobLogService,
            ITransportationTaskService transportationTaskService,
            IScheduleLogService scheduleLogService,
            IAlarmService alarmService,
            ISysConfigManager sysConfigManager,
            IDeviceServiceInvocationService deviceServiceInvocationService,
            IDeviceRecordsService deviceRecordsService,
            IScheduleService scheduleService,
            IOptions<InnerOptions> options)
        {
            _logger = logger;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(options.Value != null ? options.Value.DeleteUnusedDataPerMinutes : 300));

            _deviceRecordsService = deviceRecordsService;
            _scheduleService = scheduleService;
            _sysConfigManager = sysConfigManager;
            _tranferJobLogService = tranferJobLogService;
            _scheduleLogService = scheduleLogService;
            _alarmService = alarmService;
            _deviceServiceInvocationService = deviceServiceInvocationService;
            _transportationTaskService = transportationTaskService;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested
                && !_isBusy
                && await _timer.WaitForNextTickAsync())
            {
                try
                {
                    _autoResetEvent.WaitOne();
                    _isBusy = true;
                    _logger.LogDebug($"=== Begin to do RegularDeleteDataJob ExecuteAsync");
                    var TransferScheduleHistoryDataSwitch = await _sysConfigManager.GetBoolValue(MESConfigConstants.TRANSFER_SCHEDULE_HISTORY_DATA_SWITCH);
                    if (TransferScheduleHistoryDataSwitch)
                    {
                        await _scheduleService.RegularDeleteHisData();
                        await Task.Delay(50);
                        var request = new TransferScheduleHistoryDataReq { TransferTime = DateTime.Now.AddDays(-2).Date };
                        await _scheduleService.TransferScheduleHistoryData(request);
                        //迁移料仓历史任务数据
                        var jobRequest = new TransferJobHistoryDataReq { TransferTime = DateTime.Now.AddDays(-2).Date };
                        await _transportationTaskService.TransferTransportationHistoryTaskData(jobRequest);
                        await Task.Delay(50);
                        await _transportationTaskService.RegularDeleteHisData();
                        await Task.Delay(50);
                        await _tranferJobLogService.RegularDeleteData();
                        await Task.Delay(50);
                        await _scheduleLogService.RegularDeleteData();
                        await Task.Delay(50);
                        await _deviceServiceInvocationService.RegularDeleteData();
                        await Task.Delay(50);
                        await _alarmService.RegularDeleteData();
                        await Task.Delay(50);
                        await _deviceRecordsService.RegularDeleteData();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                finally
                {
                    _isBusy = false;
                    _autoResetEvent.Set();
                    _logger.LogDebug($"=== End to do RegularDeleteDataJob ExecuteAsync");
                }
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"RegularDeleteDataJob stopped at: {DateTime.Now}");
            return base.StopAsync(cancellationToken);
        }

    }
}
