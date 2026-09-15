using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VegaIot.External.Agv.Executor;
using VegaIot.External.AgvEntity.Hik;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

using static VegaIot.External.AgvEntity.Hik.HikCarStatus;

namespace VegaIot.External.HikAgv.Executor.ForkOutside;

/// <summary>
/// 熟料从内部中转位运转外部熟料区
/// </summary>
internal class DrilledFromForkToOutsideExecutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly HikAgvConfig _hikAgvConfig;
    private readonly ILogger<DrilledFromForkToOutsideExecutor> _logger;

    public DrilledFromForkToOutsideExecutor(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _workOrderTaskAdapter = serviceProvider.GetRequiredService<IWorkOrderTaskAdapter>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _hikAgvConfig = serviceProvider.GetRequiredService<HikAgvConfig>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<DrilledFromForkToOutsideExecutor>();
    }

    public async Task<bool> Execute(TransferJob job)
    {
        _logger.LogInformation($"HikAgvScheduler - DrilledFromForkToOutsideExecutor,task:{job.Id}, begin to ExecuteAsync==========================\r\n");

        var podStatus = "0";//托盘状态

        // 检查熟料板数量，确保不为空
        var podLotNum = job.ClinkerCount ?? 0;
        var specGroup = await _workOrderTaskAdapter.GetWorkOrderInfo(job.InternalLotNo!);
        if (_hikAgvConfig.IsCustomPanelCount() && specGroup.ContainsKey("PanelCount"))
        {
            podLotNum *= (int)specGroup["PanelCount"]!;
        }

        _logger.LogInformation($"HikAgvScheduler - DrilledFromForkToOutsideExecutor,task:{job.Id},ClinkerCount:{job.ClinkerCount},InternalLotNo:{job.InternalLotNo},specGroup:{specGroup},podLotNum:{podLotNum}.\r\n");

        if (podLotNum == 0)
        {
            job.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
            await _transferPlanManager.TryUpdateTransferJob(job);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"Fail,料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}没有熟料数量为0，不能执行"
            });

            _logger.LogInformation($"HikAgvScheduler - DrilledFromForkToOutsideExecutor,task:{job.Id},料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}数量为0.\r\n");

            return false;
        }

        // 料号检查
        if (string.IsNullOrEmpty(job.ExternalLotNo)
            || !job.ExternalLotNo.Contains("@"))
        {
            job.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
            await _transferPlanManager.TryUpdateTransferJob(job);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"Fail,料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}没有熟料料号"
            });

            _logger.LogInformation($"HikAgvScheduler - DrilledFromForkToOutsideExecutor,task:{job.Id},料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}没有料号.\r\n");

            return false;
        }

        // 料号检查
        if (job.ExternalLotNo.Split('@')[0] != job.InternalLotNo)
        {
            job.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
            await _transferPlanManager.TryUpdateTransferJob(job);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"Fail,料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}对内料号与对外料号不一致"
            });

            _logger.LogInformation($"HikAgvScheduler - DrilledFromForkToOutsideExecutor,task:{job.Id},料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}对内料号与对外料号不一致.\r\n");

            return false;
        }

        // 起始(中转位)位置
        var fromLocation = new HikLocation();
        if (!string.IsNullOrEmpty(job.StartLocationCode)
            && _locationManager.TryGetLocation(job.StartLocationCode, out var startLocation)
            && startLocation != null)
        {
            fromLocation.positionCode = startLocation.TransAGVInnerPoint;
            _logger.LogInformation($"HikAgvScheduler - DrilledFromForkToOutsideExecutor,task:{job.Id},Start location: {startLocation.TransAGVInnerPoint}.\r\n");
        }

        var url = await _hikAgvConfig.GetGenAgvSchedulingTaskUrl();

        var isSuccess = false;

        //熟料区域
        var clinkerStoreArray = (await _hikAgvConfig.GetClinkerStore()).Split(',');
        foreach (var item in clinkerStoreArray)
        {
            var path = new object[] { fromLocation, $"{item}${{04}}" };
            var taskTyp = "WJ02";
            var siloCode = job.SiloCode ?? "";
            var materialLot = string.IsNullOrEmpty(job.ExternalLotNo) ? job.InternalLotNo! : job.ExternalLotNo;
            var request = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Id, podLotNum, podStatus, materialLot, taskTyp, siloCode);

            var ret = await _httpRequestInvoker.PostAsJsonAsync<GenAgvSchedulingTaskRequest, GenAgvSchedulingTaskResponse>(url, request);
            if (ret != null
                && ret.code == "0"
                && job.StartScheduleId.HasValue
                && _scheduleTaskManager.TryGetScheduleTaskById(job.StartScheduleId.Value, out var startSchedule)
                && startSchedule != null
                && !string.IsNullOrEmpty(job.StartDeviceId)
                && !string.IsNullOrEmpty(startSchedule.Code))
            {
                isSuccess = true;

                _logger.LogInformation($"HikAgvScheduler - DrilledFromForkToOutsideExecutor,task:{job.Id},send request data to hik,Time:{DateTime.Now}.\r\n");

                job.EndLocationCode = item;
                job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
                job.HikResponseKey = ret.data.ToStr();
                job.HkResponse = JsonSerializer.Serialize(ret);
                await _transferPlanManager.TryUpdateTransferJob(job);

                _logger.LogInformation($"HikAgvScheduler - DrilledFromForkToOutsideExecutor,task:{job.Id},StartSchedule{startSchedule.Id}----SetScheduleAsWorking:TraceId{startSchedule.Code}.\r\n");

                await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
                {
                    DeviceId = job.StartDeviceId,
                    TraceId = startSchedule.Code,
                    Params = new Dictionary<string, object?>
                    {
                        {"CarCurrentPos",job.StartLocationCode},
                        {"BehaivorName",$"调度路线：从内部中转位:{job.StartLocationCode}【{startSchedule.Id}】到外部熟料区域:{item}.\r\n"}
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

        _logger.LogInformation($"HikAgvScheduler - DrilledFromForkToOutsideExecutor,task:{job.Id},end to ExecuteAsync==================================\r\n");

        return isSuccess;
    }
}
