using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VegaIot.External.HikAgv;

public class HikAgvConfig
{
    private readonly ISysConfigManager _sysConfigManager;
    private HikAgvSchedulerOptions _hikAgvSchedulerOptions;

    public HikAgvConfig(ISysConfigManager sysConfigManager,
        IOptions<HikAgvSchedulerOptions> options)
    {
        _sysConfigManager = sysConfigManager;
        _hikAgvSchedulerOptions = options.Value;
    }

    /// <summary>
    /// 钻机机台边缘中转位
    /// </summary>
    /// <returns></returns>
    public string TransShelfInnerPositions() => string.Empty;

    /// <summary>
    /// 生料仓区
    /// </summary>
    public async Task<string> GetLineStore()
    {
        var LineStore = await _sysConfigManager.GetStringValue("LineStore");
        if (string.IsNullOrWhiteSpace(LineStore))
        {
            return _hikAgvSchedulerOptions.LineStore;
        }
        else
        {
            return LineStore;
        }
    }

    /// <summary>
    /// 空料仓区
    /// </summary>
    public async Task<string> GetEmptyStore()
    {
        var EmptyStore = await _sysConfigManager.GetStringValue("EmptyStore");
        if (string.IsNullOrWhiteSpace(EmptyStore))
        {
            return _hikAgvSchedulerOptions.EmptyStore;
        }
        else
        {
            return EmptyStore;
        }
    }

    /// <summary>
    /// 熟料仓区
    /// </summary>
    public async Task<string> GetClinkerStore()
    {
        var ClinkerStore = await _sysConfigManager.GetStringValue("ClinkerStore");
        if (string.IsNullOrWhiteSpace(ClinkerStore))
        {
            return _hikAgvSchedulerOptions.ClinkerStore;
        }
        else
        {
            return ClinkerStore;
        }
    }

    /// <summary>
    /// 首件区
    /// </summary>
    public async Task<string> GetFirstStore()
    {
        var FirstStore = await _sysConfigManager.GetStringValue("FirstStore");
        if (string.IsNullOrWhiteSpace(FirstStore))
        {
            return _hikAgvSchedulerOptions.FirstStore;
        }
        else
        {
            return FirstStore;
        }
    }

    /// <summary>
    /// 是否自定义片数
    /// </summary>
    /// <returns></returns>
    public bool IsCustomPanelCount() => _hikAgvSchedulerOptions.IsCustomPanelCount;

    /// <summary>
    /// EAP下机配送物料信息通知
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetMaterialDistributionNotifyUrl()
    {
        var MaterialDistributionNotifyUrl = await _sysConfigManager.GetStringValue("MaterialDistributionNotifyUrl");
        if (string.IsNullOrWhiteSpace(MaterialDistributionNotifyUrl))
        {
            return _hikAgvSchedulerOptions.MaterialDistributionNotifyUrl;
        }
        else
        {
            return MaterialDistributionNotifyUrl;
        }
    }

    /// <summary>
    /// 查询潜伏式AGV绑定料仓点位
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetStockInfoQueryUrl()
    {
        var stockInfoQuery = await _sysConfigManager.GetStringValue("stockInfoQuery");
        if (string.IsNullOrWhiteSpace(stockInfoQuery))
        {
            return _hikAgvSchedulerOptions.StockInfoQueryUrl;
        }
        else
        {
            return stockInfoQuery;
        }
    }

    /// <summary>
    /// 生成任务单
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetGenAgvSchedulingTaskUrl()
    {
        var genAgvSchedulingTaskUrl = await _sysConfigManager.GetStringValue(MESConfigConstants.CHONGDA_GEN_AGV_SCHEDULING_TASK_URL);
        if (string.IsNullOrWhiteSpace(genAgvSchedulingTaskUrl))
        {
            return _hikAgvSchedulerOptions.GenAgvSchedulingTaskUrl;
        }
        else
        {
            return genAgvSchedulingTaskUrl;
        }
    }

    /// <summary>
    /// 继续执行任务
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetContinueTaskUrl()
    {
        var continueTaskUrl = await _sysConfigManager.GetStringValue(MESConfigConstants.CHONGDA_CONTINUE_TASK_URL);
        if (string.IsNullOrWhiteSpace(continueTaskUrl))
        {
            return _hikAgvSchedulerOptions.ContinueTaskUrl;
        }
        else
        {
            return continueTaskUrl;
        }
    }

    /// <summary>
    /// 取消任务
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetCancelTaskUrl()
    {
        var cancelTaskUrl = await _sysConfigManager.GetStringValue(MESConfigConstants.CHONGDA_CANCEL_TASK_URL);
        if (string.IsNullOrWhiteSpace(cancelTaskUrl))
        {
            return _hikAgvSchedulerOptions.CancelTaskUrl;
        }
        else
        {
            return cancelTaskUrl;
        }
    }

    /// <summary>
    /// 查询任务状态
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetQueryTaskStatusUrl()
    {
        var queryTaskStatusUrl = await _sysConfigManager.GetStringValue(MESConfigConstants.CHONGDA_QUERY_TASK_STATUS_URL);
        if (string.IsNullOrWhiteSpace(queryTaskStatusUrl))
        {
            return _hikAgvSchedulerOptions.QueryTaskStatusUrl;
        }
        else
        {
            return queryTaskStatusUrl;
        }
    }

    /// <summary>
    /// 查询AGV状态
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetQueryAgvStatusUrl()
    {
        var queryAgvStatusUrl = await _sysConfigManager.GetStringValue(MESConfigConstants.CHONGDA_QUERY_AGV_STATUS_URL);
        if (string.IsNullOrWhiteSpace(queryAgvStatusUrl))
        {
            return _hikAgvSchedulerOptions.QueryAgvStatusUrl;
        }
        else
        {
            return queryAgvStatusUrl;
        }
    }

    /// <summary>
    /// 地图位置信息同步
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetSyncMapDatasUrl()
    {
        var syncMapDatasUrl = await _sysConfigManager.GetStringValue(MESConfigConstants.CHONGDA_SYNC_MAP_DATAS_URL);
        if (string.IsNullOrWhiteSpace(syncMapDatasUrl))
        {
            return _hikAgvSchedulerOptions.SyncMapDatasUrl;
        }
        else
        {
            return syncMapDatasUrl;
        }
    }

    /// <summary>
    /// 任务类型，与在RCS-2000端配置的主任务类型编号一致。
    /// </summary>
    public async Task<string> GetHikTyp()
    {
        var hikTaskType = await _sysConfigManager.GetStringValue(MESConfigConstants.HIK_TASK_TYP);
        if (string.IsNullOrWhiteSpace(hikTaskType))
        {
            return _hikAgvSchedulerOptions.HikTyp;
        }
        else
        {
            return hikTaskType;
        }
    }

    /// <summary>
    ///是否启用监控海康任务
    /// </summary>
    ///
    public bool GetEnableHikAgvTaskMonitor() => _hikAgvSchedulerOptions.EnableHikAgvTaskMonitor;
}
