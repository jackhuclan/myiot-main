using System.Text.Json;
using Microsoft.Extensions.Logging;
using VegaIot.External.Agv.Executor;
using VegaIot.External.AgvEntity.Hik;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.OpenAPI;
using static VegaIot.External.AgvEntity.Hik.HikCarStatus;

namespace VegaIot.External.HikAgv.Executor.None;

internal class AutoTrackSiloExcutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ILogger<AutoTrackSiloExcutor> _logger;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly HikAgvConfig _hikAgvConfig;

    public AutoTrackSiloExcutor(IScheduleTaskManager scheduleTaskManager,
        IHttpRequestInvoker httpRequestInvoker,
        ILoggerFactory loggerFactory,
        ILocationManager locationManager,
        ITransferPlanManager transferPlanManager,
        HikAgvConfig hikAgvConfig)
    {
        _scheduleTaskManager = scheduleTaskManager;
        _httpRequestInvoker = httpRequestInvoker;
        _logger = loggerFactory.CreateLogger<AutoTrackSiloExcutor>();
        _locationManager = locationManager;
        _transferPlanManager = transferPlanManager;
        _hikAgvConfig = hikAgvConfig;
    }

    public async Task<bool> Execute(TransferJob job)
    {
        var sendTaskToHikAgvUrl = await _hikAgvConfig.GetGenAgvSchedulingTaskUrl();

        _logger.LogInformation($"HikAgvScheduler - AutoTrackSiloExcutor,task:【{job.Id}】,begin to ExecuteAsync==========================\r\n");

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = job.Id,
            Message = $"开始分配执行···"
        });

        var locationList = new List<HikLocation>();
        var fromLocation = new HikLocation();
        if (!string.IsNullOrEmpty(job.StartLocationCode)
            && _locationManager.TryGetLocation(job.StartLocationCode, out var startLocation)
            && startLocation != null
            && startLocation.TransAGVInnerPoint != null)
        {
            fromLocation.positionCode = startLocation.TransAGVInnerPoint;
            fromLocation.type = "00";

            locationList.Add(fromLocation);
            _logger.LogInformation($"HikAgvScheduler - AutoTrackSiloExcutor,task:【{job.Id}】,startLocation:【{startLocation.TransAGVInnerPoint}】.\r\n");

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"起始点位:{startLocation.TransAGVInnerPoint}"
            });
        }

        var toLocation = new HikLocation();
        if (!string.IsNullOrEmpty(job.EndLocationCode)
            && _locationManager.TryGetLocation(job.EndLocationCode, out var endLocation)
            && endLocation != null
            && endLocation.TransAGVInnerPoint != null)
        {
            toLocation.positionCode = endLocation.TransAGVInnerPoint;
            toLocation.type = "00";

            locationList.Add(toLocation);
            _logger.LogInformation($"HikAgvScheduler - AutoTrackSiloExcutor,task:【{job.Id}】,endLocation:【{endLocation.TransAGVInnerPoint}】.\r\n");

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"目标点位:{endLocation.TransAGVInnerPoint}"
            });
        }

        var taskCode = "VegaTask_" + job.Id.ToStr();

        var hikTyp = await _hikAgvConfig.GetHikTyp();

        var sentTaskToHikAgvData = new HikTaskRequest()
        {
            reqCode = taskCode,
            reqTime = "",
            clientCode = "",
            tokenCode = "",
            taskTyp = hikTyp,
            ctnrTyp = "",
            ctnrCode = "",
            ctnrNum = "",
            taskMode = "",
            wbCode = "",
            positionCodePath = locationList,
            podCode = "",
            podDir = "",
            podTyp = "",
            materialLot = "",
            materialType = "",
            priority = "",
            taskCode = taskCode,
            agvCode = "",
            groupId = "",
            agvTyp = "",
            positionSelStrategy = "",
            data = ""
        };

        var ret = await _httpRequestInvoker.PostAsJsonAsync<HikTaskRequest, GenAgvSchedulingTaskResponse>(sendTaskToHikAgvUrl, sentTaskToHikAgvData);
        if (ret != null
            && ret.code == "0"
            && job.StartScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(job.StartScheduleId.Value, out var startSchedule)
            && startSchedule != null
            && job.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(job.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && !string.IsNullOrEmpty(job.StartDeviceId) && !string.IsNullOrEmpty(startSchedule.Code)
            && !string.IsNullOrEmpty(job.EndDeviceId) && !string.IsNullOrEmpty(endSchedule.Code))
        {
            _logger.LogInformation($"HikAgvScheduler - AutoTrackSiloExcutor,task:【{job.Id}】,send request data to hik.\r\n");

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"已发送给海康料仓转运任务。"
            });

            job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
            job.HikResponseKey = ret.data.ToStr();
            job.HkResponse = JsonSerializer.Serialize(ret);
            await _transferPlanManager.TryUpdateTransferJob(job);

            _logger.LogInformation($"HikAgvScheduler - AutoTrackSiloExcutor,task:【{job.Id}】,startSchedule:【{startSchedule.Id}】----SetScheduleAsWorking:TraceId【{startSchedule.Code}】.\r\n");

            await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
            {
                DeviceId = job.StartDeviceId,
                TraceId = startSchedule.Code
            });

            _logger.LogInformation($"HikAgvScheduler - AutoTrackSiloExcutor,task:【{job.Id}】,endSchedule:【{endSchedule.Id}】---SetScheduleAsWorking:TraceId【{endSchedule.Code}】,fromLocation:【{fromLocation}】,startScheduleId:【{startSchedule.Id}】,toLocation:【{toLocation}】,endScheduleId【{endSchedule.Id}】.\r\n");

            await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
            {
                DeviceId = job.EndDeviceId,
                TraceId = endSchedule.Code,
                Params = new Dictionary<string, object?>
                {
                    {"CarCurrentPos",fromLocation},
                    {"BehaivorName",$"承接【{startSchedule.Id}】，调度路线：从 {job.StartLocationCode}【{startSchedule.Id}】到{job.EndLocationCode}【{endSchedule.Id}】"}
                }
            });

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"调度路线：从 {job.StartLocationCode}【{startSchedule.Id}】到{job.EndLocationCode}【{endSchedule.Id}】."
            });
        }
        else
        {
            job.HkResponse = JsonSerializer.Serialize(ret);
            await _transferPlanManager.TryUpdateTransferJob(job);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"发送海康任务失败，原因:{job.HkResponse}。"
            });
        }

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = job.Id,
            Message = $"等待海康分配AGV···"
        });

        _logger.LogInformation($"HikAgvScheduler - AutoTrackSiloExcutor,task:【{job.Id}】,end to ExecuteAsync==========================\r\n");

        return ret != null && ret.code == "0";
    }
}
