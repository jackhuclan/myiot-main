using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VegaIot.External.Agv.Executor;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.StdAgv.Executor.ForkOutside;

/// <summary>
/// 生料从外部生料区运转到内部中转位
/// </summary>
internal class UndrilledFromOutsideToForkExecutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly StdAgvConfig _stdAgvConfig;
    private readonly ILogger<UndrilledFromOutsideToForkExecutor> _logger;

    public UndrilledFromOutsideToForkExecutor(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _stdAgvConfig = serviceProvider.GetRequiredService<StdAgvConfig>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<UndrilledFromOutsideToForkExecutor>();
    }

    public async Task<bool> Execute(TransferJob job)
    {
        _logger.LogInformation($"StdAgvScheduler_UndrilledFromOutsideToForkExecutor,task:{job.Id}, begin to ExecuteAsync======================\r\n");

        var podStatus = "1";//托盘状态

        // 结束(中转位)位置
        var endLocation = new Location();
        if (string.IsNullOrEmpty(job.EndLocationCode)
            || !_locationManager.TryGetLocation(job.EndLocationCode, out endLocation)
            || endLocation == null)
        {
            //  endLocation.positionCode = endLocation.PositionCode;
            _logger.LogWarning($"StdAgvScheduler_UndrilledFromOutsideToForkExecutor,task:{job.Id} not find any endLocations.\r\n");
            return false;
        }

        var url = await _stdAgvConfig.GetGenAgvSchedulingTaskUrl();

        var isSuccess = false;

        // 获取转运区域
        var lineStoreArray = (await _stdAgvConfig.GetLineStore()).Split(',');
        foreach (var item in lineStoreArray)
        {
            var path = new String[] { job.InternalLotNo, endLocation.PositionCode };
            var taskTyp = "F07";
            var siloCode = job.SiloCode ?? "";
            var materialLot = job.InternalLotNo;

            var request = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Id, 0, podStatus, materialLot, taskTyp, siloCode, "1");

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"调用斯坦德生成拉生料任务: url: {url}, 请求参数: \r\n {request.ToJson()} "
            });

            var ret = await _httpRequestInvoker.PostAsJsonAsync<STDGenAgvSchedulingTaskRequestEntity, STDResponse>(url, request);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"调用斯坦德生成拉生料任务: 返回内容: \r\n {ret.ToJson()} "
            });

            if (ret != null
                && ret.code == 0
                && job.EndScheduleId.HasValue
                && _scheduleTaskManager.TryGetScheduleTaskById(job.EndScheduleId.Value, out var endSchedule)
                && endSchedule != null
                && !string.IsNullOrEmpty(job.EndDeviceId)
                && !string.IsNullOrEmpty(endSchedule.Code))
            {
                isSuccess = true;

                _logger.LogInformation($"StdAgvScheduler_UndrilledFromOutsideToForkExecutor,task:{job.Id}, Sent request data to Std.\r\n");

                job.StartLocationCode = item;
                job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
                job.HikResponseKey = ret.reqCode;
                job.HkResponse = JsonSerializer.Serialize(ret, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                });

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
                job.HkResponse = string.Join('丨', JsonSerializer.Serialize(ret, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                }));
                await _transferPlanManager.TryUpdateTransferJob(job);
            }
        }

        _logger.LogInformation($"StdAgvScheduler_UndrilledFromOutsideToForkExecutor,task:{job.Id}, end to ExecuteAsync======================\r\n");

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

        var queryDataUrl = await _stdAgvConfig.GetStockInfoQueryUrl();
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
