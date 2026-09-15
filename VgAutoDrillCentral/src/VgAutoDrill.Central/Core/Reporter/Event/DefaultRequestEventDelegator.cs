using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Reporter.Event;

internal class DefaultRequestEventDelegator : IDeviceEventDelegator
{
    private readonly ILogger<DefaultRequestEventDelegator> _logger;
    private readonly IDeviceManager _deviceHolder;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITaskService _taskService;
    private readonly IItemAdapter _itemAdapter;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;

    public DefaultRequestEventDelegator(IDeviceManager deviceHolder,
        ISysConfigManager sysConfigManager,
        ITaskService taskService,
        IItemAdapter itemAdapter,
        IServiceProvider serviceProvider,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<DefaultRequestEventDelegator>();
        _deviceHolder = deviceHolder;
        _sysConfigManager = sysConfigManager;
        _taskService = taskService;
        _itemAdapter = itemAdapter;
        _taskScheduleOptions = serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>().Value;
    }

    public async Task<DeviceEventReportResponse> HandleEvent(DeviceEventReportRequest request)
    {
        if (!CentralFlags.SystemPreloadCompleted)
        {
            _logger.LogWarning($"系统已启用预加载模式，但是尚未加载完成，请等待.");
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"系统已启用预加载模式，但是尚未加载完成，请等待"
            };
        }

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"系统即将停机维护");
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"系统即将停机维护"
            };
        }

        //如果自带trace id，则应用设备自带的traceid
        var eventTraceId = request.TraceId;
        if (string.IsNullOrEmpty(eventTraceId))
        {
            eventTraceId = Guid.NewGuid().ToString();
        }

        //DRILL_REQUEST_RECIPE_EVENT 入参IncodeNumber，获取自动扫码对应的文件
        //CHECK_PROGRAM_AND_DIA
        //var str = Events.Drill.CHECK_PROGRAM_AND_DIA_EVENT; 验证是否能自动打板接口

        if (request.EventId == Events.Drill.DRILL_REQUEST_RECIPE_EVENT)
        {
            if (!request.Params.ContainsKey("IncodeNumber")
                && !request.Params.ContainsKey("ItemCode"))
            {
                return new DeviceEventReportResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"Device {request.DeviceId},Cannot find param IncodeNumber or ItemCode in request.Params.",
                    TraceId = eventTraceId,
                };
            }
            var incodeNumber = request.Params.ContainsKey("IncodeNumber") ? request.Params["IncodeNumber"].ToStr() : string.Empty;
            var itemCode = request.Params.ContainsKey("ItemCode") ? request.Params["ItemCode"].ToStr() : string.Empty;

            //todo,查询本地数据库中的物料的文件路径
            var DrlPath = string.Empty;
            var DiaPath = string.Empty;
            float PanelLength = 0;
            var specGroup = string.Empty;

            var item = await _itemAdapter.GetItemAsync(itemCode, incodeNumber);
            if (item != null)
            {
                PanelLength = item.PanelLength;
            }

            if (string.IsNullOrEmpty(DrlPath) && string.IsNullOrEmpty(DiaPath))
            {
                var drill = _deviceHolder.GetOnlineDevice(request.DeviceId);
                if (drill != null
                    && drill.Descriptor.Extra.ContainsKey("DrlSearchPath")
                    && drill.Descriptor.Extra.ContainsKey("DiaSearchPath"))
                {
                    DrlPath = drill.Descriptor.Extra["DrlSearchPath"].ToStr();
                    DiaPath = drill.Descriptor.Extra["DiaSearchPath"].ToStr();
                }
            }

            var tasks = await _taskService.GetTaskByDevice(new Admin.Model.ViewModels.Mes.Device.GetDrillOrAgvDeviceInfoReq
            {
                DeviceCode = request.DeviceId,
                ItemCode = itemCode,
                TaskStatusList = new List<TaskStatusEnum>
                    {
                        TaskStatusEnum.COMMITED,
                        TaskStatusEnum.SENDING,
                        TaskStatusEnum.BUFFERED,
                        //TaskStatusEnum.BEGIN,
                    }
            });
            if (tasks != null && tasks.Data != null && tasks.Data.List.Any())
            {
                var currentTask = tasks.Data.List.FirstOrDefault();
                if (currentTask != null)
                {
                    specGroup = currentTask.SpecGroup;
                }
            }

            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = string.Empty,
                ProductId = request.ProductId,
                DeviceId = request.DeviceId,
                EventId = request.EventId,
                TraceId = eventTraceId,
                Params = new Dictionary<string, object?>
                    {
                        {"DrlPath",DrlPath },
                        {"DiaPath",DiaPath },
                        { "PanelLength",PanelLength},
                        { "SpecGroup", specGroup},
                    }
            };
        }
        else if (request.EventId == Events.Drill.CHECK_PROGRAM_AND_DIA_EVENT)
        {
            //todo 检查能否自动打板
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = string.Empty,
                ProductId = request.ProductId,
                DeviceId = request.DeviceId,
                EventId = request.EventId,
                TraceId = eventTraceId,
                Data = request.Params
            };
        }

        return new DeviceEventReportResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty,
            ProductId = request.ProductId,
            DeviceId = request.DeviceId,
            EventId = request.EventId,
            TraceId = eventTraceId,
            Data = request.Params
        };
    }
}
