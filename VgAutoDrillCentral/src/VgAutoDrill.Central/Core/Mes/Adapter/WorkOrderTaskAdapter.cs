using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAlterLog;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

public class WorkOrderTaskAdapter : IWorkOrderTaskAdapter
{
    private readonly ITaskService _taskService;
    private readonly IScheduleService _scheduleService;
    private readonly IMapper _mapper;
    private readonly IWorkOrderDomainService _workOrderDomainService;
    private readonly ITaskDomainService _taskDomainService;
    private readonly IWorkOrderService _workOrderService;
    private readonly ICutterGroupService _cutterGroupService;
    private readonly IWorkOrderAlterLogService _workOrderAlterLogService;
    private readonly IConfiguration _configuration;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ILogger<WorkOrderTaskAdapter> _logger;

    public WorkOrderTaskAdapter(ITaskService taskService,
        IScheduleService scheduleService,
        IWorkOrderDomainService workOrderDomainService,
        ITaskDomainService taskDomainService,
        IWorkOrderService workOrderService,
        ICutterGroupService cutterGroupService,
        IWorkOrderAlterLogService workOrderAlterLogService,
        IMapper mapper,
        IConfiguration configuration,
        ISysConfigManager sysConfigManager,
        ILogger<WorkOrderTaskAdapter> logger)
    {
        _taskService = taskService;
        _scheduleService = scheduleService;
        _mapper = mapper;
        _cutterGroupService = cutterGroupService;
        _workOrderDomainService = workOrderDomainService;
        _taskDomainService = taskDomainService;
        _workOrderService = workOrderService;
        _workOrderAlterLogService = workOrderAlterLogService;
        _configuration = configuration;
        _sysConfigManager = sysConfigManager;
        _logger = logger;
    }
    public async Task<WorkOrderTaskResponse> ApplyNextWorkOrderTask(WorkOrderTaskRequest request)
    {
        var response = await _taskService.GetNextTask(request.DeviceId, request.RealNeedCount, request.ExistRawNum, request.SpindleUseNum, request.UndrilledItemCodesFromDrill);
        return new WorkOrderTaskResponse
        {
            Message = response.Message,
            Data = _mapper.Map<WorkOrderTask>(response.Data)
        };
    }
    public async Task<FinishTaskResponse> FinishTask(FinishTaskRequest request)
    {
        string taskId = string.Empty;
        if (request.Params.ContainsKey("TaskCode") && !string.IsNullOrWhiteSpace(request.Params["TaskCode"].ToStr()))
        {
            taskId = request.Params["TaskCode"].ToStr();
        }
        else
        {
            var schedule = await _scheduleService.FindScheduleByTraceId(request.TraceId);
            if (schedule == null)
            {
                return new FinishTaskResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"FinishTask 没有找到对应的调度记录，trace id：{request.TraceId}"
                };
            }
            else if (string.IsNullOrEmpty(schedule.TaskId))
            {
                return new FinishTaskResponse
                {
                    Code = ErrorCodes.Sys.SUCCESS,
                    Message = string.Empty
                };
            }

            taskId = schedule.TaskId;
        }

        var response = await _taskService.UpdateByOutSide(new UpdateTaskByOutSideReq
        {
            Code = taskId,
            TaskStatus = TaskStatusEnum.FINISH,
        });
        if (response == null || !string.IsNullOrEmpty(response.Message))
        {
            return new FinishTaskResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"FinishTask 更新生产任务失败，trace id：{request.TraceId}，原因：{response?.Message}"
            };
        }

        return new FinishTaskResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = ""
        };
    }
    public async Task<BeginTaskResponse> BeginTask(BeginTaskRequest request)
    {
        string taskId = string.Empty;
        if (request.Params.ContainsKey("TaskCode") && !string.IsNullOrWhiteSpace(request.Params["TaskCode"].ToStr()))
        {
            taskId = request.Params["TaskCode"].ToStr();
        }
        else
        {
            var schedule = await _scheduleService.FindScheduleByTraceId(request.TraceId);
            if (schedule == null)
            {
                return new BeginTaskResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"BeginTask 没有找到对应的调度记录，trace id：{request.TraceId}"
                };
            }
            else if (string.IsNullOrEmpty(schedule.TaskId))
            {
                return new BeginTaskResponse
                {
                    Code = ErrorCodes.Sys.SUCCESS,
                    Message = string.Empty
                };
            }

            taskId = schedule.TaskId;
        }

        return await BeginTask(taskId);
    }
    public async Task<BeginTaskResponse> BeginTask(string taskId)
    {
        var req = new UpdateTaskByOutSideReq
        {
            Code = taskId,
            TaskStatus = TaskStatusEnum.BEGIN,
        };

        var response = await _taskService.UpdateByOutSide(req);
        if (response == null || !string.IsNullOrEmpty(response.Message))
        {
            return new BeginTaskResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"BeginTask 更新生产任务失败，taskId：{taskId}，原因：{response?.Message}"
            };
        }

        return new BeginTaskResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = ""
        };
    }

    public async Task<BeginTaskResponse> SetBufferReady(string taskId)
    {
        var req = new UpdateTaskByOutSideReq
        {
            Code = taskId,
            TaskStatus = TaskStatusEnum.BUFFERED,
        };

        var response = await _taskService.UpdateByOutSide(req);
        if (response == null || !string.IsNullOrEmpty(response.Message))
        {
            return new BeginTaskResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"SetBufferReady 更新生产任务失败，taskId：{taskId}，原因：{response?.Message}"
            };
        }

        return new BeginTaskResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = ""
        };
    }
    public async Task<Dictionary<string, object?>> GetWorkOrderInfo(string itemCode)
    {
        if (string.IsNullOrEmpty(itemCode))
        {
            return new Dictionary<string, object?>
            {
                { "ItemCode", itemCode },
                { "SpecGroup", string.Empty},
                { "PanelCount", 1},
            };
        }

        var orders = await _workOrderService.GetList(new Admin.Model.ViewModels.Mes.ProWorkOrder.GetWorkOrderListReq
        {
            ItemCode = itemCode,
            QueryOrderBy = QueryOrderByEnum.OrderByCreateTimeDesc
        });

        if (orders != null && orders.Data != null && orders.Data.List.Any())
        {
            var specGroup = string.Empty;
            int? panelCount = 1;
            var currentTask = orders.Data.List.FirstOrDefault();
            if (currentTask != null)
            {
                specGroup = currentTask.SpecGroup;
                panelCount = currentTask.PanelCount;
            }

            return new Dictionary<string, object?>
            {
                { "ItemCode", itemCode },
                { "SpecGroup", specGroup},
                { "PanelCount", panelCount},
            };
        }
        else
        {
            return new Dictionary<string, object?>
            {
                { "ItemCode", itemCode },
                { "SpecGroup", string.Empty},
                { "PanelCount", 1},
            };
        }
    }

    public async Task<Dictionary<string, object?>> GetTaskInfo(string deviceId, string itemCode)
    {
        if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(itemCode))
        {
            return new Dictionary<string, object?> { };
        }

        var tasks = await _taskService.GetTaskByDevice(new Admin.Model.ViewModels.Mes.Device.GetDrillOrAgvDeviceInfoReq
        {
            DeviceCode = deviceId,
            ItemCode = itemCode,
            TaskStatusList = new List<TaskStatusEnum>
                {
                    TaskStatusEnum.COMMITED,
                    TaskStatusEnum.SENDING,
                    TaskStatusEnum.BUFFERED,
                    TaskStatusEnum.BEGIN,
                }
        });

        if (tasks != null && tasks.Data != null && tasks.Data.List.Any())
        {
            var specGroup = string.Empty;
            var currentTask = tasks.Data.List.FirstOrDefault();
            if (currentTask != null)
            {
                specGroup = currentTask.SpecGroup;
            }
            return new Dictionary<string, object?>
            {
                { "SpecGroup", specGroup},
            };
        }
        else
        {
            return new Dictionary<string, object?> { };
        }
    }

    public async Task<List<WorkOrderTask>> GetPendingTask(List<string> deviceIds)
    {
        if (deviceIds == null || deviceIds.Count == 0)
        {
            return new List<WorkOrderTask>();
        }

        var tasks = await _taskDomainService.QueryAsync(p => !string.IsNullOrEmpty(p.WorkStationCode)
        && deviceIds.Contains(p.WorkStationCode.ToLower())
        && (p.TaskStatus == TaskStatusEnum.COMMITED || p.TaskStatus == TaskStatusEnum.SENDING)
        && p.IsDeleted == 0, p => p.Code, OrderByType.Asc);

        if (tasks != null && tasks.Any())
        {
            return _mapper.Map<List<WorkTask>, List<WorkOrderTask>>(tasks);
        }

        return new List<WorkOrderTask>();
    }

    public async Task<Dictionary<string, object?>> GetDrillPath(string deviceId, string beforeDrillPath, string itemCode, string machineSize = "")
    {
        //todo, 引入系统配置变量 是否启用多机型
        //db中传回的转换后路径，都是多个机型的合并路径，
        //todo , 使用传入的machineSize，和配置参数，关联到合适的那个路径

        var request = new Admin.Model.ViewModels.Mes.Device.GetDrillOrAgvDeviceInfoReq
        {
            DeviceCode = deviceId,
            IsRebrush = 0,
            IsVerifyAfterDrillPath = true,
            TaskStatusList = new List<TaskStatusEnum>
                {
                    TaskStatusEnum.COMMITED,
                    TaskStatusEnum.SENDING,
                    TaskStatusEnum.BUFFERED,
                }
        };

        if (!string.IsNullOrEmpty(itemCode))
        {
            request.ItemCode = itemCode;
        }

        var taskResult = await _taskService.GetTaskByDevice(request);

        if (taskResult == null || taskResult.Data == null || !taskResult.Data.List.Any())
        {
            _logger.LogInformation($"GetDrillPath not find to be produced task data");
            return new Dictionary<string, object?> { };
        }

        var afterDrillPath = string.Empty;
        var isFirstCutter = false;
        var cutterGroupNo = string.Empty;

        var currentTask = taskResult.Data.List.FirstOrDefault();
        if (currentTask != null)
        {
            if (beforeDrillPath == currentTask.BeforeDrillFilePath)
            {
                var exsist = await _workOrderService.ExsistAfterDrillFilePath(currentTask.WorkOrderCode, deviceId, beforeDrillPath);
                if (!exsist)
                {
                    _logger.LogInformation($"WorkOrderCode:{currentTask.WorkOrderCode},GetDrillPath ExsistAfterDrillFilePath return false!");
                    return new Dictionary<string, object?> {
                        {
                            "ErrorMsg",$"WorkOrderCode:{currentTask.WorkOrderCode},GetDrillPath ExsistAfterDrillFilePath return false!"
                        }
                    };
                }

                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.HAVE_MULTI_MACHINES))
                {
                    if (string.IsNullOrEmpty(machineSize))
                    {
                        return new Dictionary<string, object?>
                        {
                            {
                                "ErrorMsg",$"请检查[{deviceId}]钻机代理是否在线？machineSize 已经启用多机型钻带设定时，必须传入钻机的机型machineSize;"
                            }
                        };
                    }

                    var lstFilePath = currentTask.AfterDrillFilePath?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (lstFilePath != null)
                    {
                        string machineConfig = await _sysConfigManager.GetStringValue(MESConfigConstants.MACHINE_TYPE_CONFIG);
                        if (!string.IsNullOrWhiteSpace(machineConfig))
                        {
                            var machineConfigs = JsonSerializer.Deserialize<Dictionary<string, SysConfigOfMachineTypeConfig>>(machineConfig);

                            if (machineConfigs.TryGetValue(machineSize, out SysConfigOfMachineTypeConfig? MachineTypeConfig))
                            {
                                machineSize = MachineTypeConfig.DrillMachine!;
                                afterDrillPath = lstFilePath.FirstOrDefault(p => p.Contains($"ConvDuo{machineSize}"));
                            }
                        }
                    }
                }
                else
                {
                    afterDrillPath = currentTask.AfterDrillFilePath;
                }

                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_AUTO_CUTTER))
                {
                    //根据任务CODE, 调度此任务的配刀信息
                    //是否首次使用此配刀计划
                    //当前对应的配刀组计划No
                    var cutterGroupDetail = await _cutterGroupService.DetailByTaskCode(currentTask.Code);
                    isFirstCutter = cutterGroupDetail?.IsFirstCutter ?? false;
                    cutterGroupNo = cutterGroupDetail?.CutterGroupNo;
                }
            }
            else
            {
                if (!await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_TRANSFER_DRILL_FILE_ENABLE))
                {
                    _logger.LogInformation("GetDrillPath beforeDrillPath change, JingWangTransferDrillFileEnable config is false !");
                    return new Dictionary<string, object?>
                    {
                        {
                            "ErrorMsg","GetDrillPath beforeDrillPath change, JingWangTransferDrillFileEnable config is false !"
                        }
                    };
                }

                await _workOrderDomainService.UpdateAsync(p => new WorkOrder
                {
                    BeforeDrillFilePath = beforeDrillPath,
                    AfterDrillFilePath = "",
                    IsRebrush = 1,
                    ModifyTime = DateTime.Now,
                }, p => !string.IsNullOrEmpty(p.Code) && !string.IsNullOrEmpty(currentTask.WorkOrderCode)
                && p.Code.ToLower() == currentTask.WorkOrderCode.ToLower() && p.IsRebrush == 0);

                await _workOrderAlterLogService.Add(new AddOrUpdateWorkOrderAlterLogReq
                {
                    WorkOrderCode = currentTask.WorkOrderCode,
                    DeviceCode = deviceId,
                    ItemCode = itemCode,
                    ActionTime = DateTime.Now,
                    ActionDetail = $"Refresh WorkOrder {currentTask.WorkOrderCode} old BeforeDrillFilePath {currentTask.BeforeDrillFilePath}, new BeforeDrillFilePath {beforeDrillPath}. ",
                    BeforeDrillFilePath = currentTask.BeforeDrillFilePath,
                    AfterDrillFilePath = currentTask.AfterDrillFilePath
                });

                var taskCodes = taskResult.Data.List.Select(p => p.Code).ToList();
                await _taskDomainService.UpdateAsync(p => new WorkTask
                {
                    BeforeDrillFilePath = beforeDrillPath,
                    AfterDrillFilePath = "",
                    IsRebrush = 1,
                    ModifyTime = DateTime.Now,
                }, p => taskCodes.Contains(p.Code) && p.IsRebrush == 0);

                _logger.LogInformation($"GetDrillPath beforeDrillPath change, need refreshing afterDrillPath!");
                return new Dictionary<string, object?>
                {
                    {
                        "ErrorMsg","GetDrillPath beforeDrillPath change, need refreshing afterDrillPath!!"
                    }
                };
            }
        }
        return new Dictionary<string, object?>
        {
            {
                "AfterDrillPath", afterDrillPath
            },
            {
                "IsFirstCutter",isFirstCutter
            },
            {
                "CutterGroupNo",cutterGroupNo
            }
        };
    }
}
