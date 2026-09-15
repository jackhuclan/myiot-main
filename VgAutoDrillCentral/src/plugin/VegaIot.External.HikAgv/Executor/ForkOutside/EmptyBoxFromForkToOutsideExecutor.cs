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
/// 空料仓从内部中转位运转外部空仓区
/// </summary>
internal class EmptyBoxFromForkToOutsideExecutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly HikAgvConfig _hikAgvConfig;
    private readonly ILogger<EmptyBoxFromForkToOutsideExecutor> _logger;

    public EmptyBoxFromForkToOutsideExecutor(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _hikAgvConfig = serviceProvider.GetRequiredService<HikAgvConfig>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromForkToOutsideExecutor>();
    }

    public async Task<bool> Execute(TransferJob job)
    {
        _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromForkToOutsideExecutor,task:{job.Id},begin to ExecuteAsync======================\r\n");

        var podStatus = "0";//托盘状态

        // 起始(中转位)位置
        var fromLocation = new HikLocation();
        if (!string.IsNullOrEmpty(job.StartLocationCode)
            && _locationManager.TryGetLocation(job.StartLocationCode, out var startLocation)
            && startLocation != null)
        {
            fromLocation.positionCode = startLocation.TransAGVInnerPoint;
            _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromForkToOutsideExecutor,task:{job.Id},Start location: {startLocation.TransAGVInnerPoint}.\r\n");
        }

        var url = await _hikAgvConfig.GetGenAgvSchedulingTaskUrl();

        var isSuccess = false;

        var emptyStoreArray = (await _hikAgvConfig.GetEmptyStore()).Split(',');
        foreach (var item in emptyStoreArray)
        {
            var path = new object[] { fromLocation, $"{item}${{04}}" };
            var taskTyp = "WJ02";
            var siloCode = job.SiloCode ?? "";
            var materialLot = "";
            var podLotNum = 0;
            var lotData = new StockInfoQueryResult();
            var request = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Id, podLotNum, podStatus, materialLot, taskTyp, siloCode);

            var ret = await _httpRequestInvoker.PostAsJsonAsync<GenAgvSchedulingTaskRequest, GenAgvSchedulingTaskResponse>(url, request);
            if (ret != null
                && ret.code == "0"
                && job.StartScheduleId.HasValue
                && job.StartDeviceId != null
                && _scheduleTaskManager.TryGetScheduleTaskById(job.StartScheduleId.Value, out var startSchedule)
                && startSchedule != null
                && !string.IsNullOrEmpty(job.StartDeviceId)
                && !string.IsNullOrEmpty(startSchedule.Code))
            {
                isSuccess = true;

                _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromForkToOutsideExecutor,task:{job.Id},send request data to hik.\r\n");

                job.EndLocationCode = item;
                job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
                job.HikResponseKey = ret.data.ToStr();
                job.HkResponse = JsonSerializer.Serialize(ret);
                await _transferPlanManager.TryUpdateTransferJob(job);

                _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromForkToOutsideExecutor,task:{job.Id},startSchedule{startSchedule.Id}----SetScheduleAsWorking:TraceId{startSchedule.Code}.\r\n");

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
                job.HkResponse = string.Join('丨', JsonSerializer.Serialize(ret)); ;
                await _transferPlanManager.TryUpdateTransferJob(job);
            }
        }

        _logger.LogInformation($"HikAgvScheduler - EmptyBoxFromForkToOutsideExecutor,task:{job.Id},end to ExecuteAsync======================\r\n");

        return isSuccess;
    }
}
