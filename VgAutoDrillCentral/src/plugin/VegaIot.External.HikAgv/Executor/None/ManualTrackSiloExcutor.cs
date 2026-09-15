using System.Text.Json;
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

namespace VegaIot.External.HikAgv.Executor.None;

internal class ManualTrackSiloExcutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ILogger<ManualTrackSiloExcutor> _logger;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly HikAgvConfig _hikAgvConfig;
    private readonly IPartitionManager _partitionManager;
    private readonly IObjectFactory _objectFactory;

    public ManualTrackSiloExcutor(IScheduleTaskManager scheduleTaskManager,
        IHttpRequestInvoker httpRequestInvoker,
        ILoggerFactory loggerFactory,
        ILocationManager locationManager,
        ITransferPlanManager transferPlanManager,
        HikAgvConfig hikAgvConfig,
        IPartitionManager partitionManager,
        IObjectFactory objectFactory)
    {
        _scheduleTaskManager = scheduleTaskManager;
        _httpRequestInvoker = httpRequestInvoker;
        _logger = loggerFactory.CreateLogger<ManualTrackSiloExcutor>();
        _locationManager = locationManager;
        _transferPlanManager = transferPlanManager;
        _hikAgvConfig = hikAgvConfig;
        _partitionManager = partitionManager;
        _objectFactory = objectFactory;
    }

    public async Task<bool> Execute(TransferJob job)
    {
        _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,begin to ExecuteAsync==========================\r\n");

        var sendTaskToHikAgvUrl = await _hikAgvConfig.GetGenAgvSchedulingTaskUrl();
        var locationList = new List<HikLocation>();
        var fromLocation = new HikLocation();
        var toLocation = new HikLocation();
        var taskCode = "VegaTask_" + job.Id.ToStr();
        var hikTyp = await _hikAgvConfig.GetHikTyp();

        if (string.IsNullOrEmpty(job.StartLocationCode) && string.IsNullOrEmpty(job.EndLocationCode))
        {
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,transportation task startLocationCode and endLocationCode is null!");
            return false;
        }

        var locationHandle = _objectFactory.GetOrCreate<HandlePartitionRack>();
        var outLocation = await locationHandle.Handle(job);

        if (!string.IsNullOrEmpty(job.StartLocationCode)
            && string.IsNullOrEmpty(job.EndLocationCode)
            && outLocation != null
            && outLocation.TransAGVInnerPoint != null
            && _locationManager.TryGetLocation(job.StartLocationCode, out var fromLoc)
            && fromLoc != null
            && fromLoc.TransAGVInnerPoint != null)
        {
            fromLocation.positionCode = fromLoc.TransAGVInnerPoint;
            fromLocation.type = "00";
            locationList.Add(fromLocation);
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,fromLocation:【{fromLoc.TransAGVInnerPoint}】.\r\n");

            toLocation.positionCode = outLocation.TransAGVInnerPoint;
            toLocation.type = "00";
            locationList.Add(toLocation);
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,toLocation:【{outLocation.TransAGVInnerPoint}】.\r\n");
        }

        if (!string.IsNullOrEmpty(job.EndLocationCode)
            && string.IsNullOrEmpty(job.StartLocationCode)
            && outLocation != null
            && outLocation.TransAGVInnerPoint != null
            && _locationManager.TryGetLocation(job.EndLocationCode, out var endLoc)
            && endLoc != null
            && endLoc.TransAGVInnerPoint != null)
        {
            fromLocation.positionCode = outLocation.TransAGVInnerPoint;
            fromLocation.type = "00";
            locationList.Add(fromLocation);
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,fromLocation:【{outLocation.TransAGVInnerPoint}】.\r\n");

            toLocation.positionCode = endLoc.TransAGVInnerPoint;
            toLocation.type = "00";
            locationList.Add(toLocation);
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,toLocation:【{endLoc.TransAGVInnerPoint}】.\r\n");
        }

        if (string.IsNullOrEmpty(fromLocation.positionCode)
            || string.IsNullOrEmpty(toLocation.positionCode))
        {
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,send hik fromLocation:【{fromLocation.positionCode}】 or toLocation:【{toLocation.positionCode}】 is null.\r\n");
            return false;
        }

        if (!_locationManager.TryGetLocation(fromLocation.positionCode, out var startLocation)
            || startLocation == null
            || startLocation.Schedule == null)
        {
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,not find startLocation【{fromLocation.positionCode}】.\r\n");
            return false;
        }

        if (!_locationManager.TryGetLocation(toLocation.positionCode, out var endLocation)
            || endLocation == null
            || endLocation.Schedule == null)
        {
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,not find endLocation【{toLocation.positionCode}】.\r\n");
            return false;
        }

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
            && startLocation != null
            && startLocation.ScheduleId != null
            && _scheduleTaskManager.TryGetScheduleTaskById(startLocation.ScheduleId.Value, out var startSchedule)
            && startSchedule != null
            && endLocation != null
            && endLocation.ScheduleId != null
            && _scheduleTaskManager.TryGetScheduleTaskById(endLocation.ScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && !string.IsNullOrEmpty(job.StartDeviceId) && !string.IsNullOrEmpty(startSchedule.Code)
            && !string.IsNullOrEmpty(job.EndDeviceId) && !string.IsNullOrEmpty(endSchedule.Code))
        {
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,send request data to hik.\r\n");

            startSchedule.Appointed = true;
            endSchedule.Appointed = true;

            job.StartLocationCode = startLocation.Code;
            job.StartDeviceId = startLocation.DeviceId;
            job.StartScheduleId = startLocation.ScheduleId;

            job.EndLocationCode = endLocation.Code;
            job.EndDeviceId = endLocation.DeviceId;
            job.EndScheduleId = endLocation.ScheduleId;

            job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
            job.HikResponseKey = ret.data.ToStr();
            job.HkResponse = JsonSerializer.Serialize(ret);
            await _transferPlanManager.TryUpdateTransferJob(job);

            await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
            {
                DeviceId = job.StartDeviceId,
                TraceId = startSchedule.Code
            });

            await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
            {
                DeviceId = job.EndDeviceId,
                TraceId = endSchedule.Code,
                Params = new Dictionary<string, object?>
                {
                    {"CarCurrentPos",fromLocation},
                    {"BehaivorName",$"承接【{startSchedule.Id}】，调度路线：从 {startLocation.Code}【{startSchedule.Id}】到{endLocation.Code}【{endSchedule.Id}】.\r\n"}
                }
            });
        }
        else if (startLocation != null
            && startLocation.Schedule != null
            && endLocation != null
            && endLocation.Schedule != null)
        {
            _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,send hik data failed.\r\n");

            startLocation.Schedule.Appointed = false;
            endLocation.Schedule.Appointed = false;

            job.HkResponse = JsonSerializer.Serialize(ret);
            await _transferPlanManager.TryUpdateTransferJob(job);
        }

        _logger.LogInformation($"HikAgvScheduler - ManualTrackSiloExcutor,task:【{job.Id}】,end to ExecuteAsync==========================\r\n");

        return true;
    }
}
