using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.WebApi.ScheduleJobs
{
    /// <summary>
    /// 定时处理解析外部推送的工单
    /// </summary>
    public class ExternalOrderWorkerJob : BackgroundService
    {
        private readonly ILogger<ExternalOrderWorkerJob> _logger;
        private readonly ExternalOptions _externalOptions;
        private readonly IExternalWorkOrderService _externalWorkOrderService;
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        private volatile bool _isBusy = false;
        private readonly PeriodicTimer _timer;
        private readonly ISysConfigManager _sysConfigManager;

        public ExternalOrderWorkerJob(ILogger<ExternalOrderWorkerJob> logger,
            IOptions<ExternalOptions> options,
            IExternalWorkOrderService externalWorkOrderService,
            ISysConfigManager sysConfigManager)
        {
            _logger = logger;
            _externalOptions = options.Value;
            _externalWorkOrderService = externalWorkOrderService;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(_externalOptions.ConsumingPerSeconds));
            _sysConfigManager = sysConfigManager;
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
                    _logger.LogDebug($"=== Begin to do ExternalOrderWorkerJob ExecuteAsync");

                    await _sysConfigManager.InitializeSysConfigs(false);

                    if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JIANGXI_KINWONG_WIP_ENABLE
                        , Model.Enum.SysConfigCategoryEnum.None, false))
                    {
                        string urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JIANGXI_KINWONG_WIP_URL,
                            Model.Enum.SysConfigCategoryEnum.None, false);
                        if (string.IsNullOrEmpty(urlAdress))
                        {
                            _logger.LogWarning("Lost key configure: ExternalOptions/UrlJiangXiKinWongWIP");
                            throw new Exception("Lost key configure: ExternalOptions/UrlJiangXiKinWongWIP");
                        }
                        _logger.LogInformation(urlAdress);

                        _logger.LogDebug($"Begin sub step :Auto Import JiangXi KinWong");
                        await _externalWorkOrderService.ImportJiangXiKinWongWIP();
                        _logger.LogDebug($"End sub step :Auto Import JiangXi KinWong");

                        _logger.LogDebug($"Begin sub step :RefreshLotStockNum");
                        await _externalWorkOrderService.RefreshLotStockNum();
                        _logger.LogDebug($"End sub step :RefreshLotStockNum");
                    }

                    if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_GEN_AGV_SCHEDULING_TASK_ENABLE,
                        Model.Enum.SysConfigCategoryEnum.None, false))
                    {
                        if (string.IsNullOrEmpty(await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_GEN_AGV_SCHEDULING_TASK_URL,
                            Model.Enum.SysConfigCategoryEnum.None, false)))
                        {
                            _logger.LogWarning("Lost key configure: SysConfig/JingWangGenAgvSchedulingTaskUrl");
                            throw new Exception("Lost key configure: SysConfig/JingWangGenAgvSchedulingTaskUrl");
                        }

                        _logger.LogDebug($"Begin sub step :Send Move Silo Command");
                        await _externalWorkOrderService.SendMoveSiloCommand();
                        _logger.LogDebug($"End sub step :Send Move Silo Command");

                        _logger.LogDebug($"Begin sub step :Send KwAGVStockIn");
                        await _externalWorkOrderService.KwAGVStockVerifyAndIn();
                        _logger.LogDebug($"End sub step :Send KwAGVStockIn");
                    }

                    _logger.LogDebug($"Begin sub step :AnalyzeExternalWorkOrder");
                    await _externalWorkOrderService.AnalyzeExternalWorkOrder();
                    _logger.LogDebug($"End sub step :AnalyzeExternalWorkOrder");

                    if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_SET_MOVE_IN_LOT_ENABLE,
                        Model.Enum.SysConfigCategoryEnum.None, false))
                    {
                        if (string.IsNullOrEmpty(await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_SET_MOVE_IN_LOT_URL,
                        Model.Enum.SysConfigCategoryEnum.None, false)))
                        {
                            _logger.LogWarning("Lost key configure: ExternalOptions/JingWangSetMoveInLotUrl");
                            throw new Exception("Lost key configure: ExternalOptions/JingWangSetMoveInLotUrl");
                        }

                        _logger.LogDebug($"Begin sub step :SignMoveInLot");
                        await _externalWorkOrderService.SignMoveInLot();
                        _logger.LogDebug($"End sub step :SignMoveInLot");
                    }

                    if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_SET_MOVE_OUT_LOT_ENABLE,
                        Model.Enum.SysConfigCategoryEnum.None, false))
                    {
                        if (string.IsNullOrEmpty(await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_SET_MOVE_OUT_LOT_URL,
                        Model.Enum.SysConfigCategoryEnum.None, false)))
                        {
                            _logger.LogWarning("Lost key configure: ExternalOptions/JingWangSetMoveOutLotUrl");
                            throw new Exception("Lost key configure: ExternalOptions/JingWangSetMoveOutLotUrl");
                        }

                        _logger.LogDebug($"Begin sub step :SignMoveOutLot");
                        await _externalWorkOrderService.SignMoveOutLot();
                        _logger.LogDebug($"End sub step :SignMoveOutLot");
                    }

                    if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_SET_TRACK_IN_LOT_ENABLE,
                        Model.Enum.SysConfigCategoryEnum.None, false))
                    {
                        if (string.IsNullOrEmpty(await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_SET_TRACK_IN_LOT_URL,
                        Model.Enum.SysConfigCategoryEnum.None, false)))
                        {
                            _logger.LogWarning("Lost key configure: ExternalOptions/JingWangSetTrackInLotUrl");
                            throw new Exception("Lost key configure: ExternalOptions/JingWangSetTrackInLotUrl");
                        }

                        _logger.LogDebug($"Begin sub step :SignTrackInLot");
                        await _externalWorkOrderService.SignTrackInLot();
                        _logger.LogDebug($"End sub step :SignTrackInLot");
                    }

                    if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_SET_TRACK_OUT_LOT_ENABLE,
                        Model.Enum.SysConfigCategoryEnum.None, false))
                    {
                        if (string.IsNullOrEmpty(await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_SET_TRACK_OUT_LOT_URL,
                        Model.Enum.SysConfigCategoryEnum.None, false)))
                        {
                            _logger.LogWarning("Lost key configure: ExternalOptions/JingWangSetTrackOutLotUrl");
                            throw new Exception("Lost key configure: ExternalOptions/JingWangSetTrackOutLotUrl");
                        }

                        _logger.LogDebug($"Begin sub step :SignTrackOutLot");
                        await _externalWorkOrderService.SignTrackOutLot();
                        _logger.LogDebug($"End sub step :SignTrackOutLot");
                    }

                    if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_TRANSFER_DRILL_FILE_ENABLE, Model.Enum.SysConfigCategoryEnum.None, false))
                    {
                        if (string.IsNullOrEmpty(await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_TRANSFER_DRILL_FILE_URL,
                            Model.Enum.SysConfigCategoryEnum.None, false)))
                        {
                            _logger.LogWarning("Lost key configure: SysConfig/JingWangTransferDrillFileUrl");
                            throw new Exception("Lost key configure: SysConfig/JingWangTransferDrillFileUrl");
                        }

                        _logger.LogDebug($"Begin sub step :GetAfterDrillFilePath");
                        await _externalWorkOrderService.GetAfterDrillFilePath();
                        _logger.LogDebug($"End sub step :GetAfterDrillFilePath");

                        _logger.LogDebug($"Begin sub step :RebrushAfterDrillFilePath");
                        await _externalWorkOrderService.RebrushAfterDrillFilePath();
                        _logger.LogDebug($"End sub step :RebrushAfterDrillFilePath");

                        _logger.LogDebug($"Begin sub step :RefreshWorkOrderToTask");
                        await _externalWorkOrderService.RefreshWorkOrderToTask();
                        _logger.LogDebug($"End sub step :RefreshWorkOrderToTask");
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
                    _logger.LogDebug($"=== End to do ExternalOrderWorkerJob ExecuteAsync");
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