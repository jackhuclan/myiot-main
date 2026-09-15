using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VegaIot.External.StdAgv.Handler.MointorAgvTask;

public class MointorTaskSucessHandler
{
    private readonly ILogger<MointorTaskSucessHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IItemAdapter _itemAdapter;

    public MointorTaskSucessHandler(ILogger<MointorTaskSucessHandler> logger,
        IItemAdapter itemAdapter,
        IScheduleTaskManager scheduleTaskManager,
        ITransferPlanManager transferPlanManager)
    {
        _logger = logger;
        _itemAdapter = itemAdapter;
        _scheduleTaskManager = scheduleTaskManager;
        _transferPlanManager = transferPlanManager;
    }

    public async Task<StdArrivedResponseEntityV2> Handle(TransferJob agvTask,
        Location startLocation,
        Location endLocation,
        DeviceProxy startDevice,
        DeviceProxy endDevice,
        QueryTaskStatusData hikAgvData)
    {
        var response = new StdArrivedResponseEntityV2();
        var payloadPanels = new PanelList();

        _logger.LogInformation($"StdAgvTaskMonitor:Excute MointorTaskSucessHandler... first schedule...agvTask:{agvTask.Id},startSchedule:{agvTask.StartScheduleId}");

        //<---执行第一段任务---->

        if (agvTask.StartScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startSchedule)
            && startSchedule != null
            && startSchedule.Code != null
            && startSchedule.EventRequest != null)
        {
            startSchedule.AllocatedAgv = hikAgvData.agvCode ?? "";

            var result = await _scheduleTaskManager.SetScheduleAsFinished(new CompleteScheduleTaskRequest
            {
                ProductId = startDevice.ProductId,
                DeviceId = startDevice.DeviceId,
                TraceId = startSchedule.Code,
                Params = new Dictionary<string, object?>
                {
                    { "routingKey", startSchedule.RoutingKey },
                    { "AllocatedAgv", startSchedule.AllocatedAgv }
                }
            }, null);
        }

        await Task.Delay(500);

        await startDevice.InvokeService(new DeviceServiceInvokeRequest
        {
            ProductId = startDevice.ProductId,
            DeviceId = startDevice.DeviceId,
            ClientId = startDevice.ClientId,
            HostAddress = startDevice.Descriptor.HostAddress,
            ServiceId = Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID,
            ServiceName = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
            TargetProductId = startDevice.ProductId,
            TargetDeviceId = startDevice.DeviceId,
            TargetClientId = startDevice.ClientId,
            TargetHostAddress = startDevice.Descriptor.HostAddress,
            Params = new Dictionary<string, object?>
            {
                { "ShelfIndex",startLocation.Index}
            }
        });

        _logger.LogInformation($"StdAgvTaskMonitor:Excute MointorTaskSucessHandler... second schedule...agvTask:{agvTask.Id},startSchedule:{agvTask.EndScheduleId}");

        //<---执行第二段任务---->

        if (agvTask.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && endSchedule.Code != null
            && endSchedule.EventRequest != null)
        {
            endSchedule.AllocatedAgv = hikAgvData.agvCode ?? "";

            var result = await _scheduleTaskManager.SetScheduleAsFinished(new CompleteScheduleTaskRequest
            {
                ProductId = endDevice.ProductId,
                DeviceId = endDevice.DeviceId,
                TraceId = endSchedule.Code,
                Params = new Dictionary<string, object?>
                {
                    { "routingKey", endSchedule.RoutingKey },
                    { "AllocatedAgv", endSchedule.AllocatedAgv }
                }
            }, null);

            await Task.Delay(500);

            if (agvTask.StartScheduleId.HasValue
                && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startedSchedule)
                && startedSchedule != null
                && startedSchedule.EventRequest != null)
            {
                var panels = startedSchedule.EventRequest.PayloadPanels;
                if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_PIN_TO_FORK)
                {
                    await _itemAdapter.LoadPanelInfo(panels);
                    panels.UpdatePanelInfo(ProductStatus.PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_1);
                }

                await endDevice.InvokeService(new DeviceServiceInvokeRequest
                {
                    ProductId = endDevice.ProductId,
                    DeviceId = endDevice.DeviceId,
                    ClientId = endDevice.ClientId,
                    HostAddress = endDevice.Descriptor.HostAddress,
                    ServiceId = Topics.Services.COMPLETE_LOAD_MATERIAL_SERVICE_ID,
                    ServiceName = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
                    TargetProductId = endDevice.ProductId,
                    TargetDeviceId = endDevice.DeviceId,
                    TargetClientId = endDevice.ClientId,
                    TargetHostAddress = endDevice.Descriptor.HostAddress,
                    Params = new Dictionary<string, object?>
                    {
                        { "LoadingPanel", new { PanelList = panels}},
                        { "ShelfIndex",endLocation.Index}
                    },
                });
            }
        }

        agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
        agvTask.AllocatedAgv = hikAgvData.agvCode ?? "";
        await _transferPlanManager.TryUpdateTransferJob(agvTask);

        return response;
    }
}
