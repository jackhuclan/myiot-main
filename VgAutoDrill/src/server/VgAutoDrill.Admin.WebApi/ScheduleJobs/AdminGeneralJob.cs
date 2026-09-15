using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Configuration;

namespace VgAutoDrill.Admin.WebApi.ScheduleJobs
{
    /// <summary>
    /// 后台定时任务
    /// </summary>
    public class AdminGeneralJob : BackgroundService
    {
        private readonly ILogger<AdminGeneralJob> _logger;
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        private volatile bool _isBusy = false;
        private readonly PeriodicTimer _timer;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IDeviceRecordsService _deviceRecordsService;
        private readonly IAlarmService _alarmService;
        private readonly InnerOptions _innerOptions;

        public AdminGeneralJob(ILogger<AdminGeneralJob> logger,
            IOptions<InnerOptions> options,
            ISysConfigManager sysConfigService,
            IDeviceRecordsService deviceRecordsService)
        {
            _logger = logger;
            _timer = new PeriodicTimer(TimeSpan.FromMinutes(options.Value != null ? options.Value.ConsumingPerMinutes : 10));
            _sysConfigManager = sysConfigService;
            _deviceRecordsService = deviceRecordsService;
            _innerOptions= options.Value!;
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
                    _logger.LogDebug($"=== Begin to do AdminGeneralJob ExecuteAsync");

                    await _sysConfigManager.InitializeSysConfigs(false);

                    var enablePropertiesFlusher = await _sysConfigManager.GetBoolValue("EnablePropertiesFlusher");
                    if (enablePropertiesFlusher)
                    {
                        await _deviceRecordsService.RefreshSummaryDatas();
                    }

                    // 检查指定前缀外部工单告警（根据配置开关决定是否执行）
                    await CheckSampleOrderAlarms();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                finally
                {
                    _isBusy = false;
                    _autoResetEvent.Set();
                    _logger.LogDebug($"=== End to do AdminGeneralJob ExecuteAsync");
                }
            }
        }

        /// <summary>
        /// 检查指定前缀开头外部工单告警
        /// </summary>
        /// <returns></returns>
        private async Task CheckSampleOrderAlarms()
        {
            try
            {
                // 检查是否启用sampleorder告警功能
                if (!_innerOptions.EnableSampleOrderAlarm)
                {
                    _logger.LogDebug($"SampleOrder告警功能已禁用，跳过检查");
                    return;
                }

                string workOrderPrefixConfig = _innerOptions.SampleOrderAlarmWorkOrderPrefix;
                var workOrderPrefixes = !string.IsNullOrEmpty(workOrderPrefixConfig)
                    ? workOrderPrefixConfig.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray()
                    : new string[] { "S" };
                string prefixDisplay = string.Join(",", workOrderPrefixes);
                _logger.LogDebug($"开始执行{prefixDisplay}开头外部工单告警检查");
                int newAlarmCount = await _alarmService.CheckSampleOrderAlarms();
                _logger.LogDebug($"完成{prefixDisplay}开头外部工单告警检查，新增了 {newAlarmCount} 条告警记录");
            }
            catch (Exception ex)
            {
                string workOrderPrefixConfig = _innerOptions.SampleOrderAlarmWorkOrderPrefix;
                var workOrderPrefixes = !string.IsNullOrEmpty(workOrderPrefixConfig)
                    ? workOrderPrefixConfig.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray()
                    : new string[] { "S" };
                string prefixDisplay = string.Join(",", workOrderPrefixes);
                _logger.LogError(ex, $"执行{prefixDisplay}开头外部工单告警检查时发生错误");
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"AdminGeneralJob stopped at: {DateTime.Now}");
            return base.StopAsync(cancellationToken);
        }

    }
}
