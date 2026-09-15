using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Handler.Base;

namespace VgAutoDrill.Central.Core.Schedule.Handler;

internal class TransferRackExchangeSiloHandler : GeneralExchangeSiloHandler
{
    private readonly ILogger<TransferRackExchangeSiloHandler> _logger;
    private readonly IDeviceManager _deviceManager;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ILoggerFactory _loggerFactory;

    private readonly ITransferPlanManager _transferPlanManager;

    public TransferRackExchangeSiloHandler(IServiceProvider serviceProvider)
    : base(serviceProvider)
    {
        _loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = _loggerFactory.CreateLogger<TransferRackExchangeSiloHandler>();
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
    }

    protected override async Task ExchangeSilo(IEnumerable<DrillScheduleTask> orderedDrillSchedules)
    {
        if (!CentralFlags.SystemPreloadCompleted)
        {
            return;
        }

        var CentralVerifyFunction01 = await _sysConfigManager.GetBoolValue("CentralVerifyFunction01", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (CentralVerifyFunction01) return;

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"中转区正在维护，暂停料仓转运的任务，请稍候.");
            return;
        }

        //int countOfEmptyForkAtLeast = await _sysConfigManager.GetIntValue("CountOfEmptyForkAtLeast", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        //if (countOfEmptyForkAtLeast < 1)
        //{
        //    countOfEmptyForkAtLeast = 1;
        //}

        _logger.LogInformation($"schedulePath:ChangeSiloWithTransferLocationLogin begin");

        //主逻辑, AGV取放料仓
        var readyAgvDevices = _deviceManager.GetIdleBackPanelAgvs();
        foreach (var agv in readyAgvDevices.Where(x => x.RouteCodes.Any()))
        {
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
            {
                _logger.LogWarning($"schedulePath:中转区正在维护，暂停料仓转运的任务，请稍候.");
                return;
            }

            _logger.LogInformation($"schedulePath:ChangeSilo for agv {agv.DeviceKind} {agv.DeviceId}");
            await Task.Delay(5);

            //1.查找agv关联的工艺路线
            var filteredDrillSchedules = FilterDrillSchedulesByAgvRouteCode(agv, orderedDrillSchedules);

            //2.匹配料仓（插齿或者料架）
            //2.1.从插齿里面找，如果找到, 在插齿的单独处理方法里面去处理。
            await TryExchangingForkSilo(filteredDrillSchedules, agv);
        }

        ////料仓转运任务
        //_logger.LogWarning("CalculateTransferJob--新的计算方式");
        //await _transferPlanManager.TryGenerateTransferJob();

        _logger.LogInformation($"schedulePath:TransferRackExchangeSiloHandler end");
    }

    protected override async Task<ForkScheduleTask?> TryReleaseTransferJobAsync(string? partitionCode)
    {
        return await _transferPlanManager.TryReleaseTransferJobAsync(partitionCode);
    }
}
