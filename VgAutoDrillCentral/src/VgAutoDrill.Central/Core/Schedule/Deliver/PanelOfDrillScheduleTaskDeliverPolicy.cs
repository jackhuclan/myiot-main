using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Schedule.Deliver;

internal class PanelOfDrillScheduleTaskDeliverPolicy : IScheduleTaskDeliverPolicy
{
    private readonly ILogger<PanelOfDrillScheduleTaskDeliverPolicy> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;

    public PanelOfDrillScheduleTaskDeliverPolicy(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<PanelOfDrillScheduleTaskDeliverPolicy>>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
    }

    public async Task<AgvAllocationResult> DeliverScheduleTask(DeliverScheduleTaskRequirement scheduleTaskRequirement)
    {
        DeviceProxy callerDevice = scheduleTaskRequirement.CallerDevice;
        var agvDevice = scheduleTaskRequirement.AgvDevice;
        DeviceEventReportRequest eventRequest = scheduleTaskRequirement.ScheduleTask.EventRequest;
        var scheduleTask = scheduleTaskRequirement.ScheduleTask;

        var eventTraceId = eventRequest.TraceId;
        _logger.LogWarning($"ScheduleDbCheck检查通过，开始分配,current eventTraceId:{eventTraceId},source device:{eventRequest.DeviceId}");

        if (!string.IsNullOrEmpty(agvDevice.TargetDevice) && agvDevice.TargetDevice.ToLower() != scheduleTaskRequirement.CallerDevice.DeviceId.ToLower())
        {
            var msg = $"当前AGV已有分配任务 已分配给:{agvDevice.TargetLocation},取消本次任务分配:{eventRequest.DeviceId}，等待下次分配";
            _logger.LogWarning(msg);
            //任务分发失败，释放AGV标志
            scheduleTaskRequirement.ScheduleTask.RealtimeScheduleLog = msg;
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
        }
        else
        {
            //任务开始分发，锁定AGV为已分配，禁止其他线程
            agvDevice.TargetDevice = callerDevice.Descriptor.DeviceId;
            agvDevice.TargetLocation = scheduleTaskRequirement.ScheduleTask.LocationCode;
        }

        var spindleUseNum = eventRequest.Params.ContainsKey("SpindleUseNum") ? eventRequest.Params["SpindleUseNum"].ToInt() : 0;
        var rawSpindleNum = eventRequest.Params.ContainsKey("RawSpindleNum") ? eventRequest.Params["RawSpindleNum"].ToInt() : 0;
        var originSpindles = eventRequest.Params.ContainsKey("Spindles") ? eventRequest.Params["Spindles"].ToStr() : "";
        var originBehavior = eventRequest.Params.ContainsKey("SpindleBehavior") ? eventRequest.Params["SpindleBehavior"].ToStr() : "";
        var tmpSpindles = originSpindles.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var tmpSpindleBehavior = originBehavior.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var taskCode = eventRequest.Params[ScheduleConstants.PARAMS_TASK_ID].ToStr();
        var rawItemCode = eventRequest.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE].ToStr().Trim();
        var taskItemCount = eventRequest.Params[ScheduleConstants.PARAMS_TASK_ITEM_COUNT].ToInt();
        var totalPlanRawCount = Math.Min(taskItemCount, spindleUseNum);
        eventRequest.Params["IsAllPanelSent"] = false;
        agvDevice.Properties["IsAllPanelSent"] = false;

        var agvRemainRawCount = agvDevice.PayloadPanels.Count(p => p.ProductStatus == eventRequest.RequestInputProductStatus && p.ItemCode.Trim().ToLower() == rawItemCode.ToLower());

        if (taskItemCount > 0 && taskItemCount < agvRemainRawCount)
        {
            agvRemainRawCount = taskItemCount;
        }
        agvDevice.Properties["agvRemainRawCount"] = agvRemainRawCount;
        agvDevice.Properties["taskItemCount"] = taskItemCount;

        agvDevice.Properties["changedSpindles"] = "";
        agvDevice.Properties["changedBehavior"] = "";
        eventRequest.Params["TotalPlanRawCount"] = totalPlanRawCount;

        var itemSummaryFromAgv = agvDevice.PayloadPanels
            .UndrilledPanels
            .GroupBy(x => x.ItemCode)
            .Select(x => new ItemSummary
            {
                ItemCode = x.Key,
                ItemCount = x.Key.ToLower() == rawItemCode.ToLower() ? x.Count() - totalPlanRawCount : x.Count()
            });
        agvDevice.Properties["ItemSummaryFromAgv"] = JsonSerializer.Serialize(itemSummaryFromAgv);

        eventRequest.Params[ScheduleConstants.PARAMS_PLAN_RAW_COUNT] = 0;

        _logger.LogDebug($"判断是否处理尾料 RawSpindleNum:{rawSpindleNum}, Spindles:{originSpindles},agvRemainRawCount：{agvRemainRawCount}，tmpSpindles.Count():{tmpSpindles.Count()},tmpSpindleBehavior.Count():{tmpSpindleBehavior.Count()} ");

        //钻机呼叫包含有上有下时， 比对钻机呼叫的下熟料，是否与agv现有的熟料料号是否一致？
        var drilledItemCode = string.Empty;
        if (scheduleTask.DrilledItemCodes.Any())
        {
            drilledItemCode = scheduleTask.DrilledItemCodes.FirstOrDefault();
        }

        //呼叫生料，并且有熟料时
        var changedToOnlyLoadUndrillItem = false;
        agvDevice.Properties["RemarkForOnlyLoadUndrillItem"] = "";
        var agvDrilledItemCode = "";
        if (!string.IsNullOrEmpty(taskCode) && !string.IsNullOrEmpty(drilledItemCode))
        {
            if (agvDevice.PayloadPanels.Any(p => ProductStatusConstants.Finished_DRILL.Contains(p.ProductStatus)
                    && !string.IsNullOrEmpty(p.ItemCode)
                    && p.ItemCode.ToLower() != drilledItemCode.ToLower()))
            {
                agvDrilledItemCode = agvDevice.PayloadPanels.FirstOrDefault(p => ProductStatusConstants.Finished_DRILL.Contains(p.ProductStatus)
                    && !string.IsNullOrEmpty(p.ItemCode)
                    && p.ItemCode.ToLower() != drilledItemCode.ToLower()).ItemCode;

                for (var i = 0; i < tmpSpindles.Count(); i++)
                {
                    if (tmpSpindleBehavior[i] == "2")
                    {
                        tmpSpindleBehavior[i] = "0";
                        changedToOnlyLoadUndrillItem = true;
                    }
                }
            }
        }
        if (changedToOnlyLoadUndrillItem)
        {
            agvDevice.Properties["RemarkForOnlyLoadUndrillItem"] = $"取消下熟料：drill:{drilledItemCode.ToUpper()},agv:{agvDrilledItemCode}";
        }

        //需要的生料数量大于1块 并且尾料不足时，才需要做尾料处理
        if (rawSpindleNum == 0
            || (agvRemainRawCount < rawSpindleNum
                && tmpSpindles.Count() == tmpSpindleBehavior.Count()))
        {
            for (var i = 0; i < tmpSpindles.Count(); i++)
            {
                if (tmpSpindleBehavior[i] == "0" || tmpSpindleBehavior[i] == "2")
                {
                    if (agvRemainRawCount <= 0)
                    {
                        if (tmpSpindleBehavior[i] == "0")
                        {
                            tmpSpindles[i] = "null";
                            tmpSpindleBehavior[i] = "-1";
                        }
                        else
                        {
                            tmpSpindleBehavior[i] = "1";
                        }
                    }
                    else
                    {
                        agvRemainRawCount--;
                    }
                }
            }
        }

        var agvHasDrilledPanels = agvDevice.PayloadPanels.DrilledItemCodes.Any();
        var drillHasDrilledPanels = eventRequest.PayloadPanels.DrilledItemCodes.Any();
        var agvDrilledPanlesIsNotRequiredByDrill = agvHasDrilledPanels && drillHasDrilledPanels
            && !agvDevice.PayloadPanels.DrilledItemCodes.Intersect(eventRequest.PayloadPanels.DrilledItemCodes).Any();

        //AGV剩余空层数不够，为仅上料; 或者agv没有钻机需要的熟料
        if (agvDevice.PayloadPanels.CountEmptySiloBoxPanels < tmpSpindleBehavior.Count(s => s == "1")
            || agvDrilledPanlesIsNotRequiredByDrill)
        {
            for (var i = 0; i < tmpSpindles.Count(); i++)
            {
                if (tmpSpindleBehavior[i] == "1")
                {
                    tmpSpindles[i] = "null";
                    tmpSpindleBehavior[i] = "-1";
                }
                else if (tmpSpindleBehavior[i] == "2")
                {
                    tmpSpindleBehavior[i] = "0";
                }
            }

            agvDevice.Properties["RemarkForOnlyLoadUndrillItem"] = $"AGV剩余空层数不够，取消下熟料：drill仅下料数量:{tmpSpindleBehavior.Count(s => s == "1")},agv空层数:{agvDevice.PayloadPanels.CountEmptySiloBoxPanels}";
        }

        //如果所有轴都没有任务，那么取消本次任务分配，等待下次分配；
        if (tmpSpindles.All(x => x == "null"))
        {
            _logger.LogWarning($"如果所有轴都没有任务，那么取消本次任务分配，等待下次分配");
            //任务分发失败，释放AGV标志
            agvDevice.TargetDevice = string.Empty;
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
        }

        //防止混收熟料，再次检查
        if (!string.IsNullOrEmpty(drilledItemCode)
            && agvDevice.PayloadPanels.DrilledItemCodes.Any(x => x.ToLower() != drilledItemCode.ToLower())
            && tmpSpindleBehavior.Any(x => x == "1" || x == "2"))
        {
            _logger.LogError($"识别到混收熟料，暂停本次分配--PanelOfDrillScheduleTaskDeliverPolicy");
            //任务分发失败，释放AGV标志
            agvDevice.TargetDevice = string.Empty;
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
        }

        var convertedBehavior = (InteractionBehavior)eventRequest.RequestInteractionBehavior;
        var convertedInteractionSequence = convertedBehavior.InteractionSequence;
        if (tmpSpindleBehavior.All(x => x == "-1" || x == "0") && tmpSpindleBehavior.Any(x => x == "0"))
        {
            _logger.LogWarning($"仅上料- agv {agvDevice.DeviceId},drill {callerDevice.DeviceId}");
            convertedInteractionSequence = InteractionSequence.LoadOnly;
            eventRequest.RequestInteractionBehavior = convertedBehavior.ChangeSequence(InteractionSequence.LoadOnly);
        }
        else if (tmpSpindleBehavior.All(x => x == "-1" || x == "1") && tmpSpindleBehavior.Any(x => x == "1"))
        {
            _logger.LogWarning($"仅下料-agv {agvDevice.DeviceId},drill {callerDevice.DeviceId}");
            convertedInteractionSequence = InteractionSequence.UnloadOnly;
            eventRequest.RequestInteractionBehavior = convertedBehavior.ChangeSequence(InteractionSequence.UnloadOnly);
        }

        agvDevice.Properties["RequestInteractionBehavior"] = eventRequest.RequestInteractionBehavior;
        eventRequest.Params["RequestInteractionBehavior"] = eventRequest.RequestInteractionBehavior;

        var changedSpindles = string.Join(",", tmpSpindles);
        var changedBehavior = string.Join(",", tmpSpindleBehavior);
        eventRequest.Params["Spindles"] = changedSpindles;
        eventRequest.Params["SpindleBehavior"] = changedBehavior;
        if (originBehavior != changedBehavior)
        {
            agvDevice.Properties["changedSpindles"] = changedSpindles;
            agvDevice.Properties["changedBehavior"] = changedBehavior;
            _logger.LogWarning($"需要的生料数量大于1块 并且尾料不足时，才需要做尾料处理 {originSpindles}==>{changedSpindles},{originBehavior}==>{changedBehavior}");
        }

        //仅下料时，设置本次调度为 已全部下发
        if (convertedInteractionSequence == InteractionSequence.UnloadOnly)
        {
            eventRequest.Params["IsAllPanelSent"] = true;
            eventRequest.Params[ScheduleConstants.PARAMS_TASK_ID] = string.Empty;
            eventRequest.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = string.Empty;

            agvDevice.Properties["IsAllPanelSent"] = true;
            agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ID] = string.Empty;
            agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = string.Empty;
        }
        else
        {
            //如果任何一轴上料时，才关联生产任务; 否则不关联生产任务
            if (tmpSpindleBehavior.Any(x => x == "0" || x == "2"))
            {
                agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ID] = taskCode;
                agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = rawItemCode.Trim();

                //统计经过尾料处理后，计划下发的生料数量PlanRawCount
                var planRawCount = tmpSpindleBehavior.Count(x => x == "0" || x == "2");
                eventRequest.Params[ScheduleConstants.PARAMS_PLAN_RAW_COUNT] = planRawCount;
                //判断生料，是否是不完全的 一次上料
                var IsAllPanelSent = planRawCount + eventRequest.Params["ExistRawNum"].ToInt() >= totalPlanRawCount;
                eventRequest.Params["IsAllPanelSent"] = IsAllPanelSent;
                agvDevice.Properties["IsAllPanelSent"] = IsAllPanelSent;
            }
        }

        var serviceRequest = new DeviceServiceInvokeRequest
        {
            ProductId = eventRequest.ProductId,
            DeviceId = eventRequest.DeviceId,
            ClientId = eventRequest.ClientId,
            ServiceId = Topics.Services.SCHEDULE_TASK_SERVICE_ID,
            EventId = eventRequest.EventId,
            EventName = eventRequest.EventName,
            HostAddress = callerDevice.Descriptor.HostAddress,
            CallerRequestInteractionDirection = eventRequest.RequestInteractionDirection,
            CallerRequestMaterialKind = eventRequest.RequestMaterialKind,
            CallerRequestInteractionBehavior = eventRequest.RequestInteractionBehavior,
            CallerRequestInputProductStatus = eventRequest.RequestInputProductStatus,
            CallerRequestOutputProductStatus = eventRequest.RequestOutputProductStatus,
            //CallerInputCapabilities = callerDevice.Descriptor.InputCapabilities,
            //CallerOutputCapabilities = callerDevice.Descriptor.OutputCapabilities,
            TargetProductId = agvDevice.Descriptor.ProductId,
            TargetDeviceId = agvDevice.Descriptor.DeviceId,
            TargetClientId = agvDevice.ClientId,
            TargetHostAddress = agvDevice.Descriptor.HostAddress,
            Params = eventRequest.Params,
            PayloadPanels = eventRequest.PayloadPanels,
            LocationCode = eventRequest.LocationCode,
        };
        serviceRequest.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME] = DateTime.MinValue;
        serviceRequest.Params[ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID] = eventTraceId;
        agvDevice.Properties["AgvDeviceId"] = agvDevice.DeviceId;

        _logger.LogInformation($"device {agvDevice.Descriptor.ProductId}-{agvDevice.Descriptor.DeviceId}- begin to call InvokeService,");
        var dbResponse = await _scheduleTaskManager.AllocateDrillScheduleTask(scheduleTask, agvDevice, convertedInteractionSequence);
        if (string.IsNullOrEmpty(dbResponse))
        {
            var agvResponse = await agvDevice.InvokeService(serviceRequest);

            if (agvResponse != null && agvResponse.Code == ErrorCodes.Sys.SUCCESS)
            {
                var msg = $"{DateTime.Now.ToLongTimeString()} {eventRequest.ProductId}-{eventRequest.DeviceId} AGV调用请求 已下发给 {agvDevice.Descriptor.DeviceId}！";
                _logger.LogInformation(msg);
                scheduleTaskRequirement.ScheduleTask.RealtimeScheduleLog = msg;

                return new AgvAllocationResult(AgvAllocationResultCode.Success, AgvAllocationFailedReason.Completed);
            }
            else
            {
                var msg = $"mqtt deliver task failed: agv-{agvDevice.Descriptor.DeviceId}[{agvDevice.Status.ToString()}] for {scheduleTaskRequirement.CallerDevice.DeviceId}[{scheduleTaskRequirement.CallerDevice.Status.ToString()}]--下发任务失败";
                scheduleTaskRequirement.ScheduleTask.RealtimeScheduleLog = msg;
                _logger.LogWarning(msg);
                agvDevice.PreBookedInfo = msg;
                _ = _scheduleTaskManager.AddLog(scheduleTaskRequirement.ScheduleTask.Id, msg);

                var responseMsg = await _scheduleTaskManager.CancelSingleSchedule(scheduleTaskRequirement.ScheduleTask.Code, true, msg);
                _logger.LogWarning($"取消调度：{responseMsg}");
                _ = _scheduleTaskManager.AddLog(scheduleTaskRequirement.ScheduleTask.Id, $"取消调度：{responseMsg}");
            }
        }
        else
        {
            var msg = $"分配任务，保存数据时失败: {agvDevice.Descriptor.DeviceId} to {scheduleTaskRequirement.CallerDevice}--下发任务失败";
            scheduleTaskRequirement.ScheduleTask.RealtimeScheduleLog = msg;
            _logger.LogWarning(msg);
            _ = _scheduleTaskManager.AddLog(scheduleTaskRequirement.ScheduleTask.Id, msg);
            agvDevice.PreBookedInfo = msg;
        }

        //任务分发失败，释放AGV标志
        agvDevice.TargetDevice = string.Empty;
        agvDevice.Properties["changedSpindles"] = string.Empty;
        agvDevice.Properties["changedBehavior"] = string.Empty;
        agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ID] = string.Empty;
        agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = string.Empty;
        agvDevice.Properties["RequestInteractionBehavior"] = InteractionBehavior.Noop;

        return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
    }
}
