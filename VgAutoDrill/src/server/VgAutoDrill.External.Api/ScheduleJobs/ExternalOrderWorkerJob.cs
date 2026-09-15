using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;

namespace VgAutoDrill.External.WebApi.ScheduleJobs
{
    /// <summary>
    /// 定时处理解析外部推送的工单
    /// </summary>
    public class ExternalOrderWorkerJob : BackgroundService
    {
        private readonly ILogger<ExternalOrderWorkerJob> _logger;
        private readonly IExternalWorkOrderService _externalWorkOrderService;
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        private volatile bool _isBusy = false;
        private readonly PeriodicTimer _timer;
        private readonly ExternalOptions _externalOptions;
        public ExternalOrderWorkerJob(ILogger<ExternalOrderWorkerJob> logger,
            IOptions<ExternalOptions> options,
            IExternalWorkOrderService externalWorkOrderService)
        {
            _logger = logger;
            _externalOptions = options.Value;
            _externalWorkOrderService = externalWorkOrderService;
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
                    _logger.LogDebug($"Begin to do ExternalOrderWorkerJob ExecuteAsync");

                    //if(_externalOptions.EnableAutoImportJiangXiKinWong)
                    //{
                    //    _logger.LogDebug($"Begin to do EnableAutoImportJiangXiKinWong");
                    //}
                    await _externalWorkOrderService.AnalyzeExternalWorkOrder();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                finally
                {
                    _isBusy = false;
                    _autoResetEvent.Set();
                    _logger.LogDebug($"End to do ExternalOrderWorkerJob ExecuteAsync");
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
            _logger.LogInformation($"ExternalOrderWorkerJob stopped at: {DateTime.Now}");
            return base.StopAsync(cancellationToken);
        }
    }
}