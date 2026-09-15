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
/// 空料仓从内部中转位运转外部空仓区
/// </summary>
internal class EmptyBoxFromForkToOutsideExecutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly StdAgvConfig _stdAgvConfig;
    private readonly ILogger<EmptyBoxFromForkToOutsideExecutor> _logger;

    public EmptyBoxFromForkToOutsideExecutor(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _stdAgvConfig = serviceProvider.GetRequiredService<StdAgvConfig>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromForkToOutsideExecutor>();
    }

    public async Task<bool> Execute(TransferJob job)
    {
        _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromForkToOutsideExecutor,task:{job.Id},begin to ExecuteAsync======================\r\n");

        var podStatus = "0";//托盘状态

        // 起始(中转位)位置
        var fromLocation = new Location();
        if (!string.IsNullOrEmpty(job.StartLocationCode)
            && _locationManager.TryGetLocation(job.StartLocationCode, out var startLocation)
            && startLocation != null)
        {
            fromLocation = startLocation;
            _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromForkToOutsideExecutor,task:{job.Id},Start location: {startLocation.TransAGVInnerPoint}.\r\n");
        }
        else
        {
            _logger.LogError($"StdAgvScheduler_EmptyBoxFromForkToOutsideExecutor,task:{job.Id},TryGetLocation error.\r\n");
            return false;
        }

        var url = await _stdAgvConfig.GetGenAgvSchedulingTaskUrl();
        var _BindPodAndMatUrl = await _stdAgvConfig.GetBindPodAndMatUrl();
        var bindLineSideStockUrl = await _stdAgvConfig.GetBindLineSideStockUrl();
        var isSuccess = false;

        var _Request210 = new BindSiloEntity()
        {
            clientCode = "VEGA001",
            indBind = "0",
            podCode = fromLocation.Panels?.FirstOrDefault()?.SiloCode,
            reqCode = $"Vega_Mat2_{DateTime.Now.Ticks.ToString()}",
            reqTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
        };

        _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromForkToOutsideExecutor:task:{job.Id}: 货架与物料解绑 - bindPodAndMat: 请求参数: \r\n {JsonSerializer.Serialize(_Request210)}");

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = job.Id,
            Message = $"货架与物料解绑 - bindPodAndMat: task:{job.Id}: url: {_BindPodAndMatUrl},请求参数: \r\n {JsonSerializer.Serialize(_Request210)} "
        });

        var res210 = await _httpRequestInvoker.PostAsJsonAsync<BindSiloEntity?, STDResponse>(_BindPodAndMatUrl, _Request210, new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });

        _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromForkToOutsideExecutor : task:{job.Id}: 货架与物料解绑 - bindPodAndMat: 返回内容: \r\n {JsonSerializer.Serialize(res210, new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        })}");

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = job.Id,
            Message = $"货架与物料解绑 - bindPodAndMat: 返回内容: \r\n {JsonSerializer.Serialize(res210, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            })} "
        });

        var emptyStoreArray = (await _stdAgvConfig.GetEmptyStore()).Split(',');
        foreach (var item in emptyStoreArray)
        {
            var path = new String[] { fromLocation.PositionCode, $"{item}" };
            var taskTyp = "F09";
            var siloCode = job.SiloCode ?? "";
            var materialLot = "";
            var podLotNum = 0;
            var lotData = new StockInfoQueryResult();
            var request = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Id, podLotNum, podStatus, materialLot, taskTyp, siloCode);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"调用斯坦德生成下空仓任务: url: {url}, 请求参数: \r\n {request.ToJson()} "
            });

            var ret = await _httpRequestInvoker.PostAsJsonAsync<STDGenAgvSchedulingTaskRequestEntity, STDResponse>(url, request);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"调用斯坦德生成下空仓任务: 返回内容: \r\n {ret.ToJson()} "
            });

            if (ret != null
                && ret.code == 0
                && job.StartScheduleId.HasValue
                && job.StartDeviceId != null
                && _scheduleTaskManager.TryGetScheduleTaskById(job.StartScheduleId.Value, out var startSchedule)
                && startSchedule != null
                && !string.IsNullOrEmpty(job.StartDeviceId)
                && !string.IsNullOrEmpty(startSchedule.Code))
            {
                isSuccess = true;

                _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromForkToOutsideExecutor,task:{job.Id},send request data to Std.\r\n");

                job.EndLocationCode = item;
                job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
                job.HikResponseKey = ret.reqCode;
                job.HkResponse = JsonSerializer.Serialize(ret, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                });

                await _transferPlanManager.TryUpdateTransferJob(job);

                _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromForkToOutsideExecutor,task:{job.Id},startSchedule{startSchedule.Id}----SetScheduleAsWorking:TraceId{startSchedule.Code}.\r\n");

                await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
                {
                    DeviceId = job.StartDeviceId,
                    TraceId = startSchedule.Code,
                    Params = new Dictionary<string, object?>
                    {
                        {"CarCurrentPos",job.StartLocationCode},
                        {"BehaivorName",$"调度路线：从内部中转位:{job.StartLocationCode}【{startSchedule.Id}】到外部空仓区域:{item}"}
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

        _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromForkToOutsideExecutor,task:{job.Id},end to ExecuteAsync======================\r\n");

        return isSuccess;
    }
}
