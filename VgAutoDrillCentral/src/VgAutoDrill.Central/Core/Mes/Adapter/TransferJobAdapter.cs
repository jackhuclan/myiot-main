using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

public class TransferJobAdapter : ITransferJobAdapter
{
    private readonly IMapper _mapper;
    private readonly IRackService _rackService;
    private readonly ILogger<TransferJobAdapter> _logger;
    private readonly ITransportationTaskService _transportationTaskService;
    private readonly ITransportationTaskDomainService _transportationTaskDomainService;
    private readonly MysqlTaskSchedulerOptions _taskSchedulerOptions;

    public TransferJobAdapter(IMapper mapper,
        IRackService rackService,
        IScheduleService scheduleService,
        IOptions<MysqlTaskSchedulerOptions> taskScheduleOptions,
        ILogger<TransferJobAdapter> logger,
        ITransportationTaskService transportationTaskService,
        ITransportationTaskDomainService transportationTaskDomainService)
    {
        _taskSchedulerOptions = taskScheduleOptions.Value;
        _mapper = mapper;
        _rackService = rackService;
        _logger = logger;
        _transportationTaskService = transportationTaskService;
        _transportationTaskDomainService = transportationTaskDomainService;
    }

    public async Task<List<TransferJob>> GetTransportations(QueryTransportationRequest request)
    {
        var queryCondition = _mapper.Map<GetTransferJobListReq>(request);
        queryCondition.PageSize = int.MaxValue;

        var queryResult = await _transportationTaskService.GetList(queryCondition);
        if (queryResult == null || queryResult.Data == null || !queryResult.Data.List.Any())
        {
            return new List<TransferJob>();
        }

        return _mapper.Map<List<TransferJob>>(queryResult.Data.List);
    }

    public async Task<TransferJob?> FindTransferJobByCode(string jobCode)
    {
        var transDto = await _transportationTaskService.FindSingle(jobCode);
        if (transDto == null) return null;
        var transferJob = _mapper.Map<TransferJob>(transDto);
        return transferJob;
    }

    public async Task<string> Create(TransferJob input)
    {
        var addOrUpdate = _mapper.Map<AddOrUpdateTransferJobReq>(input);
        //addOrUpdate.Code = Guid.NewGuid().ToString();
        addOrUpdate.ScheduledTaskStatus = ScheduledTaskStatus.Created;

        var todoScheduleStatus = new List<ScheduledTaskStatus>
            {
               ScheduledTaskStatus.Created,
               ScheduledTaskStatus.Allocated,
               ScheduledTaskStatus.Running
            };

        var queryResult = await _transportationTaskService.GetList(new GetTransferJobListReq
        {
            ScheduledTaskStatusList = todoScheduleStatus,
            StartLocationCode = input.StartLocationCode,
            EndLocationCode = input.EndLocationCode
        });

        if (queryResult == null || queryResult.Data == null || !queryResult.Data.List.Any())
        {
            return (await _transportationTaskService.AddData(addOrUpdate)).Message;
        }
        else
        {
            return $"{input.StartLocationCode},已存在呼叫记录!";
        }
    }

    public async Task<ServiceResponse> UpdateStatus(ServiceRequest request, ScheduledTaskStatus scheduledTaskStatus)
    {
        return await HandleUpdateStatus(request, scheduledTaskStatus);
    }

    public async Task<bool> TryCancel(string code, string reason)
    {
        var transDto = await _transportationTaskService.FindSingle(code);
        if (transDto == null || transDto.ScheduledTaskStatus != ScheduledTaskStatus.Created) return false;

        var updateRequest = new AddOrUpdateTransferJobReq
        {
            Code = transDto.Code,
            ScheduledTaskStatus = ScheduledTaskStatus.Canceled,
            RelatedDrillTrace = $"{reason} - {transDto.RelatedDrillTrace}",
        };

        var response = await _transportationTaskService.UpdateTransportation(updateRequest);
        if (response == null || !string.IsNullOrEmpty(response)) return false;

        return true;
    }

    private async Task<ServiceResponse> HandleUpdateStatus(ServiceRequest serviceRequest,
        ScheduledTaskStatus scheduledTaskStatus)
    {
        var transDto = await _transportationTaskService.FindSingle(serviceRequest.TraceId);
        if (transDto == null)
        {
            return new ServiceResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"trace id:{serviceRequest.TraceId},cannot find this TransportationTask.",
            };
        }

        var updateRequest = new AddOrUpdateTransferJobReq
        {
            Code = transDto.Code,
            ScheduledTaskStatus = scheduledTaskStatus,
        };

        if (transDto.InteractionSequence == InteractionSequence.LoadOnly)
        {
            updateRequest.SiloCode = serviceRequest.Params.ContainsKey("SiloCode") ? serviceRequest.Params["SiloCode"].ToStr() : string.Empty;
            updateRequest.ExternalLotNo = serviceRequest.Params.ContainsKey("ExternalLotNo") ? serviceRequest.Params["ExternalLotNo"].ToStr() : string.Empty;
            updateRequest.RawCount = serviceRequest.Params.ContainsKey("RawCount") ? serviceRequest.Params["RawCount"].ToInt() : 0;
        }

        var taskCode = serviceRequest.Params.ContainsKey("TaskCode") ? serviceRequest.Params["TaskCode"].ToStr() : "";
        var errMsg = serviceRequest.Params.ContainsKey("errMsg") ? serviceRequest.Params["errMsg"].ToStr() : "";
        updateRequest.HkResponse = $"Response：{taskCode}，{errMsg}";
        updateRequest.HikResponseKey = taskCode;

        var responseMessage = await _transportationTaskService.UpdateTransportation(updateRequest);

        return new ServiceResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = responseMessage,
        };
    }

    public async Task<int> RefreshTimeoutTransportationTask()
    {
        var affectedRows = await _transportationTaskService.ClearTimeoutTransportationTask();
        _logger.LogDebug($"RefreshTimeoutTransportationTask, count:{affectedRows}");
        return affectedRows;
    }

    public async Task<List<TransferJob>> GetTodoTransferRacks()
    {
        var todoTransferRacks = await GetTransportations(new QueryTransportationRequest
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.Allocated,
                ScheduledTaskStatus.Running,
                ScheduledTaskStatus.WaitingForAgv
            },
        });

        return todoTransferRacks;
    }

    public async Task<string> Update(TransferJob input)
    {
        var addOrUpdate = _mapper.Map<AddOrUpdateTransferJobReq>(input);

        return (await _transportationTaskService.UpdateData(addOrUpdate)).Message;
    }

    public async Task<TransferJob> QueryByID(long id)
    {
        var task = await _transportationTaskService.QueryByID(id);
        return _mapper.Map<TransferJob>(task.Data);
    }

    public async Task<string> CreateManualJob(TransferJob input)
    {
        var addOrUpdate = _mapper.Map<AddOrUpdateTransferJobReq>(input);
        addOrUpdate.ScheduledTaskStatus = ScheduledTaskStatus.Created;

        var todoScheduleStatus = new List<ScheduledTaskStatus>
            {
               ScheduledTaskStatus.Created,
               ScheduledTaskStatus.Allocated,
               ScheduledTaskStatus.Running
            };

        var queryResult = await _transportationTaskService.GetList(new GetTransferJobListReq
        {
            ScheduledTaskStatusList = todoScheduleStatus,
            StartLocationCode = input.StartLocationCode,
            EndLocationCode = input.EndLocationCode
        });

        if (queryResult == null || queryResult.Data == null || !queryResult.Data.List.Any())
        {
            var transJobModel = _mapper.Map<VgAutoDrill.Admin.Model.Entites.Mes.TransferJob>(addOrUpdate);
            transJobModel.CreateTime = DateTime.Now;
            transJobModel.CreatorId = 8888;
            transJobModel.Status = 1;
            return await _transportationTaskDomainService.Add(transJobModel) ? "" : "生成料仓任务失败";

        }
        else
        {
            return $"{input.MasterLocationCode},已存在任务记录!";
        }
    }
}
