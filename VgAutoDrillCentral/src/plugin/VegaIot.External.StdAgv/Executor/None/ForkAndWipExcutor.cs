using Microsoft.Extensions.Logging;

using VegaIot.External.Agv.Executor;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

using static VegaIot.External.AgvEntity.STD.StdCarStatus;

namespace VegaIot.External.StdAgv.Executor.None;

internal class ForkAndWipExcutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ILogger<ForkAndWipExcutor> _logger;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly StdAgvConfig _stdAgvConfig;

    public ForkAndWipExcutor(IScheduleTaskManager scheduleTaskManager,
        IHttpRequestInvoker httpRequestInvoker,
        ILoggerFactory loggerFactory,
        ILocationManager locationManager,
        ITransferPlanManager transferPlanManager,
        StdAgvConfig stdAgvConfig)
    {
        _scheduleTaskManager = scheduleTaskManager;
        _httpRequestInvoker = httpRequestInvoker;
        _logger = loggerFactory.CreateLogger<ForkAndWipExcutor>();
        _locationManager = locationManager;
        _transferPlanManager = transferPlanManager;
        _stdAgvConfig = stdAgvConfig;
    }

    public async Task<bool> Execute(TransferJob job)
    {
        var sendTaskToStdAgvUrl = await _stdAgvConfig.GetGenAgvSchedulingTaskUrl();

        _logger.LogInformation($"StdAgvScheduler - ForkAndWipExcutor,task:{job.Id},begin to ExecuteAsync==========================\r\n");
        //var tmpLocationList = new List<string>();
        //if (job.InteractionSequence == InteractionSequence.LoadOnly)
        //{
        //    job.EndLocationCode = "1";
        //}
        //else
        //{
        //    job.StartLocationCode = "1";
        //}
        var locationList = new List<StdLocation>();

        var fromLocation = new StdLocation();
        if (!string.IsNullOrEmpty(job.StartLocationCode)
            && _locationManager.TryGetLocation(job.StartLocationCode, out var startLocation)
            && startLocation != null
            && startLocation.PositionCode != null)
        {
            fromLocation.positionCode = startLocation.PositionCode;
            fromLocation.type = "00";

            locationList.Add(fromLocation);
            _logger.LogInformation($"StdAgvScheduler - ForkAndWipExcutor,task:{job.Id},startLocation:{startLocation.TransAGVInnerPoint}.\r\n");
        }

        var toLocation = new StdLocation();
        if (!string.IsNullOrEmpty(job.EndLocationCode)
            && _locationManager.TryGetLocation(job.EndLocationCode, out var endLocation)
            && endLocation != null
            && endLocation.PositionCode != null)
        {
            toLocation.positionCode = endLocation.PositionCode;
            toLocation.type = "00";

            locationList.Add(toLocation);
            _logger.LogInformation($"StdAgvScheduler - ForkAndWipExcutor,task:{job.Id},endLocation:{endLocation.TransAGVInnerPoint}.\r\n");
        }

        var taskCode = "VegaTask_" + job.Id.ToStr();

        var _BindPodAndMatUrl = await _stdAgvConfig.GetBindPodAndMatUrl();

        var lineStore = await _stdAgvConfig.GetLineStore();
        var model = new STDGenAgvSchedulingTaskRequestEntity()
        {
            reqCode = SnowflakeIdGenerator.nextId().ToString(),
            clientCode = "VEGA",
            taskTyp = job.InteractionSequence == InteractionSequence.LoadOnly ? "F10" : "F07",
            data = new GenAgvSchedulingTaskData()
            {
                materialLot = job.SiloCode,
            },
            userCallCodePath = new[] { job.InteractionSequence == InteractionSequence.LoadOnly ? lineStore : job.StartLocationCode, job.InteractionSequence == InteractionSequence.LoadOnly ? job.EndLocationCode : lineStore },
            //wbCode = "testStation",
            userCallCode = job.PartitionCode,
        };
        var ret = await _httpRequestInvoker.PostAsJsonAsync<STDGenAgvSchedulingTaskRequestEntity, STDResponse>(sendTaskToStdAgvUrl, model);
        if (
            ret != null
            && ret.code == 0
            && job.StartScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(job.StartScheduleId.Value, out var startSchedule)
            && startSchedule != null
            && job.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(job.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && !string.IsNullOrEmpty(job.StartDeviceId) && !string.IsNullOrEmpty(startSchedule.Code)
            && !string.IsNullOrEmpty(job.EndDeviceId) && !string.IsNullOrEmpty(endSchedule.Code))
        {
            _logger.LogInformation($"StdAgvScheduler - ForkAndWipExcutor,task:{job.Id},send request data to Std.\r\n");

            job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
            job.HikResponseKey = ret.data;
            job.HkResponse = ret.ToJson();
            await _transferPlanManager.TryUpdateTransferJob(job);

            _logger.LogInformation($"StdAgvScheduler - ForkAndWipExcutor,task:{job.Id},startSchedule{startSchedule.Id}----SetScheduleAsWorking:TraceId{startSchedule.Code}.\r\n");

            await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
            {
                DeviceId = job.StartDeviceId,
                TraceId = startSchedule.Code
            });

            _logger.LogInformation($"StdAgvScheduler - ForkAndWipExcutor,task:{job.Id},endSchedule{endSchedule.Id}---SetScheduleAsWorking:TraceId{endSchedule.Code},fromLocation:{fromLocation},startScheduleId:{startSchedule.Id},toLocation:{toLocation},endScheduleId{endSchedule.Id}.\r\n");

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
        }
        else
        {
            job.HkResponse = ret.ToJson();
            await _transferPlanManager.TryUpdateTransferJob(job);
        }

        _logger.LogInformation($"StdAgvScheduler - ForkAndWipExcutor,task:{job.Id},end to ExecuteAsync==========================\r\n");

        return ret != null && ret.code == 0;
    }
}
