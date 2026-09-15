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

using static VegaIot.External.AgvEntity.STD.StdCarStatus;

namespace VegaIot.External.StdAgv.Executor.ForkOutside;

/// <summary>
/// 空料仓从外部空仓区运转到内部中转位
/// </summary>
internal class EmptyBoxFromOutsideToForkExecutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly StdAgvConfig _stdAgvConfig;
    private readonly ILogger<EmptyBoxFromOutsideToForkExecutor> _logger;

    public EmptyBoxFromOutsideToForkExecutor(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _stdAgvConfig = serviceProvider.GetRequiredService<StdAgvConfig>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromOutsideToForkExecutor>();
    }

    public async Task<bool> Execute(TransferJob job)
    {
        _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromOutsideToForkExecutor,task:{job.Id}, begin to ExecuteAsync======================\r\n");

        var podStatus = "";//托盘状态

        //结束(中转位)位置
        var toLocation = new StdLocation();
        if (!string.IsNullOrEmpty(job.EndLocationCode)
            && _locationManager.TryGetLocation(job.EndLocationCode, out var endLocation)
            && endLocation != null)
        {
            toLocation.positionCode = endLocation.PositionCode;
            _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromOutsideToForkExecutor,task:{job.Id},End location: {endLocation.PositionCode}.\r\n");
        }

        var url = await _stdAgvConfig.GetGenAgvSchedulingTaskUrl();

        var isSuccess = false;

        var emptyStoreArray = (await _stdAgvConfig.GetEmptyStore()).Split(',');
        foreach (var item in emptyStoreArray)
        {
            var path = new String[] { $"{item}", toLocation.positionCode };
            var taskTyp = "F08";
            var siloCode = job.SiloCode ?? "";
            var podLotNum = 0;
            var materialLot = "";
            var request = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Id, podLotNum, podStatus, materialLot, taskTyp, siloCode);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"调用斯坦德生成上空仓任务: url: {url}, 请求参数: \r\n {request.ToJson()} "
            });

            var ret = await _httpRequestInvoker.PostAsJsonAsync<STDGenAgvSchedulingTaskRequestEntity, STDResponse>(url, request);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"调用斯坦德生成上空仓任务: url: {url}, 返回内容: \r\n {ret.ToJson()} "
            });

            if (ret != null
                && ret.code == 0
                && job.EndDeviceId != null
                && job.EndScheduleId.HasValue
                && _scheduleTaskManager.TryGetScheduleTaskById(job.EndScheduleId.Value, out var endSchedule)
                && endSchedule != null
                && !string.IsNullOrEmpty(job.EndDeviceId)
                && !string.IsNullOrEmpty(endSchedule.Code))
            {
                isSuccess = true;

                _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromOutsideToForkExecutor,task:{job.Id}, Sent request data to Std.\r\n");

                job.StartLocationCode = item;
                job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
                job.HikResponseKey = ret.reqCode;
                job.HkResponse = JsonSerializer.Serialize(ret, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                });

                await _transferPlanManager.TryUpdateTransferJob(job);

                _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromOutsideToForkExecutor,task:{job.Id},End schedule {endSchedule.Id} --- SetScheduleAsWorking: TraceId {endSchedule.Code}, toLocation {toLocation}, endScheduleId {endSchedule.Id}.\r\n");

                await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
                {
                    DeviceId = job.EndDeviceId,
                    TraceId = endSchedule.Code,
                    Params = new Dictionary<string, object?>
                    {
                        {"CarCurrentPos",item },
                        { "BehaivorName", $"调度路线：从外部空料仓区域:{item}到内部中转位:{job.EndLocationCode}【{endSchedule.Id}】" }
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

        _logger.LogInformation($"StdAgvScheduler_EmptyBoxFromOutsideToForkExecutor,task:{job.Id}: end to ExecuteAsync======================\r\n");

        return isSuccess;
    }
}
