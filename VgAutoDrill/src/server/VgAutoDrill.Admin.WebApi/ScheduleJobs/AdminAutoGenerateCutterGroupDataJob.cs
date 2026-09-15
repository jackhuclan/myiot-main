using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.WebApi.ScheduleJobs
{
    public class AdminAutoGenerateCutterGroupDataJob : BackgroundService
    {
        private readonly ILogger<AdminAutoGenerateCutterGroupDataJob> _logger;
        private readonly ICutterGroupService _cutterGroupService;
        private volatile bool _isBusy = false;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        private readonly PeriodicTimer _timer;

        public AdminAutoGenerateCutterGroupDataJob(ILogger<AdminAutoGenerateCutterGroupDataJob> logger,
            ICutterGroupService cutterGroupService,
            ISysConfigManager sysConfigManager,
            IOptions<InnerOptions> options)
        {
            _logger = logger;
            _sysConfigManager = sysConfigManager;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(10));//设置job定时运行间隔
            _cutterGroupService = cutterGroupService;
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
                    _logger.LogDebug($"=== Begin to do AdminAutoGenerateCutterGroupDataJob ExecuteAsync");

                    var autoGenerateSwitch = await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_AUTO_CUTTER);
                    if (autoGenerateSwitch)
                    {
                        await _cutterGroupService.AutoGenerateCutterGroupData();
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
                    _logger.LogDebug($"=== End to do AdminAutoGenerateCutterGroupDataJob ExecuteAsync");
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
            _logger.LogInformation($"AdminAutoGenerateCutterGroupDataJob stopped at: {DateTime.Now}");
            return base.StopAsync(cancellationToken);
        }
    }
}
