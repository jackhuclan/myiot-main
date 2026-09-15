using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockOverview;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockStorage;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProduceTask;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 钻孔任务
    /// </summary>
    public class DrillWorkOrderService : BaseServiceWithoutTree<DrillWorkOrder, DrillWorkOrderDto, AddOrUpdateDrillWorkOrderReq>, IDrillWorkOrderService
    {
        private readonly IWorkOrderDomainService _workOrderDomainService;
        private readonly ITaskDomainService _taskDomainService;
        private readonly IMaterialStockDomainService _materialStockDomainService;
        private readonly IMaterialStockStorageDomainService _materialStockStorageDomainService;
        private readonly IMaterialStockStorageHistoryDomainService _materialStockStorageHistoryDomainService;
        private readonly IProduceTaskDomainService _produceTaskDomainService;
        private readonly IProduceTaskHistoryDomainService _produceTaskHistoryDomainService;
        private readonly IMaterialStockOverviewDomainService _materialStockOverviewDomainService;
        private readonly IMaterialStockOverviewHistoryDomainService _materialStockOverviewHistoryDomainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemDomainService _itemDomainService;
        private readonly IWorkOrderAndWorkStationDomainService _workOrderAndWorkStationDomainService;
        private readonly IDeviceDomainService _deviceDomainService;
        private readonly ISysConfigManager _sysConfigManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        /// <param name="workOrderDomainService"></param>
        /// <param name="taskDomainService"></param>
        /// <param name="materialStockDomainService"></param>
        /// <param name="materialStockStorageDomainService"></param>
        /// <param name="materialStockStorageHistoryDomainService"></param>
        /// <param name="produceTaskDomainService"></param>
        /// <param name="produceTaskHistoryDomainService"></param>
        /// <param name="materialStockOverviewDomainService"></param>
        /// <param name="materialStockOverviewHistoryDomainService"></param>
        /// <param name="sysConfigManager"></param>
        public DrillWorkOrderService(IDrillTaskDomainService domainService,
            IMapper mapper,
            IWorkOrderDomainService workOrderDomainService,
            ITaskDomainService taskDomainService,
            IMaterialStockDomainService materialStockDomainService,
            IMaterialStockStorageDomainService materialStockStorageDomainService,
            IMaterialStockStorageHistoryDomainService materialStockStorageHistoryDomainService,
            IProduceTaskDomainService produceTaskDomainService,
            IProduceTaskHistoryDomainService produceTaskHistoryDomainService,
            IMaterialStockOverviewDomainService materialStockOverviewDomainService,
            IUnitOfWork unitOfWork,
            IMaterialStockOverviewHistoryDomainService materialStockOverviewHistoryDomainService,
            IItemDomainService itemDomainService,
            IWorkOrderAndWorkStationDomainService workOrderAndWorkStationDomainService,
            IDeviceDomainService deviceDomainService,
            ISysConfigManager sysConfigManager)
            : base(domainService, mapper)
        {
            _workOrderDomainService = workOrderDomainService;
            _taskDomainService = taskDomainService;
            _materialStockDomainService = materialStockDomainService;
            _materialStockStorageDomainService = materialStockStorageDomainService;
            _materialStockStorageHistoryDomainService = materialStockStorageHistoryDomainService;
            _produceTaskDomainService = produceTaskDomainService;
            _produceTaskHistoryDomainService = produceTaskHistoryDomainService;
            _materialStockOverviewDomainService = materialStockOverviewDomainService;
            _materialStockOverviewHistoryDomainService = materialStockOverviewHistoryDomainService;
            _unitOfWork = unitOfWork;
            _itemDomainService = itemDomainService;
            _workOrderAndWorkStationDomainService = workOrderAndWorkStationDomainService;
            _deviceDomainService = deviceDomainService;
            _sysConfigManager = sysConfigManager;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DrillWorkOrderDto>>> GetList(GetDrillWorkOrderListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DrillWorkOrderDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DrillWorkOrder>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.Contains(req.WorkOrderCode));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (req.WorkOrderId > 0)
            {
                where = where.And(p => p.WorkOrderId == req.WorkOrderId);
            }
            if (req.ItemId > 0)
            {
                where = where.And(p => p.ItemId == req.ItemId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DrillWorkOrder>, List<DrillWorkOrderDto>>(result.ToList());
            return Success(pageDto);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Commit(CommitDrillWorkOrderReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (req.WorkOrderCode == null || req.ProcessCode == null)
            {
                return Fail("WorkOrderCode和ProcessCode未填写!");
            }

            var drillWorkOrder = await _domainService.FindSingleAsync(p => p.WorkOrderCode == req.WorkOrderCode);
            if (drillWorkOrder == null)
            {
                return Fail("未查到唯一钻孔工单!");
            }

            var taskDatas = await _taskDomainService.QueryAsync(p => p.IsDeleted == 0 && p.WorkOrderCode == req.WorkOrderCode
            && !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.ToLower() == req.ProcessCode.ToLower()
            && p.TaskStatus != null && p.TaskStatus == TaskStatusEnum.DRAFT,
            p => p.CreateTime, OrderByType.Desc);

            if (taskDatas == null || taskDatas.Count == 0)
            {
                return Fail("未查询到待提交的任务!");
            }

            var workOrderIds = new List<int>(); //用来记录需要刷新的workOrder,不记录数量；
            var updateTaskList = new List<Model.Entites.Mes.WorkTask>();

            foreach (var taskData in taskDatas)
            {
                taskData.TaskStatus = TaskStatusEnum.COMMITED;
                taskData.ModifierId = UserId;
                taskData.ModifyTime = DateTime.Now;

                updateTaskList.Add(taskData);

                if (taskData.WorkOrderId != null && !string.IsNullOrEmpty(taskData.KeyFlag) && taskData.KeyFlag.Equals("1"))
                {
                    var workOrder = await _workOrderDomainService.QueryByID(taskData.WorkOrderId);
                    if (workOrder != null)
                    {
                        if (!workOrderIds.Contains(taskData.WorkOrderId.Value))
                        {
                            workOrderIds.Add(taskData.WorkOrderId.Value);
                        }
                    }
                }
            }

            await _taskDomainService.BulkUpdate(updateTaskList);

            //更新workOrder表
            if (workOrderIds.Count > 0)
            {
                var updateWorkOrderList = new List<WorkOrder>();
                foreach (var item in workOrderIds)
                {
                    var workOrder = await _workOrderDomainService.QueryByID(item);
                    if (workOrder != null)
                    {
                        workOrder.ModifyTime = DateTime.Now;
                        workOrder.ModifierId = UserId;

                        if (workOrder.ManuOrderStatus == ManuOrderStatusEnum.COMMITED)
                        {
                            workOrder.ManuOrderStatus = ManuOrderStatusEnum.SCHEDULED;
                        }

                        updateWorkOrderList.Add(workOrder);
                    }
                }

                await _workOrderDomainService.BulkUpdate(updateWorkOrderList);
            }

            drillWorkOrder.IsSubmited = 1;
            drillWorkOrder.SubmitTime = DateTime.Now;
            drillWorkOrder.SubmitUser = UserId;

            await _domainService.Update(drillWorkOrder);

            return Success();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateDrillWorkOrderReq req)
        {
            if (req == null)
            {
                return Fail("信息错误！");
            }

            if (req.WorkOrderId == null || req.WorkOrderId == 0)
            {
                return Fail("未绑定有效工单");
            }

            var workOrder = await _workOrderDomainService.QueryByID(req.WorkOrderId);
            if (workOrder == null)
            {
                return Fail("未查询到绑定的工单");
            }

            if (workOrder.IsAddWorkOrder == 1)
            {
                return Fail("该工单已新增钻孔工单，无法重复新增");
            }

            // 防呆校验：检查产品板长与机台最大板长
            var enablePanelLengthValidation = await _sysConfigManager.GetBoolValue("EnablePanelLengthValidation");
            if (enablePanelLengthValidation)
            {
                var validationResult = await ValidatePanelLength(req.ItemCode, req.WorkOrderId.Value, req.DeviceCodes);
                if (validationResult.Code != ResponseCode.Success)
                {
                    return validationResult;
                }
            }

            var model = _mapper.Map<DrillWorkOrder>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            var result = await _domainService.Add(model);

            if (result)
            {
                workOrder.IsAddWorkOrder = 1;
                workOrder.AddWorkOrderTime = DateTime.Now;
                workOrder.AddWorkOrderId = UserId;
                await _workOrderDomainService.Update(workOrder);
            }

            return Success();
        }

        /// <summary>
        /// 刷新库存数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<string>> RefreshStockData()
        {
            //先删除再添加
            await _materialStockStorageDomainService.DeleteAsync(p => p.Id > 0);
            Dictionary<string, decimal> storageDic = new Dictionary<string, decimal>();

            var materialStockList = await _materialStockDomainService.QueryAsync(p => p.IsDeleted == 0 && p.Status == 1
            && !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.ToLower().Equals("pin")
            && !string.IsNullOrEmpty(p.InOrOut) && p.InOrOut.ToLower().Equals("in")
            && !string.IsNullOrEmpty(p.ItemCode) && p.QuantityOnhand > 0, p => p.Code, OrderByType.Desc);

            if (materialStockList != null && materialStockList.Count > 0)
            {
                List<MaterialStockStorage> addStorages = new List<MaterialStockStorage>();
                List<MaterialStockStorageHistory> addStorageHis = new List<MaterialStockStorageHistory>();
                foreach (var item in materialStockList)
                {
                    if (string.IsNullOrEmpty(item.ItemCode) || item.QuantityOnhand == null)
                    {
                        continue;
                    }
                    addStorages.Add(new MaterialStockStorage
                    {
                        MaterialStockCode = item.Code,
                        BatchCode = item.BatchCode,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ProcessCode = item.ProcessCode,
                        ProcessName = item.ProcessName,
                        QuantityOnhand = item.QuantityOnhand,
                        Status = item.Status,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId
                    });
                    addStorageHis.Add(new MaterialStockStorageHistory
                    {
                        MaterialStockCode = item.Code,
                        BatchCode = item.BatchCode,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ProcessCode = item.ProcessCode,
                        ProcessName = item.ProcessName,
                        QuantityOnhand = item.QuantityOnhand,
                        Status = item.Status,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId
                    });
                    if (storageDic.ContainsKey(item.ItemCode))
                    {
                        storageDic[item.ItemCode] += (decimal)item.QuantityOnhand;
                    }
                    else
                    {
                        storageDic.Add(item.ItemCode, (decimal)item.QuantityOnhand);
                    }
                }
                var isSuccese = await _materialStockStorageDomainService.BulkInsert(addStorages);
                await _materialStockStorageHistoryDomainService.BulkInsert(addStorageHis);
                if (!isSuccese)
                {
                    storageDic = null;
                }
            }

            await _produceTaskDomainService.DeleteAsync(p => p.Id > 0);
            Dictionary<string, decimal> produceDic = new Dictionary<string, decimal>();

            var taskList = await _taskDomainService.QueryAsync(p => p.IsDeleted == 0 && p.Status == 1
            && !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.ToLower().Equals("drill")
            && p.TaskStatus != null && (p.TaskStatus == TaskStatusEnum.DRAFT || p.TaskStatus == TaskStatusEnum.COMMITED)
            && p.NowWadCount > 0, p => p.Code, OrderByType.Desc);

            if (taskList != null && taskList.Count > 0)
            {
                List<ProduceTask> addProduces = new List<ProduceTask>();
                List<ProduceTaskHistory> addProduceHis = new List<ProduceTaskHistory>();
                foreach (var item in taskList)
                {
                    if (string.IsNullOrEmpty(item.ItemCode) || item.NowWadCount == null)
                    {
                        continue;
                    }
                    addProduces.Add(new ProduceTask
                    {
                        TaskCode = item.Code,
                        WorkOrderCode = item.WorkOrderCode,
                        WorkOrderName = item.WorkOrderName,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ProcessCode = item.ProcessCode,
                        ProcessName = item.ProcessName,
                        TaskStatus = item.TaskStatus.ToString(),
                        NowWadCount = item.NowWadCount,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Status = item.Status,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId
                    });
                    addProduceHis.Add(new ProduceTaskHistory
                    {
                        TaskCode = item.Code,
                        WorkOrderCode = item.WorkOrderCode,
                        WorkOrderName = item.WorkOrderName,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ProcessCode = item.ProcessCode,
                        ProcessName = item.ProcessName,
                        TaskStatus = item.TaskStatus.ToString(),
                        NowWadCount = item.NowWadCount,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Status = item.Status,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId
                    });

                    if (produceDic.ContainsKey(item.ItemCode))
                    {
                        produceDic[item.ItemCode] += (decimal)item.NowWadCount;
                    }
                    else
                    {
                        produceDic.Add(item.ItemCode, (decimal)item.NowWadCount);
                    }
                }
                var isSuccese = await _produceTaskDomainService.BulkInsert(addProduces);
                await _produceTaskHistoryDomainService.BulkInsert(addProduceHis);
                if (!isSuccese)
                {
                    produceDic = null;
                }
            }

            await _materialStockOverviewDomainService.DeleteAsync(p => p.Id > 0);
            if (materialStockList != null && materialStockList.Count > 0 && storageDic != null)
            {
                List<MaterialStockOverview> overviews = new List<MaterialStockOverview>();
                List<MaterialStockOverviewHistory> overviewHis = new List<MaterialStockOverviewHistory>();
                foreach (var item in materialStockList.DistinctBy(p => p.ItemCode).ToList())
                {
                    if (string.IsNullOrEmpty(item.ItemCode) || item.QuantityOnhand == null)
                    {
                        continue;
                    }
                    if (!storageDic.ContainsKey(item.ItemCode))
                    {
                        continue;
                    }
                    decimal sumOnhand = storageDic[item.ItemCode];
                    decimal sumPlan = 0;
                    if (produceDic != null && produceDic.ContainsKey(item.ItemCode))
                    {
                        sumPlan = produceDic[item.ItemCode];
                    }
                    decimal usableCount = 0;
                    if (sumOnhand > sumPlan)
                    {
                        usableCount = sumOnhand - sumPlan;
                    }

                    overviews.Add(new MaterialStockOverview
                    {
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ProcessCode = item.ProcessCode,
                        ProcessName = item.ProcessName,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        Status = item.Status,
                        SumOnhand = sumOnhand,
                        SumPlan = sumPlan,
                        UsableCount = usableCount
                    });

                    overviewHis.Add(new MaterialStockOverviewHistory
                    {
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ProcessCode = item.ProcessCode,
                        ProcessName = item.ProcessName,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        Status = item.Status,
                        SumOnhand = sumOnhand,
                        SumPlan = sumPlan,
                        UsableCount = usableCount
                    });
                }

                await _materialStockOverviewDomainService.BulkInsert(overviews);
                await _materialStockOverviewHistoryDomainService.BulkInsert(overviewHis);
            }

            return Success();
        }

        /// <summary>
        /// 获取库存流转信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<GetMaterialDataDto>> GetMaterialData(GetMaterialDataReq req)
        {
            GetMaterialDataDto result = new GetMaterialDataDto();

            if (req != null && !string.IsNullOrEmpty(req.WorkOrderCode) && !string.IsNullOrEmpty(req.ItemCode))
            {
                var overviewData = await _materialStockOverviewDomainService.FindSingleAsync(p => p.ItemCode == req.ItemCode);
                if (overviewData != null)
                {
                    result.MaterialStockOverview = _mapper.Map<MaterialStockOverview, MaterialStockOverviewDto>(overviewData);
                }

                if (req.PageNum < 1) req.PageNum = 1;
                if (req.PageSize < 1) req.PageSize = 10;

                var storageDatas = await _materialStockStorageDomainService.QueryPageAsync(p => p.ItemCode == req.ItemCode,
                    p => p.MaterialStockCode, OrderByType.Asc, req.PageNum, req.PageSize);
                if (storageDatas != null)
                {
                    var pageDto = new PageDto<MaterialStockStorageDto>(req.PageNum, req.PageSize);
                    pageDto.Total = storageDatas.TotalCount;
                    pageDto.List = _mapper.Map<List<MaterialStockStorage>, List<MaterialStockStorageDto>>(storageDatas.ToList());
                    result.Storages = pageDto;
                }

                var produceTasks = await _produceTaskDomainService.QueryPageAsync(p => p.WorkOrderCode == req.WorkOrderCode && p.ItemCode == req.ItemCode,
                    p => p.EndTime, OrderByType.Asc, req.PageNum, req.PageSize);
                if (produceTasks != null)
                {
                    var pageDto = new PageDto<ProduceTaskDto>(req.PageNum, req.PageSize);
                    pageDto.Total = produceTasks.TotalCount;
                    pageDto.List = _mapper.Map<List<ProduceTask>, List<ProduceTaskDto>>(produceTasks.ToList());
                    result.ProduceTasks = pageDto;
                }
            }

            return Success(result);
        }

        public async Task<ResponseDto<DrillWorkOrderDto>> QueryListByID(long id)
        {
            DrillWorkOrderDto drillWorkOrderDto = new DrillWorkOrderDto();
            var db = _unitOfWork.GetDbClient();
            var res = db.Queryable<DrillWorkOrder, WorkOrder>
                ((dw, w) => new object[]
                {
                    JoinType.Left, dw.WorkOrderId== w.Id,
                });
            res = res.Where((dw, w) => dw.IsDeleted == 0 && w.IsDeleted == 0);
            if (id > 0)
            {
                res = res.Where((dw, w) => dw.Id == id);
            }

            var routeCode = res.Select((dw, w) => w.RouteCode).First();
            var routeName = res.Select((dw, w) => w.RouteName).First();
            string route = routeCode + " " + routeName;

            var data = await res.Select((dw, w) => new DrillWorkOrderDto
            {
                Id = dw.Id,
                WorkOrderId = dw.WorkOrderId,
                WorkOrderCode = dw.WorkOrderCode,
                ItemId = dw.ItemId,
                ItemCode = dw.ItemCode,
                PanelCount = dw.PanelCount,
                Quantity = dw.Quantity,
                WadCount = dw.WadCount,
                ShaftCount = dw.ShaftCount,
                AllPassesCount = dw.AllPassesCount,
                RemainderPassesCount = dw.RemainderPassesCount,
                DrillCount = dw.DrillCount,
                ScheduledCount = dw.ScheduledCount,
                UsableCount = dw.UsableCount,
                SingleTripTime = dw.SingleTripTime,
                DrillAllTime = dw.DrillAllTime,
                SingleTrips = dw.SingleTrips,
                DispenseMachines = dw.DispenseMachines,
                IsSubmited = dw.IsSubmited,
                SubmitUser = dw.SubmitUser,
                SubmitTime = dw.SubmitTime,
                IsAddTask = dw.IsAddTask,
                AddTaskUser = dw.AddTaskUser,
                AddTaskTime = dw.AddTaskTime,
                IsUrgent = dw.IsUrgent,
                route = route,
            }).ToListAsync();

            if (data != null && data.Count > 0)
            {
                drillWorkOrderDto = data.FirstOrDefault();
            }
            else
            {
                return Fail<DrillWorkOrderDto>("信息不存在!");
            }
            return Success<DrillWorkOrderDto>(drillWorkOrderDto);
        }
        /// <summary>
        /// 防呆校验：检查产品板长与机台最大板长的匹配
        /// </summary>
        /// <param name="itemCode">产品编码</param>
        /// <param name="workOrderId">生产工单ID</param>
        /// <param name="deviceCodes">机台编码列表（可选）</param>
        /// <returns></returns>
        private async Task<ResponseDto<string>> ValidatePanelLength(string itemCode, long workOrderId, List<string> deviceCodes = null)
        {
            try
            {
                // 获取产品板长
                if (string.IsNullOrEmpty(itemCode))
                {
                    return Fail("产品编码不能为空");
                }

                var item = await _itemDomainService.FindSingleAsync(i => i.Code == itemCode && i.IsDeleted == 0);
                if (item == null)
                {
                    return Fail($"未找到产品编码为 {itemCode} 的产品信息");
                }

                var panelLength = item.PanelLength;
                if (panelLength <= 0)
                {
                    return Fail($"产品 {itemCode} 的板长信息无效或未设置");
                }

                //获取工单关联的工作站
                var workOrderAndWorkStations = await _workOrderAndWorkStationDomainService.QueryAsync(
                    w => w.WorkOrderId == workOrderId && w.IsDeleted == 0,
                    w => w.Id,
                    OrderByType.Asc);

                if (workOrderAndWorkStations == null || !workOrderAndWorkStations.Any())
                {
                    return Fail("该工单未关联任何工作站，无法进行板长校验");
                }

                //获取工作站关联的设备最大板长
                var db = _unitOfWork.GetDbClient();
                var workStationIds = workOrderAndWorkStations.Select(ws => ws.WorkStationId).ToList();
                
                var query = db.Queryable<Device, WorkStation>((d, w) => new object[]
                    {
                        JoinType.Inner, d.WorkStationId == w.Id
                    })
                    .Where((d, w) => d.IsDeleted == 0 && w.IsDeleted == 0)
                    .Where((d, w) => workStationIds.Contains(w.Id))
                    .Where((d, w) => d.MaxBoardLength.HasValue && d.MaxBoardLength > 0);

                // 如果指定了机台编码列表，则只验证这些设备
                if (deviceCodes != null && deviceCodes.Any())
                {
                    query = query.Where((d, w) => deviceCodes.Contains(d.Code));
                }

                var deviceMaxLengths = await query
                    .Select((d, w) => new {
                        DeviceCode = d.Code,
                        DeviceName = d.Name,
                        MaxBoardLength = d.MaxBoardLength,
                        WorkStationCode = w.Code,
                        WorkStationName = w.Name
                    })
                    .ToListAsync();

                if (deviceMaxLengths == null || !deviceMaxLengths.Any())
                {
                    return Fail("该工单关联的工作站未配置设备或设备未设置最大板长");
                }

                //检查板长是否超过所有设备中最小板长限制
                var minAllowedLength = deviceMaxLengths.Min(d => d.MaxBoardLength.Value);
                if (panelLength > minAllowedLength)
                {
                    var deviceInfo = string.Join("、", deviceMaxLengths.Select(d => $"{d.DeviceName}({d.DeviceCode})最大板长{d.MaxBoardLength}mm"));
                    return Fail($"产品板长 {panelLength}mm 超过所选机台的最大板长限制。\n" +
                               $"工单关联的设备信息：{deviceInfo}\n" +
                               $"设备板长限制：{minAllowedLength}mm\n" +
                               $"请选择适合的机台或调整产品规格");
                }

                return Success();
            }
            catch (Exception ex)
            {
                return Fail($"板长校验异常：{ex.Message}");
            }
        }
    }
}
