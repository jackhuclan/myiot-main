using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Schedule.Deliver;

public abstract class PinAndUnpinBaseDeliverPolicy
{
    private readonly ILogger<PinAndUnpinBaseDeliverPolicy> _logger;
    protected readonly MysqlTaskSchedulerOptions _taskScheduleOptions;
    protected readonly IScheduleService _scheduleService;

    protected PinAndUnpinBaseDeliverPolicy(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<PanelSiloScheduleTaskDeliverPolicy>>();
        _taskScheduleOptions = (serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>()).Value;
        _scheduleService = serviceProvider.GetRequiredService<IScheduleService>();
    }

    protected async Task<ScheduleDto> GetMatchedSchedule(DeviceEventReportRequest eventRequest)
    {
        //标识为当前的AGV；
        //master schedule id, 标识为当前的schedule id；
        //不更改调度记录的状态，保持已上报。
        var req = new GetScheduleListReq
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus?> { ScheduledTaskStatus.Created },
            HasAgvSetted = false,
            IsAuxiliary = true,
            RequestDeviceKindList = new List<DeviceKind?> { DeviceKind.PublicPanelSiloWIP },
            PageSize = int.MaxValue,
        };

        var convertedInteractionBehavior = (InteractionBehavior)eventRequest.RequestInteractionBehavior;
        if (convertedInteractionBehavior.InteractionSequence == InteractionSequence.LoadOnly)
        {
            req.InteractionSequenceList = new() { InteractionSequence.UnloadOnly };
        }
        else if (convertedInteractionBehavior.InteractionSequence == InteractionSequence.UnloadOnly)
        {
            req.InteractionSequenceList = new() { InteractionSequence.LoadOnly };
        }
        else
        {
            _logger.LogError($"Wrong InteractionSequence {eventRequest.TraceId}");
            return null;
        }

        if (_taskScheduleOptions.EnableUsingForkForPinAndUnpinUnloading
            && convertedInteractionBehavior.InteractionSequence == InteractionSequence.UnloadOnly)
        {
            req.RequestDeviceKindList.Add(DeviceKind.PanelSiloFork);
        }
        if (eventRequest.RequestDeviceKind == DeviceKind.UnPin
            && convertedInteractionBehavior.InteractionSequence == InteractionSequence.UnloadOnly)
        {
            req.RequestDeviceKindList.Add(DeviceKind.Pin);
        }

        if (convertedInteractionBehavior.InteractionSequence == InteractionSequence.LoadOnly)
        {
            req.RequestDeviceKindList.Add(DeviceKind.PanelSiloFork);
        }

        if (convertedInteractionBehavior.InteractionSequence == InteractionSequence.LoadOnly
             && eventRequest.RequestDeviceKind == DeviceKind.Pin)
        {
            foreach (var schedule in (await _scheduleService.GetScheduleTasks(req)).Data.OrderBy(x => x.RequestDeviceKind))
            {
                var eventOptional = JsonSerializer.Deserialize<DeviceEventReportRequest>(schedule.RequestJson);
                if (eventOptional.PayloadPanels.IsEmptySiloBox)
                {
                    return schedule;
                }
            }
            return null;
        }
        else if (convertedInteractionBehavior.InteractionSequence == InteractionSequence.LoadOnly
             && eventRequest.RequestDeviceKind == DeviceKind.UnPin)
        {
            foreach (var schedule in (await _scheduleService.GetScheduleTasks(req)).Data.OrderBy(x => x.RequestDeviceKind))
            {
                var shelfRequest = JsonSerializer.Deserialize<DeviceEventReportRequest>(schedule.RequestJson);

                if (_taskScheduleOptions.EnableUnpinLoadRequestByItem
                    && eventRequest.Params.ContainsKey("UnpinItemCode")
                    && !string.IsNullOrEmpty(eventRequest.Params["UnpinItemCode"].ToStr()))
                {
                    var itemCode = eventRequest.Params["UnpinItemCode"].ToStr().ToLower();

                    if (shelfRequest.PayloadPanels.All(x => ((x.ProductStatus == ProductStatus.Finished_POST_BUFFER || x.ProductStatus == ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1)
                                            && x.ItemCode.ToLower() == itemCode) || x.ProductStatus == ProductStatus.EmptySiloBox)
                        && shelfRequest.PayloadPanels.Any(x => ((x.ProductStatus == ProductStatus.Finished_POST_BUFFER || x.ProductStatus == ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1)
                                            && x.ItemCode.ToLower() == itemCode)))
                    {
                        return schedule;
                    }
                }
                else
                {
                    if (shelfRequest.PayloadPanels.All(x => x.ProductStatus == ProductStatus.Finished_POST_BUFFER || x.ProductStatus == ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1 || x.ProductStatus == ProductStatus.EmptySiloBox)
                        && shelfRequest.PayloadPanels.Any(x => x.ProductStatus == ProductStatus.Finished_POST_BUFFER || x.ProductStatus == ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1))
                    {
                        return schedule;
                    }
                }
            }

            return null;
        }
        else
        {
            return (await _scheduleService.GetScheduleTasks(req)).Data.OrderBy(x => x.RequestDeviceKind).FirstOrDefault();
        }
    }
}
