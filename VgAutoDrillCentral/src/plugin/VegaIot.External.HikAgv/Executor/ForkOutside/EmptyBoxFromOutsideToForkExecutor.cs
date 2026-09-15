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
/// 空料仓从外部空仓区运转到内部中转位
/// </summary>
internal class EmptyBoxFromOutsideToForkExecutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly HikAgvConfig _hikAgvConfig;
    private readonly ILogger<EmptyBoxFromOutsideToForkExecutor> _logger;

    public EmptyBoxFromOutsideToForkExecutor(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _hikAgvConfig = serviceProvider.GetRequiredService<HikAgvConfig>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromOutsideToForkExecutor>();
    }

    public async Task<bool> Execute(TransferJob job)
    {
        _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromOutsideToForkExecutor,task:{job.Id}, begin to ExecuteAsync======================\r\n");

        var podStatus = "";//托盘状态

        //结束(中转位)位置
        var toLocation = new HikLocation();
        if (!string.IsNullOrEmpty(job.EndLocationCode)
            && _locationManager.TryGetLocation(job.EndLocationCode, out var endLocation)
            && endLocation != null)
        {
            toLocation.positionCode = endLocation.TransAGVInnerPoint;
            _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromOutsideToForkExecutor,task:{job.Id},End location: {endLocation.TransAGVInnerPoint}.\r\n");
        }

        var url = await _hikAgvConfig.GetGenAgvSchedulingTaskUrl();

        var isSuccess = false;

        var emptyStoreArray = (await _hikAgvConfig.GetEmptyStore()).Split(',');
        foreach (var item in emptyStoreArray)
        {
            var path = new object[] { $"{item}${{04}}", toLocation };
            var taskTyp = "WJ02";
            var siloCode = job.SiloCode ?? "";
            var podLotNum = 0;
            var materialLot = "";
            var request = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Id, podLotNum, podStatus, materialLot, taskTyp, siloCode);

            var ret = await _httpRequestInvoker.PostAsJsonAsync<GenAgvSchedulingTaskRequest, GenAgvSchedulingTaskResponse>(url, request);
            if (ret != null
                && ret.code == "0"
                && job.EndDeviceId != null
                && job.EndScheduleId.HasValue
                && _scheduleTaskManager.TryGetScheduleTaskById(job.EndScheduleId.Value, out var endSchedule)
                && endSchedule != null
                && !string.IsNullOrEmpty(job.EndDeviceId)
                && !string.IsNullOrEmpty(endSchedule.Code))
            {
                isSuccess = true;

                _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromOutsideToForkExecutor,task:{job.Id}, Sent request data to Hik.\r\n");

                job.StartLocationCode = item;
                job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
                job.HikResponseKey = ret.data.ToStr();
                job.HkResponse = JsonSerializer.Serialize(ret);
                await _transferPlanManager.TryUpdateTransferJob(job);

                _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromOutsideToForkExecutor,task:{job.Id},End schedule {endSchedule.Id} --- SetScheduleAsWorking: TraceId {endSchedule.Code}, toLocation {toLocation}, endScheduleId {endSchedule.Id}.\r\n");

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
                job.HkResponse = string.Join('丨', JsonSerializer.Serialize(ret)); ;
                await _transferPlanManager.TryUpdateTransferJob(job);
            }
        }

        _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromOutsideToForkExecutor,task:{job.Id}: end to ExecuteAsync======================\r\n");

        return isSuccess;
    }
}
