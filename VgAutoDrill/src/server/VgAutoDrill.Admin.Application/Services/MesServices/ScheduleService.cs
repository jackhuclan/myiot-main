using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceServiceInvocation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ScheduleHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SchedulementDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    ///
    /// </summary>
    public class ScheduleService : BaseServiceWithoutTree<Schedule, ScheduleDto, AddOrUpdateScheduleReq>, IScheduleService
    {
        private readonly ITaskService taskService;
        private readonly IRouteService _routeService;
        private readonly ISysConfigManager sysConfigManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDeviceServiceInvocationService deviceServiceInvocationService;
        private readonly ILogger<ScheduleService> logger;
        private readonly IScheduleLogDomainService scheduleLogDomainService;
        private readonly IScheduleDomainService _scheduleDomainService;
        private readonly IScheduleHistoryDomainService _scheduleHistoryDomainService;
        private readonly IConfiguration _configuration;
        private readonly ITransportationTaskDomainService _transportationTaskDomainService;

        private readonly List<ScheduledTaskStatus> _newDrillScheduleStatus = new List<ScheduledTaskStatus>
        {
            ScheduledTaskStatus.Created,
            ScheduledTaskStatus.PartCompleted
        };

        private readonly List<ScheduledTaskStatus?> _noCancelStatus = new List<ScheduledTaskStatus?>
        {
            ScheduledTaskStatus.Failed,
            ScheduledTaskStatus.Completed,
            ScheduledTaskStatus.Canceled,
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        /// <param name="redisClient"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="scheduleLogDomainService"></param>
        public ScheduleService(IScheduleDomainService domainService,
            IScheduleHistoryDomainService scheduleHistoryDomainService,
            ITaskService taskService,
            IMapper mapper,
            ISysConfigManager sysConfigManager,
            IUnitOfWork unitOfWork,
            ILoggerFactory loggerFactory,
            IRouteService routeService,
            IDeviceServiceInvocationService deviceServiceInvocationService,
            IScheduleLogDomainService scheduleLogDomainService,
            IConfiguration configuration,
            ITransportationTaskDomainService transportationTaskDomainService)
            : base(domainService, mapper)
        {
            this.taskService = taskService;
            this.sysConfigManager = sysConfigManager;
            this.unitOfWork = unitOfWork;
            _routeService = routeService;
            _scheduleHistoryDomainService = scheduleHistoryDomainService;
            this.deviceServiceInvocationService = deviceServiceInvocationService;
            this.logger = loggerFactory.CreateLogger<ScheduleService>();
            this.scheduleLogDomainService = scheduleLogDomainService;
            this._scheduleDomainService = domainService;
            this._configuration = configuration;
            _transportationTaskDomainService = transportationTaskDomainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ScheduleDto>>> GetList(GetScheduleListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _scheduleDomainService.GetList(req);
            if (result.List.Count > 0)
            {
                var routeAndPartitionSettings = await _routeService.GetRouteAndPartitionSetting();
                if (routeAndPartitionSettings?.Count > 0)
                {
                    foreach (var item in result.List)
                    {
                        var routeAndPartitionSetting = routeAndPartitionSettings.FirstOrDefault(s => s.RouteCode == item.RouteCode);
                        item.WareHouseCode = routeAndPartitionSetting?.PartitionCode;
                    }
                }
            }
            return Success(result);
        }

        public async Task<ResponseDto<PageDto<ScheduleInfo>>> GetFullDataList(GetScheduleListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _scheduleDomainService.GetFullDataList(req);
            return Success(result);
        }

        /// <summary>
        /// -1 -- 异常中止
        /// -2 -- 取消（排队的任务，可以被取消）
        /// 0  -- 默认--未设置
        /// 1 -- 已上报
        /// 2 -- 已分配
        /// 3 -- 开始调度
        /// 4 -- 调度完成
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateCentralTask(AddOrUpdateScheduleReq req)
        {
            // 未提供status值时，退出方法
            if (req.ScheduledTaskStatus == ScheduledTaskStatus.None)
            {
                logger.LogError($"UpdateCentralTask, code:{req.Code},未提供status值时，退出方法");
                return Fail($"UpdateCentralTask, code:{req.Code},未提供status值时，退出方法");
            }

            //防止FindSingleAsync异常
            if (!await _domainService.IsExistAsync(x => x.Code == req.Code))
            {
                logger.LogError($"UpdateCentralTask, code:{req.Code},不存在该项");
                return Fail($"UpdateCentralTask, code:{req.Code},不存在该项");
            }

            var schedule = await _domainService.FindSingleAsync(x => x.Code == req.Code);
            if (schedule == null)
            {
                logger.LogError($"UpdateCentralTask, code:{req.Code},不存在该项");
                return Fail($"UpdateCentralTask, code:{req.Code},不存在该项");
            }

            if (req.ScheduledTaskStatus == ScheduledTaskStatus.Allocated)
            {
                if (schedule.ScheduledTaskStatus != ScheduledTaskStatus.Created && schedule.ScheduledTaskStatus != ScheduledTaskStatus.PartCompleted)
                {
                    logger.LogWarning($"UpdateCentralTask, code:{req.Code},状态变更非法,old:{schedule.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                    return Fail($"UpdateCentralTask, code:{req.Code},状态变更非法,old:{schedule.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                }

                if (!string.IsNullOrEmpty(req.ChangedSpindles))
                {
                    schedule.ChangedBehavior = req.ChangedBehavior;
                    schedule.ChangedSpindles = req.ChangedSpindles;
                }

                if (!string.IsNullOrEmpty(req.Remark))
                {
                    schedule.Remark = $"{schedule.Remark},{Environment.NewLine}{req.Remark}";
                }

                schedule.TaskId = req.TaskId;
                schedule.ItemCode = req.ItemCode;

                if (!string.IsNullOrEmpty(req.RequireDeviceId))
                {
                    schedule.RequireDeviceId = req.RequireDeviceId;
                }

                if (req.RequestInteractionBehavior > 0)
                {
                    schedule.RequestInteractionBehavior = req.RequestInteractionBehavior;
                }
                if (req.InteractionSequence.HasValue)
                {
                    schedule.InteractionSequence = req.InteractionSequence.Value;
                }

                if (req.IsMaster == true)
                {
                    schedule.IsMaster = true;
                }
                if (req.IsUrgent.HasValue)
                {
                    schedule.IsUrgent = req.IsUrgent;
                }

                schedule.IsAllPanelSent = req.IsAllPanelSent;
                schedule.AllocateTime = DateTime.Now;
                schedule.AGVPayloadPanels = req.AGVPayloadPanels;
                if (!string.IsNullOrEmpty(req.RequestInteractionBehaviorName))
                {
                    schedule.RequestInteractionBehaviorName = $"{schedule.RequestInteractionBehaviorName},{req.RequestInteractionBehaviorName}";
                }
            }
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.Running) //AGV 开始送料
            {
                if (!string.IsNullOrEmpty(req.RequestInteractionBehaviorName))
                {
                    schedule.RequestInteractionBehaviorName = $"{schedule.RequestInteractionBehaviorName},{req.RequestInteractionBehaviorName}";
                }
                schedule.RunningTime = DateTime.Now;
            }
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.Completed) //AGV 配送完成
            {
                if (schedule.ScheduledTaskStatus == ScheduledTaskStatus.Failed
                    || schedule.ScheduledTaskStatus == ScheduledTaskStatus.Canceled
                    || schedule.ScheduledTaskStatus == ScheduledTaskStatus.Completed)
                {
                    logger.LogWarning($"UpdateCentralTask, code:{req.Code},状态变更非法-重复报告异常,old:{schedule.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                    return Fail($"UpdateCentralTask, code:{req.Code},状态变更非法-重复报告异常,old:{schedule.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                }
                schedule.CompletedTime = DateTime.Now;
                schedule.TotalRawCount = req.TotalRawCount;
            }
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted) //AGV 调度部分完成
            {
                schedule.ModifyTime = DateTime.Now;
                schedule.TotalRawCount = req.TotalRawCount;
            }
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.Failed) //重复报告异常时
            {
                if (schedule.ScheduledTaskStatus == ScheduledTaskStatus.Failed
                    || schedule.ScheduledTaskStatus == ScheduledTaskStatus.Canceled
                    || schedule.ScheduledTaskStatus == ScheduledTaskStatus.Completed)
                {
                    logger.LogWarning($"UpdateCentralTask, code:{req.Code},状态变更非法-重复报告异常,old:{schedule.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                    return Fail($"UpdateCentralTask, code:{req.Code},状态变更非法-重复报告异常,old:{schedule.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                }

                schedule.FailedTime = DateTime.Now;
                if (!string.IsNullOrEmpty(req.WarningCode))
                {
                    schedule.WarningCode = req.WarningCode;
                }

                if (!string.IsNullOrEmpty(req.WarningMessage))
                {
                    schedule.WarningMessage = req.WarningMessage.Length > 1000 ? req.WarningMessage.Substring(0, 1000) : req.WarningMessage;
                }
                schedule.TotalRawCount = req.TotalRawCount;
            }
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.Canceled) //取消
            {
                if (schedule.ScheduledTaskStatus == ScheduledTaskStatus.Failed
                    || schedule.ScheduledTaskStatus == ScheduledTaskStatus.Canceled
                    || schedule.ScheduledTaskStatus == ScheduledTaskStatus.Completed)
                {
                    logger.LogWarning($"UpdateCentralTask, code:{req.Code},状态变更非法-重复报告异常,old:{schedule.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                    return Fail($"UpdateCentralTask, code:{req.Code},状态变更非法-重复报告异常,old:{schedule.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                }

                //redisClient.ResetRoutingKey(schedule.RoutingKey);
                schedule.CanceledTime = DateTime.Now;
                logger.LogWarning($"UpdateCentralTask, code:{req.Code},取消调度计划,old:{schedule.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
            }

            schedule.ScheduledTaskStatus = req.ScheduledTaskStatus;
            schedule.ModifyTime = DateTime.Now;
            await _domainService.Update(schedule);

            if (req.ScheduledTaskStatus == ScheduledTaskStatus.Canceled)
            {
                if (schedule.ScheduledTaskStatus == ScheduledTaskStatus.Allocated
                    && !string.IsNullOrEmpty(req.RequireDeviceId)
                    && !string.IsNullOrEmpty(schedule.TaskId))
                {
                    //设置生产任务 Sending配送中
                    var response = await taskService.UpdateByOutSide(new UpdateTaskByOutSideReq { Code = schedule.TaskId, TaskStatus = TaskStatusEnum.COMMITED });
                    logger.LogWarning($"调度已取消，设置生产任务为，已提交, {schedule.TaskId},{TaskStatusEnum.COMMITED}, response is {response.Code}，{response.Message}.");
                }

                await UpdateTransTask(
                    new List<long> { schedule.Id },
                    !string.IsNullOrEmpty(schedule.SubDeviceCode) ? new List<string> { schedule.SubDeviceCode.ToLower() } : new List<string>());

                //添加后台处理任务
                _ = AddDeviceServiceInvocation(schedule);
            }
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.Allocated
                && !string.IsNullOrEmpty(req.RequireDeviceId)
                && !string.IsNullOrEmpty(schedule.TaskId)) //已分配车辆
            {
                //设置生产任务 Sending配送中
                var response = await taskService.UpdateByOutSide(new UpdateTaskByOutSideReq { Code = schedule.TaskId, TaskStatus = TaskStatusEnum.SENDING });
                logger.LogWarning($"设置生产任务 Sending配送中, {schedule.TaskId},{TaskStatusEnum.SENDING}, response is {response.Code}，{response.Message}.");
            }
            //AGV 配送完成 ,并且已经标识全部宋辽完成，并且有关联到生料的生产任务
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.Completed
                && schedule.IsAllPanelSent
                && !string.IsNullOrEmpty(schedule.TaskId))
            {
                //更新生产任务的状态
                var changedTaskStatus = TaskStatusEnum.BUFFERED;

                var response = await taskService.UpdateByOutSide(new UpdateTaskByOutSideReq { Code = schedule.TaskId, TaskStatus = changedTaskStatus });
                logger.LogWarning($"设置生产任务 状态, {schedule.TaskId},{changedTaskStatus}, response is {response.Code}，{response.Message}.");
            }

            return Success();
        }

        /// <summary>
        /// 清除过期无效的任务: 上报已超时5分钟、或者分配后5分钟未继续执行的
        /// 清除redis中的无效锁定标识：设备在线 & 设备没有在途的调度任务 & 创建时间超过5分钟
        /// </summary>
        /// <returns></returns>
        public async Task<int> ClearTimeoutSchedule()
        {
            int defaultCreatedScheduleTimeout = 5;
            var valueCreatedTimeout = await sysConfigManager.GetIntValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_CREATED_TIMEOUT);
            if (valueCreatedTimeout > defaultCreatedScheduleTimeout)
            {
                defaultCreatedScheduleTimeout = valueCreatedTimeout;
            }

            var affectedRows = 0;
            //设备在线，但是所有任务都已经结束（或取消、异常中止）时，清除redis标识
            var inWorkingStatus = new List<ScheduledTaskStatus?> { ScheduledTaskStatus.PartCompleted, ScheduledTaskStatus.Created, ScheduledTaskStatus.Allocated, ScheduledTaskStatus.Running };
            var newStatus = new List<ScheduledTaskStatus?> { ScheduledTaskStatus.Created, ScheduledTaskStatus.PartCompleted };

            List<string> clearedKeys = new List<string>();
            //清除过期无效的任务: 上报已超时5分钟
            var timeoutScheduleList = await _domainService.QueryAsync(x => (newStatus.Contains(x.ScheduledTaskStatus)
                                                    && DateTime.Now.AddSeconds(-1 * defaultCreatedScheduleTimeout) > x.CreateTime
                                                    && string.IsNullOrEmpty(x.RequireDeviceId)),
                                                x => x.SourceDeviceId,
                                                OrderByType.Asc);

            if (timeoutScheduleList.Count > 0)
            {
                foreach (var schedule in timeoutScheduleList)
                {
                    logger.LogWarning($"CheckSchedule, 已自动取消超时未响应的历史任务，更早一些的调度任务 code:{schedule.Code},device:{schedule.SourceDeviceId}");
                    schedule.ScheduledTaskStatus = ScheduledTaskStatus.Canceled;
                    schedule.CanceledTime = DateTime.Now;
                    schedule.Remark = $"{schedule.Remark}-未分配，超过限定时间，已自动取消";

                    await _domainService.Update(schedule);
                    affectedRows++;

                    var routingKey = schedule.RoutingKey;
                    if (await _domainService.IsExistAsync(x => x.RoutingKey == routingKey && inWorkingStatus.Contains(x.ScheduledTaskStatus)))
                    {
                        logger.LogDebug($"CheckSchedule, routingKey：{routingKey} 有在途的调度记录，暂不清除redis标识");
                    }
                    else
                    {
                        //添加后台处理任务
                        _ = AddDeviceServiceInvocation(new List<Schedule> { schedule });
                    }
                }
            }

            logger.LogDebug($"ClearTimeoutSchedule affectedRows:{affectedRows}, clearedKeys:{string.Join(',', clearedKeys)}");
            return affectedRows;
        }

        /// <summary>
        /// 取消已分配的、正在执行的调度记录
        /// </summary>
        /// <returns></returns>
        public async Task<int> CancelExpireSchedule()
        {
            int defaultloadScheduleTaskDays = 1;
            var loadScheduleTaskDays = await sysConfigManager.GetIntValue(MESConfigConstants.LOAD_SCHEDULE_TASK_SETTINGSDAYS_DATA);
            if (loadScheduleTaskDays > defaultloadScheduleTaskDays)
            {
                defaultloadScheduleTaskDays = loadScheduleTaskDays;
            }

            var affectedRows = 0;
            var inWorkingStatus = new List<ScheduledTaskStatus?> { ScheduledTaskStatus.PartCompleted, ScheduledTaskStatus.Created, ScheduledTaskStatus.Allocated, ScheduledTaskStatus.Running };
            //var newStatus = new List<ScheduledTaskStatus?> { ScheduledTaskStatus.Allocated, ScheduledTaskStatus.Running };
            List<string> clearedKeys = new List<string>();

            //取消[系统配置]天数之前的在途调度记录（已分配的、正在运行）
            var cancelScheduleList = await _domainService.QueryAsync(x => inWorkingStatus.Contains(x.ScheduledTaskStatus) &&
                                                                        DateTime.Now.AddDays(-1 * defaultloadScheduleTaskDays) > x.CreateTime &&
                                                                        string.IsNullOrEmpty(x.RequireDeviceId),
                                                                        x => x.SourceDeviceId,
                                                                        OrderByType.Asc);
            if (cancelScheduleList.Count > 0)
            {
                foreach (var schedule in cancelScheduleList)
                {
                    logger.LogWarning($"CheckSchedule, 已自动取消超时未响应的历史任务，更早一些的调度任务 code:{schedule.Code},device:{schedule.SourceDeviceId}");
                    schedule.ScheduledTaskStatus = ScheduledTaskStatus.Canceled;
                    schedule.CanceledTime = DateTime.Now;
                    schedule.Remark = $"{schedule.Remark}-未分配，超过限定天数，已自动取消";

                    await _domainService.Update(schedule);
                    affectedRows++;

                    var routingKey = schedule.RoutingKey;
                    //添加后台处理任务
                    _ = AddDeviceServiceInvocation(new List<Schedule> { schedule });
                }
            }

            logger.LogDebug($"CancelExpireSchedule affectedRows:{affectedRows}, clearedKeys:{string.Join(',', clearedKeys)}");
            return affectedRows;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateScheduleReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<Schedule>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);

            return Success();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateScheduleReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code && p.Id != req.Id);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<Schedule>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        public async Task<string> CancelSingle(string traceId, bool isForced = false, string reason = "")
        {
            var schedule = await FindScheduleByTraceId(traceId);
            if (schedule == null)
            {
                return $"Cannot find a schecule by the trace Id:{traceId}";
            }

            if (isForced == false
                && schedule.ScheduledTaskStatus == ScheduledTaskStatus.Created
                && (schedule.RequestDeviceKind == DeviceKind.PanelSiloFork || schedule.RequestDeviceKind == DeviceKind.PublicPanelSiloWIP)
                && schedule.IsAuxiliary == false)
            {
                logger.LogWarning($"CancelSingle cannot cancel a schedule when IsAuxiliary is false.");
                return $"不能取消料架或者插齿的主叫指令. trace Id:{traceId}，{schedule.RequestInteractionBehaviorName}";
            }

            await BulkCanceled(new List<long> { schedule.Id }, reason);

            return string.Empty;
        }

        public async Task<string> CancelDeviceSingle(string traceId, string reason = "")
        {
            var schedule = await FindScheduleByTraceId(traceId);
            if (schedule == null)
            {
                return $"Cannot find a schecule by the trace Id:{traceId}";
            }

            await BulkCanceled(new List<long> { schedule.Id }, reason);

            return string.Empty;
        }

        public async Task<string> FindSingleBySubDeviceCode(string subDeviceCode)
        {
            if (await _scheduleDomainService.IsExistAsync(s => !string.IsNullOrEmpty(subDeviceCode)
                    && !string.IsNullOrEmpty(s.SubDeviceCode)
                    && s.SubDeviceCode.ToLower() == subDeviceCode.ToLower()
                    && s.ScheduledTaskStatus == ScheduledTaskStatus.Created))
            {
                var schedule = await _scheduleDomainService.FindSingleAsync(s => !string.IsNullOrEmpty(subDeviceCode)
                        && !string.IsNullOrEmpty(s.SubDeviceCode)
                        && s.SubDeviceCode.ToLower() == subDeviceCode.ToLower()
                        && s.ScheduledTaskStatus == ScheduledTaskStatus.Created);

                return schedule.Code;
            }
            return string.Empty;
        }

        public async Task<string> FindSingleByDrillDeviceCode(string drillDeviceCode)
        {
            if (await _scheduleDomainService.IsExistAsync(s => !string.IsNullOrEmpty(drillDeviceCode)
                && !string.IsNullOrEmpty(s.SourceDeviceId)
                && s.SourceDeviceId.ToLower() == drillDeviceCode.ToLower()
                && _newDrillScheduleStatus.Contains((ScheduledTaskStatus)s.ScheduledTaskStatus)))
            {
                var schedule = await _scheduleDomainService.FindSingleAsync(s => !string.IsNullOrEmpty(drillDeviceCode)
                                    && !string.IsNullOrEmpty(s.SourceDeviceId)
                                    && s.SourceDeviceId.ToLower() == drillDeviceCode.ToLower()
                                    && _newDrillScheduleStatus.Contains((ScheduledTaskStatus)s.ScheduledTaskStatus));

                return schedule.Code;
            }

            return string.Empty;
        }

        /// <summary>
        /// 批量取消调度
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkCanceled(List<long> idList, string reason = "")
        {
            if (idList == null || idList.Count == 0)
            {
                logger.LogError($"BulkCanceled, 未传入有效ID集合时，退出方法");
                return Fail($"BulkCanceled, 未传入有效ID集合时，退出方法");
            }

            var scheduleList = await _scheduleDomainService.QueryAsync(x => idList.Contains(x.Id), x => x.Id, OrderByType.Asc);
            var updateList = new List<Schedule>();
            foreach (var id in idList)
            {
                var model = scheduleList.SingleOrDefault(p => p.Id == id);
                if (model == null)
                {
                    logger.LogWarning($"BulkCanceled, code:{model.Code},取消调度计划失败,未找到此ID，{id}");
                    continue;
                }

                if (_noCancelStatus.Contains(model.ScheduledTaskStatus))
                {
                    logger.LogWarning($"BulkCanceled, code:{model.Code},取消调度计划失败,old:{model.ScheduledTaskStatus},new:{ScheduledTaskStatus.Canceled}");
                    continue;
                }

                logger.LogWarning($"BulkCanceled, code:{model.Code},取消调度计划,old:{model.ScheduledTaskStatus},new:{ScheduledTaskStatus.Canceled}");

                model.ScheduledTaskStatus = ScheduledTaskStatus.Canceled;
                model.CancelReason = reason;
                model.Remark = $"{model.Remark}，{reason}";
                model.CanceledTime = DateTime.Now;
                model.ModifierId = UserId;
                model.ModifyTime = DateTime.Now;
                updateList.Add(model);
            }

            if (!updateList.Any())
            {
                return Fail($"没有需要取消的调度记录");
            }

            await _domainService.BulkUpdate(updateList);

            var scheduleIds = updateList.Select(p => p.Id).ToList();
            var subDeviceCodes = updateList.Where(p => !string.IsNullOrEmpty(p.SubDeviceCode)).Select(p => p.SubDeviceCode.ToLower()).ToList();

            await UpdateTransTask(
                scheduleIds != null ? scheduleIds : new List<long>(),
                subDeviceCodes != null ? subDeviceCodes : new List<string>());

            //添加后台处理任务
            _ = AddDeviceServiceInvocation(updateList);

            return Success();
        }

        private async Task UpdateTransTask(List<long> scheduleIds, List<string> subDeviceCodes)
        {
            if (subDeviceCodes != null && scheduleIds != null)
            {
                await _transportationTaskDomainService.UpdateAsync(p => new TransferJob()
                {
                    ScheduledTaskStatus = ScheduledTaskStatus.Canceled,
                    CanceledTime = DateTime.Now,
                    ModifierId = UserId,
                    ModifyTime = DateTime.Now
                }, p => (scheduleIds.Contains((long)p.RelatedScheduleId)
                        || (!string.IsNullOrEmpty(p.ForkCode) && subDeviceCodes.Contains(p.ForkCode.ToLower())))
                        && p.ScheduledTaskStatus == ScheduledTaskStatus.Created);
            }
        }

        public async Task<ResponseDto<List<ScheduleDto>>> GetScheduleTasks(GetScheduleListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 1000;

            var pagedResult = await _scheduleDomainService.GetScheduleWithRequestList(req);
            return Success(pagedResult.List);
        }

        public async Task<ScheduleDto> FindScheduleByTraceId(string traceId)
        {
            var schedule = await _scheduleDomainService.FindSingleAsync(s => s.Code == traceId);
            return _mapper.Map<ScheduleDto>(schedule);
        }

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> ExsistSchedule(GetScheduleListReq req)
        {
            return await _scheduleDomainService.ExsistSchedule(req);
        }

        public async Task<ScheduleDto> FindScheduleByRoutingKey(string routingKey)
        {
            var todoScheduleStatus = new List<ScheduledTaskStatus> { ScheduledTaskStatus.Created, ScheduledTaskStatus.PartCompleted };
            var schedule = await _scheduleDomainService.FindSingleAsync(s => todoScheduleStatus.Contains((ScheduledTaskStatus)s.ScheduledTaskStatus)
                && s.RoutingKey.ToLower().Equals(routingKey.ToLower()));

            return _mapper.Map<ScheduleDto>(schedule);
        }

        public async Task<bool> SetMatchSchedule(string traceId, string matchAgv, string masterCode, string message = "")
        {
            var schedule = await _domainService.FindSingleAsync(d => d.Code.ToLower() == traceId.ToLower());
            var masterSchedule = await _domainService.FindSingleAsync(d => d.Code.ToLower() == masterCode.ToLower());
            if (schedule == null || masterSchedule == null) return false;

            schedule.RequireDeviceId = matchAgv;
            schedule.MasterScheduleId = masterSchedule.Id;
            schedule.IsAuxiliary = true;
            //var descForIsAuxiliary = schedule.IsAuxiliary == false && DeviceKindExtensions.IsAuxiliary(schedule.RequestDeviceKind.Value) ? "主叫" : $"承接{schedule.MasterScheduleId}，";

            if (!string.IsNullOrEmpty(message))
            {
                schedule.RequestInteractionBehaviorName = $"{schedule.RequestInteractionBehaviorName},{message}";
            }

            try
            {
                var eventRequest = JsonSerializer.Deserialize<DeviceEventReportRequest>(schedule.RequestJson);
                eventRequest.Params["IsAuxiliary"] = true;

                schedule.RequestJson = JsonSerializer.Serialize(eventRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Wrong format RequestJson");
            }

            return await _domainService.Update(schedule);
        }

        /// <summary>
        /// 根据调度ID获取详细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<ScheduleLogsDto>> QueryLogsByID(long id)
        {
            ScheduleLogsDto result = null;

            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                var hisEntity = await _scheduleHistoryDomainService.QueryByID(id);
                if (hisEntity == null)
                {
                    return Fail<ScheduleLogsDto>("信息不存在!");
                }
                else
                {
                    result = _mapper.Map<ScheduleLogsDto>(hisEntity);
                    try
                    {
                        result.RequestJson = JsonSerializer.Deserialize<DeviceEventReportRequest>(hisEntity.RequestJson);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Deserialize RequestJson", ex);
                    }
                }
            }
            else
            {
                result = _mapper.Map<ScheduleLogsDto>(entity);
                try
                {
                    result.RequestJson = JsonSerializer.Deserialize<DeviceEventReportRequest>(entity.RequestJson);
                }
                catch (Exception ex)
                {
                    logger.LogError("Deserialize RequestJson", ex);
                }
            }

            var details = await scheduleLogDomainService.QueryAsync(p => p.MasterId == id, p => p.Id, OrderByType.Desc);
            if (details != null)
            {
                result.ScheduleLogs = _mapper.Map<List<ScheduleLog>, List<ScheduleLogDto>>(details.ToList());
            }

            return Success(result);
        }

        public async Task<ResponseDto<string>> SetScheduleUrgent(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            if (entity.ScheduledTaskStatus != ScheduledTaskStatus.PartCompleted && entity.ScheduledTaskStatus != ScheduledTaskStatus.Created)
            {
                return Fail("调度记录状态不支持设置!");
            }

            entity.IsUrgent = 1;
            entity.ModifierId = UserId;
            entity.ModifyTime = DateTime.Now;

            await _domainService.Update(entity);
            return Success();
        }

        public async Task<ResponseDto<string>> SetBarcodeCheckResult(long id, bool isBarcodeOk)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            entity.IsBarcodeOk = isBarcodeOk;
            entity.ModifierId = UserId;
            entity.ModifyTime = DateTime.Now;

            await _domainService.Update(entity);
            return Success();
        }

        /// <summary>
        /// 添加后台处理任务，等待中控通知钻机等设备
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private async System.Threading.Tasks.Task AddDeviceServiceInvocation(List<Schedule> schedules)
        {
            try
            {
                var requestList = new List<AddOrUpdateDeviceServiceInvocationReq>();

                foreach (var schedule in schedules)
                {
                    var serviceRequest = JsonSerializer.Deserialize<DeviceServiceInvokeRequest>(schedule.RequestJson);
                    serviceRequest.ScheduledStatus = schedule.ScheduledTaskStatus.Value;
                    serviceRequest.ServiceId = Topics.Services.COMPLETE_SCHEDULE_SERVICE_ID;

                    requestList.Add(new AddOrUpdateDeviceServiceInvocationReq
                    {
                        MessageId = schedule.Code,
                        Reason = "Cancel",
                        RequestTopic = serviceRequest.RequestTopic,// .GetRequestTopic(),
                        ResponseTopic = serviceRequest.ReplyTopic, //.GetReplyTopic(),
                        Retries = 0,
                        MqttQualityOfServiceLevel = 0,
                        Payload = schedule.RequestJson,
                        RoutingKey = schedule.RoutingKey,
                    });
                }

                await deviceServiceInvocationService.AddDeviceServiceInvocationList(requestList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 添加后台处理任务，等待中控通知钻机等设备
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private async System.Threading.Tasks.Task AddDeviceServiceInvocation(Schedule schedule)
        {
            await AddDeviceServiceInvocation(new List<Schedule> { schedule });
        }

        public async Task<List<ScheduleToExcelDto>> GetToExcelList(GetScheduleListReq req)
        {
            List<ScheduleToExcelDto> resultDtos = new List<ScheduleToExcelDto>();

            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 100) req.PageSize = 1000;

            var result = await _scheduleDomainService.GetList(req);
            if (result == null || result.List == null || result.List.Count == 0)
            {
                return resultDtos;
            }

            foreach (var item in result.List)
            {
                string scheduledTaskStatus = "未知";
                switch (item.ScheduledTaskStatus)
                {
                    case ScheduledTaskStatus.Created:
                        scheduledTaskStatus = "已上报";
                        break;

                    case ScheduledTaskStatus.Allocated:
                        scheduledTaskStatus = "已分配AGV，等待下发";
                        break;

                    case ScheduledTaskStatus.Canceled:
                        scheduledTaskStatus = "取消计划任务";
                        break;

                    case ScheduledTaskStatus.Failed:
                        scheduledTaskStatus = "计划任务执行失败";
                        break;

                    case ScheduledTaskStatus.Completed:
                        scheduledTaskStatus = "调度完成";
                        break;

                    case ScheduledTaskStatus.PartCompleted:
                        scheduledTaskStatus = "部分完成 等待AGV下次上料";
                        break;

                    case ScheduledTaskStatus.Delivered:
                        scheduledTaskStatus = "下发成功";
                        break;

                    case ScheduledTaskStatus.WaitingForAgv:
                        scheduledTaskStatus = "等待分配agv";
                        break;

                    case ScheduledTaskStatus.Running:
                        scheduledTaskStatus = "开始调度";
                        break;

                    default:
                        scheduledTaskStatus = "未知";
                        break;
                }

                string interactionSequence = "未知";
                switch (item.InteractionSequence)
                {
                    case InteractionSequence.LoadThenUnload:
                        interactionSequence = "先上再下";
                        break;

                    case InteractionSequence.LoadOnly:
                        interactionSequence = "只上";
                        break;

                    case InteractionSequence.UnloadThenLoad:
                        interactionSequence = "先下再上";
                        break;

                    case InteractionSequence.UnloadOnly:
                        interactionSequence = "只下";
                        break;

                    default:
                        interactionSequence = "未知";
                        break;
                }

                resultDtos.Add(new ScheduleToExcelDto
                {
                    Id = item.Id,
                    ScheduledTaskStatus = scheduledTaskStatus,
                    SourceDeviceId = item.SourceDeviceId,
                    SubDeviceCode = item.SubDeviceCode,
                    InteractionSequence = interactionSequence,
                    AllocateTime = item.AllocateTime,
                    CanceledTime = item.CanceledTime,
                    CompletedTime = item.CompletedTime,
                    CreateTime = item.CreateTime,
                    FailedTime = item.FailedTime,
                    IsUrgent = item.IsUrgent,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    Remark = item.Remark,
                    RequireDeviceId = item.RequireDeviceId,
                    RouteCode = item.RouteCode,
                    RouteName = item.RouteName,
                    RunningTime = item.RunningTime,
                    TaskId = item.TaskId,
                });
            }

            return resultDtos;
        }


        public async Task<List<ScheduleToExcelDto>> GetHisToExcelList(GetScheduleListReq req)
        {
            List<ScheduleToExcelDto> resultDtos = new List<ScheduleToExcelDto>();

            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 100) req.PageSize = 1000;

            var result = await _scheduleHistoryDomainService.GetList(req);
            if (result == null || result.List == null || result.List.Count == 0)
            {
                return resultDtos;
            }

            foreach (var item in result.List)
            {
                string scheduledTaskStatus = "未知";
                switch (item.ScheduledTaskStatus)
                {
                    case ScheduledTaskStatus.Created:
                        scheduledTaskStatus = "已上报";
                        break;

                    case ScheduledTaskStatus.Allocated:
                        scheduledTaskStatus = "已分配AGV，等待下发";
                        break;

                    case ScheduledTaskStatus.Canceled:
                        scheduledTaskStatus = "取消计划任务";
                        break;

                    case ScheduledTaskStatus.Failed:
                        scheduledTaskStatus = "计划任务执行失败";
                        break;

                    case ScheduledTaskStatus.Completed:
                        scheduledTaskStatus = "调度完成";
                        break;

                    case ScheduledTaskStatus.PartCompleted:
                        scheduledTaskStatus = "部分完成 等待AGV下次上料";
                        break;

                    case ScheduledTaskStatus.Delivered:
                        scheduledTaskStatus = "下发成功";
                        break;

                    case ScheduledTaskStatus.WaitingForAgv:
                        scheduledTaskStatus = "等待分配agv";
                        break;

                    case ScheduledTaskStatus.Running:
                        scheduledTaskStatus = "开始调度";
                        break;

                    default:
                        scheduledTaskStatus = "未知";
                        break;
                }

                string interactionSequence = "未知";
                switch (item.InteractionSequence)
                {
                    case InteractionSequence.LoadThenUnload:
                        interactionSequence = "先上再下";
                        break;

                    case InteractionSequence.LoadOnly:
                        interactionSequence = "只上";
                        break;

                    case InteractionSequence.UnloadThenLoad:
                        interactionSequence = "先下再上";
                        break;

                    case InteractionSequence.UnloadOnly:
                        interactionSequence = "只下";
                        break;

                    default:
                        interactionSequence = "未知";
                        break;
                }

                var barcodeResult = "N/A";
                switch (item.IsBarcodeOk)
                {
                    case true:
                        barcodeResult = "是";
                        break;
                    case false:
                        barcodeResult = "否";
                        break;
                    default:
                        break;
                }

                resultDtos.Add(new ScheduleToExcelDto
                {
                    Id = item.Id,
                    ScheduledTaskStatus = scheduledTaskStatus,
                    SourceDeviceId = item.SourceDeviceId,
                    SubDeviceCode = item.SubDeviceCode,
                    InteractionSequence = interactionSequence,
                    AllocateTime = item.AllocateTime,
                    CanceledTime = item.CanceledTime,
                    CompletedTime = item.CompletedTime,
                    CreateTime = item.CreateTime,
                    FailedTime = item.FailedTime,
                    IsUrgent = item.IsUrgent,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    Remark = item.Remark,
                    RequireDeviceId = item.RequireDeviceId,
                    RouteCode = item.RouteCode,
                    RouteName = item.RouteName,
                    RunningTime = item.RunningTime,
                    TaskId = item.TaskId,
                    BarcodeResult = barcodeResult,
                });
            }

            return resultDtos;
        }

        /// <summary>
        /// 转移调度历史数据
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TransferScheduleHistoryData(TransferScheduleHistoryDataReq req)
        {

            var time = req.TransferTime != null ? req.TransferTime : DateTime.Parse(DateTime.Now.AddDays(-3).ToShortDateString());
            var data = await _scheduleDomainService.QueryAsync(s => s.CreateTime <= time, r => r.Id, OrderByType.Asc);
            logger.LogInformation($"转移调度历史数据:查询到{time}之前有{data?.Count()}条数据");
            if (data?.Count() > 0)
            {
                var history = _mapper.Map<List<ScheduleHistory>>(data);
                var historyIds = history.Select(r => r.Id).ToList();
                var beforeInsertExistDatas = await _scheduleHistoryDomainService.QueryAsync(s => historyIds.Contains(s.Id), r => r.Id, OrderByType.Asc);
                if (beforeInsertExistDatas?.Count > 0)//过滤掉未知原因没有删除成功的数据，防止重复添加，引起主键冲突
                {
                    history = history.Where(s => !beforeInsertExistDatas.Any(m => m.Id == s.Id)).ToList();//取差集
                }
                //批量插入历史表
                for (var i = 0; i <= history.Count / 2000; i++)
                {
                    var insertDatas = history.Skip(i * 2000).Take(2000).ToList();
                    if (insertDatas?.Count > 0)
                    {
                        await _scheduleHistoryDomainService.BulkInsert(insertDatas);
                    }
                }
                logger.LogInformation($"转移调度历史数据:插入t_schedule_his表{history?.Count()}条数据");
                //批量删除原有表数据
                var ids = data.Select(r => r.Id).ToList();
                var afterInsertExistDatas = await _scheduleHistoryDomainService.QueryAsync(s => ids.Contains(s.Id), r => r.Id, OrderByType.Asc);
                if (afterInsertExistDatas?.Count > 0)//检验已添加的数据
                {
                    var deleteDatas = data.Where(r => afterInsertExistDatas.Any(m => m.Id == r.Id)).ToList();
                    for (var i = 0; i <= deleteDatas.Count / 2000; i++)
                    {
                        var removeDatas = deleteDatas.Skip(i * 2000).Take(2000).ToList();
                        if (removeDatas?.Count > 0)
                        {
                            await _scheduleDomainService.BulkDelete(removeDatas);
                        }
                    }
                    logger.LogInformation($"转移调度历史数据:删除t_schedule表{deleteDatas?.Count()}条数据");
                }
            }
            return true;
        }


        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ScheduleDto>>> GetHistoryList(GetScheduleListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _scheduleHistoryDomainService.GetList(req);
            if (result.List.Count > 0)
            {
                var routeAndPartitionSettings = await _routeService.GetRouteAndPartitionSetting();
                if (routeAndPartitionSettings?.Count > 0)
                {
                    foreach (var item in result.List)
                    {
                        var routeAndPartitionSetting = routeAndPartitionSettings.FirstOrDefault(s => s.RouteCode == item.RouteCode);
                        item.WareHouseCode = routeAndPartitionSetting?.PartitionCode;
                    }
                }
            }
            return Success(result);
        }

        public virtual async Task<ResponseDto<ScheduleDto>> QueryHistoryByID(long id)
        {
            var entity = await _scheduleHistoryDomainService.QueryByID(id);
            if (entity == null)
            {
                return Fail<ScheduleDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<ScheduleDto>("信息已标记为删除!");
            }
            var model = _mapper.Map<ScheduleDto>(entity);

            return Success(model);
        }

        /// <summary>
        /// 根据调度ID获取详细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<ScheduleLogsDto>> QueryHistoryLogsByID(long id)
        {
            var entity = await _scheduleHistoryDomainService.QueryByID(id);
            if (entity == null)
            {
                return Fail<ScheduleLogsDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<ScheduleLogsDto>("信息错误!");
            }

            var result = _mapper.Map<ScheduleLogsDto>(entity);
            try
            {
                result.RequestJson = JsonSerializer.Deserialize<DeviceEventReportRequest>(entity.RequestJson);
            }
            catch (Exception ex)
            {
                logger.LogError("Deserialize RequestJson", ex);
            }
            var details = await scheduleLogDomainService.QueryAsync(p => p.MasterId == id, p => p.Id, OrderByType.Desc);
            if (details != null)
            {
                result.ScheduleLogs = _mapper.Map<List<ScheduleLog>, List<ScheduleLogDto>>(details.ToList());
            }

            return Success(result);
        }

        public async Task RegularDeleteHisData()
        {
            DateTime recordsTime = DateTime.Now.AddMonths(-12);
            await _scheduleHistoryDomainService.DeleteAsync(p => p.CreateTime < recordsTime);
        }

        /// <summary>
        /// 根据调度记录id更新requestJson
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="requestJson"></param>
        /// <returns></returns>
        public async Task<string> UpdateRequestJson(long scheduleId, string requestJson)
        {
            if (scheduleId <= 0)
            {
                return "调度记录id不能为空";
            }
            var data = await _scheduleDomainService.QueryByID(scheduleId);
            if (data == null)
            {
                return "查询不到该调度记录";
            }
            data.RequestJson = requestJson;
            data.ModifyTime = DateTime.Now;
            var isSuccess = await _scheduleDomainService.Update(data);
            return isSuccess ? "" : "更新失败，请查看log日志";
        }

        /// <summary>
        /// 根据scheduleId更新库位panel信息
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="scheduleLocationPanels"></param>
        /// <returns></returns>
        public async Task<bool> UpdateLocationPanels(long scheduleId, List<ScheduleLocationPanel> scheduleLocationPanels)
        {
            return await _scheduleDomainService.UpdateLocationPanels(scheduleId, scheduleLocationPanels);
        }

        public async Task<List<string?>> FindSingleByDrillDeviceCodes(List<string> drillDeviceCodes)
        {

            var schedule = await _scheduleDomainService.QueryAsync(s => drillDeviceCodes.Contains(s.SourceDeviceId)
                                && !string.IsNullOrEmpty(s.SourceDeviceId)
                                && _newDrillScheduleStatus.Contains((ScheduledTaskStatus)s.ScheduledTaskStatus),p=>p.Id,OrderByType.Asc);

            return schedule?.Select(p=>p.Code).ToList();

        }

    }
}