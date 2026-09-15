using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalXianJin;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Reporter.Event;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VegaIot.External.XianJin;

/// <summary>
/// 先进 接口回调
/// </summary>
[ApiController]
[Route("v1/external/xianjin")]
public class XianJinController : Controller
{
    private readonly IDeviceManager _deviceManager;
    private readonly XianJinOptions _xianjinOptions;
    private readonly ILogger<XianJinController> _logger;
    private readonly ITaskDomainService _taskDomainService;
    private readonly IDeviceEventReporter _deviceEventHandler;

    public XianJinController(IDeviceManager deviceManager,
        IOptions<XianJinOptions> options,
        ITaskDomainService taskDomainService,
        IDeviceEventReporter deviceEventHandler,
        ILoggerFactory loggerFactory)
    {
        _deviceManager = deviceManager;
        _xianjinOptions = options.Value;
        _logger = loggerFactory.CreateLogger<XianJinController>();
        _taskDomainService = taskDomainService;
        _deviceEventHandler = deviceEventHandler;
    }

    [HttpPost]
    [Route("SetBeginLoadPanel")]
    public async Task<string> SetBeginLoadPanel([FromBody] SetDrillCommandReq req)
    {
        if (req == null || string.IsNullOrEmpty(req.DeviceId) || string.IsNullOrEmpty(req.TaskCode))
        {
            return "无法识别入参！";
        }

        var taskData = await _taskDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == req.TaskCode.ToLower());
        if (taskData == null)
        {
            return $"未找到任务{req.TaskCode}";
        }

        if (_deviceManager.TryGetOnlineDrill(req.DeviceId, out var drill)
            && drill != null)
        {
            if (!drill.Descriptor.Extra.ContainsKey("AgvOperationTypes"))
            {
                _logger.LogInformation("SetBeginLoadPanel drill属性不包含AgvOperationTypes");
            }
            else
            {
                _logger.LogInformation("SetBeginLoadPanel  AgvOperationTypes " + drill.Descriptor.Extra["AgvOperationTypes"].ToString());
            }
            if (drill.Descriptor.Extra.ContainsKey("AgvOperationTypes") && drill.Descriptor.Extra["AgvOperationTypes"].ToString() == "8")
            {
                await GenerateScheduleInfo(taskData, drill);

                drill.TaskCode = taskData.Code;
                drill.TaskBarCode = taskData.BarCode;
                drill.TaskItemCode = taskData.ItemCode;
                drill.TaskWadCount = (int)taskData.NowWadCount;

                var invokeRequst = new DeviceServiceInvokeRequest
                {
                    DeviceId = drill.DeviceId,
                    ProductId = drill.ProductId,
                    ClientId = drill.ClientId,
                    ServiceId = Topics.Services.PREPARE_LOAD_MATERIAL_SERVICE_ID,
                    ServiceName = Topics.Services.PREPARE_LOAD_MATERIAL_SERVICE_ID,
                    TargetProductId = drill.ProductId,
                    TargetDeviceId = drill.DeviceId,
                    TargetClientId = drill.ClientId,
                    Params = new Dictionary<string, object?>
                    {
                        {"Position", "1"}
                    }
                };

                var result = await drill.InvokeService(invokeRequst);
                _logger.LogInformation("SetBeginLoadPanel" + result.ToString());
            }
        }
        else
        {
            return $"未找到设备{req.DeviceId}";
        }

        return string.Empty;
    }

    [HttpGet]
    [Route("UnderClinkerPanel")]
    public async Task<string> UnderClinkerPanel(string deviceId)
    {
        if (_deviceManager.TryGetOnlineDevice<Drill>(deviceId, out var drill)
            && drill != null)
        {
            if (drill.Descriptor.Extra.ContainsKey("AgvOperationTypes") && drill.Descriptor.Extra["AgvOperationTypes"].ToString() == "8")
            {
                var invokeRequst = new DeviceServiceInvokeRequest
                {
                    DeviceId = drill.DeviceId,
                    ProductId = drill.ProductId,
                    ClientId = drill.ClientId,
                    ServiceId = Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID,
                    ServiceName = Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID,
                    TargetProductId = drill.ProductId,
                    TargetDeviceId = drill.DeviceId,
                    TargetClientId = drill.ClientId,
                    Params = new Dictionary<string, object?>
                    {
                        {"Position", "1"}
                    }
                };

                var result = await drill.InvokeService(invokeRequst);
                _logger.LogInformation("UnderClinkerPanel" + result.ToString());
            }
        }
        else
        {
            return $"未找到设备{deviceId}";
        }

        return string.Empty;
    }

    [HttpGet]
    [Route("SetCompleteUnderClinkerPanel")]
    public async Task<string> SetCompleteUnderClinkerPanel(string deviceId)
    {
        if (_deviceManager.TryGetOnlineDevice<Drill>(deviceId, out var drill)
            && drill != null)
        {
            if (drill.Descriptor.Extra.ContainsKey("AgvOperationTypes") && drill.Descriptor.Extra["AgvOperationTypes"].ToString() == "8")
            {
                var invokeRequst = new DeviceServiceInvokeRequest
                {
                    DeviceId = drill.DeviceId,
                    ProductId = drill.ProductId,
                    ClientId = drill.ClientId,
                    ServiceId = Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID,
                    ServiceName = Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID,
                    TargetProductId = drill.ProductId,
                    TargetDeviceId = drill.DeviceId,
                    TargetClientId = drill.ClientId,
                    Params = new Dictionary<string, object?>
                    {
                        {"Position", "1"}
                    }
                };

                var result = await drill.InvokeService(invokeRequst);
                _logger.LogInformation("SetCompleteUnderClinkerPanel" + result.ToString());
            }

            //Todo下料查询钻机是否真的下料结束
        }
        else
        {
            return $"未找到设备{deviceId}";
        }

        return string.Empty;
    }

    private async Task GenerateScheduleInfo(WorkTask taskData, Drill drill)
    {
        var tempTranscationId = Guid.NewGuid().ToString();
        var request = new DeviceEventReportRequest()
        {
            ProductId = drill.ProductId,
            DeviceId = drill.DeviceId,
            ClientId = "",
            EventId = "REQUEST_AGV_LOAD_PANEL_THEN_UNLOAD_PANEL",
            RequestInputProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1,
            RequestOutputProductStatus = ProductStatus.Finished_DRILL,
            RequestInteractionDirection = InteractionPosition.Rear,
            RequestInteractionBehavior = InteractionBehavior.Make(InteractionMode.Active, drill.DeviceKind,
            InteractionSequence.LoadOnly,
            InteractionPosition.Rear,
            MaterialKind.Panel),
            RequestDeviceKind = drill.DeviceKind,
            TraceId = tempTranscationId,
            Params = new Dictionary<string, object?>()
                         {
                                    { "TaskId", taskData.Code },
                                    { "TaskItemCode", taskData.ItemCode },
                                    { "TaskItemCount", taskData.NowWadCount },
                                    { "TaskRouteCode", taskData.RouteCode },
                                    { "SpindleNum", drill.Descriptor.SpindleNum },
                         },
            PayloadPanels = drill.PayloadPanels,
        };

        var result = await _deviceEventHandler.Report(request);
        _logger.LogInformation("GenerateScheduleInfo" + result.ToString());
    }
}
