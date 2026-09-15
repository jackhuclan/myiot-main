
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using SqlSugar;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices.External;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Fundation.Iot.Schedule;
using static NPOI.HSSF.Util.HSSFColor;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskService : BaseServiceWithTree<Model.Entites.Mes.WorkTask, TaskTreeDto, TaskDto, AddOrUpdateTaskReq>, ITaskService
    {

        private readonly ITaskDomainService _taskDomainService;
        private readonly IWorkOrderDomainService _workOrderService;
        private readonly IExternalWorkOrderDomainService _externalWorkOrderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IAPIHelper _apiHelper;
        private readonly IEncodeBuildRulesService _encodeBuildRulesService;
        private readonly IDrillTaskDomainService _rdillTaskDomainService;
        private readonly IMaterialStockOverviewDomainService _materialStockOverviewDomainService;
        private readonly IMaterialStockOverviewHistoryDomainService _materialStockOverviewHistoryDomainService;
        private readonly IWorkOrderAndWorkStationDomainService _workOrderAndWorkStationDomainService;
        private readonly IProduceTaskDomainService _produceTaskDomainService;
        private readonly ICutterGroupService _cutterGroupService;
        private readonly IProduceTaskHistoryDomainService _produceTaskHisDomainService;
        private readonly ILogger<TaskService> logger;
        private readonly IWorkstationDomainService _workstationDomainService;
        private readonly IDrillWorkOrderService _drillWorkOrderService;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IScheduleDomainService _scheduleDomainService;
        private readonly ITransportationTaskDomainService _transportationTaskDomainService;
        private readonly ICentralOnlineDevice _centralOnlineDevice;
        private readonly InnerOptions _innerOptions;
        private readonly IItemDomainService _itemDomainService;
        private readonly IDeviceDomainService _deviceDomainService;
        private readonly List<TaskStatusEnum?> taskStatusList = new List<TaskStatusEnum?>()
        {
            TaskStatusEnum.BEGIN,
            TaskStatusEnum.BUFFERED,
            TaskStatusEnum.SENDING
        };
        /// <summary>
        /// 
        /// </summary>
        public TaskService(ITaskDomainService domainService,
            IWorkOrderDomainService workOrderDomainService,
            IEncodeBuildRulesService encodeBuildRulesService,
            IDrillTaskDomainService rdillTaskDomainService,
            IMaterialStockOverviewDomainService materialStockOverviewDomainService,
            IMaterialStockOverviewHistoryDomainService materialStockOverviewHistoryDomainService,
            IWorkOrderAndWorkStationDomainService workOrderAndWorkStationDomainService,
            IProduceTaskDomainService produceTaskDomainService,
            IProduceTaskHistoryDomainService produceTaskHisDomainService,
            IWorkstationDomainService workstationDomainService,
            IMapper mapper,
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICutterGroupService cutterGroupService,
            IAPIHelper apiHelper,
            IExternalWorkOrderDomainService externalWorkOrderService,
            IDrillWorkOrderService drillWorkOrderService,
            ISysConfigManager sysConfigManager,
            IScheduleDomainService scheduleDomainService,
            IConfiguration configuration,
            ITransportationTaskDomainService transportationTaskDomainService,
            ICentralOnlineDevice centralOnlineDevice,
            IOptions<InnerOptions> options,
            IItemDomainService itemDomainService,
            IDeviceDomainService deviceDomainService
           )
            : base(domainService, mapper)
        {
            logger = loggerFactory.CreateLogger<TaskService>();
            _taskDomainService = domainService;
            _workOrderService = workOrderDomainService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _apiHelper = apiHelper;
            _encodeBuildRulesService = encodeBuildRulesService;
            _rdillTaskDomainService = rdillTaskDomainService;
            _externalWorkOrderService = externalWorkOrderService;
            _materialStockOverviewDomainService = materialStockOverviewDomainService;
            _materialStockOverviewHistoryDomainService = materialStockOverviewHistoryDomainService;
            _workOrderAndWorkStationDomainService = workOrderAndWorkStationDomainService;
            _produceTaskDomainService = produceTaskDomainService;
            _produceTaskHisDomainService = produceTaskHisDomainService;
            _workstationDomainService = workstationDomainService;
            _drillWorkOrderService = drillWorkOrderService;
            _sysConfigManager = sysConfigManager;
            _cutterGroupService = cutterGroupService;
            _scheduleDomainService = scheduleDomainService;
            _transportationTaskDomainService = transportationTaskDomainService;
            _centralOnlineDevice = centralOnlineDevice;
            _innerOptions = options.Value;
            _itemDomainService = itemDomainService;
            _deviceDomainService = deviceDomainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<TaskDto>>> GetList(GetTaskListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _taskDomainService.GetList(req);
            return Success(result);
        }
        public async Task<TaskDto> GetTaskByCode(string code)
        {
            return await _taskDomainService.GetTaskByCode(code);
        }




        /// <summary>
        /// 根据ItemTypeId获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<TaskDto>>> GetEquipmentList(GetTaskListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _taskDomainService.GetEquipmentList(req);
            return Success(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<TaskTreeDto>>> GetTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<TaskTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }

            var allCodes = AddChildN(list, 0);

            result.Data = allCodes;

            return result;
        }

        /// <summary>
        /// 删除Task回撤排产数量
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> SchedulingQtyRoBack(CommitTaskReq req)
        {
            if (req.Ids != null)
            {
                Dictionary<int, decimal?> dicWorkOrder = new Dictionary<int, decimal?>(); //用来记录需要刷新的workOrder已排产数量

                List<Model.Entites.Mes.WorkTask> updateTaskList = new List<Model.Entites.Mes.WorkTask>();
                foreach (var id in req.Ids)
                {
                    var entity = await _domainService.QueryByID(id);
                    if (entity == null)
                    {
                        continue;
                    }

                    if (req.TaskStatus == null || req.TaskStatus != TaskStatusEnum.DRAFT)
                    {
                        return Fail("TaskStatus未填写或填写错误!");
                    }

                    if (entity.WorkOrderId != null && !string.IsNullOrEmpty(entity.KeyFlag) && entity.KeyFlag.Equals("1"))
                    {
                        bool isExist = await _workOrderService.IsExistAsync(p => p.Id == entity.WorkOrderId);
                        if (isExist)
                        {
                            if (dicWorkOrder.ContainsKey((int)entity.WorkOrderId))
                            {
                                dicWorkOrder[(int)entity.WorkOrderId] += entity.Quantity * entity.PanelCount;
                            }
                            else
                            {
                                dicWorkOrder.Add((int)entity.WorkOrderId, entity.Quantity * entity.PanelCount);
                            }
                        }
                    }

                }

                ////更新workOrder表
                if (dicWorkOrder.Count > 0)
                {
                    List<WorkOrder> updateWorkOrderList = new List<WorkOrder>();
                    foreach (var item in dicWorkOrder)
                    {
                        var workModel = await _workOrderService.QueryByID(item.Key);
                        if (workModel != null)
                        {
                            workModel.ModifyTime = DateTime.Now;
                            workModel.ModifierId = UserId;
                            if (workModel.QuantityScheduled > item.Value)
                            {
                                workModel.QuantityScheduled -= item.Value;
                            }
                            else
                            {
                                workModel.QuantityScheduled = 0;
                            }

                            updateWorkOrderList.Add(workModel);
                        }
                    }

                    await _workOrderService.BulkUpdate(updateWorkOrderList);
                }
            }

            return Success();
        }
        /// <summary>
        /// 排产数量提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> SchedulingQtyCommit(CommitTaskReq req)
        {

            Dictionary<int, decimal?> dicWorkOrder = new Dictionary<int, decimal?>(); //用来记录需要刷新的workOrder已排产数量

            List<Model.Entites.Mes.WorkTask> updateTaskList = new List<Model.Entites.Mes.WorkTask>();
            foreach (var id in req.Ids)
            {
                var entity = await _domainService.QueryByID(id);
                if (entity == null)
                {
                    continue;
                }

                if (entity.TaskStatus != null && entity.TaskStatus != TaskStatusEnum.COMMITED)
                {
                    return Fail("只有提交状态的任务，才可以排产!");
                }

                if (req.TaskStatus == null || req.TaskStatus != TaskStatusEnum.COMMITED)
                {
                    return Fail("TaskStatus未填写或填写错误!");
                }

                if (entity.WorkOrderId != null && !string.IsNullOrEmpty(entity.KeyFlag) && entity.KeyFlag.Equals("1"))
                {
                    var workModel = await _workOrderService.QueryByID(entity.WorkOrderId);
                    if (workModel != null)
                    {
                        if (dicWorkOrder.ContainsKey((int)entity.WorkOrderId))
                        {
                            if (workModel.QuantityChanged != null &&
                                workModel.QuantityChanged < workModel.QuantityScheduled + dicWorkOrder[(int)entity.WorkOrderId] + entity.Quantity * entity.PanelCount)
                            {
                                int sum = Convert.ToInt32(workModel.QuantityScheduled + dicWorkOrder[(int)entity.WorkOrderId]);

                                if (workModel.QuantityChanged > sum)
                                {
                                    int last = Convert.ToInt32(workModel.QuantityChanged) - sum;
                                    if (last * entity.PanelCount + sum >= Convert.ToInt32(workModel.QuantityChanged))
                                    {
                                        dicWorkOrder[(int)entity.WorkOrderId] += last;
                                    }
                                    else
                                    {
                                        dicWorkOrder[(int)entity.WorkOrderId] += last * entity.PanelCount;
                                    }
                                }
                                updateTaskList.Remove(entity);
                                continue;
                            }
                            dicWorkOrder[(int)entity.WorkOrderId] += entity.Quantity * entity.PanelCount;
                        }
                        else
                        {
                            if (workModel.QuantityChanged != null &&
                                workModel.QuantityChanged < workModel.QuantityScheduled + entity.Quantity * entity.PanelCount)
                            {
                                updateTaskList.Remove(entity);
                                continue;
                            }
                            dicWorkOrder.Add((int)entity.WorkOrderId, entity.Quantity * entity.PanelCount);
                        }
                    }
                }
            }

            ////更新workOrder表
            if (dicWorkOrder.Count > 0)
            {
                List<WorkOrder> updateWorkOrderList = new List<WorkOrder>();
                foreach (var item in dicWorkOrder)
                {
                    var workModel = await _workOrderService.QueryByID(item.Key);
                    if (workModel != null)
                    {
                        workModel.ModifyTime = DateTime.Now;
                        workModel.ModifierId = UserId;
                        //todo，回写工单时，乘以层数
                        workModel.QuantityScheduled += item.Value;
                        //if (workModel.ManuOrderStatus == ManuOrderStatusEnum.COMMITED)
                        //{
                        workModel.ManuOrderStatus = ManuOrderStatusEnum.SCHEDULED;
                        //}
                        updateWorkOrderList.Add(workModel);
                    }
                }

                await _workOrderService.BulkUpdate(updateWorkOrderList);
            }
            return Success();
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Commit(CommitTaskReq req)
        {
            if (req.Ids != null)
            {
                var updateTaskList = new List<WorkTask>();
                var workStationCodes = new List<string>();
                foreach (var id in req.Ids)
                {
                    var task = await _domainService.QueryByID(id);
                    if (task == null)
                    {
                        continue;
                    }

                    if (task.TaskStatus != null && task.TaskStatus != TaskStatusEnum.DRAFT)
                    {
                        return Fail("只有草稿状态的任务，才可以提交!");
                    }

                    if (req.TaskStatus == null || req.TaskStatus != TaskStatusEnum.COMMITED)
                    {
                        return Fail("TaskStatus未填写或填写错误!");
                    }

                    task.TaskStatus = req.TaskStatus;
                    task.ModifierId = UserId;
                    task.ModifyTime = DateTime.Now;

                    updateTaskList.Add(task);
                    if (!string.IsNullOrEmpty(task.WorkStationCode))
                    {
                        workStationCodes.Add(task.WorkStationCode.ToLower());
                    }
                }
                var result = await _domainService.BulkUpdate(updateTaskList);
                //撤销再提交，startTime排序在钻机已配刀计划中变了，需要清除任务上涉及到的所有钻机下的任务配刀组计划
                var autoGenerateSwitch = await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_AUTO_CUTTER);
                if (autoGenerateSwitch)
                {
                    await CutterGroupHandle(updateTaskList);//需要先更新状态，再统一删除配刀计划
                }
                if (result)
                {
                    var where = PredicateBuilder.True<Schedule>();
                    where = where.And(p => !string.IsNullOrEmpty(p.SourceDeviceId) && workStationCodes.Contains(p.SourceDeviceId.ToLower()));

                    where = where.And(p => p.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                        || p.ScheduledTaskStatus == ScheduledTaskStatus.Created);

                    await CanceledSchedule(where);
                }
            }

            return Success();
        }

        /// <summary>
        /// 撤销提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> RevokeCommit(CommitTaskReq req)
        {
            if (req.Ids == null)
            {
                return Fail("Task信息id为空!");
            }
            if (req.TaskStatus == null || req.TaskStatus != TaskStatusEnum.DRAFT)
            {
                return Fail("TaskStatus未填写或填写错误!");
            }
            var autoGenerateSwitch = await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_AUTO_CUTTER);
            var tasks = await _domainService.QueryAsync(s => req.Ids.Contains(s.Id) && s.IsDeleted == 0, s => s.Id, OrderByType.Asc);
            if (tasks?.Count > 0)
            {
                var cutterGroupTaskCodes = tasks.Where(s => !string.IsNullOrWhiteSpace(s.CutterGroupNo)).Select(s => s.Code ?? "").Distinct().ToList();
                if (cutterGroupTaskCodes?.Count > 0 && autoGenerateSwitch)
                {
                    var result = await _cutterGroupService.IsLockedByTaskCode(cutterGroupTaskCodes);
                    if (result.IsLoked)
                    {
                        return Fail($"{string.Join(",", result.LockedTaskCodes)}任务已被配刀计划锁定，不能更改");
                    }
                }
                List<string> taskCodeList = new List<string>();
                foreach (var entity in tasks)
                {
                    if (entity.TaskStatus != null && entity.TaskStatus != TaskStatusEnum.COMMITED)
                    {
                        return Fail("当前状态不能执行批量撤销的操作!");
                    }
                    entity.TaskStatus = req.TaskStatus;
                    entity.ModifierId = UserId;
                    entity.ModifyTime = DateTime.Now;
                    taskCodeList.Add(entity.Code);
                }
                await _domainService.BulkUpdate(tasks);
                //清除任务上的配刀计划，task上的配刀标识，要先改变状态再清空。要是标识先清空的话，自动生成脚本可能又会生成新的配刀计划
                //清除任务上涉及到的所有钻机下的任务配刀组计划
                await CutterGroupHandle(tasks);
                var where = PredicateBuilder.True<Schedule>();
                where = where.And(p => taskCodeList.Contains(p.TaskId));

                where = where.And(p => p.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                    || p.ScheduledTaskStatus == ScheduledTaskStatus.Created);

                await CanceledSchedule(where);

            }
            return Success();
        }

        public async Task CanceledSchedule(Expression<Func<Schedule, bool>> where)
        {
            var schedules = await _scheduleDomainService.QueryAsync(where, p => p.Id, OrderByType.Asc);
            if (schedules != null && schedules.Count > 0)
            {
                var scheduleIds = schedules.Select(p => p.Id).ToList();
                scheduleIds = scheduleIds == null ? new List<long>() : scheduleIds;
                var subDeviceCodes = schedules.Where(p => !string.IsNullOrEmpty(p.SubDeviceCode)).Select(p => p.SubDeviceCode.ToLower()).ToList();
                subDeviceCodes = subDeviceCodes == null ? new List<string>() : subDeviceCodes;

                if (string.IsNullOrEmpty(_innerOptions.CancelScheduleUrl))
                {
                    logger.LogCritical("没有设置innerOptions.CancelScheduleUrl");
                    return;
                }
                else
                {
                    foreach (var subDeviceCode in subDeviceCodes)
                    {
                        var request = new CancelScheduleTaskRequest
                        {
                            Params = new Dictionary<string, object?>
                            {
                                { "DeviceCode", subDeviceCode },
                                { "CancelReason", "生产任务变更" }
                            }
                        };

                        _apiHelper.RequestData(_innerOptions.CancelScheduleUrl, "post", JsonSerializer.Serialize(request));
                    }
                }

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

        /// <summary>
        /// 钻孔工单批量生成钻孔任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkAddDrillTask(AddOrUpdateDrillWorkOrderReq req)
        {
            if (req.WorkOrderId == null || req.WorkOrderCode == null)
            {
                return Fail("缺少工单ID");
            }

            var workOrderData = await _workOrderService.QueryByID(req.WorkOrderId);
            if (workOrderData == null)
            {
                return Fail("工单信息不存在");
            }

            if (!await _rdillTaskDomainService.IsExistAsync(p => p.WorkOrderId == req.WorkOrderId))
            {
                return Fail("未添加钻孔工单数据！");
            }

            var drillWorkOrder = await _rdillTaskDomainService.FindSingleAsync(p => p.WorkOrderId == req.WorkOrderId);
            if (drillWorkOrder == null)
            {
                return Fail("未添加钻孔工单数据！");
            }

            string produceTaskType = await _sysConfigManager.GetStringValue(MESConfigConstants.PRODUCE_TASK_TYPE);
            if (string.IsNullOrEmpty(produceTaskType))
            {
                produceTaskType = "WadCount";
            }
            decimal usableCount = 0;
            switch (produceTaskType)
            {
                case "UsableCount":
                    if (req.UsableCount == null || req.UsableCount == 0)
                    {
                        return Fail("可用叠数未填写！");
                    }
                    usableCount = (decimal)req.UsableCount;
                    break;

                case "WadCount":
                    if (drillWorkOrder.IsAddTask == 1)
                    {
                        return Fail("按照计划叠数，只能生成一次任务！");
                    }
                    if (req.WadCount == null || req.WadCount == 0)
                    {
                        return Fail("计划叠数未填写！");
                    }
                    usableCount = (decimal)req.WadCount;
                    break;
            }
            if (usableCount == 0)
            {
                return Fail("ProduceTaskType 配置参数错误！");
            }

            //if (req.ShaftCount == null || req.ShaftCount == 0
            //    || req.PanelCount == null || req.PanelCount == 0)
            if (req.PanelCount == null || req.PanelCount == 0)
            {
                return Fail("轴数或叠板层数未填写！");
            }

            var workStationDataList = await _workOrderAndWorkStationDomainService.QueryAsync(p => p.WorkOrderCode == req.WorkOrderCode
            , p => p.WorkStationCode, OrderByType.Asc);
            if (workStationDataList == null || workStationDataList.Count == 0)
            {
                return Fail("缺少机台");
            }

            decimal taskCount = 0;
            if (req.ShaftCount == 0)
            {
                req.ShaftCount = await _sysConfigManager.GetIntValue(MESConfigConstants.DRILL_TASK_SHAFT_COUNT);//获取配置中的轴数
            }
            switch (produceTaskType)
            {
                case "UsableCount":
                    taskCount = Math.Floor(usableCount / (decimal)req.ShaftCount);
                    break;

                case "WadCount":
                    taskCount = Math.Ceiling(usableCount / (decimal)req.ShaftCount);
                    break;
            }

            bool isEnd = false;
            if (taskCount == 0)
            {
                if (drillWorkOrder.AllPassesCount - drillWorkOrder.ScheduledCount != 1)
                {
                    return Fail("不是尾料，需等待可用叠数达到轴数！");
                }
                taskCount = 1;
                isEnd = true;
            }

            await ProduceData(req, workOrderData, drillWorkOrder, workStationDataList, taskCount, usableCount, isEnd, produceTaskType);

            return Success();
        }

        /// <summary>
        /// 生成任务
        /// </summary>
        /// <param name="req"></param>
        /// <param name="workOrderData"></param>
        /// <param name="drillWorkOrder"></param>
        /// <param name="workStationDatas"></param>
        /// <param name="taskCount"></param>
        /// <param name="usableCount"></param>
        /// <param name="isEnd"></param>
        /// <param name="produceTaskType"></param>
        /// <returns></returns>
        private async Task ProduceData(AddOrUpdateDrillWorkOrderReq req,
            WorkOrder workOrderData, DrillWorkOrder drillWorkOrder, List<WorkOrderAndWorkStation> workStationDatas,
            decimal taskCount, decimal usableCount, bool isEnd, string produceTaskType)
        {
            bool isRefreshStock = await _sysConfigManager.GetBoolValue(MESConfigConstants.REFRESH_STOCK);

            bool isAutoVettingDrillTask = await _sysConfigManager.GetBoolValue(MESConfigConstants.AUTO_VETTING_DRILL_TASK);

            List<WorkTask> tasks = new List<WorkTask>();

            List<string> encodes = _encodeBuildRulesService.GetEncodeList(
                new GetEncodeByRulesListReq
                {
                    RulesCode = "TASK_CODE",
                    BuildCount = taskCount.ToInt(),
                }).Result.Data;

            var processData = await _taskDomainService.GetRouteAndProcessList(
                new GetRouteAndProcessByItemReq
                {
                    ItemId = workOrderData.ItemId,
                    RouteId = workOrderData.RouteId,
                });
            RouteAndProcessDto pData = new RouteAndProcessDto();
            if (processData != null && processData.ProcessInfos != null)
            {
                pData = processData.ProcessInfos.SingleOrDefault(p => p.ProcessCode == _configuration["AppConfig:ProcessCode"]);
            }

            List<ProduceTask> addProduceTasks = new List<ProduceTask>();
            List<ProduceTaskHistory> addProduceHis = new List<ProduceTaskHistory>();
            var tempTimeNow = DateTime.Now;
            Dictionary<string, DateTime> stations = GetStationMaxEndTime(tempTimeNow, workStationDatas, req);//每个站的Task最大结束时间
            var stationName = stations.ToList()[0].Key;
            var stationMaxTime = stations.ToList()[0].Value;
            int count = 0;//需要补齐到最大开始时间的Task总数
            int qty = 0;  //每个station补齐到最大开始时间的Task数量
            Dictionary<string, int> stationQty = new Dictionary<string, int>();//每个站分配的Task数量

            if (pData.RequiredTime == null || pData.RequiredTime <= 0)
            {
                pData.RequiredTime = 40;
            }

            for (int i = stations.Count() - 1; i >= 1; i--)
            {
                var tempStation = stations.ToList()[i].Key;
                var tempTime = stations.ToList()[i].Value;
                var timecount = stationMaxTime - tempTime;

                //需要补齐开始时间和结束时间的任务
                if ((timecount.Ticks / 10000000) > 0)//时间差值(秒)
                {
                    var countTask = timecount.TotalMinutes / pData.RequiredTime;
                    qty = GetTaskCount(countTask, tempStation, taskCount.ToInt());//每个station需要补齐的数量

                    if (qty >= (taskCount - count))
                    {
                        qty = taskCount.ToInt() - count;
                    }

                    //note if qty==0,也要加入stationQty

                    count = count + qty;
                    stationQty.Add(stations.ToList()[i].Key, qty);
                }
                //与首个站点的时间差值一致时，按0计算
                else
                {
                    stationQty.Add(stations.ToList()[i].Key, 0);
                }
            }

            //开始批量添加，每个站平均分配任务数量
            if (count <= taskCount.ToInt())
            {
                Dictionary<int, int> StationQtyLot = GetStationQty(taskCount.ToInt(), count, stations.Count());
                if (stationQty.Count() == 0)
                {
                    for (int i = 0; i < stations.Count(); i++)
                    {
                        stationQty.Add(stations.ToList()[i].Key, StationQtyLot.ToList()[0].Key);
                    }
                }
                else
                {
                    for (int i = 0; i < stationQty.Count(); i++)
                    {
                        stationQty[stationQty.ToList()[i].Key] = stationQty.ToList()[i].Value + StationQtyLot.ToList()[0].Key;
                    }
                    stationQty.Add(stations.ToList()[0].Key, StationQtyLot.ToList()[0].Key);//添加排序时间最大的
                }

                //count个Task已经被分配
                count = count + stations.Count() * StationQtyLot.ToList()[0].Key;

                //批量添加之后，如果还有任务没有分配，继续分配
                if (StationQtyLot.ToList()[0].Value > 0 && (count + StationQtyLot.ToList()[0].Value) <= taskCount.ToInt())
                {
                    for (int i = 0; i < StationQtyLot.ToList()[0].Value; i++)
                    {
                        var stationCode = stations.ToList()[i].Key;
                        stationQty[stationCode] = stationQty[stationCode] + 1;
                    }
                }
            }

            int sumTask = 0;//开始分配之后，已经分配TaskCode的数量
            for (int i = 0; i < stationQty.Count(); i++)
            {
                var station = stationQty.ToList()[i].Key;
                var stationId = workStationDatas.ToList().Where(t => t.WorkStationCode == station).Select(t => t.WorkStationId).First().ToInt();
                var stationNames = workStationDatas.ToList().Where(t => t.WorkStationCode == station).Select(t => t.WorkStationName).First();
                for (int j = 0; j < stationQty[station]; j++)
                {

                    Model.Entites.Mes.WorkTask model = new Model.Entites.Mes.WorkTask();
                    //if (usableCount - sumTask * counts < counts)
                    //{
                    //    if (  usableCount - sumTask * counts < 0)
                    //    {
                    //        continue;
                    //    }
                    //    model.Quantity = usableCount - sumTask * counts;
                    //}
                    //else
                    //{
                    //    model.Quantity = counts;
                    //}
                    model.WorkOrderId = (int?)workOrderData.Id;
                    model.WorkOrderName = workOrderData.Name;
                    model.WorkOrderCode = workOrderData.Code;
                    model.ClientId = workOrderData.ClientId;
                    model.ClientCode = workOrderData.ClientCode;
                    model.ClientName = workOrderData.ClientName;
                    model.BatchCode = workOrderData.BatchCode;
                    model.ParentId = 0;
                    model.ItemId = workOrderData.ItemId;
                    model.ItemName = workOrderData.ItemName;
                    model.ItemCode = workOrderData.ItemCode;
                    model.ItemTypeId = workOrderData.ItemTypeId;
                    model.Specification = workOrderData.Specification;
                    model.UnitOfMeasure = workOrderData.UnitOfMeasure;
                    model.TaskStatus = TaskStatusEnum.DRAFT;
                    model.RouteId = workOrderData.RouteId;
                    model.RouteCode = workOrderData.RouteCode;
                    model.RouteName = workOrderData.RouteName;
                    model.IsUrgent = workOrderData.IsUrgent;
                    model.SpecGroup = workOrderData.SpecGroup;
                    model.IncodeNumber = workOrderData.IncodeNumber;
                    model.LayerNum = workOrderData.LayerNum;
                    model.BeforeDrillFilePath = workOrderData.BeforeDrillFilePath;
                    model.AfterDrillFilePath = workOrderData.AfterDrillFilePath;
                    model.IsRebrush = workOrderData.IsRebrush;
                    model.BarCode = workOrderData.BarCode;
                    if (j < stationQty.ToList()[i].Value)
                    {
                        //按照stations设置预计的开始，结束时间
                        model.StartTime = stations[station].AddMinutes(pData.RequiredTime.Value * j);
                        model.EndTime = model.StartTime.Value.AddMinutes(pData.RequiredTime.Value);
                        model.WorkStationCode = station;
                        model.WorkStationId = stationId.ToInt();
                        model.WorkStationName = stationNames == null ? null : stationNames.ToString();
                    }
                    //todo, 叠板、钻孔工序的排产数量，按叠数计算，不需要乘以层数；这也是他们的入库数量；
                    //哪些工序需要乘以层数，将在工序定义中设定
                    switch (produceTaskType)
                    {
                        case "UsableCount":
                            if (isEnd)
                            {
                                model.Quantity = usableCount;
                            }
                            else
                            {
                                model.Quantity = req.ShaftCount;
                            }
                            break;

                        case "WadCount":
                            if (usableCount % (decimal)req.ShaftCount > 0 && i == taskCount - 1)
                            {
                                model.Quantity = usableCount - Math.Floor(usableCount / (decimal)req.ShaftCount) * req.ShaftCount;
                            }
                            else
                            {
                                var sum = usableCount - sumTask * req.ShaftCount;
                                if (sum < req.ShaftCount)
                                {
                                    if (sum < 0)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        model.Quantity = sum;
                                    }

                                }
                                else
                                {
                                    model.Quantity = req.ShaftCount;
                                }

                            }

                            break;
                    }


                    model.NowWadCount = model.Quantity;
                    model.PanelCount = req.PanelCount;
                    model.QuantityProduced = 0;
                    model.QuantityUnquanlify = 0;
                    model.CreateTime = tempTimeNow;
                    model.CreatorId = UserId;
                    model.Status = (int)DataStatusEnum.Enable;
                    model.ProcessCode = _configuration["AppConfig:ProcessCode"];
                    model.IsStarted = 0;
                    if (encodes != null && encodes.Count == taskCount.ToInt())
                    {
                        model.Name = model.Code = encodes[sumTask];
                        sumTask++;
                    }

                    if (pData != null)
                    {
                        model.ProcessId = (int?)pData.ProcessId;
                        model.ProcessName = pData.ProcessName;
                        model.Color = pData.Color;
                        model.KeyFlag = pData.KeyFlag;
                        model.Duration = pData.RequiredTime;
                    }

                    tasks.Add(model);

                    if (isRefreshStock)
                    {
                        addProduceTasks.Add(new ProduceTask
                        {
                            TaskCode = model.Code,
                            WorkOrderCode = model.WorkOrderCode,
                            WorkOrderName = model.WorkOrderName,
                            ItemCode = model.ItemCode,
                            ItemName = model.ItemName,
                            ProcessCode = model.ProcessCode,
                            ProcessName = model.ProcessName,
                            NowWadCount = model.NowWadCount,
                            Status = model.Status,
                            TaskStatus = model.TaskStatus.ToString(),
                            StartTime = model.StartTime,
                            EndTime = model.EndTime,
                            CreateTime = model.CreateTime,
                            CreatorId = model.CreatorId,
                        });

                        addProduceHis.Add(new ProduceTaskHistory
                        {
                            TaskCode = model.Code,
                            WorkOrderCode = model.WorkOrderCode,
                            WorkOrderName = model.WorkOrderName,
                            ItemCode = model.ItemCode,
                            ItemName = model.ItemName,
                            ProcessCode = model.ProcessCode,
                            ProcessName = model.ProcessName,
                            NowWadCount = model.NowWadCount,
                            Status = model.Status,
                            TaskStatus = model.TaskStatus.ToString(),
                            StartTime = model.StartTime,
                            EndTime = model.EndTime,
                            CreateTime = model.CreateTime,
                            CreatorId = model.CreatorId,
                        });
                    }
                }
            }

            _unitOfWork.BeginTran();
            await _domainService.BulkInsert(tasks);

            if (drillWorkOrder.ScheduledCount == null)
            {
                drillWorkOrder.ScheduledCount = taskCount;
            }
            else
            {
                drillWorkOrder.ScheduledCount += taskCount;
            }
            drillWorkOrder.ModifierId = UserId;
            drillWorkOrder.ModifyTime = DateTime.Now;
            drillWorkOrder.IsAddTask = 1;
            drillWorkOrder.AddTaskTime = DateTime.Now;
            drillWorkOrder.AddTaskUser = UserId;
            await _rdillTaskDomainService.Update(drillWorkOrder);

            workOrderData.ManuOrderStatus = ManuOrderStatusEnum.SCHEDULED;
            workOrderData.ModifierId = UserId;
            workOrderData.ModifyTime = DateTime.Now;
            await _workOrderService.Update(workOrderData);

            _unitOfWork.CommitTran();

            //是否刷新库存
            if (isRefreshStock)
            {
                await _produceTaskDomainService.BulkInsert(addProduceTasks);
                await _produceTaskHisDomainService.BulkInsert(addProduceHis);

                if (await _materialStockOverviewDomainService.IsExistAsync(p => p.ItemCode == workOrderData.ItemCode
                && p.ProcessCode == _configuration["AppConfig:ProcessCode"]))
                {
                    var overviewData = await _materialStockOverviewDomainService.FindSingleAsync(p => p.ItemCode == workOrderData.ItemCode
                    && p.ProcessCode == _configuration["AppConfig:ProcessCode"]);
                    if (overviewData != null)
                    {
                        if (isEnd)
                        {
                            overviewData.SumPlan += usableCount;
                            overviewData.UsableCount -= usableCount;
                        }
                        else
                        {
                            overviewData.SumPlan += taskCount * req.ShaftCount;
                            overviewData.UsableCount -= taskCount * req.ShaftCount;
                        }
                        overviewData.ModifyTime = DateTime.Now;
                        overviewData.ModifierId = UserId;
                        await _materialStockOverviewDomainService.Update(overviewData);
                    }
                }
            }

            //是否自动审批钻孔任务
            if (isAutoVettingDrillTask)
            {
                var taskDataList = await _domainService.QueryAsync(p => p.TaskStatus == TaskStatusEnum.DRAFT && p.WorkOrderCode == workOrderData.Code
                , p => p.CreateTime, OrderByType.Desc);
                if (taskDataList == null || taskDataList.Count == 0)
                {
                    return;
                }

                var vettingTaskList = taskDataList.FindAll(p => tasks.Any(r => r.Code == p.Code)).Select(p => p.Id).ToList();
                if (vettingTaskList == null || vettingTaskList.Count == 0)
                {
                    return;
                }

                await Commit(new CommitTaskReq
                {
                    Ids = vettingTaskList,
                    TaskStatus = TaskStatusEnum.COMMITED,
                });

                ///排产数量
                await SchedulingQtyCommit(new CommitTaskReq
                {
                    Ids = vettingTaskList,
                    TaskStatus = TaskStatusEnum.COMMITED,
                });
            }

        }

        private Dictionary<int, int> GetStationQty(int count, int qty, int stationCount)
        {
            Dictionary<int, int> res = new Dictionary<int, int>();
            var temp = (count - qty) % stationCount;
            if (temp == 0)
            {
                res.Add((count - qty) / stationCount, 0);
            }
            else
            {
                double result = (count - qty) / stationCount;
                int rest = (int)Math.Floor(result);
                int number = (count - qty) - rest * stationCount;
                res.Add(rest, number);
            }
            return res;
        }

        private Dictionary<string, DateTime> GetStationMaxEndTime(DateTime tempTime, List<WorkOrderAndWorkStation> workStationDatas, AddOrUpdateDrillWorkOrderReq req)
        {
            var where = PredicateBuilder.True<Model.Entites.Mes.WorkTask>();
            var taskList = where.And(t => t.IsDeleted == 0);
            var taskItem = _domainService.QueryAsync(where, p => p.WorkStationCode, OrderByType.Asc).Result;
            Dictionary<string, DateTime> stations = new Dictionary<string, DateTime>();
            foreach (var StationmaxEndTime in workStationDatas)
            {
                var times = taskItem.Where(t => t.WorkStationCode == StationmaxEndTime.WorkStationCode).Select(t => t.RealEndTime).Max();//上一个工单实际最大结束时间
                var endTime = taskItem.Where(t => t.WorkStationCode == StationmaxEndTime.WorkStationCode).Select(t => t.EndTime).Max();//上一个工单预计结束最大时间
                times = times > endTime ? times : endTime;
                times = times > tempTime ? times : tempTime;
                stations.Add(StationmaxEndTime.WorkStationCode, (DateTime)times);
            }
            if (stations.Count > 0)
            {
                Dictionary<string, DateTime> stationMaxTime = stations.OrderBy(d => d.Key).OrderByDescending(d => d.Value).ToDictionary(d => d.Key, d => d.Value);
                stations.Clear();
                stations = stationMaxTime;
            }
            return stations;
        }

        //每个站补齐到做大的结束时间所需的Task数
        private int GetTaskCount(double? countTask, string staion, int taskCount)
        {
            int res = 0;
            if (countTask % 1 != 0)
            {
                if (countTask % 1 > 0)
                {
                    res = (int)(countTask - countTask % 1) + 1;
                }
            }
            else
            {
                res = (int)countTask;
            }

            if (res >= taskCount)
            {
                res = taskCount;
            }
            return res;
        }

        /// <summary>
        /// 理顺当天草稿状态任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> RationalizeTask(RationalizeTaskReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (string.IsNullOrEmpty(req.ProcessCode))
            {
                if (_configuration["AppConfig:ProcessCode"] != null)
                {
                    req.ProcessCode = _configuration["AppConfig:ProcessCode"];
                }
                else
                {
                    req.ProcessCode = "drill";
                }
            }

            if ((req.ItemId == null || req.ItemId == 0) && !string.IsNullOrEmpty(req.WorkOrderCode)
                && await _workOrderService.IsExistAsync(p => p.Code == req.WorkOrderCode))
            {
                var workOrderData = await _workOrderService.FindSingleAsync(p => p.Code == req.WorkOrderCode);
                if (workOrderData != null)
                {
                    req.ItemId = workOrderData.ItemId;
                }
            }

            var where = PredicateBuilder.True<Model.Entites.Mes.WorkTask>();
            where = where.And(p => p.IsDeleted == 0
            && ((p.StartTime != null && p.StartTime.Value >= DateTime.Today)
             || (p.EndTime != null && p.EndTime.Value >= DateTime.Today)
             || (p.RealStartTime != null && p.RealStartTime.Value >= DateTime.Today)
             || (p.RealEndTime != null && p.RealEndTime.Value >= DateTime.Today)));

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.Equals(req.ProcessCode));
            }
            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.Equals(req.WorkStationCode));
            }

            var taskList = await _domainService.QueryAsync(where, p => p.CreateTime, OrderByType.Asc);
            if (taskList == null || taskList.Count == 0)
            {
                return Fail("未查询到当天的任务!");
            }

            var workStationList = taskList.DistinctBy(p => p.WorkStationCode).ToList();
            if (workStationList == null || workStationList.Count == 0)
            {
                return Fail("未查询到需要理顺的工作站!");
            }

            List<Model.Entites.Mes.WorkTask> updateTasks = new List<Model.Entites.Mes.WorkTask>();
            foreach (var workStation in workStationList)
            {
                var workStationTasks = taskList.FindAll(p => p.WorkStationCode == workStation.WorkStationCode).ToList();

                //todo重新理顺任务，根据xx进行排序
                //理顺未实际开始的草稿状态任务
                var needRationalizeTasks = workStationTasks.FindAll(p => p.WorkOrderCode == req.WorkOrderCode
                    && p.TaskStatus == TaskStatusEnum.DRAFT)
                    .OrderByDescending(p => p.IsUrgent)
                    .OrderBy(p => p.StartTime)
                    .ToList();
                if (needRationalizeTasks == null || needRationalizeTasks.Count == 0)
                {
                    continue;
                }

                DateTime taskBeginTime = DateTime.Today.Date;

                var realEndTask = workStationTasks.FindAll(p => p.RealEndTime != null).OrderByDescending(p => p.RealEndTime).ToList();
                if (realEndTask != null && realEndTask.Count > 0)
                {
                    taskBeginTime = realEndTask[0].RealEndTime.Value;
                }

                var realStartTasks = workStationTasks.FindAll(p => p.RealStartTime != null).OrderByDescending(p => p.RealStartTime).ToList();
                if (realStartTasks != null && realStartTasks.Count > 0)
                {
                    DateTime maxTime = DateTime.Today.Date;
                    foreach (var item in realStartTasks)
                    {
                        DateTime time = DateTime.Today.Date;
                        if (item.Duration == null || item.Duration == 0)
                        {
                            time = item.RealStartTime.Value;
                        }
                        else
                        {
                            time = item.RealStartTime.Value.AddMinutes((double)item.Duration);
                        }
                        if (time > maxTime)
                        {
                            maxTime = time;
                        }
                    }

                    if (taskBeginTime < maxTime)
                    {
                        taskBeginTime = maxTime;
                    }
                }

                var commitTask = workStationTasks.FindAll(p => p.TaskStatus == TaskStatusEnum.COMMITED).OrderByDescending(p => p.EndTime).ToList();
                if (commitTask != null && commitTask.Count > 0)
                {
                    if (commitTask[0].EndTime != null && commitTask[0].EndTime > taskBeginTime)
                    {
                        taskBeginTime = commitTask[0].EndTime.Value;
                    }
                    else if (commitTask[0].StartTime != null && commitTask[0].StartTime > taskBeginTime)
                    {
                        taskBeginTime = commitTask[0].StartTime.Value;
                    }
                }

                RouteAndProcessDto? pData = null;
                if (req.ItemId != null && req.ItemId != 0)
                {
                    pData = await GetRouteAndProcessData((long)req.ItemId, req.ProcessCode);
                }

                foreach (var item in needRationalizeTasks)
                {
                    item.StartTime = taskBeginTime;
                    if (item.Duration == null || item.Duration == 0)
                    {
                        if (pData == null && item.ItemId != null && !string.IsNullOrEmpty(item.ProcessCode))
                        {
                            pData = await GetRouteAndProcessData((long)item.ItemId, item.ProcessCode);
                        }

                        if (pData != null && pData.RequiredTime != null)
                        {
                            item.Duration = pData.RequiredTime;
                            item.EndTime = item.StartTime.Value.AddMinutes((double)item.Duration);
                        }
                        else
                        {
                            item.EndTime = item.StartTime;
                        }
                    }
                    else
                    {
                        item.EndTime = item.StartTime.Value.AddMinutes((double)item.Duration);
                    }
                    item.ModifierId = UserId;
                    item.ModifyTime = DateTime.Now;
                    updateTasks.Add(item);

                    taskBeginTime = item.EndTime.Value;
                }
            }

            await _domainService.BulkUpdate(updateTasks);
            return Success();
        }

        /// <summary>
        /// 批量重算生产任务
        /// 目前暂时限定一个机台
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BatchRationalizeTask(RationalizeTaskReq req)
        {
            if (req == null || req.WorkStationList == null || !req.WorkStationList.Any())
            {
                return Fail("信息格式错误或未指定机台!");
            }

            if (string.IsNullOrEmpty(req.ProcessCode))
            {
                if (_configuration["AppConfig:ProcessCode"] != null)
                {
                    req.ProcessCode = _configuration["AppConfig:ProcessCode"];
                }
                else
                {
                    req.ProcessCode = "drill";
                }
            }

            var where = PredicateBuilder.True<Model.Entites.Mes.WorkTask>();
            where = where.And(p => p.IsDeleted == 0
                && p.StartTime >= req.StartDate
             );

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.Equals(req.ProcessCode));
            }

            var lowerStations = new List<string>();
            req.WorkStationList.ForEach(p => lowerStations.Add(p.ToLower()));

            //限定多个机台
            where = where.And(p => lowerStations.Contains(p.WorkStationCode.ToLower()));

            var taskList = await _domainService.QueryAsync(where, p => p.CreateTime, OrderByType.Asc);

            List<Model.Entites.Mes.WorkTask> updateTasks = new List<Model.Entites.Mes.WorkTask>();
            var tempTime = DateTime.Now;
            foreach (var workStation in req.WorkStationList)
            {
                var workStationTasks = taskList.FindAll(p => p.WorkStationCode == workStation).ToList();

                //todo重新理顺任务，根据xx进行排序
                //理顺未实际开始的草稿状态任务
                var needRationalizeTasks = workStationTasks
                    .FindAll(p => p.TaskStatus == TaskStatusEnum.DRAFT)
                    .OrderByDescending(p => p.IsUrgent)
                    .ThenBy(p => p.StartTime)
                    .ToList();
                if (needRationalizeTasks == null || needRationalizeTasks.Count == 0)
                {
                    continue;
                }

                #region 计算 taskBeginTime
                //DateTime taskBeginTime = DateTime.Today.Date;
                DateTime taskBeginTime = DateTime.Now;

                var realEndTask = workStationTasks
                    .FindAll(p => p.RealEndTime != null)
                    .OrderByDescending(p => p.RealEndTime).ToList();
                if (realEndTask != null && realEndTask.Count > 0)
                {
                    //taskBeginTime = realEndTask[0].RealEndTime.Value;
                    if (tempTime > realEndTask[0].RealEndTime.Value)
                    {
                        taskBeginTime = tempTime;
                    }
                    else
                    {
                        taskBeginTime = realEndTask[0].RealEndTime.Value;
                    }

                }
                var realStartTasks = workStationTasks
                    .FindAll(p => p.RealStartTime != null)
                    .OrderByDescending(p => p.RealStartTime).ToList();
                if (realStartTasks != null && realStartTasks.Count > 0)
                {
                    //DateTime maxTime = DateTime.Today.Date;
                    DateTime maxTime = DateTime.Now;
                    foreach (var item in realStartTasks)
                    {
                        //DateTime time = DateTime.Today.Date;
                        DateTime time = DateTime.Now;
                        if (item.Duration == null || item.Duration == 0)
                        {
                            time = item.RealStartTime.Value;
                        }
                        else
                        {
                            time = item.RealStartTime.Value.AddMinutes((double)item.Duration);
                        }
                        if (time > maxTime)
                        {
                            maxTime = time;
                        }
                    }
                    if (taskBeginTime < maxTime)
                    {
                        taskBeginTime = maxTime;
                    }
                }
                var commitTask = workStationTasks.FindAll(p => p.TaskStatus == TaskStatusEnum.COMMITED).OrderByDescending(p => p.EndTime).ToList();
                if (commitTask != null && commitTask.Count > 0)
                {
                    if (commitTask[0].EndTime != null && commitTask[0].EndTime > taskBeginTime)
                    {
                        taskBeginTime = commitTask[0].EndTime.Value;
                    }
                    else if (commitTask[0].StartTime != null && commitTask[0].StartTime > taskBeginTime)
                    {
                        taskBeginTime = commitTask[0].StartTime.Value;
                    }
                }
                #endregion

                RouteAndProcessDto? pData = null;
                if (req.ItemId != null && req.ItemId != 0)
                {
                    pData = await GetRouteAndProcessData((long)req.ItemId, req.ProcessCode);
                }

                foreach (var item in needRationalizeTasks)
                {
                    item.StartTime = taskBeginTime;
                    if (item.Duration == null || item.Duration == 0)
                    {
                        if (pData == null && item.ItemId != null && !string.IsNullOrEmpty(item.ProcessCode))
                        {
                            pData = await GetRouteAndProcessData((long)item.ItemId, item.ProcessCode);
                        }

                        if (pData != null && pData.RequiredTime != null)
                        {
                            item.Duration = pData.RequiredTime;
                            item.EndTime = item.StartTime.Value.AddMinutes((double)item.Duration);
                        }
                        else
                        {
                            item.EndTime = item.StartTime;
                        }
                    }
                    else
                    {
                        item.EndTime = item.StartTime.Value.AddMinutes((double)item.Duration);
                    }
                    item.ModifierId = UserId;
                    item.ModifyTime = DateTime.Now;
                    updateTasks.Add(item);

                    taskBeginTime = item.EndTime.Value;
                }
            }

            if (updateTasks.Count == 0)
            {
                return Fail("未查询到可以重排的草稿状态任务!");
            }

            await _domainService.BulkUpdate(updateTasks);

            return Success();
        }

        /// <summary>
        /// 根据ItemID和工序编码获取RouteAndProcessDto
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="processCode"></param>
        /// <returns></returns>
        private async Task<RouteAndProcessDto> GetRouteAndProcessData(long itemId, string processCode)
        {
            var processData = await _taskDomainService.GetRouteAndProcessList(
                                        new GetRouteAndProcessByItemReq
                                        {
                                            ItemId = itemId
                                        });
            RouteAndProcessDto pData = new RouteAndProcessDto();
            if (processData != null && processData.ProcessInfos != null)
            {
                pData = processData.ProcessInfos.SingleOrDefault(p => p.ProcessCode == processCode);
            }

            return pData;
        }

        /// <summary>
        /// 钻孔任务分配机台
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Allocate(AddOrUpdateDrillWorkOrderReq req)
        {
            if (req.WorkOrderId == null)
            {
                return Fail("缺少工单ID");
            }

            var entity = await _workOrderService.QueryByID(req.WorkOrderId);
            if (entity == null)
            {
                return Fail("无此工单");
            }

            var workStationFitList = await GetFitWorkStationList(new GetFitWorkStationListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });//目前直接获取，需对工作站的时间空闲进行排序
            if (workStationFitList == null || workStationFitList.Data.List == null || workStationFitList.Data.List.Count == 0)
            {
                return Fail("未查询到合适的工作站！");
            }

            var workStationList = workStationFitList.Data.List;
            if (workStationList.Count < req.DispenseMachines)
            {
                return Fail("分配机台数大于工作站数量");
            }

            var where = PredicateBuilder.True<Model.Entites.Mes.WorkTask>();
            where = where.And(p => p.ProcessCode == _configuration["AppConfig:ProcessCode"]
            && p.WorkOrderId == req.WorkOrderId);
            var result = await _domainService.QueryAsync(where, q => q.StartTime,
                SqlSugar.OrderByType.Desc);

            int workStationIndex = 0;
            List<Model.Entites.Mes.WorkTask> tasks = new List<Model.Entites.Mes.WorkTask>();
            foreach (var item in result)
            {
                item.WorkStationCode = workStationList[workStationIndex].Code;
                item.WorkStationId = (int?)workStationList[workStationIndex].Id;
                item.WorkStationName = workStationList[workStationIndex].Name;
                item.ModifyTime = DateTime.Now;
                item.ModifierId = UserId;
                item.IsDeleted = 0;
                if (workStationIndex >= req.DispenseMachines - 1)
                {
                    workStationIndex = 0;
                }
                else
                {
                    workStationIndex++;
                }

                tasks.Add(item);
            }

            await _domainService.BulkUpdate(tasks);
            return Success();
        }

        /// <summary>
        /// 获取工艺路线和关联工序列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<RouteInfoAndProcessInfo>> GetRouteAndProcessList(GetRouteAndProcessByItemReq req)
        {
            var result = await _taskDomainService.GetRouteAndProcessList(req);
            return Success(result);
        }

        /// <summary>
        /// 修改生产任务甘特图时间
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateGantt(UpdatTaskGanttReq req)
        {
            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            entity.StartTime = req.StartTime;
            entity.EndTime = req.EndTime;
            entity.ModifierId = UserId;
            entity.ModifyTime = DateTime.Now;
            await _domainService.Update(entity);

            return Success();
        }

        /// <summary>
        /// 重置生产任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Reset(ResetTaskReq req)
        {
            if (req == null || req.ResetReq == null || req.ResetReq.Count == 0)
            {
                return Fail("错误的入参信息");
            }

            //仅支持重置钻孔工序的任务
            if (string.IsNullOrEmpty(req.ProcessCode))
            {
                req.ProcessCode = _configuration["AppConfig:ProcessCode"];
            }

            var taskList = await _domainService.QueryAsync(p => p.ProcessCode == req.ProcessCode
            && p.TaskStatus != null && p.TaskStatus == TaskStatusEnum.DRAFT,
            p => p.Code, OrderByType.Asc);
            if (taskList == null || taskList.Count == 0)
            {
                return Fail("未查询到可重置的任务！");
            }

            var resetTask = taskList.FindAll(p => req.ResetReq.Any(c => c.WorkOrderCode == p.WorkOrderCode && c.ItemCode == p.ItemCode));
            if (resetTask == null || resetTask.Count == 0)
            {
                return Fail("未查询到可重置的任务！");
            }

            bool isRefreshStock = await _sysConfigManager.GetBoolValue(MESConfigConstants.REFRESH_STOCK);

            object[] deleteTaskList = new object[resetTask.Count];
            Dictionary<string, decimal> overViewDic = new Dictionary<string, decimal>();
            for (int i = 0; i < resetTask.Count; i++)
            {
                deleteTaskList[i] = resetTask[i].Id;

                if (resetTask[i].NowWadCount == null || resetTask[i].NowWadCount == 0)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(resetTask[i].ItemCode))
                {
                    continue;
                }

                if (isRefreshStock)
                {
                    if (overViewDic.ContainsKey(resetTask[i].ItemCode))
                    {
                        overViewDic[resetTask[i].ItemCode] += (decimal)resetTask[i].NowWadCount;
                    }
                    else
                    {
                        overViewDic.Add(resetTask[i].ItemCode, (decimal)resetTask[i].NowWadCount);
                    }
                }

            }

            var result = await _domainService.DeleteByIds(deleteTaskList);
            if (!result)
            {
                return Fail("重置失败");
            }

            var drillWorkOrderList = await _rdillTaskDomainService.QueryAsync(p => p.IsAddTask == 1, p => p.Id, OrderByType.Asc);
            if (drillWorkOrderList != null && drillWorkOrderList.Count > 0)
            {
                var updateDrillWorkOrders = drillWorkOrderList.FindAll(p => resetTask.Any(r => r.WorkOrderCode == p.WorkOrderCode));
                if (updateDrillWorkOrders != null && updateDrillWorkOrders.Count > 0)
                {
                    foreach (var item in updateDrillWorkOrders)
                    {
                        item.IsAddTask = 0;
                        item.ModifierId = UserId;
                        item.ModifyTime = DateTime.Now;
                    }
                    await _rdillTaskDomainService.BulkUpdate(updateDrillWorkOrders);
                }
            }

            if (isRefreshStock)
            {
                var produceTaskList = await _produceTaskDomainService.QueryAsync(p => p.ProcessCode == req.ProcessCode
                          && !string.IsNullOrEmpty(p.TaskStatus) && p.TaskStatus.ToUpper().Equals(CommitStatus.DRAFT.ToString()),
                          p => p.TaskCode, OrderByType.Asc);
                if (produceTaskList != null && produceTaskList.Count > 0)
                {
                    var deleteProduceTask = produceTaskList.FindAll(p => resetTask.Any(r => r.Code == p.TaskCode));
                    if (deleteProduceTask != null && deleteProduceTask.Count > 0)
                    {
                        object[] deleteList = new object[deleteProduceTask.Count];
                        for (int i = 0; i < deleteProduceTask.Count; i++)
                        {
                            deleteList[i] = produceTaskList[i].Id;
                        }
                        await _produceTaskDomainService.DeleteByIds(deleteList);
                    }
                }

                if (overViewDic.Count > 0)
                {
                    var overViewList = await _materialStockOverviewDomainService.QueryAsync(p => p.ProcessCode == req.ProcessCode, p => p.ItemCode, OrderByType.Asc);
                    if (overViewList != null && overViewList.Count > 0)
                    {
                        List<MaterialStockOverview> updateList = new List<MaterialStockOverview>();
                        List<MaterialStockOverviewHistory> addHisList = new List<MaterialStockOverviewHistory>();
                        foreach (var item in overViewDic)
                        {
                            var model = overViewList.SingleOrDefault(p => p.ItemCode == item.Key);
                            if (model == null)
                            {
                                continue;
                            }

                            if (model.SumPlan > item.Value)
                            {
                                model.SumPlan -= item.Value;
                            }
                            else
                            {
                                model.SumPlan = 0;
                            }
                            model.UsableCount = model.SumOnhand - model.SumPlan;
                            model.ModifierId = UserId;
                            model.ModifyTime = DateTime.Now;

                            updateList.Add(model);

                            addHisList.Add(new MaterialStockOverviewHistory
                            {
                                ItemCode = model.ItemCode,
                                ItemName = model.ItemName,
                                ProcessCode = model.ProcessCode,
                                ProcessName = model.ProcessName,
                                SumOnhand = model.SumOnhand,
                                SumPlan = model.SumPlan,
                                UsableCount = model.UsableCount,
                                CreateTime = DateTime.Now,
                                CreatorId = UserId,
                                Status = model.Status,
                            });
                        }
                        await _materialStockOverviewDomainService.BulkUpdate(updateList);

                        await _materialStockOverviewHistoryDomainService.BulkInsert(addHisList);
                    }
                }
            }

            return Success();
        }
        /// <summary>
        /// 获取钻孔生产任务三层结构
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<DrillWorkOrderDto>> GetDrillTaskList(GetDrillTaskReq req)
        {
            if (req == null)
            {
                return Fail<DrillWorkOrderDto>("未识别有效的入参！");
            }

            if (req.StartDate == null || req.TaskNumber == null)
            {
                return Fail<DrillWorkOrderDto>("未识别有效的入参StartDate、TaskNumber！");
            }

            if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
            {
                List<string> routeCodes = req.RouteCodeList.Select(p => p.ToLower()).ToList();
                req.RouteCodeList = routeCodes;
            }

            var where = PredicateBuilder.True<Model.Entites.Mes.WorkTask>();
            where = where.And(p => p.IsDeleted == 0);
            DrillWorkOrderDto drillTaskDto = new DrillWorkOrderDto();

            if (string.IsNullOrEmpty(req.ProcessCode))
            {
                req.ProcessCode = _configuration["AppConfig:ProcessCode"];
            }

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                where = where.And(p => p.ProcessCode == req.ProcessCode && p.StartTime >= req.StartDate
                && p.StartTime < req.StartDate.Value.Date.AddDays(req.TaskNumber.Value));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.ToLower().Contains(req.ItemCode.ToLower()));
            }

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.ToLower().Contains(req.WorkOrderCode.ToLower()));
            }

            if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
            {
                where = where.And(p => req.RouteCodeList.Contains(p.RouteCode.ToLower()));
            }

            if (req.TaskStatusList != null && req.TaskStatusList.Count > 0)
            {
                // 将字符串状态列表转换为TaskStatusEnum列表
                var taskStatusEnums = req.TaskStatusList
                    .Select(s => Enum.TryParse<TaskStatusEnum>(s, true, out var status) ? (TaskStatusEnum?)status : null)
                    .Where(s => s.HasValue)
                    .Select(s => s.Value)
                    .ToList();

                if (taskStatusEnums.Count > 0)
                {
                    where = where.And(p => p.TaskStatus.HasValue && taskStatusEnums.Contains(p.TaskStatus.Value));
                }
            }

            var result = await _domainService.QueryAsync(where, q => q.StartTime,
                SqlSugar.OrderByType.Asc);

            if (result == null)
            {
                return Success<DrillWorkOrderDto>(drillTaskDto);
            }

            var list = new List<Dictionary<String, Object>>();
            var workStationList = await GetWorkStationLists(req);

            for (var i = req.TaskNumber - 1; i >= 0; i--)
            {
                var arr = new Dictionary<String, Object>();
                var date = req.StartDate.Value.Date.AddDays((double)i).ToShortDateString().ToString();
                arr.Add("date", date);

                var children = new List<Object>();
                foreach (var station in workStationList)
                {
                    var childrenObj = new Dictionary<String, Object>();
                    var detailArr = new List<Object>();
                    if (station.Code != null)
                    {
                        childrenObj.Add("id", station.Code);
                    }

                    foreach (var item in result)
                    {
                        if (station.Code == item.WorkStationCode && item.StartTime != null
                            && item.StartTime.Value.Date.ToShortDateString().ToString().Equals(date))
                        {
                            detailArr.Add(item);
                        }
                    }
                    childrenObj.Add("arr", detailArr);
                    children.Add(childrenObj);
                }
                arr.Add("children", children);
                list.Add(arr);
            }

            if (req.IsHistory == true)
            {
                var arrhistory = new Dictionary<String, Object>();
                arrhistory = await GetHistoryTask(req, workStationList);
                list.Add(arrhistory);
            }

            drillTaskDto.WorkStationList = workStationList;
            list.Reverse();
            drillTaskDto.List = list;
            return Success<DrillWorkOrderDto>(drillTaskDto);
        }

        private async Task<Dictionary<string, object>> GetHistoryTask(GetDrillTaskReq req, List<WorkStationLoadTaskDto> workStationList)
        {
            var arrhis = new Dictionary<String, Object>();
            arrhis.Add("date", "history");
            List<TaskStatusEnum> lst = new List<TaskStatusEnum> { TaskStatusEnum.COMMITED, TaskStatusEnum.DRAFT };
            var query = PredicateBuilder.True<Model.Entites.Mes.WorkTask>();
            query = query.And(p => p.IsDeleted == 0 && p.StartTime.Value.Date < req.StartDate.Value.Date && lst.Contains((TaskStatusEnum)p.TaskStatus));
            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                query = query.And(p => p.WorkOrderCode.Trim().ToLower().Contains(req.WorkOrderCode.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.And(p => p.ItemCode.Trim().ToLower().Contains(req.ItemCode.Trim().ToLower()));
            }
            var hisData = await _domainService.QueryAsync(query, q => q.StartTime, SqlSugar.OrderByType.Asc);
            var hisDataList = hisData.ToList();

            var children = new List<Object>();
            foreach (var station in workStationList)
            {
                var childrenObj = new Dictionary<String, Object>();
                var detailArr = new List<Object>();
                if (station.Code != null)
                {
                    childrenObj.Add("id", station.Code);
                }

                foreach (var item in hisDataList)
                {
                    if (station.Code == item.WorkStationCode && item.StartTime != null)
                    {
                        detailArr.Add(item);
                    }
                }

                childrenObj.Add("arr", detailArr);
                children.Add(childrenObj);
            }
            arrhis.Add("children", children);
            return arrhis;
        }

        /// <summary>
        /// 获取工作站数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkStation>> GetWorkStationList(GetDrillTaskReq req)
        {
            List<WorkStation> result = new List<WorkStation>();

            var db = _unitOfWork.GetDbClient();

            if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
            {
                List<string> routeCodes = new List<string>();
                foreach (var item in req.RouteCodeList)
                {
                    routeCodes.Add(item.ToString().ToLower());
                }

                var queryByRoute = db.Queryable<Route, RouteAndProcess, Process, RouteProcessAndWorkStation, WorkStation>
                ((r, rp, p, rpw, w) => new object[]
                    {
                        JoinType.Left, r.Id == rp.RouteId,
                        JoinType.Left, p.Id == rp.ProcessId,
                        JoinType.Left, rp.Id == rpw.RouteAndProcessId,
                        JoinType.Left, rpw.WorkStationId == w.Id,
                    });

                queryByRoute = queryByRoute.Where((r, rp, p, rpw, w) => r.IsDeleted == 0 && w.IsDeleted == 0
                && p.IsDeleted == 0 && p.Code == _configuration["AppConfig:ProcessCode"]);

                queryByRoute = queryByRoute.Where((r, rp, p, rpw, w) => !string.IsNullOrEmpty(r.Code) && routeCodes.Contains(r.Code.ToLower()));

                queryByRoute = queryByRoute.OrderBy((r, rp, p, rpw, w) => w.Code);

                var dataByRoute = await queryByRoute.Select((r, rp, p, rpw, w) => new WorkStation
                {
                    Code = w.Code,
                    Name = w.Name,
                    Id = w.Id,
                    CreateTime = w.CreateTime,
                    Status = w.Status,
                }).ToListAsync();
                if (dataByRoute != null && dataByRoute.Count > 0)
                {
                    result = dataByRoute.DistinctBy(t => t.Id).ToList();
                }
            }
            else
            {
                var query = db.Queryable<Device, WorkStation, DeviceType>
                    ((d, w, dt) => new object[]
                    {
                        JoinType.Inner, d.WorkStationId == w.Id,
                        JoinType.Inner, d.DeviceTypeId == dt.Id,
                    });

                query = query.Where((d, w, dt) => d.IsDeleted == 0 && w.IsDeleted == 0
                && dt.IsDeleted == 0 && dt.Code == _configuration["AppConfig:ProcessCode"]);

                query = query.OrderBy((d, w, dt) => w.Code);

                var data = await query.Select((d, w, dt) => new WorkStation
                {
                    Code = w.Code,
                    Name = w.Name,
                    Id = w.Id,
                    CreateTime = w.CreateTime,
                    Status = w.Status,
                }).ToListAsync();

                if (data != null && data.Count > 0)
                {
                    result = data.DistinctBy(p => p.Id).ToList();
                }
            }

            return result;
        }

        public async Task<List<WorkStationLoadTaskDto>> GetWorkStationLists(GetDrillTaskReq req)
        {
            var db = _unitOfWork.GetDbClient();

            var queryByRoute = db.Queryable<Device, WorkStation, RouteProcessAndWorkStation, RouteAndProcess, Route, Process>
               ((d, w, rpw, rp, r, p) => new object[]
                   {
                        JoinType.Left, d.WorkStationId == w.Id,
                        JoinType.Left, w.Id == rpw.WorkStationId,
                        JoinType.Left, rpw.RouteAndProcessId == rp.Id,
                        JoinType.Left, rp.RouteId == r.Id,
                        JoinType.Inner,rp.ProcessId == p.Id
                   });

            queryByRoute = queryByRoute.Where((d, w, rpw, rp, r, p) => d.IsDeleted == 0 && w.IsDeleted == 0);

            queryByRoute = queryByRoute.Where((d, w, rpw, rp, r, p) => r.IsDeleted == 0 && rpw.IsDeleted == 0 && rp.IsDeleted == 0
            && p.IsDeleted == 0 && p.Code == _configuration["AppConfig:ProcessCode"]);
            if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
            {
                queryByRoute = queryByRoute.Where((d, w, rpw, rp, r, p) => !string.IsNullOrEmpty(r.Code) && req.RouteCodeList.Contains(r.Code.ToLower()));
            }
            if (req.IsAuto != null)
            {
                queryByRoute = queryByRoute.Where((d, w, rpw, rp, r, p) => d.IsAuto == req.IsAuto);
            }
            queryByRoute = queryByRoute.OrderBy((d, w, rpw, rp, r, p) => w.Code);

            var dataByRoute = await queryByRoute.Select((d, w, rpw, rp, r, p) => new WorkStationLoadTaskDto
            {
                Code = w.Code,
                Name = w.Name,
                Id = w.Id,
                CreateTime = w.CreateTime,
                Status = w.Status,
                RouteCode = r.Code,
                //RouteId = r.Id,
                RouteName = r.Name,
            }).ToListAsync();

            if (dataByRoute == null || dataByRoute.Count == 0)
            {
                return new List<WorkStationLoadTaskDto>();
            }

            var workStationList = dataByRoute.DistinctBy(p => p.Code).ToList();
            var centralDevices = await _centralOnlineDevice.GetOnlineDevices();

            List<WorkStationLoadTaskDto> lst = new List<WorkStationLoadTaskDto>();
            foreach (var item in workStationList)
            {
                if (string.IsNullOrEmpty(item.Code))
                {
                    continue;
                }

                var WSRoute = dataByRoute.FindAll(p => p.Code == item.Code).OrderBy(p => p.RouteCode).ToList();
                if (WSRoute == null || WSRoute.Count == 0)
                {
                    continue;
                }
                WSRoute = WSRoute.DistinctBy(p => p.RouteCode).ToList();
                StringBuilder routeCodeStr = new StringBuilder();
                StringBuilder routeNameStr = new StringBuilder();

                foreach (var route in WSRoute)
                {
                    if (!string.IsNullOrEmpty(route.RouteCode))
                    {
                        routeCodeStr.Append(route.RouteCode + ",");
                    }

                    if (!string.IsNullOrEmpty(route.RouteName))
                    {
                        routeNameStr.Append(route.RouteName + ",");
                    }
                }

                item.RouteCode = routeCodeStr.ToString().TrimEnd(',');
                item.RouteName = routeNameStr.ToString().TrimEnd(',');

                if (centralDevices != null)
                {
                    var devices = centralDevices.Where(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower() == item.Code.ToLower()).ToList();
                    var device = (devices != null && devices.Count > 0) ? devices[0] : null;
                    if (device != null)
                    {
                        item.DeviceConsoleAddress = device.Descriptor != null ? device.Descriptor.HostAddress : "";
                    }
                }
                lst.Add(item);
            }
            return lst;
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteData(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            if (entity.TaskStatus != TaskStatusEnum.DRAFT)
            {
                return Fail("非草稿状态的任务，不允许删除!");
            }

            List<long> idLst = new List<long>();
            idLst.Add(id);
            await SchedulingQtyRoBack(new CommitTaskReq
            {
                Ids = idLst,
                TaskStatus = TaskStatusEnum.COMMITED,
            });

            var result = await _domainService.DeleteById(id);
            if (result)
            {
                return Success("");
            }
            return Fail("删除失败");
        }

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteDataList(List<long> idList)
        {
            if (idList != null)
            {
                List<long> idLst = new List<long>();
                object[] deleteList = new object[idList.Count];
                for (int i = 0; i < idList.Count; i++)
                {
                    var entity = await _domainService.QueryByID(idList[i]);
                    if (entity == null)
                    {
                        continue;
                    }

                    if (entity.TaskStatus != TaskStatusEnum.DRAFT)
                    {
                        return Fail(idList[i] + " 非草稿状态的任务，不允许删除!");
                    }
                    deleteList[i] = idList[i];
                    idLst.Add(idList[i]);
                }
                await SchedulingQtyRoBack(new CommitTaskReq
                {
                    Ids = idLst,
                    TaskStatus = TaskStatusEnum.DRAFT,
                });
                var result = await _domainService.DeleteByIds(deleteList);
                if (result)
                {

                    return Success("");
                }
            }
            return Fail("删除失败");

        }





        /// <summary>
        /// 中控调用修改信息
        /// Set TaskStatus:BEGIN/FINISH
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateByOutSide(UpdateTaskByOutSideReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (!await _domainService.IsExistAsync(p => p.Code == req.Code))
            {
                return Fail("信息不存在!");
            }

            var entity = await _domainService.FindSingleAsync(p => p.Code == req.Code);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            if (req.TaskStatus == null)
            {
                return Fail("任务状态未填写!");
            }
            if (req.IsFromAdminWeb ?? false)
            {
                if (string.IsNullOrWhiteSpace(entity.WorkStationCode))
                {
                    return Fail("任务分配的机台WorkStationCode为空!");
                }
            }

            if (!

                (
                //派送失败或者调度取消时，设置COMMITED
                (entity.TaskStatus == TaskStatusEnum.SENDING
                    && req.TaskStatus == TaskStatusEnum.COMMITED
                 )
                ||
                //设置SENDING
                (entity.TaskStatus == TaskStatusEnum.COMMITED
                    && req.TaskStatus == TaskStatusEnum.SENDING
                 )
                ||
                //设置BUFFER
                ((entity.TaskStatus == TaskStatusEnum.COMMITED || entity.TaskStatus == TaskStatusEnum.SENDING)
                    && req.TaskStatus == TaskStatusEnum.BUFFERED
                 )
                ||
                //设置开始
                ((entity.TaskStatus == TaskStatusEnum.COMMITED || entity.TaskStatus == TaskStatusEnum.SENDING || entity.TaskStatus == TaskStatusEnum.BUFFERED)
                    && req.TaskStatus == TaskStatusEnum.BEGIN
                 )
                ||
                //设置完成
                (entity.TaskStatus == TaskStatusEnum.BEGIN
                    && req.TaskStatus == TaskStatusEnum.FINISH)
                ))
            {
                return Fail(entity.Code + " 当前数据的状态不支持此操作!");
            }

            if (!await _workOrderService.IsExistAsync(x => x.Code == entity.WorkOrderCode))
            {
                return Fail(entity.Code + " 找不到对应的生产工单!");
            }

            var workOrder = await _workOrderService.FindSingleAsync(x => x.Code == entity.WorkOrderCode);
            if (workOrder == null)
            {
                return Fail(entity.Code + " 找不到对应的生产工单!");
            }

            if (req.TaskStatus == TaskStatusEnum.BEGIN || req.TaskStatus == TaskStatusEnum.FINISH)
            {
                if (req.TaskStatus == TaskStatusEnum.BEGIN)
                {
                    entity.IsStarted = 1;
                    entity.RealStartTime = DateTime.Now;

                    if (req.NowWadCount > 0)
                    {
                        logger.LogWarning($"请求数量：{req.NowWadCount} 当前任务数量：{entity.NowWadCount}，调整任务数量为请求数量");
                        entity.NowWadCount = req.NowWadCount;
                    }
                }
                else
                {
                    entity.RealEndTime = DateTime.Now;
                }

                var expireTasks = await _domainService.QueryAsync(x => x.WorkStationCode == entity.WorkStationCode
                                                               && x.StartTime < entity.StartTime
                                                               && taskStatusList.Contains(x.TaskStatus)
                                                               , x => x.Code, OrderByType.Asc);

                if (expireTasks != null && expireTasks.Any())
                {
                    expireTasks.ForEach(x =>
                    {
                        x.TaskStatus = TaskStatusEnum.FINISH;
                        x.ModifierId = UserId;
                        x.ModifyTime = DateTime.Now;
                    });

                    logger.LogInformation($"UpdateByOutSide BulkUpdate DeviceCode:{entity.WorkStationCode},ExpireCount: {expireTasks.Count} ");
                    try
                    {
                        await _domainService.BulkUpdate(expireTasks);
                    }
                    catch (SqlSugarException ex)
                    {
                        logger.LogError(ex, $"UpdateByOutSide BulkUpdate SqlSugarException：Message:{ex.Message},DeviceCode:{entity.WorkStationCode} ");
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"UpdateByOutSide BulkUpdate Error：Message:{e.Message},DeviceCode:{entity.WorkStationCode} ");
                    }
                
                }
            }

            entity.TaskStatus = req.TaskStatus;
            entity.ModifierId = UserId;
            entity.ModifyTime = DateTime.Now;

            var result = await _domainService.Update(entity);
            if (!result)
            {
                return Fail<string>("更新任务表失败");
            }

            //开始更新生产工单
            if (req.TaskStatus == TaskStatusEnum.BEGIN)
            {
                logger.LogWarning($"任务投产时，工单的状态：{workOrder.ManuOrderStatus.ToString()}");
                workOrder.ManuOrderStatus = ManuOrderStatusEnum.BEGIN;
                if (!workOrder.TrackInTime.HasValue)
                {
                    workOrder.TrackInTime = DateTime.Now;
                }
            }
            else if (req.TaskStatus == TaskStatusEnum.FINISH)
            {
                var finishedTasks = await _domainService.QueryAsync(x => x.WorkOrderCode == entity.WorkOrderCode && x.TaskStatus == TaskStatusEnum.FINISH, x => x.Id, OrderByType.Desc);
                workOrder.QuantityProduced = finishedTasks.Sum(x => x.NowWadCount * entity.PanelCount);

                //排产数量
                if (workOrder.QuantityProduced > workOrder.QuantityScheduled)
                {
                    workOrder.QuantityProduced = workOrder.QuantityScheduled;
                }

                //工单下，其他任务都已完工时
                if (!await _domainService.IsExistAsync(t => t.WorkOrderCode == entity.WorkOrderCode && t.TaskStatus != TaskStatusEnum.FINISH))
                {
                    workOrder.ManuOrderStatus = ManuOrderStatusEnum.FINISH;
                    workOrder.TrackOutTime = DateTime.Now;
                }
            }
            workOrder.ModifierId = UserId;
            workOrder.ModifyTime = DateTime.Now;
            var response = await _workOrderService.Update(workOrder);
            if (!response)
            {
                return Fail<string>("更新关联的生产工单失败");
            }

            return Success();
        }

        /// <summary>
        /// 中控调用修改信息（批量）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateListByOutSideBatch(UpdateTaskListByOutSideReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (req.UpdateTasks == null || req.UpdateTasks.Count == 0)
            {
                return Fail("UpdateTasks未填写!");
            }

            if (req.TaskStatus == null)
            {
                return Fail("任务状态未填写!");
            }

            if (req.TaskStatus != TaskStatusEnum.SENDING
                && req.TaskStatus != TaskStatusEnum.BUFFERED
                && req.TaskStatus != TaskStatusEnum.BEGIN
                && req.TaskStatus != TaskStatusEnum.FINISH)
            {
                return Fail("任务状态填写错误!");
            }

            List<Model.Entites.Mes.WorkTask> updateTasks = new List<Model.Entites.Mes.WorkTask>();
            List<WorkOrder> updateWorkOrders = new List<WorkOrder>();
            Dictionary<int, decimal?> dicWorkOrder = new Dictionary<int, decimal?>();

            foreach (var item in req.UpdateTasks)
            {
                if (!await _domainService.IsExistAsync(p => p.Code == item.Code))
                {
                    return Fail(item.Code + " 信息不存在!");
                }

                var entity = await _domainService.FindSingleAsync(p => p.Code == item.Code);
                if (entity == null)
                {
                    return Fail(item.Code + " 信息不存在!");
                }

                if (!(
                //设置开始
                ((entity.TaskStatus == TaskStatusEnum.COMMITED || entity.TaskStatus == TaskStatusEnum.SENDING || entity.TaskStatus == TaskStatusEnum.BUFFERED)
                    && req.TaskStatus == TaskStatusEnum.BEGIN
                 )
                ||
                //设置完成
                (entity.TaskStatus == TaskStatusEnum.BEGIN
                    && req.TaskStatus == TaskStatusEnum.FINISH)
                ))
                {
                    return Fail(item.Code + " 当前数据的状态不支持此操作!");
                }

                if (!await _workOrderService.IsExistAsync(x => x.Code.ToLower() == entity.WorkOrderCode.ToLower()))
                {
                    return Fail(item.Code + " 找不到对应的生产工单!");
                }

                var workModel = await _workOrderService.FindSingleAsync(x => x.Code.ToLower() == entity.WorkOrderCode.ToLower());
                if (workModel == null)
                {
                    return Fail(item.Code + " 找不到对应的生产工单!");
                }

                entity.ModifierId = UserId;
                entity.ModifyTime = DateTime.Now;
                entity.TaskStatus = req.TaskStatus;

                if (item.NowWadCount > 0)
                {
                    entity.NowWadCount = item.NowWadCount;
                }
                if (req.TaskStatus == TaskStatusEnum.BEGIN)
                {
                    entity.IsStarted = 1;
                    workModel.ManuOrderStatus = ManuOrderStatusEnum.BEGIN;
                    workModel.ModifierId = UserId;
                    workModel.ModifyTime = DateTime.Now;
                    updateWorkOrders.Add(workModel);

                    logger.LogWarning($"任务投产时，工单的状态：{workModel.ManuOrderStatus.ToString()}");
                    entity.RealStartTime = DateTime.Now;
                }
                else if (req.TaskStatus == TaskStatusEnum.FINISH)
                {
                    if (dicWorkOrder.ContainsKey((int)workModel.Id))
                    {
                        dicWorkOrder[(int)workModel.Id] += entity.NowWadCount * entity.PanelCount;

                    }
                    else
                    {
                        dicWorkOrder.Add((int)workModel.Id, entity.NowWadCount * entity.PanelCount);
                    }
                    entity.RealEndTime = DateTime.Now;
                }

                updateTasks.Add(entity);

            }

            _unitOfWork.BeginTran();
            var result = await _domainService.BulkUpdate(updateTasks);
            if (!result)
            {
                return Fail<string>("更新任务表失败");
            }

            if (req.TaskStatus == TaskStatusEnum.FINISH && dicWorkOrder.Count > 0)
            {
                foreach (var item in dicWorkOrder)
                {
                    var model = await _workOrderService.QueryByID(item.Key);
                    if (model == null)
                    {
                        continue;
                    }
                    model.QuantityProduced += item.Value;
                    model.ModifierId = UserId;
                    model.ModifyTime = DateTime.Now;
                    //工单下，其他任务都已完工时
                    if (!await _domainService.IsExistAsync(t => t.WorkOrderCode == model.Code && t.TaskStatus != TaskStatusEnum.FINISH))
                    {
                        model.ManuOrderStatus = ManuOrderStatusEnum.FINISH;
                    }

                    updateWorkOrders.Add(model);
                }
            }

            var response = await _workOrderService.BulkUpdate(updateWorkOrders);
            if (!response)
            {
                return Fail<string>("更新关联的生产工单失败");
            }

            _unitOfWork.CommitTran();

            return Success();
        }

        /// <summary>
        /// 根据空闲时间，查询适合的机台集合
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<FitWorkStationDto>>> GetFitWorkStationList(GetFitWorkStationListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _taskDomainService.GetFitWorkStationList(req);
            return Success(result);
        }

        public async Task<ResponseDto<string>> BulkAddDrillTaskByMOList(List<AddOrUpdateDrillWorkOrderReq> req)
        {
            if (req == null)
            {
                return Fail("未传入有效数据");
            }
            if (req.Any(order => !order.RequestDate.HasValue || !order.IsUrgent.HasValue || string.IsNullOrEmpty(order.WorkOrderCode)))
            {
                return Fail("未传入有效数据");
            }

            string wo = string.Empty;
            string drillWo = string.Empty;
            var wolst = req.OrderByDescending(order => order.IsUrgent).ThenBy(order => order.RequestDate).ToList();

            if (wolst != null)
            {
                foreach (var item in wolst)
                {
                    var result = await _drillWorkOrderService.AddData(item);
                    if (result.Code.ToString() != "Success")
                    {
                        string temp = item.WorkOrderCode + ":" + result.Message.ToString() + " ";
                        drillWo = temp + "," + drillWo;
                        //continue;
                    }
                    var res = await BulkAddDrillTask(item);
                    if (res.Code.ToString() != "Success")
                    {
                        string temp = item.WorkOrderCode + ":" + res.Message.ToString() + " ";
                        wo = temp + "," + wo;
                        continue;
                    }
                }
            }

            if (wo.Length > 0 && wo != "true")
            {
                if (drillWo.Length > 0)
                {
                    drillWo = drillWo.Substring(0, (drillWo.Length - 1)) + $"生成钻孔工单失败";
                }
                return Fail(wo.Substring(0, (wo.Length - 1)) + $"生成钻孔任务失败!" + drillWo);
            }
            return Success();
        }
        /// <summary>
        /// 获取钻机的生产任务Model,
        /// 增加限制，新任务的物料，必须和已有生料的料号一致；
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="realNeedCount">钻机本次呼叫的数量，0表示未限制；如果所给数量少于初始分配的数量时，将多出来的数量转移到其他任务或者新增任务中</param>
        /// <param name="existRawNum">钻机本次呼叫时，已有的生料数量</param>
        /// <param name="spindleUseNum">钻机本次呼叫时，使用的轴数</param>
        /// <returns></returns>
        public async Task<ResponseDto<TaskDto>> GetNextTask(string deviceId, int realNeedCount, int existRawNum, int spindleUseNum, IReadOnlyList<string> undrilledItemCodesFromDrill)
        {
            var includedTaskStatuses = new List<TaskStatusEnum> { TaskStatusEnum.COMMITED, TaskStatusEnum.SENDING };
            var where = PredicateBuilder.True<WorkTask>();
            where = where.And(x => includedTaskStatuses.Contains(x.TaskStatus.Value) && !string.IsNullOrEmpty(x.WorkStationCode) && x.WorkStationCode.ToLower() == deviceId.ToLower());

            var tasks = await _domainService.QueryAsync(where, x => x.StartTime, OrderByType.Asc);
            var task = tasks.FirstOrDefault();

            if (task != null)
            {
                if (undrilledItemCodesFromDrill.Any(x => !string.IsNullOrEmpty(x))
                    && undrilledItemCodesFromDrill.FirstOrDefault(x => !string.IsNullOrEmpty(x)).ToUpper() != task.ItemCode.ToUpper())
                {
                    return Fail<TaskDto>($"生料仓中的料号【{undrilledItemCodesFromDrill.FirstOrDefault(x => !string.IsNullOrEmpty(x)).ToUpper()}】，" +
                        $"与下个任务的料号【{task.ItemCode.ToUpper()}】 不一致。");
                }

                // 如果请求数量 少于任务数量时，将任务中的多余数量拆分到其他任务
                if (realNeedCount + existRawNum < task.NowWadCount && realNeedCount > 0)
                {
                    logger.LogWarning($"因请求数量：{realNeedCount} + 已有生料数量 {existRawNum} 少于任务数量：{task.NowWadCount}，调整任务数量为请求数量");
                    _unitOfWork.BeginTran();
                    var ricePanelCount = task.NowWadCount - realNeedCount - existRawNum;
                    task.NowWadCount = realNeedCount + existRawNum;
                    task.ModifyTime = DateTime.Now;
                    await _domainService.Update(task);

                    if (await _domainService.IsExistAsync(x => x.WorkOrderCode == task.WorkOrderCode
                        && !string.IsNullOrEmpty(x.WorkStationCode) && x.WorkStationCode.ToLower() == deviceId.ToLower()
                        && x.TaskStatus == TaskStatusEnum.COMMITED
                        && x.Code != task.Code))
                    {
                        var anotherTask = (await _domainService.QueryAsync(x => x.WorkOrderCode == task.WorkOrderCode
                            && !string.IsNullOrEmpty(x.WorkStationCode) && x.WorkStationCode.ToLower() == deviceId.ToLower()
                            && x.TaskStatus == TaskStatusEnum.COMMITED
                            && x.Code != task.Code
                            , x => x.StartTime, OrderByType.Asc)).FirstOrDefault();

                        anotherTask.NowWadCount = anotherTask.NowWadCount + ricePanelCount;
                        await _domainService.Update(anotherTask);
                    }
                    else
                    {
                        var newTaskCode = _encodeBuildRulesService.GetEncodeList(
                            new GetEncodeByRulesListReq
                            {
                                RulesCode = "TASK_CODE",
                                BuildCount = 1,
                            }).Result.Data.FirstOrDefault();

                        //基于原有的任务，拆分新任务，复制原有的属性
                        var model = ReplicateTask(task, ricePanelCount, newTaskCode);
                        await _domainService.Add(model);
                    }
                    _unitOfWork.CommitTran();
                    return Success(_mapper.Map<TaskDto>(task));
                }
                else
                // 如果未设置实际需求数量，或者 需求数量等于任务数量时，直接返回此任务
                // 如果申请数量  大于任务数量时，按任务现有数量下发              
                {
                    return Success(_mapper.Map<TaskDto>(task));
                }
            }

            return Fail<TaskDto>("没有安排生产任务");
        }

        /// <summary>
        /// 转发任务到其他工作站
        /// todo：在线设备轴数一样
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> TransferTask(TransferTaskReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误！");
            }
            if (req.TaskIds == null || req.TaskIds.Count == 0)
            {
                return Fail("未识别有效的任务ID！");
            }
            if (!await _workstationDomainService.IsExistAsync(p => p.Id == req.WorkStationId))
            {
                return Fail("工作站ID不存在！");
            }

            List<RationalizeTaskReq> rationalizeTaskReqList = new List<RationalizeTaskReq>();
            List<Model.Entites.Mes.WorkTask> updateList = new List<Model.Entites.Mes.WorkTask>();
            foreach (var id in req.TaskIds)
            {
                var taskModel = await _domainService.QueryByID(id);
                if (taskModel == null)
                {
                    continue;
                }
                if (taskModel.TaskStatus != TaskStatusEnum.DRAFT)
                {
                    return Fail(taskModel.Code + " 任务状态不是草稿状态，不支持转发！");
                }

                taskModel.WorkStationId = req.WorkStationId;
                taskModel.WorkStationCode = req.WorkStationCode;
                taskModel.WorkStationName = req.WorkStationName;
                taskModel.ModifierId = UserId;
                taskModel.ModifyTime = DateTime.Now;

                updateList.Add(taskModel);

                if (!rationalizeTaskReqList.Exists(p => p.WorkOrderCode == taskModel.WorkOrderCode))
                {
                    rationalizeTaskReqList.Add(new RationalizeTaskReq
                    {
                        WorkOrderCode = taskModel.WorkOrderCode,
                        ProcessCode = taskModel.WorkOrderCode,
                        ItemId = taskModel.ItemId,
                    });
                }
            }

            _unitOfWork.BeginTran();

            var result = await _domainService.BulkUpdate(updateList);
            if (!result)
            {
                return Fail("转发任务失败！");
            }

            if (rationalizeTaskReqList.Count > 0)
            {
                foreach (var item in rationalizeTaskReqList)
                {
                    await RationalizeTask(item);  //理顺转发后的任务
                }
            }

            await Commit(new CommitTaskReq
            {
                Ids = req.TaskIds,
                TaskStatus = TaskStatusEnum.COMMITED
            });

            _unitOfWork.CommitTran();

            return Success();
        }

        /// <summary>
        /// 判断多个任务是否属于同一个工艺路线、同一个工序
        /// </summary>
        /// <param name="taskList"></param>
        /// <returns></returns>
        public async Task<ResponseDto<RouteAndProcessDtoByTask>> VerifyTaskBelongOneRouteAndProcess(List<TaskDto> taskList)
        {
            if (taskList == null || taskList.Count == 0)
            {
                return Fail<RouteAndProcessDtoByTask>("信息格式错误!");
            }
            var model = taskList[0];
            if (model == null)
            {
                return Fail<RouteAndProcessDtoByTask>("信息格式错误!");
            }

            var isExsit = taskList.Exists(p => p.ProcessCode != model.ProcessCode);
            if (isExsit)
            {
                return Fail<RouteAndProcessDtoByTask>("选中的任务不属于同一个工序!");
            }

            string routeCode = string.Empty;
            var distictWorkOderTasks = taskList.DistinctBy(p => p.WorkOrderId).ToList();
            foreach (var item in distictWorkOderTasks)
            {
                if (item.WorkOrderId == null)
                {
                    return Fail<RouteAndProcessDtoByTask>(item.Code + " 找不到对应的工单!");
                }
                var workOrderModel = await _workOrderService.QueryByID(item.WorkOrderId);
                if (workOrderModel == null)
                {
                    return Fail<RouteAndProcessDtoByTask>(item.Code + " 找不到对应的工单!");
                }

                if (string.IsNullOrEmpty(workOrderModel.RouteCode))
                {
                    return Fail<RouteAndProcessDtoByTask>(item.Code + " 对应的工单找不到工艺路线!");
                }

                if (string.IsNullOrEmpty(routeCode))
                {
                    routeCode = workOrderModel.RouteCode;
                }
                else if (routeCode != workOrderModel.RouteCode)
                {
                    return Fail<RouteAndProcessDtoByTask>("选中的任务不属于同一个工艺路线!");
                }
            }

            RouteAndProcessDtoByTask result = new RouteAndProcessDtoByTask();
            result.ProcessCode = model.ProcessCode;
            result.RouteCode = routeCode;

            return Success(result);
        }

        /// <summary>
        /// 基于原有的任务，拆分新任务，复制原有的属性
        /// </summary>
        /// <param name="task"></param>
        /// <param name="ricePanelCount">新任务的数量</param>
        /// <param name="newTaskCode">新任务代号</param>
        /// <returns></returns>
        private Model.Entites.Mes.WorkTask ReplicateTask(Model.Entites.Mes.WorkTask? task, decimal? ricePanelCount, string? newTaskCode)
        {
            var model = new Model.Entites.Mes.WorkTask();
            model.Name = model.Code = newTaskCode;
            model.WorkOrderId = task.WorkOrderId;
            model.WorkOrderName = task.WorkOrderName;
            model.WorkOrderCode = task.WorkOrderCode;
            model.ClientId = task.ClientId;
            model.ClientCode = task.ClientCode;
            model.ClientName = task.ClientName;
            model.BatchCode = task.BatchCode;
            model.ParentId = 0;
            model.ItemId = task.ItemId;
            model.ItemName = task.ItemName;
            model.ItemCode = task.ItemCode;
            model.ItemTypeId = task.ItemTypeId;
            model.Specification = task.Specification;
            model.UnitOfMeasure = task.UnitOfMeasure;
            model.TaskStatus = task.TaskStatus;
            model.Quantity = ricePanelCount;
            model.NowWadCount = ricePanelCount;
            model.PanelCount = task.PanelCount;
            model.QuantityProduced = 0;
            model.QuantityUnquanlify = 0;
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            model.WorkStationCode = task.WorkStationCode;
            model.WorkStationId = task.WorkStationId;
            model.WorkStationName = task.WorkStationName;
            model.ProcessCode = task.ProcessCode;
            model.ProcessId = task.ProcessId;
            model.ProcessName = task.ProcessName;
            model.Color = task.Color;
            model.KeyFlag = task.KeyFlag;
            model.Duration = task.Duration;
            model.IsStarted = 0;
            model.StartTime = task.StartTime.Value.AddMinutes(5);
            model.EndTime = task.EndTime.Value.AddMinutes(5);
            model.RouteId = task.RouteId;
            model.RouteName = task.RouteName;
            model.RouteCode = task.RouteCode;
            model.IsUrgent = task.IsUrgent;
            model.SpecGroup = task.SpecGroup;
            model.IncodeNumber = task.IncodeNumber;
            model.LayerNum = task.LayerNum;
            model.AfterDrillFilePath = task.AfterDrillFilePath;
            model.BeforeDrillFilePath = task.BeforeDrillFilePath;
            model.IsRebrush = task.IsRebrush;
            model.BarCode = task.BarCode;
            return model;
        }

        /// <summary>
        /// 根据设备编码查询Commit状态的任务
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<TaskDto>>> GetTaskByDevice(GetDrillOrAgvDeviceInfoReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _taskDomainService.GetTaskByDevice(req);
            return Success(result);
        }

        public async Task<List<AddOrUpdateDrillWorkOrderReq>> BulkAddDrillTaskByMOListRequestHandle(List<AddOrUpdateDrillWorkOrderReq> req)
        {
            var workOrderItemCodes = req.Where(s => !string.IsNullOrWhiteSpace(s.ItemCode)).Select(s => s.ItemCode.ToLower()).Distinct().ToList();

            var externalWorkOrders = await _externalWorkOrderService.QueryAsync(s => workOrderItemCodes.Contains(s.ItemCode.ToLower()),
                                                                                        p => p.Id, OrderByType.Asc);
            if (externalWorkOrders?.Count > 0)
            {
                foreach (var item in req)
                {
                    var externalWorkOrder = externalWorkOrders.FirstOrDefault(s => s.ItemCode.ToLower() == item.ItemCode.ToLower());
                    if (externalWorkOrder != null)
                    {
                        item.SingleTripTime = 40;
                        item.AllPassesCount = item.ShaftCount > 0 ? Math.Ceiling((decimal)item.WadCount / (decimal)item.ShaftCount) : 0;
                        item.DrillAllTime = item.SingleTripTime * item.AllPassesCount;
                    }
                }
            }
            return req;
        }
        public async Task<ResponseDto<string>> BulkAddDrillTaskByMOList(List<AddOrUpdateDrillWorkOrderReq> req, bool isReqHandle = false)
        {
            if (req == null)
            {
                return Fail("未传入有效数据");
            }
            if (req.Any(order => !order.RequestDate.HasValue || !order.IsUrgent.HasValue || string.IsNullOrEmpty(order.WorkOrderCode)))
            {
                return Fail("未传入有效数据");
            }
            if (isReqHandle)
            {
                req = await BulkAddDrillTaskByMOListRequestHandle(req);
            }
            string wo = string.Empty;
            string drillWo = string.Empty;
            var wolst = req.OrderByDescending(order => order.IsUrgent).ThenBy(order => order.RequestDate).ToList();

            if (wolst != null)
            {
                foreach (var item in wolst)
                {
                    var result = await _drillWorkOrderService.AddData(item);
                    if (result.Code.ToString() != "Success")
                    {
                        string temp = item.WorkOrderCode + ":" + result.Message.ToString() + " ";
                        drillWo = temp + "," + drillWo;
                        //continue;
                    }
                    var res = await BulkAddDrillTask(item);
                    if (res.Code.ToString() != "Success")
                    {
                        string temp = item.WorkOrderCode + ":" + res.Message.ToString() + " ";
                        wo = temp + "," + wo;
                        continue;
                    }
                }
            }

            if (wo.Length > 0 && wo != "true")
            {
                if (drillWo.Length > 0)
                {
                    drillWo = drillWo.Substring(0, (drillWo.Length - 1)) + $"生成钻孔工单失败";
                }
                return Fail(wo.Substring(0, (wo.Length - 1)) + $"生成钻孔任务失败!" + drillWo);
            }
            return Success();
        }

        /// <summary>
        /// 任务拖动
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> MoveTaskList(List<AddOrUpdateTaskReq> req)
        {
            if (req == null || req.Count() == 0)
            {
                return Fail("数据格式错误");
            }

            // 防呆校验：检查产品板长与目标工作站设备最大板长
            var enablePanelLengthValidation = await _sysConfigManager.GetBoolValue("EnablePanelLengthValidation");
            if (enablePanelLengthValidation)
            {
                var validationResult = await ValidateTaskPanelLength(req);
                if (validationResult.Code != ResponseCode.Success)
                {
                    return validationResult;
                }
            }

            var msg = "";
            try
            {
                var StatrtData = req[0].StartTime.Value.Date;
                var dates = req[0].StartTime.Value.Date;
                var autoGenerateSwitch = await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_AUTO_CUTTER);
                List<Model.Entites.Mes.WorkTask> allTask = _taskDomainService.QueryAsync(t => t.IsDeleted == 0 && t.WorkStationCode == req[0].WorkStationCode
                          && dates <= t.EndTime.Value.Date, t => t.EndTime, SqlSugar.OrderByType.Asc).Result.OrderBy(t => t.EndTime).ToList();
                var taskIds = req.Where(s => s.Id > 0).Select(s => s.Id).Distinct().ToList();
                //源任务
                var originTasks = await _taskDomainService.QueryAsync(s => taskIds.Contains(s.Id), t => t.EndTime, OrderByType.Asc);
                logger.LogInformation($"任务分布改变前的任务信息：{JsonSerializer.Serialize(originTasks)}");
                if (originTasks?.Count > 0)//源任务不能是被锁定的
                {
                    var cutterGroupTaskCodes = originTasks.Where(s => !string.IsNullOrWhiteSpace(s.CutterGroupNo)).Select(s => s.Code ?? "").Distinct().ToList();
                    if (cutterGroupTaskCodes?.Count > 0 && autoGenerateSwitch)
                    {
                        var result = await _cutterGroupService.IsLockedByTaskCode(cutterGroupTaskCodes);
                        if (result.IsLoked)
                        {
                            return Fail($"{string.Join(",", result.LockedTaskCodes)}任务已被配刀计划锁定，不能更改");
                        }
                    }
                }
                List<Model.Entites.Mes.WorkTask> beforeTask = new List<Model.Entites.Mes.WorkTask>();
                List<Model.Entites.Mes.WorkTask> afterTask = new List<Model.Entites.Mes.WorkTask>();
                List<Model.Entites.Mes.WorkTask> temLst = new List<Model.Entites.Mes.WorkTask>();

                DateTime? targetTime = req[0].StartTime.Value;
                DateTime? endTimeTemp = targetTime;

                if (targetTime < DateTime.Now)
                {
                    return Fail("目标任务时间错误，不能拖动");
                }

                if (req[0].DataId != 1)//DataId = 1 表示批量拖动整个格子， DataId = 0 表示批量拖动时，目标位置没有Task
                {
                    if (req[0].DataId == 0)//拖动的task目标位置没有Task
                    {
                        targetTime = await GetStatrTime(req[0]);
                    }
                    else
                    {
                        targetTime = allTask.ToList().Where(t => t.Id == req[0].DataId).Select(t => t.EndTime).ToList().First().Value;
                        targetTime = targetTime > DateTime.Now ? targetTime : DateTime.Now;
                    }
                }
                else  //拖动整个格子的task，放在目标格子Task的后面
                {
                    if (allTask.Count() > 0)
                    {
                        var tempTask = allTask.Where(t => t.StartTime.Value.Date == req[0].StartTime.Value.Date).ToList();
                        if (tempTask.Count() == 0)
                        {
                            var targetTimeTemp = allTask.Where(t => t.StartTime.Value.Date == req[0].StartTime.Value.Date.AddDays(-1)).ToList();
                            if (targetTimeTemp.Count() > 0)
                            {
                                targetTime = targetTimeTemp.Select(t => t.StartTime).Max().Value;
                                var tempDuration = allTask.Where(t => t.StartTime == targetTime).ToList().Select(t => t.Duration).First();
                                if (targetTime != null)
                                {
                                    targetTime = targetTime.Value.AddMinutes((double)tempDuration);
                                }
                            }
                            else
                            {
                                targetTime = req[0].StartTime.Value.Date;
                            }
                        }
                        else
                        {
                            targetTime = allTask.Where(t => t.StartTime.Value.Date == req[0].StartTime.Value.Date).ToList().Select(t => t.StartTime).Max().Value;
                            var tempDuration = allTask.Where(t => t.StartTime == targetTime).ToList().Select(t => t.Duration).First();
                            if (targetTime != null)
                            {
                                targetTime = targetTime.Value.AddMinutes((double)tempDuration);
                            }
                        }
                        targetTime = targetTime > DateTime.Now ? targetTime : DateTime.Now;
                    }
                    else
                    {
                        targetTime = await GetStatrTime(req[0]);
                    }
                }

                //将要拖动的所有Task装进List中
                for (int i = 0; i < req.Count(); i++)
                {
                    req[i].StartTime = targetTime.Value.AddMinutes((double)req[i].Duration * i);
                    if (StatrtData < req[i].StartTime.Value.Date)
                    {
                        msg = "目标日期已排满，已向后顺延";
                    }
                    req[i].EndTime = req[i].StartTime.Value.AddMinutes((double)req[i].Duration);
                    beforeTask.Add(_mapper.Map<Model.Entites.Mes.WorkTask>(req[i]));
                }
                int beforeTaskCount = beforeTask.Count();

                //取目标站点全部有Task的日期
                var dateDayList = allTask.DistinctBy(t => new { t.StartTime.Value.Date }).Select(t => t.EndTime.Value.Date).Distinct().ToList();
                if (req[0].DataId == 1) //拖动整个格子的task，放在目标格子Task的后面(遥控器按键图形拖动)
                {
                    for (int i = 0; i < dateDayList.Count(); i++)
                    {
                        var tempLst = allTask.Where(t => t.StartTime.Value.Date == dateDayList[i]).ToList();
                        var tempMin = tempLst.Select(t => t.StartTime).Min() > dateDayList[i].Date ? tempLst.Select(t => t.StartTime).Min() : dateDayList[i].Date;
                        var tempMax = tempLst.Select(t => t.StartTime).Max() > dateDayList[i].Date ? tempLst.Select(t => t.StartTime).Max() : dateDayList[i].Date;

                        if (i == 0)
                        {
                            afterTask.Clear();
                            var tepm = beforeTask.OrderBy(t => t.StartTime).ToList().Last();
                            if (tepm.EndTime.Value.Date > dateDayList[i].Date)
                            {
                                continue;
                            }
                            afterTask.Add(tepm);
                        }
                        else
                        {
                            var temps = allTask.Where(t => t.StartTime.Value.Date == dateDayList[i].Date.AddDays(-1)).OrderBy(t => t.StartTime).ToList();
                            if (temps.Count() > 0)
                            {
                                afterTask.Add(allTask.Where(t => t.StartTime.Value.Date == dateDayList[i].Date.AddDays(-1)).OrderBy(t => t.StartTime).ToList().Last());
                            }

                        }

                        var lastEnd = afterTask[0].StartTime.Value.AddMinutes((double)afterTask[0].Duration);
                        if (beforeTask.Count() > 0)
                        {
                            var lastTempEnd = beforeTask.Select(t => t.EndTime).Max().Value;
                            lastEnd = lastTempEnd > lastEnd ? lastTempEnd : lastEnd;
                        }

                        if (tempMin > lastEnd)
                        {
                            break;
                        }
                        else
                        {
                            if (i > 0)
                            {
                                int tempCount = 0;
                                for (int j = 0; j < tempLst.Count(); j++)
                                {
                                    var tempIdLiet = req.ToList().Select(t => t.Id);
                                    if (tempIdLiet.Contains(tempLst[j].Id))
                                    {
                                        tempCount++;
                                        continue;
                                    }

                                    tempLst[j].StartTime = beforeTask[0].StartTime.Value.AddMinutes((double)tempLst[j].Duration * (beforeTaskCount + j - tempCount));
                                    tempLst[j].EndTime = tempLst[j].StartTime.Value.AddMinutes((double)tempLst[j].Duration);
                                    if (StatrtData < tempLst[j].StartTime.Value.Date)
                                    {
                                        msg = "目标日期已排满，已向后顺延";
                                    }
                                    temLst.Add(tempLst[j]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var targetStartTime = DateTime.Now;
                    var targetTask = allTask.Where(t => t.Id == req[0].DataId).ToList();
                    if (req[0].DataId > 0)
                    {
                        targetStartTime = targetTask.Select(t => t.StartTime).First().Value.AddMinutes((double)targetTask.Select(t => t.Duration).First());
                    }

                    targetStartTime = targetStartTime <= req[0].StartTime.Value.Date ? req[0].StartTime.Value.Date : targetStartTime;
                    afterTask.Clear();
                    for (int i = 0; i < dateDayList.Count(); i++)
                    {
                        if (i == 0)
                        {
                            var temp = allTask.Where(t => t.StartTime.Value >= targetStartTime && t.StartTime.Value.Date == dateDayList[i]).ToList();
                            for (int j = 0; j < temp.Count(); j++)
                            {
                                if (beforeTask.Count() > 0)
                                {
                                    var maxBeforeTask = beforeTask.Select(t => t.EndTime).Max().Value;
                                    if (temp[j].StartTime >= maxBeforeTask)
                                    {
                                        break;
                                    }
                                }

                                if (j > 0 && temp[j].StartTime >= temp[j - 1].EndTime)
                                {
                                    break;
                                }

                                temp[j].StartTime = targetTime.Value.AddMinutes((double)temp[j].Duration * (beforeTaskCount + j));
                                if (StatrtData < temp[j].StartTime.Value.Date)
                                {
                                    msg = "目标日期已排满，已向后顺延";
                                }
                                temp[j].EndTime = temp[j].StartTime.Value.AddMinutes((double)temp[j].Duration);
                                beforeTask.Add(temp[j]);
                            }
                        }
                        else
                        {
                            var tempLst = allTask.Where(t => t.StartTime.Value.Date == dateDayList[i]).ToList();
                            var tempMin = tempLst.Select(t => t.StartTime).Min() > dateDayList[i].Date ? tempLst.Select(t => t.StartTime).Min() : dateDayList[i].Date;
                            var temp = beforeTask.Select(t => t.StartTime).Max().Value;
                            temp = temp == null ? dateDayList[i].Date : temp;
                            int s = i;
                            if (temp >= tempMin)
                            {
                                int tempCount = 0;
                                for (int j = 0; j < tempLst.Count(); j++)
                                {
                                    if (j > 0 && tempLst[j].StartTime > tempLst[j - 1].EndTime)
                                    {
                                        break;
                                    }
                                    if (tempLst[j].Id == req[0].Id)
                                    {
                                        tempCount++;
                                        continue;
                                    }

                                    tempLst[j].StartTime = targetTime.Value.AddMinutes((double)tempLst[j].Duration * (beforeTaskCount + j - tempCount));
                                    tempLst[j].EndTime = tempLst[j].StartTime.Value.AddMinutes((double)tempLst[j].Duration);
                                    if (StatrtData < tempLst[j].StartTime.Value.Date)
                                    {
                                        msg = "目标日期已排满，已向后顺延";
                                    }
                                    temLst.Add(tempLst[j]);
                                }
                            }
                        }
                    }
                }

                afterTask.Clear();
                afterTask = beforeTask.Concat(temLst).ToList();
                logger.LogInformation($"任务分布改变后的任务信息：{JsonSerializer.Serialize(afterTask)}");
                _unitOfWork.BeginTran();
                await _domainService.BulkUpdate(afterTask);
                _unitOfWork.CommitTran();

                // 自动切换工艺路线：任务移动成功后根据目标工作站获取绑定的工艺路线
                var routeSwitchResult = await AutoSwitchRouteForTasks(req);
                if (routeSwitchResult.Code != ResponseCode.Success)
                {
                    logger.LogWarning($"工艺路线切换失败: {routeSwitchResult.Message}");
                }

                if (afterTask?.Count > 0 && autoGenerateSwitch)
                {
                    var destinationDevices = req.Where(s => !string.IsNullOrWhiteSpace(s.WorkStationCode)).Select(s => s.WorkStationCode).Distinct().ToList();
                    //涉及到的任务，先提取所属钻机信息，再删除掉钻机下未锁定的配刀组信息，之后由job重新生成
                    logger.LogInformation($"任务分布配刀组计划处理：涉及到的任务：{JsonSerializer.Serialize(originTasks)},目标钻机：{JsonSerializer.Serialize(destinationDevices)}");
                    await CutterGroupHandle(originTasks, destinationDevices!);
                    await CancelSchedule(originTasks.Select(p => p.WorkStationCode).Concat(destinationDevices).Distinct().ToList()!);
                }

                return Success(msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                logger.LogError($"MoveTask:Error:{ex.Message}", ex);
            }

            return Fail($"任务移动失败:{msg}");
        }

        private async Task CutterGroupHandle(List<WorkTask> tasks, List<string>? destinationDevices = default)
        {
            var autoGenerateSwitch = await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_AUTO_CUTTER);
            if (!autoGenerateSwitch)
            {
                return;
            }
            //调整后的任务，需要清除掉对应的配刀分组，之后job会重新分配
            var drillNos = tasks.Where(s => !string.IsNullOrWhiteSpace(s.WorkStationCode)).Select(s => s.WorkStationCode ?? "").Distinct().ToList();
            if (destinationDevices?.Count > 0)
            {
                drillNos.AddRange(destinationDevices);
            }
            logger.LogInformation($"需要删除的配刀组计划的钻机：{JsonSerializer.Serialize(drillNos)}");
            if (drillNos?.Count <= 0)
            {
                return;
            }
            var data = await _cutterGroupService.GetList(new Model.ViewModels.Mes.CutterGroup.Req.GetListReq()
            {
                DrillNos = drillNos,
                CutterGroupStatus = 0,
                PageNum = 1,
                PageSize = int.MaxValue,
            });
            if (data.Code == ResponseCode.Success && data?.Data.List?.Count > 0)
            {
                var cutterGroupNos = data.Data.List.Select(s => s.GroupNo).Distinct().ToList();
                await _cutterGroupService.DeleteItemAndDetails(cutterGroupNos!);
            }
        }
        private async Task<DateTime> GetStatrTime(AddOrUpdateTaskReq req)
        {
            DateTime temp = DateTime.Now.Date;
            var rest = _taskDomainService.QueryAsync(t => t.IsDeleted == 0 && t.WorkStationCode == req.WorkStationCode
                                       && req.StartTime.Value.Date.AddDays(-1).Date == t.EndTime.Value.Date, t => t.EndTime, SqlSugar.OrderByType.Asc).Result.OrderBy(t => t.EndTime).ToList();
            if (rest.Count() > 0)
            {

                var tempTime = rest.Select(t => t.StartTime).Max().Value;
                var tempDuration = rest.Where(t => t.StartTime == tempTime).Select(t => t.Duration).First().Value;
                tempTime = tempTime.AddMinutes((double)tempDuration);
                tempTime = tempTime > DateTime.Now ? tempTime : DateTime.Now;
                if (tempTime.Date < req.StartTime.Value.Date)
                {
                    temp = req.StartTime.Value.Date;
                }
                else
                {
                    temp = tempTime;
                }
            }
            else
            {
                temp = req.StartTime.Value.Date > DateTime.Now ? req.StartTime.Value.Date : DateTime.Now;
            }

            return temp;
        }
        private async Task CancelSchedule(List<string> drillNos)
        {
            if (drillNos == null || !drillNos.Any()) return;


            var where = PredicateBuilder.True<Schedule>();
            where = where.And(p => !string.IsNullOrEmpty(p.SourceDeviceId) && drillNos.Contains(p.SourceDeviceId));

            where = where.And(p => p.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                || p.ScheduledTaskStatus == ScheduledTaskStatus.Created);

            await CanceledSchedule(where);

            //var _newDrillScheduleStatus = new List<ScheduledTaskStatus>
            //{
            //    ScheduledTaskStatus.Created,
            //    ScheduledTaskStatus.PartCompleted
            //};
            //    var schedule = await _scheduleDomainService.QueryAsync(s => drillNos.Contains(s.SourceDeviceId)
            //                        && !string.IsNullOrEmpty(s.SourceDeviceId)
            //                        && _newDrillScheduleStatus.Contains((ScheduledTaskStatus)s.ScheduledTaskStatus), p => p.Id, OrderByType.Asc);

            //    var traceCodes = schedule?.Select(p => p.Code).ToList();

            //    if (traceCodes == null || !traceCodes.Any()) return;

            //    traceCodes.ForEach(code =>
            //    {
            //        if (!string.IsNullOrWhiteSpace(code))
            //        {
            //            var request = new CancelScheduleTaskRequest
            //            {
            //                TraceId = code,
            //                Params = new Dictionary<string, object?>
            //            {
            //                { "CancelReason", "任务分布移动，取消对应钻机调度" }
            //            }
            //            };

            //            _apiHelper.RequestData(_innerOptions.CancelScheduleUrl, "post", JsonSerializer.Serialize(request));
            //        }
            //    });
        }

        /// <summary>
        /// 防呆校验：检查任务产品板长与目标工作站设备最大板长的匹配
        /// </summary>
        /// <param name="req">任务移动请求列表</param>
        /// <returns></returns>
        private async Task<ResponseDto<string>> ValidateTaskPanelLength(List<AddOrUpdateTaskReq> req)
        {
            try
            {
                // 获取目标工作站编码
                var targetWorkStationCode = req.FirstOrDefault()?.WorkStationCode;
                if (string.IsNullOrEmpty(targetWorkStationCode))
                {
                    return Success(); // 如果没有目标工作站，跳过校验
                }

                // 获取需要校验的产品编码列表
                var itemCodes = req.Where(r => !string.IsNullOrEmpty(r.ItemCode))
                                  .Select(r => r.ItemCode)
                                  .Distinct()
                                  .ToList();

                if (!itemCodes.Any())
                {
                    return Success(); // 如果没有产品编码，跳过校验
                }

                // 获取产品板长信息
                var items = await _itemDomainService.QueryAsync(i => itemCodes.Contains(i.Code) && i.IsDeleted == 0, i => i.Id, OrderByType.Asc);
                if (items == null || !items.Any())
                {
                    return Success(); // 如果找不到产品信息，跳过校验
                }

                // 获取目标工作站关联的设备最大板长
                var db = _unitOfWork.GetDbClient();
                var deviceMaxLengths = await db.Queryable<Device, WorkStation>((d, w) => new object[]
                    {
                        JoinType.Inner, d.WorkStationId == w.Id
                    })
                    .Where((d, w) => d.IsDeleted == 0 && w.IsDeleted == 0)
                    .Where((d, w) => w.Code == targetWorkStationCode)
                    .Where((d, w) => d.MaxBoardLength.HasValue && d.MaxBoardLength > 0)
                    .Select((d, w) => new
                    {
                        DeviceCode = d.Code,
                        DeviceName = d.Name,
                        MaxBoardLength = d.MaxBoardLength,
                        WorkStationCode = w.Code,
                        WorkStationName = w.Name
                    })
                    .ToListAsync();

                if (deviceMaxLengths == null || !deviceMaxLengths.Any())
                {
                    return Success(); // 如果目标工作站没有配置设备或设备未设置最大板长，跳过校验
                }

                // 检查每个产品的板长是否超过设备的最大板长
                var maxAllowedLength = deviceMaxLengths.Max(d => d.MaxBoardLength.Value);
                var invalidItems = items.Where(item => item.PanelLength > maxAllowedLength).ToList();

                if (invalidItems.Any())
                {
                    var deviceInfo = string.Join("、", deviceMaxLengths.Select(d => $"{d.DeviceName}({d.DeviceCode})最大板长{d.MaxBoardLength}mm"));
                    var invalidItemInfo = string.Join("、", invalidItems.Select(i => $"{i.Name}({i.Code})板长{i.PanelLength}mm"));

                    return Fail($"移动任务失败：产品板长超过目标工作站设备的最大板长限制。\n" +
                               $"目标工作站：{targetWorkStationCode}\n" +
                               $"设备信息：{deviceInfo}\n" +
                               $"超长产品：{invalidItemInfo}\n" +
                               $"请选择适合的工作站或调整产品规格");
                }

                return Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "任务移动板长校验异常");
                return Fail($"板长校验异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 自动切换工艺路线：根据目标工作站获取绑定的工艺路线并更新任务
        /// </summary>
        /// <param name="req">任务移动请求</param>
        /// <returns></returns>
        private async Task<ResponseDto<string>> AutoSwitchRouteForTasks(List<AddOrUpdateTaskReq> req)
        {
            try
            {
                // 获取目标工作站编码
                var targetWorkStationCode = req.FirstOrDefault()?.WorkStationCode;
                if (string.IsNullOrEmpty(targetWorkStationCode))
                {
                    return Success(); // 如果没有目标工作站，跳过工艺路线切换
                }

                // 获取当前任务信息
                var taskIds = req.Where(s => s.Id > 0).Select(s => s.Id).Distinct().ToList();
                var originTasks = await _taskDomainService.QueryAsync(s => taskIds.Contains(s.Id), t => t.EndTime, OrderByType.Asc);

                if (originTasks == null || !originTasks.Any())
                {
                    return Success();
                }

                // 获取目标工作站绑定的工艺路线和工序信息
                var workstationRouteProcesses = await GetWorkstationRouteProcesses(targetWorkStationCode);
                if (workstationRouteProcesses == null || !workstationRouteProcesses.Any())
                {
                    return Success();
                }

                var tasksByProcess = originTasks.GroupBy(t => t.ProcessCode).ToList();
                var updatedTasks = new List<Model.Entites.Mes.WorkTask>();

                foreach (var processGroup in tasksByProcess)
                {
                    var processCode = processGroup.Key;
                    var tasks = processGroup.ToList();

                    // 查找该工序在目标工作站支持的工艺路线
                    var compatibleRoutes = workstationRouteProcesses
                        .Where(wrp => !string.IsNullOrEmpty(wrp.ProcessCode) &&
                                     wrp.ProcessCode.ToLower() == processCode.ToLower())
                        .GroupBy(wrp => wrp.RouteCode)
                        .Select(g => g.First())
                        .ToList();

                    if (!compatibleRoutes.Any())
                    {
                        continue;
                    }

                    // 选择工艺路线策略：
                    // 1. 优先选择任务当前使用的工艺路线（如果工作站支持）
                    // 2. 否则选择第一个支持的工艺路线
                    var targetRoute = compatibleRoutes.FirstOrDefault(r =>
                        tasks.Any(t => t.RouteCode == r.RouteCode)) ?? compatibleRoutes.First();

                    // 检查是否需要切换工艺路线
                    var currentRouteCodes = tasks.Select(t => t.RouteCode).Where(rc => !string.IsNullOrEmpty(rc)).Distinct().ToList();
                    if (currentRouteCodes.Contains(targetRoute.RouteCode))
                    {
                        continue;
                    }

                    // 更新任务的工艺路线信息
                    foreach (var task in tasks)
                    {
                        task.RouteId = targetRoute.RouteId;
                        task.RouteCode = targetRoute.RouteCode;
                        task.RouteName = targetRoute.RouteName;
                        task.ModifyTime = DateTime.Now;
                        task.ModifierId = UserId;
                        updatedTasks.Add(task);
                    }
                }

                // 批量更新需要修改的任务
                if (updatedTasks.Any())
                {
                    await _taskDomainService.BulkUpdate(updatedTasks);
                }

                return Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "自动切换工艺路线异常");
                return Fail($"工艺路线切换异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 根据工作站编码获取绑定的工艺路线和工序信息
        /// </summary>
        /// <param name="workStationCode">工作站编码</param>
        /// <returns></returns>
        private async Task<List<WorkstationRouteProcessInfo>> GetWorkstationRouteProcesses(string workStationCode)
        {
            try
            {
                var db = _unitOfWork.GetDbClient();
                var query = db.Queryable<RouteProcessAndWorkStation, RouteAndProcess, Route, WorkStation, Process>
                    ((rpw, rp, r, w, p) => new object[]
                    {
                        JoinType.Left, rpw.RouteAndProcessId == rp.Id,
                        JoinType.Left, rp.RouteId == r.Id,
                        JoinType.Left, rpw.WorkStationId == w.Id,
                        JoinType.Left, rp.ProcessId == p.Id,
                    });

                query = query.Where((rpw, rp, r, w, p) => rpw.IsDeleted == 0 && rp.IsDeleted == 0 && r.IsDeleted == 0 && w.IsDeleted == 0 && p.IsDeleted == 0);
                query = query.Where((rpw, rp, r, w, p) => !string.IsNullOrEmpty(w.Code) && w.Code.ToLower() == workStationCode.ToLower());

                var routeProcesses = await query
                    .Select((rpw, rp, r, w, p) => new WorkstationRouteProcessInfo
                    {
                        RouteId = r.Id,
                        RouteCode = r.Code,
                        RouteName = r.Name,
                        ProcessId = p.Id,
                        ProcessCode = p.Code,
                        ProcessName = p.Name,
                        RouteAndProcessId = rp.Id
                    })
                    .Distinct()
                    .ToListAsync();

                return routeProcesses ?? new List<WorkstationRouteProcessInfo>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"获取工作站 {workStationCode} 绑定的工艺路线和工序信息异常");
                return new List<WorkstationRouteProcessInfo>();
            }
        }


        /// <summary>
        /// 工作站工艺路线工序信息
        /// </summary>
        private class WorkstationRouteProcessInfo
        {
            public long? RouteId { get; set; }
            public string? RouteCode { get; set; }
            public string? RouteName { get; set; }
            public long? ProcessId { get; set; }
            public string? ProcessCode { get; set; }
            public string? ProcessName { get; set; }
            public long? RouteAndProcessId { get; set; }
        }
    }
}
