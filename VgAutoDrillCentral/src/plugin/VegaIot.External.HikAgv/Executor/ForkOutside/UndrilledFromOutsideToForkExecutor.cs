using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VegaIot.External.Agv.Executor;
using VegaIot.External.AgvEntity.Hik;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;
using static VegaIot.External.AgvEntity.Hik.HikCarStatus;

namespace VegaIot.External.HikAgv.Executor.ForkOutside;

/// <summary>
/// 生料从外部生料区运转到内部中转位
/// </summary>
internal class UndrilledFromOutsideToForkExecutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly HikAgvConfig _hikAgvConfig;
    private readonly ILogger<UndrilledFromOutsideToForkExecutor> _logger;

    public UndrilledFromOutsideToForkExecutor(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _hikAgvConfig = serviceProvider.GetRequiredService<HikAgvConfig>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<UndrilledFromOutsideToForkExecutor>();
    }

    public async Task<bool> Execute(TransferJob job)
    {
        _logger.LogInformation($"HikAgvScheduler - UndrilledFromOutsideToForkExecutor,task:{job.Id}, begin to ExecuteAsync======================\r\n");

        var podStatus = "1";//托盘状态

        // 结束(中转位)位置
        var toLocation = new HikLocation();
        if (!string.IsNullOrEmpty(job.EndLocationCode)
            && _locationManager.TryGetLocation(job.EndLocationCode, out var endLocation)
            && endLocation != null)
        {
            toLocation.positionCode = endLocation.TransAGVInnerPoint;
            _logger.LogInformation($"HikAgvScheduler - UndrilledFromOutsideToForkExecutor,task:{job.Id},End location: {endLocation.TransAGVInnerPoint}.\r\n");
        }

        var url = await _hikAgvConfig.GetGenAgvSchedulingTaskUrl();

        var isSuccess = false;

        // 获取转运区域
        var lineStoreArray = (await _hikAgvConfig.GetLineStore()).Split(',');
        foreach (var item in lineStoreArray)
        {
            var lotData = await GetLotData(job.InternalLotNo!, item);
            if (lotData == null || string.IsNullOrEmpty(lotData.lot) || lotData.qty.ToInt() == 0)
            {
                continue;
            }

            var path = new object[] { lotData.lot + "${01}", toLocation };
            var taskTyp = "WJ01";
            var siloCode = job.SiloCode ?? "";
            var materialLot = lotData.lot;
            var podLotNum = lotData.qty.ToInt();
            var request = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Id, podLotNum, podStatus, materialLot, taskTyp, siloCode);

            var ret = await _httpRequestInvoker.PostAsJsonAsync<GenAgvSchedulingTaskRequest, GenAgvSchedulingTaskResponse>(url, request);
            if (ret != null
                && ret.code == "0"
                && job.EndScheduleId.HasValue
                && _scheduleTaskManager.TryGetScheduleTaskById(job.EndScheduleId.Value, out var endSchedule)
                && endSchedule != null
                && !string.IsNullOrEmpty(job.EndDeviceId)
                && !string.IsNullOrEmpty(endSchedule.Code))
            {
                isSuccess = true;

                _logger.LogInformation($"HikAgvScheduler - UndrilledFromOutsideToForkExecutor,task:{job.Id}, Sent request data to Hik.\r\n");

                job.StartLocationCode = item;
                job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
                job.HikResponseKey = ret.data.ToStr();
                job.HkResponse = JsonSerializer.Serialize(ret);
                await _transferPlanManager.TryUpdateTransferJob(job);

                await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
                {
                    ProductId = job.EndDeviceId,
                    DeviceId = job.EndDeviceId,
                    TraceId = endSchedule.Code,
                    Params = new Dictionary<string, object?>
                    {
                        {"CarCurrentPos",item},
                        {"BehaivorName", $"调度路线：从外部生料区域:{item}到内部中转位:{job.EndLocationCode}【{endSchedule.Id}】"}
                    }
                });

                break;
            }
            else
            {
                job.HkResponse = string.Join('丨', JsonSerializer.Serialize(ret)); ;
                await _transferPlanManager.TryUpdateTransferJob(job);
            }
        }

        _logger.LogInformation($"HikAgvScheduler - UndrilledFromOutsideToForkExecutor,task:{job.Id}, end to ExecuteAsync======================\r\n");

        return isSuccess;
    }

    /// <summary>
    /// 获取Lot号
    /// </summary>
    /// <param name="itemCode">料号，不含@</param>
    /// <param name="region">生料仓区代号</param>
    /// <returns></returns>
    private async Task<StockInfo> GetLotData(string itemCode, string region)
    {
        var selectedLot = new StockInfo();

        var request = new StockInfoQueryRequest
        {
            reqCode = $"QLot{DateTime.Now:yyyyMMddHHmmss}",
            targetPosArea = $"{region}${{{"04"}}}"
        };

        _logger.LogDebug($"查询接口 StockInfoQuery 参数: {JsonSerializer.Serialize(request)}");

        var queryDataUrl = await _hikAgvConfig.GetStockInfoQueryUrl();
        var ret = await _httpRequestInvoker.PostAsJsonAsync<StockInfoQueryRequest, StockInfoQueryResponse>(queryDataUrl, request);
        _logger.LogDebug($"查询接口 StockInfoQuery 的返回参数: {JsonSerializer.Serialize(ret)}");
        if (ret != null
            && ret.code == "0"
            && ret.data != null)
        {
            selectedLot = ret.data.Where(t => !string.IsNullOrEmpty(t.lot)
                          && t.lot.ToLower().Contains(itemCode.ToLower())
                          && t.qty.ToInt() > 0
                          && t.lot.Contains("@")
                          && t.lot.Split('@')[0] == itemCode)
                           .OrderByDescending(t => t.qty)
                           .FirstOrDefault();

            _logger.LogDebug($"查询接口 StockInfoQuery 的返回参数料号{itemCode}的selectedLot: {JsonSerializer.Serialize(selectedLot)}");

            return selectedLot!;
        }
        return selectedLot;
    }
}
