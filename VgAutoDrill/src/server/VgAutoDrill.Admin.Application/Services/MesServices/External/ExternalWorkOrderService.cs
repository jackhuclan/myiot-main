using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SqlSugar;
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
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominRequest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominResponse;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingWangRequest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingwangResponse;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndPanel;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndWorkStation;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices.External
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalWorkOrderService : BaseServiceWithoutTree<ExternalWorkOrder, ExternalWorkOrderDto, AddOrUpdateExternalWorkOrderReq>, IExternalWorkOrderService
    {
        private readonly IExternalWorkOrderDomainService _externalWorkOrderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExternalWorkOrderService> _logger;
        private readonly ExternalOptions _externalOptions;
        private readonly IWorkOrderAndWorkStationService _workOrderAndWorkStationService;
        private readonly IItemDomainService _itemDomainService;
        private readonly ITaskService _taskServiceProvider;
        private readonly IRouteDomainService _routeDomainService;
        private readonly IEncodeBuildRulesService _encodeService;
        private readonly IUnitMeasureDomainService _unitService;
        private readonly IProductCategoryDomainService _productCategoryService;
        private readonly IWorkOrderDomainService _innerWorkOrderDomainService;
        private readonly ITaskDomainService _taskDomainService;
        private readonly IWorkOrderService _innerWorkOrderService;
        private readonly IDeviceService _deviceService;
        private readonly IAPIHelper _apiHelper;
        private readonly IDrillWorkOrderService _iDrillWorkOrderService;
        private readonly IWorkstationDomainService _WorkStationDomainService;
        private readonly IWorkOrderAndPanelService _workOrderAndPanelService;
        private readonly IPanelService _panelService;
        private readonly IWorkOrderAndPanelDomainService _workOrderAndPanelDomainService;
        private readonly SqlSugarScope _sqlSugarScope;
        private readonly IWorkOrderAlterLogDomainService _workOrderAlterLogDomainService;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IExternalGetStockInfo _externalGetStockInfo;
        private static List<string> NoTargetAreas = new List<string>();
        private static List<string> IsSendedLots = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public ExternalWorkOrderService(IExternalWorkOrderDomainService domainService,
            IUnitOfWork unitOfWork,
            IOptions<ExternalOptions> options,
            ILogger<ExternalWorkOrderService> logger,
            IWorkOrderAndWorkStationService workOrderAndWorkStationService,
            IMapper mapper,
            ITaskService taskServiceProvider,
            IItemDomainService itemDomainService,
            IRouteDomainService routeDomainService,
            IEncodeBuildRulesService encodeService,
            IUnitMeasureDomainService unitService,
            IWorkOrderService workOrderService,
            IDeviceService deviceService,
            IProductCategoryDomainService productCategoryService,
            IWorkOrderDomainService innerWorkOrderDomainService,
            ITaskDomainService taskDomainService,
            IAPIHelper apiHelper,
            IDrillWorkOrderService iDrillWorkOrderService,
            IWorkstationDomainService workStationDomainService,
            IWorkOrderAndPanelService workOrderAndPanelService,
            IPanelService panelService,
            IWorkOrderAndPanelDomainService workOrderAndPanelDomainService,
            IWorkOrderAlterLogDomainService workOrderAlterLogDomainService,
            ISysConfigManager sysConfigManager,
            IExternalGetStockInfo externalGetStockInfo)
            : base(domainService, mapper)
        {
            _externalWorkOrderService = domainService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _externalOptions = options.Value;
            _workOrderAndWorkStationService = workOrderAndWorkStationService;
            _itemDomainService = itemDomainService;
            _sqlSugarScope = unitOfWork.GetDbClient();
            _taskServiceProvider = taskServiceProvider;
            _routeDomainService = routeDomainService;
            _encodeService = encodeService;
            _unitService = unitService;
            _apiHelper = apiHelper;
            _deviceService = deviceService;
            _productCategoryService = productCategoryService;
            _innerWorkOrderDomainService = innerWorkOrderDomainService;
            _taskDomainService = taskDomainService;
            _innerWorkOrderService = workOrderService;
            _iDrillWorkOrderService = iDrillWorkOrderService;
            _WorkStationDomainService = workStationDomainService;
            _workOrderAndPanelService = workOrderAndPanelService;
            _panelService = panelService;
            _workOrderAndPanelDomainService = workOrderAndPanelDomainService;
            _workOrderAlterLogDomainService = workOrderAlterLogDomainService;
            _sysConfigManager = sysConfigManager;
            _externalGetStockInfo = externalGetStockInfo;
        }
        public async Task<ResponseDto<string>> AddData(AddOrUpdateExternalWorkOrderReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }
            if (await _externalWorkOrderService
                .IsExistAsync(p => !string.IsNullOrEmpty(p.SourceCode)
                    && p.SourceCode.ToLower().Trim() == req.SourceCode.ToLower().Trim()))
            {
                return Fail("SourceCode重复!");
            }
            if (await _innerWorkOrderDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code)
                    && p.Code.ToLower().Trim() == req.Code.ToLower().Trim())
                || await _externalWorkOrderService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code)
                    && p.Code.ToLower().Trim() == req.Code.ToLower().Trim()))
            {
                return Fail("Code已存在!");
            }

            var model = _mapper.Map<ExternalWorkOrder>(req);
            model.RouteCode = string.IsNullOrEmpty(req.RouteCode) ? _externalOptions.DefaultRouteCode : req.RouteCode;
            model.UnitOfMeasure = string.IsNullOrEmpty(req.UnitOfMeasure) ? _externalOptions.DefaultUnitOfMeasure : req.UnitOfMeasure;
            model.ProductCategoryCode = string.IsNullOrEmpty(req.ProductCategoryCode) ? _externalOptions.DefaultProductCategoryCode : req.ProductCategoryCode;
            model.DeviceCodes = System.Text.Json.JsonSerializer.Serialize(req.DeviceCodes);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.ModifyTime = DateTime.Now;
            model.ModifierId = UserId;
            model.Remark = "未处理";
            model.Status = (int)HandleExternalWorkOrderStatusEnum.NotHandle;
            var siloPanelResult = await SaveSiloPanel(req);
            if (siloPanelResult?.Code != ResponseCode.Success)
            {
                return Fail($"工单添加失败: {siloPanelResult.Message}");
            }

            await _domainService.Add(model);

            return Success();
        }

        /// <summary>
        /// 批量添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BatchAddData(List<AddOrUpdateExternalWorkOrderReq> req)
        {
            if (req == null || req.Count == 0)
            {
                return Fail("信息格式错误!");
            }
            StringBuilder sb = new StringBuilder();
            List<ExternalWorkOrder> addDatas = new List<ExternalWorkOrder>();
            foreach (var item in req)
            {
                if (item == null) continue;
                if (string.IsNullOrEmpty(item.SourceCode))
                {
                    continue;
                }
                if (await _externalWorkOrderService.IsExistAsync(p => !string.IsNullOrEmpty(p.SourceCode)
                    && p.SourceCode.ToLower().Trim() == item.SourceCode.ToLower().Trim())
                    || addDatas.Exists(p => p.SourceCode.ToLower().Trim() == item.SourceCode.ToLower().Trim()))
                {
                    sb.Append("SourceCode:" + item.SourceCode + "已存在!");
                    sb.Append('\n');
                    continue;
                }
                if (await _innerWorkOrderDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code)
                        && p.Code.ToLower().Trim() == item.Code.ToLower().Trim())
                    || await _externalWorkOrderService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code)
                        && p.Code.ToLower().Trim() == item.Code.ToLower().Trim())
                    || addDatas.Exists(p => p.Code.ToLower().Trim() == item.Code.ToLower().Trim()))
                {
                    sb.Append("Code:" + item.Code + "已存在!");
                    sb.Append('\n');
                    continue;
                }

                var model = _mapper.Map<ExternalWorkOrder>(item);
                model.RouteCode = string.IsNullOrEmpty(item.RouteCode) ? _externalOptions.DefaultRouteCode : item.RouteCode;
                model.UnitOfMeasure = string.IsNullOrEmpty(item.UnitOfMeasure) ? _externalOptions.DefaultUnitOfMeasure : item.UnitOfMeasure;
                model.ProductCategoryCode = string.IsNullOrEmpty(item.ProductCategoryCode) ? _externalOptions.DefaultProductCategoryCode : item.ProductCategoryCode;
                model.DeviceCodes = System.Text.Json.JsonSerializer.Serialize(item.DeviceCodes);
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.ModifyTime = DateTime.Now;
                model.ModifierId = UserId;
                model.Remark = "未处理";
                model.Status = (int)HandleExternalWorkOrderStatusEnum.NotHandle;

                var siloPanelResult = await SaveSiloPanel(item);
                if (siloPanelResult?.Code != ResponseCode.Success)
                {
                    sb.Append($"{item.SourceCode}工单添加失败: {siloPanelResult.Message}");
                    sb.Append('\n');
                    continue;
                }

                addDatas.Add(model);
            }

            var result = await _domainService.BulkInsert(addDatas);
            if (!result)
            {
                return Fail("添加失败！");
            }

            StringBuilder returnSB = new StringBuilder();
            returnSB.Append($"成功{addDatas.Count}条，失败{req.Count - addDatas.Count}条");
            returnSB.Append('\n');
            returnSB.Append($"失败原因：");
            returnSB.Append('\n');
            returnSB.Append(sb.ToString());

            return Success(returnSB.ToString());
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteData(string sourceCode)
        {
            if (string.IsNullOrEmpty(sourceCode))
            {
                return Fail("参数不能为空");
            }
            var externalOrders = await _domainService.QueryAsync(p => p.SourceCode.Trim().ToLower() == sourceCode.Trim().ToLower(), p => p.Id, OrderByType.Desc);
            if (externalOrders.Count == 0)
            {
                return Fail("未找到此工单!");
            }
            else if (externalOrders.Count > 1)
            {
                return Fail("匹配到多条工单，不能删除");
            }

            var innnerOrderCode = externalOrders[0].Code;
            var innerOrderId = externalOrders[0].InnerOrderId;

            if (!string.IsNullOrEmpty(innnerOrderCode))
            {
                var isAnyStarted = await _taskDomainService.IsExistAsync(p => p.WorkOrderCode == innnerOrderCode
                && p.TaskStatus != TaskStatusEnum.DRAFT
                && p.TaskStatus != TaskStatusEnum.COMMITED);

                if (isAnyStarted)
                {
                    return Fail($"{innnerOrderCode} 存在生产任务已开始调度，不能删除此工单");
                }
            }

            _unitOfWork.BeginTran();

            if (!string.IsNullOrEmpty(innnerOrderCode))
            {
                if (innerOrderId == null && await _innerWorkOrderDomainService.IsExistAsync(p => p.Code == innnerOrderCode))
                {
                    var data = await _innerWorkOrderDomainService.FindSingleAsync(p => p.Code == innnerOrderCode);
                    if (data != null)
                    {
                        innerOrderId = data.Id;
                    }
                }

                await _innerWorkOrderService.RobackTask(new AddOrUpdateTaskReq
                {
                    WorkOrderCode = innnerOrderCode,
                });
                await _taskDomainService.DeleteAsync(p => p.WorkOrderCode == innnerOrderCode);

                if (innerOrderId != null)
                {
                    await _innerWorkOrderService.UnCommit(new CommitWorkOrderReq { Id = innerOrderId });
                }
                await _innerWorkOrderDomainService.DeleteAsync(p => p.Code == innnerOrderCode);
            }

            await _workOrderAndPanelService.DeleteByExternalCode(sourceCode);

            var result = await _domainService.DeleteAsync(p => p.SourceCode.Trim().ToLower() == sourceCode.Trim().ToLower());

            _unitOfWork.CommitTran();

            if (result)
            {
                return Success("");
            }

            return Fail("删除失败");
        }
        private async Task<ResponseDto<string>> SaveSiloPanel(AddOrUpdateExternalWorkOrderReq req)
        {
            if (req.Panels == null || req.Panels.Count <= 0) return Success();

            var siloPanelInfo = _mapper.Map<List<AddOrUpdateWorkOrderAndPanelReq>>(req.Panels);
            siloPanelInfo.ForEach(x =>
            {
                x.WorkOrderCode = ""; x.TaskCode = "";
                x.ExternalWorkerOrder = req.SourceCode;
            });
            var workOrderAndPanelResult = await _workOrderAndPanelService.AddWorkOrderAndPanelList(siloPanelInfo);

            var panels = _mapper.Map<List<AddOrUpdatePanelReq>>(req.Panels);
            var panelResult = await _panelService.BatchAddData(panels);

            if (string.IsNullOrEmpty(workOrderAndPanelResult.Message) && string.IsNullOrEmpty(panelResult.Message))
            {
                return Success("");
            }
            else
            {
                return Fail($"{workOrderAndPanelResult.Message} {panelResult.Message}");
            }
        }
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateExternalWorkOrderReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var workOrder = await _domainService.FindSingleAsync(p => p.SourceCode == req.SourceCode);
            if (workOrder == null) return Fail($"没有找到工单:{req.SourceCode}的数据，更新失败!");

            req.Id = workOrder.Id;
            workOrder = _mapper.Map<ExternalWorkOrder>(req);
            workOrder.ModifyTime = DateTime.Now;
            workOrder.ModifierId = UserId;
            workOrder.Status = (int)HandleExternalWorkOrderStatusEnum.NotHandle;
            await _domainService.Update(workOrder);
            return Success();
        }

        public async Task<ResponseDto<List<ExternalWorkOrderDto>>> QueryData(ExternalWorkOrderQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalWorkOrderDto>>("信息格式错误!");
            }

            var where = PredicateBuilder.True<ExternalWorkOrder>();
            where = where.And(p => p.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.SourceCode))
            {
                where = where.And(p => p.SourceCode.Contains(req.SourceCode));
            }

            if (!string.IsNullOrEmpty(req.InnerCode))
            {
                where = where.And(p => p.Code.Contains(req.InnerCode));
            }
            if (!string.IsNullOrEmpty(req.IncodeNumber))
            {
                where = where.And(p => p.IncodeNumber.ToLower().Equals(req.IncodeNumber.ToLower()));
            }

            var workOrder = await _domainService.QueryAsync(where, p => p.Id, OrderByType.Desc);
            var rsp = _mapper.Map<List<ExternalWorkOrderDto>>(workOrder);
            return Success(rsp);
        }
        public async Task<ResponseDto<List<ExternalWorkOrderDto>>> BatchQueryData(BatchWorkOrderQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalWorkOrderDto>>("信息格式错误!");
            }
            else if ((req.BatchSourceCodes == null || !req.BatchSourceCodes.Any())
                && (req.BatchInnerCodes == null || !req.BatchInnerCodes.Any()))
            {
                return Fail<List<ExternalWorkOrderDto>>("BatchSourceCodes,BatchInnerCodes 不能同时为空 !");
            }

            var where = PredicateBuilder.True<ExternalWorkOrder>();
            where = where.And(p => p.IsDeleted == 0);
            if (req.BatchSourceCodes != null && req.BatchSourceCodes.Any())
            {
                where = where.And(p => req.BatchSourceCodes.Contains(p.SourceCode));
            }

            if (req.BatchInnerCodes != null && req.BatchInnerCodes.Any())
            {
                where = where.And(p => req.BatchInnerCodes.Contains(p.Code));
            }

            var workOrder = await _domainService.QueryAsync(where, p => p.Id, OrderByType.Desc);
            var rsp = _mapper.Map<List<ExternalWorkOrderDto>>(workOrder);
            return Success(rsp);
        }
        /// <summary>
        /// Schedule 自动解析外部工单
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AnalyzeExternalWorkOrder()
        {
            bool result = true;

            var where = PredicateBuilder.True<ExternalWorkOrder>();
            where = where.And(p => p.IsDeleted == 0 && p.Status == (int)HandleExternalWorkOrderStatusEnum.NotHandle && p.IsErrorData == 0);
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_TRANSFER_DRILL_FILE_ENABLE, Model.Enum.SysConfigCategoryEnum.None, false))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.AfterDrillFilePath) && !string.IsNullOrEmpty(p.BeforeDrillFilePath));
            }
            var lstExternalWorkOrder = (await _domainService.QueryAsync(where, p => p.ModifyTime
                                     , OrderByType.Asc))
                                     .ToList()
                                    .OrderByDescending(p => p.IsUrgent).ToList();

            if (lstExternalWorkOrder == null || !lstExternalWorkOrder.Any()) return Success();

            var enableStockQuery = await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false);
            ResponseDto<List<StockInfoQueryDataDto>> exsitStockInfos = null;
            if (enableStockQuery)
            {
                exsitStockInfos = await GetStockInfoByConfig();
            }

            _unitOfWork.BeginTran();
            // 生成内部工单
            foreach (var externalWorkOrder in lstExternalWorkOrder)
            {
                WorkOrder innerWorkOrder = _mapper.Map<WorkOrder>(externalWorkOrder);
                externalWorkOrder.Status = (int)HandleExternalWorkOrderStatusEnum.HandleFail;
                externalWorkOrder.Code = innerWorkOrder.Code;
                externalWorkOrder.Name = innerWorkOrder.Name;
                externalWorkOrder.Remark = "处理失败";

                //物料信息
                if (!await GenerateItemInfo(innerWorkOrder, externalWorkOrder)) continue;

                if (enableStockQuery)
                {
                    var verifyResult = await VerifyStockQuantity(exsitStockInfos, externalWorkOrder);
                    if (verifyResult.Code != ResponseCode.Success && !string.IsNullOrEmpty(verifyResult.Message))
                    {
                        externalWorkOrder.Remark = verifyResult.Message;
                        continue;
                    }
                }

                try
                {
                    //工单其他值
                    if (!await GenerateWorkOrderOtherValues(innerWorkOrder, externalWorkOrder)) continue;
                    //回写内部工单ID
                    externalWorkOrder.InnerOrderId = innerWorkOrder.Id;
                    // 分配机台 
                    List<WorkStationData> workStationDatas = new List<WorkStationData>();
                    if (!await DispenserDevice(innerWorkOrder, externalWorkOrder, workStationDatas))
                    {
                        externalWorkOrder.Remark = "处理失败,没有匹配到任务机台";
                        continue;
                    }

                    // 生成排产任务
                    if (await _sysConfigManager.GetBoolValue(MESConfigConstants.DIRECT_BUILD_TASK, Model.Enum.SysConfigCategoryEnum.None, false))
                    {
                        if (!await GenerateScheduleTask(innerWorkOrder, externalWorkOrder, workStationDatas))
                        {
                            externalWorkOrder.Remark = "处理失败,生成排产任务时，发生错误";
                            continue;
                        }
                    }
                    else
                    {
                        externalWorkOrder.Code = innerWorkOrder.Code;
                        externalWorkOrder.Name = innerWorkOrder.Name;
                        externalWorkOrder.Status = (int)HandleExternalWorkOrderStatusEnum.HandleSuccess;
                        externalWorkOrder.Remark = "处理成功：根据配置项DirectBuildTask，未自动生成排产任务";
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    externalWorkOrder.Remark = $"处理失败: {ex.Message}";
                    result = false;
                }
            }

            await _externalWorkOrderService.BulkUpdate(lstExternalWorkOrder);

            if (!result)
            {
                _unitOfWork.RollbackTran();

                await _externalWorkOrderService.BulkUpdate(lstExternalWorkOrder);
                return Fail("工单处理失败");
            }

            _unitOfWork.CommitTran();
            return Success();
        }
        public async Task<ResponseDto<List<ExternalWorkTaskDto>>> GetWorkTask(ExternalWorkOrderQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalWorkTaskDto>>("信息格式错误!");
            }

            var workOrderQuery = _sqlSugarScope.Queryable<ExternalWorkOrder, WorkOrder>
                  ((externalwo, innerwo) => new object[]  {
                      JoinType.Left, externalwo.Code == innerwo.Code,
                  }).Where((externalwo, innerwo) =>
                       externalwo.IsDeleted == 0 &&
                       innerwo.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.SourceCode))
            {
                workOrderQuery = workOrderQuery.Where((externalwo, innerwo) => externalwo.SourceCode == req.SourceCode);
            }

            if (!string.IsNullOrEmpty(req.InnerCode))
            {
                workOrderQuery = workOrderQuery.Where((externalwo, innerwo) => innerwo.Code == req.InnerCode);
            }

            var workOrderDto = workOrderQuery.Select((externalwo, innerwo) =>
                    new ExternalAndInnerWorkOrderDto()
                    {
                        Code = externalwo.Code,
                        Name = externalwo.Name,
                        SourceCode = externalwo.SourceCode,
                        OrderSource = string.IsNullOrEmpty(innerwo.OrderSource) ? externalwo.OrderSource : innerwo.OrderSource,
                        ManuOrderStatus = innerwo.ManuOrderStatus,
                        Status = externalwo.Status,
                        Remark = externalwo.Remark,
                        SpecGroup = externalwo.SpecGroup,
                    })
                    .ToList();

            var externalWorkOrderTask = _mapper.Map<List<ExternalWorkTaskDto>>(workOrderDto);
            if (externalWorkOrderTask != null && externalWorkOrderTask.Any())
            {
                foreach (var item in externalWorkOrderTask)
                {
                    item.WorkTasks = await GetTaskByWorkOrderId(item.Code);
                }
            }
            ;

            return Success(externalWorkOrderTask!);
        }

        public async Task<ResponseDto<List<ExternalWorkTaskDto>>> BatchGetWorkTask(BatchWorkOrderQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalWorkTaskDto>>("信息格式错误!");
            }
            else if ((req.BatchSourceCodes == null || !req.BatchSourceCodes.Any())
                && (req.BatchInnerCodes == null || !req.BatchInnerCodes.Any()))
            {
                return Fail<List<ExternalWorkTaskDto>>("BatchSourceCodes,BatchInnerCodes 不能同时为空 !");
            }

            var workOrderQuery = _sqlSugarScope.Queryable<ExternalWorkOrder, WorkOrder>
                  ((externalwo, innerwo) => new object[]  {
                      JoinType.Left, externalwo.Code == innerwo.Code,
                  }).Where((externalwo, innerwo) =>
                       externalwo.IsDeleted == 0 &&
                       innerwo.IsDeleted == 0);

            if (req.BatchSourceCodes != null && req.BatchSourceCodes.Any())
            {
                workOrderQuery = workOrderQuery.Where((externalwo, innerwo) => req.BatchSourceCodes.Contains(externalwo.SourceCode));
            }

            if (req.BatchInnerCodes != null && req.BatchInnerCodes.Any())
            {
                workOrderQuery = workOrderQuery.Where((externalwo, innerwo) => req.BatchInnerCodes.Contains(innerwo.Code));
            }

            var workOrderDto = workOrderQuery.Select((externalwo, innerwo) =>
                    new ExternalAndInnerWorkOrderDto()
                    {
                        Code = externalwo.Code,
                        Name = externalwo.Name,
                        SourceCode = externalwo.SourceCode,
                        OrderSource = string.IsNullOrEmpty(innerwo.OrderSource) ? externalwo.OrderSource : innerwo.OrderSource,
                        ManuOrderStatus = innerwo.ManuOrderStatus,
                        Status = externalwo.Status,
                        Remark = externalwo.Remark,
                        SpecGroup = externalwo.SpecGroup,
                    })
                    .ToList();

            var externalWorkOrderTask = _mapper.Map<List<ExternalWorkTaskDto>>(workOrderDto);
            if (externalWorkOrderTask != null && externalWorkOrderTask.Any())
            {
                foreach (var item in externalWorkOrderTask)
                {
                    item.WorkTasks = await GetTaskByWorkOrderId(item.Code);
                }
            }
            ;

            return Success(externalWorkOrderTask!);
        }

        /// <summary>
        /// 导入江西景旺WIP数据，等待自动生成工单
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task ImportJiangXiKinWongWIP()
        {
            try
            {
                string reqStr = string.Empty;
                List<ExternalWorkOrder> addList = new List<ExternalWorkOrder>();
                List<string> souceCodes = new List<string>();

                var layerNums = await _sysConfigManager.GetStringValue(MESConfigConstants.QUERY_LAYER_NUM_LIST, Model.Enum.SysConfigCategoryEnum.None, false);

                if (!string.IsNullOrEmpty(layerNums) && layerNums != "0")
                {
                    var layerNumList = layerNums.Split('|');
                    if (layerNumList.Length > 0)
                    {
                        string pattern = @"^[0-9]*$";
                        for (int i = 0; i < layerNumList.Length; i++)
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(layerNumList[i], pattern))
                            {
                                continue;
                            }
                            if (layerNumList[i] == "0")
                            {
                                continue;
                            }

                            JiangXiKinWongWIPReq jWangRequest = new JiangXiKinWongWIPReq
                            {
                                LayerNum = layerNumList[i],
                            };

                            reqStr = JsonSerializer.Serialize(jWangRequest);
                            var datas = await GetDataFromJWApi(reqStr, souceCodes);
                            if (datas != null && datas.Count > 0)
                            {
                                addList.AddRange(datas);
                            }
                        }
                    }
                }
                else
                {
                    var datas = await GetDataFromJWApi(reqStr, souceCodes);
                    if (datas != null && datas.Count > 0)
                    {
                        addList.AddRange(datas);
                    }
                }

                if (addList.Count > 0)
                {
                    await DeleteBadData(souceCodes);
                    await BulkInert(addList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ImportJiangXiKinWongWIP Error");
            }
        }

        private async Task BulkInert(List<ExternalWorkOrder> addList)
        {
            await GenerateItemInfo(addList);
            await _externalWorkOrderService.BulkInsert(addList);
        }

        private async Task GenerateItemInfo(List<ExternalWorkOrder> externals)
        {
            var generalItems = externals.Where(x => !string.IsNullOrEmpty(x.ItemCode) && x.PanelCount > 0 && !string.IsNullOrEmpty(x.ProductCategoryCode))
                                      .Select(x => new Item
                                      {
                                          Specification = x.Specification,
                                          Code = x.ItemCode,
                                          IncodeNumber = x.IncodeNumber,
                                          DispenseMachines = x.DispenseMachines,
                                          Status = 1,
                                          CreateTime = DateTime.Now,
                                          CreatorId = UserId,
                                          ItemOrProduct = 2,
                                          ItemTypeId = 4,
                                          PanelLength = x.PanelLength,
                                          LayerNum = x.LayerNum,
                                          PanelCount = (int?)x.PanelCount,
                                          UnitOfMeasure = x.UnitOfMeasure,
                                          Name = string.IsNullOrEmpty(x.ItemName) ? x.Code : x.ItemName,
                                          ProductCategoryCode = x.ProductCategoryCode,
                                      })
                                      .Distinct()
                                      .ToList();

            foreach (var item in generalItems)
            {
                //是否已存在该物料
                if (await _itemDomainService.IsExistAsync(p => p.Code.ToLower() == item.Code.ToLower()))
                {
                    continue;
                }

                //大类                
                var productCatory = await _productCategoryService.FindSingleAsync(p => p.Code == item.ProductCategoryCode);
                if (productCatory == null)
                {
                    continue;
                }
                item.ProductCategoryId = productCatory.Id;
                item.ProductCategoryName = productCatory.Name;

                var itemId = await _itemDomainService.AddReturnId(item);
                if (itemId > 0)
                {
                    _logger.LogInformation($"GenerateItemInfo-success 生成物料 {item.Code}。");
                }
                else
                {
                    _logger.LogWarning($"GenerateItemInfo-fail 生成物料 {item.Code}。");
                }
            }
        }

        private async Task DeleteBadData(List<string> souceCodes)
        {
            if (souceCodes != null && souceCodes.Count > 0)
            {
                if (!await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_NOT_WIP_DATA, SysConfigCategoryEnum.Kinwong, false))
                {
                    _logger.LogInformation("ImportJiangXiKinWongWIP set isdelete = 1 where data not in WIP !");
                    await _externalWorkOrderService.UpdateAsync(p => new ExternalWorkOrder
                    {
                        IsDeleted = 1
                    }, p => !souceCodes.Contains(p.SourceCode.Trim().ToLower()));
                }

                var unExists = await _externalWorkOrderService.QueryAsync(p => !souceCodes.Contains(p.SourceCode.Trim().ToLower()),
                    p => p.SourceCode, OrderByType.Asc);
                if (unExists != null && unExists.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("ImportJiangXiKinWongWIP delete useless data! ");
                    var noWorkOrders = unExists.Where(p => (p.InnerOrderId == null || p.InnerOrderId == 0)
                    && p.Status == (int)HandleExternalWorkOrderStatusEnum.HandleFail).Select(p =>
                    p.SourceCode.Trim().ToLower()).ToList();
                    await _externalWorkOrderService.DeleteAsync(p => noWorkOrders.Contains(p.SourceCode.Trim().ToLower()));
                    await _workOrderAndPanelDomainService.DeleteAsync(p => noWorkOrders.Contains(p.ExternalWorkerOrder.Trim().ToLower()));
                    sb.Append(" SourceCode：{" + String.Join(",", noWorkOrders) + "}");

                    var innerCodes = unExists.Where(p => !string.IsNullOrEmpty(p.Code)).Select(p => p.Code.Trim().ToLower()).ToList();

                    var haveWorkOrders = await _innerWorkOrderDomainService.QueryAsync(p => innerCodes.Contains(p.Code.Trim().ToLower())
                    && p.ManuOrderStatus == ManuOrderStatusEnum.DRAFT && p.IsExternal == 1, p => p.Code, OrderByType.Desc);

                    if (haveWorkOrders != null && haveWorkOrders.Count > 0)
                    {
                        var deleteInnerCodes = haveWorkOrders.Select(p => p.Code.Trim().ToLower()).ToList();

                        await _externalWorkOrderService.DeleteAsync(p => deleteInnerCodes.Contains(p.Code.Trim().ToLower()));
                        await _innerWorkOrderDomainService.DeleteAsync(p => deleteInnerCodes.Contains(p.Code.Trim().ToLower()));
                        await _taskDomainService.DeleteAsync(p => deleteInnerCodes.Contains(p.WorkOrderCode.Trim().ToLower()) && p.TaskStatus == TaskStatusEnum.DRAFT);
                        await _workOrderAndPanelDomainService.DeleteAsync(p => deleteInnerCodes.Contains(p.WorkOrderCode.Trim().ToLower()));
                        sb.Append(" InnerCodes：{" + String.Join(",", deleteInnerCodes) + "}");
                    }
                    _logger.LogInformation(sb.ToString());
                }
            }
        }

        private async Task<List<ExternalWorkOrder>> GetDataFromJWApi(string reqStr, List<string> souceCodes)
        {
            try
            {
                List<ExternalWorkOrder> addList = new List<ExternalWorkOrder>();
                string result = string.Empty;

                string urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JIANGXI_KINWONG_WIP_URL,
                    Model.Enum.SysConfigCategoryEnum.None, false);
                if (string.IsNullOrEmpty(urlAdress))
                {
                    _logger.LogError($"WIP api urlAdress is null !");
                    return addList;
                }

                if (string.IsNullOrEmpty(reqStr))
                {
                    result = _apiHelper.RequestData(urlAdress, "post", "{}");
                }
                else
                {
                    result = _apiHelper.RequestData(urlAdress, "post", reqStr);
                }

                if (string.IsNullOrEmpty(result))
                {
                    _logger.LogError($"WIP api {urlAdress}, return empty ");
                    return addList;
                }

                var enableStockQuery = await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false);

                var jxKWList = JsonSerializer.Deserialize<List<JiangXiKinWongWIPResponse>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (jxKWList != null && jxKWList.Any())
                {
                    foreach (var item in jxKWList)
                    {
                        StringBuilder sb = new StringBuilder();
                        if (string.IsNullOrEmpty(item.ContainerName))
                        {
                            _logger.LogWarning("JiangXiKinWongWIPResponse.ContainerName is empty");
                            continue;
                        }

                        if (await _domainService.IsExistAsync(e => e.SourceCode.ToLower() == item.ContainerName.ToLower()))
                        {
                            _logger.LogDebug($"ContainerName {item.ContainerName} is already imported.");
                            souceCodes.Add(item.ContainerName.Trim().ToLower());
                            continue;
                        }

                        int pnlQty = 0;
                        if (string.IsNullOrEmpty(item.PnlQty)
                             || !int.TryParse(item.PnlQty, out pnlQty)
                             || pnlQty <= 0)
                        {
                            _logger.LogWarning("JiangXiKinWongWIPResponse.PnlQty 非法或者 <= 0");
                            sb.Append("PnlQty 非法或者 <= 0;");
                        }

                        int stackNum = 0;
                        if (string.IsNullOrEmpty(item.StackNum)
                             || !int.TryParse(item.StackNum, out stackNum)
                             || stackNum <= 0)
                        {
                            _logger.LogWarning("JiangXiKinWongWIPResponse.StackNum 非法或者 <= 0");
                            sb.Append("StackNum 非法或者 <= 0;");
                        }

                        DateTime shipDate = DateTime.MinValue;
                        if (string.IsNullOrEmpty(item.ShipDate)
                             || !DateTime.TryParse(item.ShipDate, out shipDate))
                        {
                            _logger.LogWarning("JiangXiKinWongWIPResponse.ShipDate 非法或者 非时间");
                            sb.Append("ShipDate 非法或者 非时间;");
                        }

                        int layerNum = 0;
                        if (string.IsNullOrEmpty(item.LayerNum)
                             || !int.TryParse(item.LayerNum, out layerNum)
                             || layerNum <= 0)
                        {
                            _logger.LogWarning("JiangXiKinWongWIPResponse.LayerNum 非法或者 <= 0");
                            sb.Append("LayerNum 非法或者 <= 0;");
                        }

                        if (string.IsNullOrEmpty(item.SpecGroup))
                        {
                            sb.Append("SpecGroup is empty;");
                        }

                        if (string.IsNullOrEmpty(item.Param01))
                        {
                            sb.Append("Param01（钻带文件路径） is empty;");
                        }

                        if (string.IsNullOrEmpty(item.Param03))
                        {
                            sb.Append("Param03（是否暂停） is empty;");
                        }
                        else if (!item.Param03.Contains("否"))
                        {
                            sb.Append($"Param03（是否暂停）:{item.Param03};");
                        }

                        string isInPlanWarehouse = "是";
                        if (enableStockQuery)
                        {
                            if (string.IsNullOrEmpty(item.Param02))
                            {
                                sb.Append("Param02（是否在钻孔计划仓） is empty;");
                                isInPlanWarehouse = "否";
                            }
                            else
                            {
                                if (item.Param02.Contains(";"))
                                {
                                    var arrInPlans = item.Param02.Split(';');
                                    if (arrInPlans == null || arrInPlans.Length != 2
                                        || string.IsNullOrEmpty(arrInPlans[0]) || string.IsNullOrEmpty(arrInPlans[1]))
                                    {
                                        sb.Append($"Param02（是否在钻孔计划仓）返回值不符合规范 ;");
                                        isInPlanWarehouse = "否";
                                    }
                                    else if (arrInPlans[0] != arrInPlans[1])
                                    {
                                        sb.Append($"Param02（是否在钻孔计划仓）返回值提示不在计划仓 ;");
                                        isInPlanWarehouse = "否";
                                    }
                                }
                                else
                                {
                                    sb.Append($"Param02（是否在钻孔计划仓）返回值不符合规范 ;");
                                    isInPlanWarehouse = "否";
                                }
                            }
                        }

                        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_AUTO_CUTTER, SysConfigCategoryEnum.Kinwong, false))
                        {
                            if (string.IsNullOrWhiteSpace(item.Param08) || item.Param08.Trim() == "{}")
                            {
                                sb.Append($"Param08（自动配刀）:内容不符合规范 !;");
                            }
                        }

                        addList.Add(new ExternalWorkOrder
                        {
                            Code = item.ContainerName,
                            OrderSource = "库存需求",
                            SourceCode = item.ContainerName,
                            ItemCode = item.ContainerName,
                            ItemName = item.ProductName,
                            Quantity = pnlQty,
                            WadCount = pnlQty == 0 || stackNum == 0 ? 0 : Math.Ceiling(pnlQty / (decimal)stackNum),
                            RequestDate = shipDate == DateTime.MinValue ? null : shipDate,
                            PanelCount = stackNum,
                            DrillCount = 10000,
                            ProductCategoryCode = _externalOptions.DefaultProductCategoryCode,
                            RouteCode = _externalOptions.DefaultRouteCode,
                            UnitOfMeasure = _externalOptions.DefaultUnitOfMeasure,
                            SpecGroup = item.SpecGroup,
                            BeforeDrillFilePath = item.Param01,
                            IsInPlanWarehouse = isInPlanWarehouse,
                            IsInPlanWarehouseRemark = item.Param02,
                            IsHold = item.Param03,
                            LayerNum = layerNum,
                            Status = (int)HandleExternalWorkOrderStatusEnum.NotHandle,
                            CreateTime = DateTime.Now,
                            CreatorId = UserId,
                            Remark = string.IsNullOrEmpty(sb.ToString()) ? "" : "处理失败：" + sb.ToString(),
                            IsErrorData = string.IsNullOrEmpty(sb.ToString()) ? 0 : 1,
                            PanelLength = item.Param07.ToFloat(),
                            CutterInfo = item.Param08
                        });
                        souceCodes.Add(item.ContainerName.Trim().ToLower());
                    }
                }
                return addList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ImportJiangXiKinWongWIP Error");
                return new List<ExternalWorkOrder>();
            }
        }

        private async Task<List<TaskViewDto>> GetTaskByWorkOrderId(string workOrderCode)
        {
            var tasks = await _taskDomainService.QueryAsync(p => p.WorkOrderCode == workOrderCode && p.IsDeleted == 0, p => p.Id, OrderByType.Asc);
            return _mapper.Map<List<TaskViewDto>>(tasks);
        }
        private async Task<bool> GenerateItemInfo(WorkOrder innerWorkOrder, ExternalWorkOrder externalWorkOrder)
        {
            var itemInfo = await _itemDomainService.FindSingleAsync(p => p.Code == externalWorkOrder.ItemCode && p.IsDeleted == 0);
            if (itemInfo != null)
            {
                innerWorkOrder.ItemId = (int)itemInfo.Id;
                innerWorkOrder.ItemName = itemInfo.Name;
                innerWorkOrder.ItemTypeId = itemInfo.ItemTypeId;
                innerWorkOrder.UnitOfMeasure = itemInfo.UnitOfMeasure;
                innerWorkOrder.Specification = itemInfo.Specification;
                innerWorkOrder.DispenseMachines = innerWorkOrder.DispenseMachines <= 0 ? (await _productCategoryService.FindSingleAsync(p => p.Id == itemInfo.ProductCategoryId))?.DispenseMachines : innerWorkOrder.DispenseMachines;
            }
            else
            {
                Item item = new Item();
                item.Specification = externalWorkOrder.Specification;
                item.Code = externalWorkOrder.ItemCode;
                item.IncodeNumber = externalWorkOrder.IncodeNumber;
                item.Name = string.IsNullOrEmpty(externalWorkOrder.ItemName) ? item.Code : externalWorkOrder.ItemName;
                item.ItemOrProduct = 2;
                item.ItemTypeId = 4;
                item.IncodeNumber = externalWorkOrder.IncodeNumber;
                item.PanelLength = externalWorkOrder.PanelLength;
                item.LayerNum = externalWorkOrder.LayerNum;
                item.PanelCount = (int?)externalWorkOrder.PanelCount;

                // 校验单位
                if (!await _unitService.IsExistAsync(p => p.Code == externalWorkOrder.UnitOfMeasure))
                {
                    externalWorkOrder.Remark = $"没有匹配到单位：{externalWorkOrder.UnitOfMeasure} 的数据,请先在中控系统中确认！";
                    return false;
                }

                item.UnitOfMeasure = externalWorkOrder.UnitOfMeasure;
                //大类
                var productCatory = await _productCategoryService.FindSingleAsync(p => p.Code == externalWorkOrder.ProductCategoryCode);
                if (productCatory == null)
                {
                    externalWorkOrder.Remark = $"没有匹配到大类：{externalWorkOrder.ProductCategoryCode} 的数据,请先在中控系统中确认！";
                    return false;
                }

                item.ProductCategoryCode = productCatory.Code;
                item.ProductCategoryId = productCatory.Id;
                item.ProductCategoryName = productCatory.Name;
                item.DispenseMachines = externalWorkOrder.DispenseMachines;
                item.Status = 1;
                item.CreateTime = DateTime.Now;
                item.CreatorId = UserId;
                // 保存Item 返回ItemId
                int itemId = await _itemDomainService.AddReturnId(item);
                innerWorkOrder.ItemId = itemId;
                innerWorkOrder.ItemName = item.Name;
                innerWorkOrder.ItemTypeId = item.ItemTypeId;
                innerWorkOrder.UnitOfMeasure = item.UnitOfMeasure;
                innerWorkOrder.Specification = item.Specification;
                innerWorkOrder.DispenseMachines = item.DispenseMachines;
            }

            return true;
        }

        private async Task<bool> DispenserDevice(WorkOrder innerWorkOrder, ExternalWorkOrder externalWorkOrder, List<WorkStationData> workStationDatas)
        {
            if (!string.IsNullOrEmpty(externalWorkOrder.DeviceCodes)
                && System.Text.Json.JsonSerializer.Deserialize<List<string>>(externalWorkOrder.DeviceCodes).Count > 0)
            {
                var lstDeviceCodes = System.Text.Json.JsonSerializer.Deserialize<List<string>>(externalWorkOrder.DeviceCodes);
                var workStations = await _WorkStationDomainService.QueryAsync(p => lstDeviceCodes.Contains(p.Code), p => p.Code, OrderByType.Asc);
                if (workStations == null || workStations.Count != lstDeviceCodes.Count)
                {
                    externalWorkOrder.Remark = "工作站与中控系统中不匹配！";
                    return false;
                }

                workStationDatas.AddRange(workStations.Select(p => new WorkStationData()
                {
                    WorkStationCode = p.Code,
                    WorkStationId = p.Id,
                    WorkStationName = p.Name,
                }).ToList()
                 );
            }
            else
            {
                GetFitWorkStationListReq workStationListReq = new GetFitWorkStationListReq()
                {
                    PageNum = 1,
                    PageSize = 1000,
                    WorkOrderCode = innerWorkOrder.Code,
                    WorkOrderId = innerWorkOrder.Id,
                    ProcessCode = "drill"
                };

                var workStations = await _taskServiceProvider.GetFitWorkStationList(workStationListReq);
                if (workStations != null && workStations.Data?.List != null && workStations.Code == ResponseCode.Success)
                {
                    workStationDatas.AddRange(workStations.Data.List.Select(p => new WorkStationData()
                    {
                        WorkStationCode = p.Code,
                        WorkStationId = p.Id,
                        WorkStationName = p.Name
                    }).ToList()
                    );
                }
            }

            return workStationDatas.Count > 0;
        }

        private async Task<bool> UpdatePanel(WorkOrder innerWorkOrder, ExternalWorkOrder externalWorkOrder)
        {
            await _workOrderAndPanelDomainService.UpdateAsync(p =>
              new WorkOrderAndPanel()
              {
                  WorkOrderCode = innerWorkOrder.Code,
              }, p => p.ExternalWorkerOrder == externalWorkOrder.SourceCode && string.IsNullOrEmpty(p.WorkOrderCode));

            return true;
        }
        private async Task<bool> GenerateWorkOrderOtherValues(WorkOrder innerWorkOrder, ExternalWorkOrder externalWorkOrder)
        {
            // 工艺路线
            if (!string.IsNullOrWhiteSpace(externalWorkOrder.RouteCode))
            {
                var routeInfo = await _routeDomainService.FindSingleAsync(p => p.Code == externalWorkOrder.RouteCode && p.IsDeleted == 0);
                if (routeInfo == null)
                {
                    externalWorkOrder.Remark = $"没有匹配到工艺路线：{externalWorkOrder.RouteCode} 的数据,请先在中控系统中确认！";
                    return false;
                }
                innerWorkOrder.RouteCode = routeInfo?.Code;
                innerWorkOrder.RouteId = routeInfo?.Id;
                innerWorkOrder.RouteName = routeInfo?.Name;
            }
            // 工单表其他值
            innerWorkOrder.ProcessCode = externalWorkOrder.ProcessCode;
            innerWorkOrder.SpecGroup = externalWorkOrder.SpecGroup;
            innerWorkOrder.BeforeDrillFilePath = externalWorkOrder.BeforeDrillFilePath;
            innerWorkOrder.AfterDrillFilePath = externalWorkOrder.AfterDrillFilePath;
            innerWorkOrder.CutterInfo = externalWorkOrder.CutterInfo;

            innerWorkOrder.IncodeNumber = externalWorkOrder.IncodeNumber;
            innerWorkOrder.IsRebrush = 0;
            innerWorkOrder.LayerNum = externalWorkOrder.LayerNum;
            innerWorkOrder.IsExternal = 1;
            innerWorkOrder.OrderSource = string.IsNullOrEmpty(innerWorkOrder.OrderSource) ? "客户订单" : innerWorkOrder.OrderSource;
            innerWorkOrder.QuantityChanged = externalWorkOrder.Quantity;
            if (string.IsNullOrEmpty(externalWorkOrder.Code))
            {
                innerWorkOrder.Code = (await _encodeService.GetEncodeList(new GetEncodeByRulesListReq() { RulesCode = "WORKORDER_CODE", BuildCount = 1 }))?.Data?[0];
            }
            else
            {
                innerWorkOrder.Code = externalWorkOrder.Code;
            }
            innerWorkOrder.Name = innerWorkOrder.Code;
            //默认，草稿
            innerWorkOrder.ManuOrderStatus = ManuOrderStatusEnum.DRAFT;
            innerWorkOrder.IsAddWorkOrder = 0;
            #region PanelCount，WadCount, DrillCount由外部传入--Command Code
            //innerWorkOrder.PanelCount = (await _unitService.FindSingleAsync(p => p.Name == innerWorkOrder.UnitOfMeasure))?.ChangeRate;
            //innerWorkOrder.WadCount = Math.Ceiling((decimal)innerWorkOrder.QuantityChanged / (decimal)innerWorkOrder.PanelCount);
            //innerWorkOrder.DrillCount = innerWorkOrder.DrillCount <= 0 ? 10000 : innerWorkOrder.DrillCount;
            #endregion
            innerWorkOrder.Status = (int)DataStatusEnum.Enable;
            innerWorkOrder.Id = await _sqlSugarScope.Insertable(innerWorkOrder).ExecuteReturnBigIdentityAsync();

            return await UpdatePanel(innerWorkOrder, externalWorkOrder);
        }

        private async Task<bool> GenerateScheduleTask(WorkOrder innerWorkOrder, ExternalWorkOrder externalWorkOrder, List<WorkStationData> workStationDatas)
        {
            if (workStationDatas != null && workStationDatas.Any())
            {
                var workStationReq = new AddOrUpdateDataReq();
                workStationReq.WorkOrderCode = innerWorkOrder.Code;
                workStationReq.WorkOrderId = innerWorkOrder.Id;
                workStationReq.workStationDatas = workStationDatas;

                var workStationResult = await _workOrderAndWorkStationService.AddData(workStationReq);
                if (workStationResult.Code == ResponseCode.Success)
                {
                    // 自动生成任务
                    var proWorkOrder = await _innerWorkOrderService.QueryDataByID(innerWorkOrder.Id);
                    if (proWorkOrder != null && proWorkOrder.Data != null && proWorkOrder.Code == ResponseCode.Success)
                    {
                        var workTask = _mapper.Map<AddOrUpdateDrillWorkOrderReq>(proWorkOrder.Data);
                        workTask.ShaftCount = proWorkOrder.Data.DrillTaskShaftCount;
                        workTask.WorkOrderCode = proWorkOrder.Data.Code;
                        workTask.WorkOrderId = innerWorkOrder.Id;
                        workTask.AllPassesCount = Math.Ceiling((decimal)workTask.WadCount / (decimal)workTask.ShaftCount);
                        workTask.SingleTripTime = 40;
                        workTask.DrillAllTime = workTask.SingleTripTime * workTask.AllPassesCount;

                        var taskResult = await _taskServiceProvider.BulkAddDrillTaskByMOList(new List<AddOrUpdateDrillWorkOrderReq> { workTask });
                        if (taskResult.Code == ResponseCode.Success)
                        {
                            externalWorkOrder.Status = (int)HandleExternalWorkOrderStatusEnum.HandleSuccess;
                            externalWorkOrder.Code = innerWorkOrder.Code;
                            externalWorkOrder.Name = innerWorkOrder.Name;
                            externalWorkOrder.Remark = "处理成功";
                        }
                        else
                        {
                            externalWorkOrder.Remark = taskResult.Message;
                            return false;
                        }
                    }
                }
                else
                {
                    externalWorkOrder.Remark = workStationResult.Message;
                    return false;
                }
            }
            else
            {
                externalWorkOrder.Remark = "分配机台出错！";
                return false;
            }

            return true;
        }

        public async System.Threading.Tasks.Task SignMoveInLot()
        {
            try
            {
                var data = await _domainService.QueryAsync(p => !string.IsNullOrEmpty(p.SourceCode) && p.InnerOrderId > 0
                    && p.MoveInTime != null && p.SignMoveInTime == null,
                    p => p.CreateTime, OrderByType.Asc);
                if (data == null)
                {
                    return;
                }

                string urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_SET_MOVE_IN_LOT_URL,
                        SysConfigCategoryEnum.None, false);
                if (string.IsNullOrEmpty(urlAdress))
                {
                    return;
                }

                List<ExternalWorkOrder> updateList = new List<ExternalWorkOrder>();
                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item.SourceCode))
                    {
                        continue;
                    }
                    JWangRequestDto jWangRequest = new JWangRequestDto
                    {
                        ContainerName = item.SourceCode,
                    };

                    string reqStr = JsonSerializer.Serialize(jWangRequest);
                    var result = _apiHelper.RequestData(urlAdress, "post", reqStr);

                    if (string.IsNullOrEmpty(result))
                    {
                        _logger.LogError($"MoveInLot api {urlAdress}, ContainerName {item.SourceCode} return empty ");
                    }
                    else
                    {
                        var response = JsonSerializer.Deserialize<JwangResponseDto>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (response != null)
                        {
                            _logger.LogInformation($"MoveInLot  ContainerName {item.SourceCode}, ResultCode {response.ResultCode}, ResultMsg {response.ResultMsg}");
                            if (response.ResultCode == "0")
                            {
                                item.SignMoveInTime = DateTime.Now;
                                updateList.Add(item);
                            }
                        }
                        else
                        {
                            _logger.LogError($"MoveInLot api {urlAdress}, ContainerName {item.SourceCode} return null, result= {result}  ");
                        }
                    }
                }

                await _domainService.BulkUpdate(updateList);
            }
            catch (Exception e)
            {
                _logger.LogError($"SignMoveInLot Error: Catch exceptions：{e}");
            }
        }

        public async System.Threading.Tasks.Task SignMoveOutLot()
        {
            try
            {
                var data = await _domainService.QueryAsync(p => !string.IsNullOrEmpty(p.SourceCode) && p.InnerOrderId > 0
                    && p.MoveOutTime != null && p.SignMoveOutTime == null,
                    p => p.CreateTime, OrderByType.Asc);
                if (data == null)
                {
                    return;
                }

                string urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_SET_MOVE_OUT_LOT_URL,
                        SysConfigCategoryEnum.None, false);
                if (string.IsNullOrEmpty(urlAdress))
                {
                    return;
                }

                List<ExternalWorkOrder> updateList = new List<ExternalWorkOrder>();
                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item.SourceCode))
                    {
                        continue;
                    }
                    JWangRequestDto jWangRequest = new JWangRequestDto
                    {
                        ContainerName = item.SourceCode,
                    };

                    string reqStr = JsonSerializer.Serialize(jWangRequest);
                    var result = _apiHelper.RequestData(urlAdress, "post", reqStr);

                    if (string.IsNullOrEmpty(result))
                    {
                        _logger.LogError($"MoveOutLot api {urlAdress}, ContainerName {item.SourceCode} return empty ");
                    }
                    else
                    {
                        var response = JsonSerializer.Deserialize<JwangResponseDto>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (response != null)
                        {
                            _logger.LogInformation($"MoveOutLot ContainerName {item.SourceCode}, ResultCode {response.ResultCode}, ResultMsg {response.ResultMsg}");

                            if (response.ResultCode == "0")
                            {
                                item.SignMoveOutTime = DateTime.Now;
                                updateList.Add(item);
                            }
                        }
                        else
                        {
                            _logger.LogError($"MoveOutLot api {urlAdress}, ContainerName {item.SourceCode} return null, result={result} ");
                        }
                    }
                }
                await _domainService.BulkUpdate(updateList);
            }
            catch (Exception e)
            {
                _logger.LogError($"SignMoveOutLot Error: Catch exceptions：{e}");
            }
        }

        public async System.Threading.Tasks.Task SignTrackInLot()
        {
            try
            {
                var data = await _domainService.QueryAsync(p => !string.IsNullOrEmpty(p.SourceCode) && p.InnerOrderId > 0
                    && p.TrackInTime != null && p.SignTrackInTime == null,
                    p => p.CreateTime, OrderByType.Asc);
                if (data == null)
                {
                    return;
                }

                string urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_SET_TRACK_IN_LOT_URL,
                        SysConfigCategoryEnum.None, false);
                if (string.IsNullOrEmpty(urlAdress))
                {
                    return;
                }

                List<ExternalWorkOrder> updateList = new List<ExternalWorkOrder>();
                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item.SourceCode))
                    {
                        continue;
                    }
                    JWangRequestDto jWangRequest = new JWangRequestDto
                    {
                        ContainerName = item.SourceCode,
                    };

                    string reqStr = JsonSerializer.Serialize(jWangRequest);
                    var result = _apiHelper.RequestData(urlAdress, "post", reqStr);

                    if (string.IsNullOrEmpty(result))
                    {
                        _logger.LogError($"TrackInLot api {urlAdress}, ContainerName {item.SourceCode} return empty ");
                    }
                    else
                    {
                        var response = JsonSerializer.Deserialize<JwangResponseDto>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (response != null)
                        {
                            _logger.LogInformation($"TrackInLot ContainerName {item.SourceCode}, ResultCode {response.ResultCode}, ResultMsg {response.ResultMsg}");

                            if (response.ResultCode == "0")
                            {
                                item.SignTrackInTime = DateTime.Now;
                                updateList.Add(item);
                            }
                        }
                        else
                        {
                            _logger.LogError($"TrackInLot api {urlAdress}, ContainerName {item.SourceCode} return null, result={result} ");
                        }
                    }
                }
                await _domainService.BulkUpdate(updateList);
            }
            catch (Exception e)
            {
                _logger.LogError($"SignTrackInLot Error: Catch exceptions：{e}");
            }
        }

        public async System.Threading.Tasks.Task SignTrackOutLot()
        {
            try
            {
                var data = await _domainService.QueryAsync(p => !string.IsNullOrEmpty(p.SourceCode) && p.InnerOrderId > 0
                    && p.TrackInTime != null && p.SignTrackOutTime == null,
                    p => p.CreateTime, OrderByType.Asc);
                if (data == null)
                {
                    return;
                }

                string urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_SET_TRACK_OUT_LOT_URL,
                        SysConfigCategoryEnum.None, false);
                if (string.IsNullOrEmpty(urlAdress))
                {
                    return;
                }

                List<ExternalWorkOrder> updateList = new List<ExternalWorkOrder>();
                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item.SourceCode))
                    {
                        continue;
                    }
                    JWangRequestDto jWangRequest = new JWangRequestDto
                    {
                        ContainerName = item.SourceCode,
                    };

                    string reqStr = JsonSerializer.Serialize(jWangRequest);
                    var result = _apiHelper.RequestData(urlAdress, "post", reqStr);

                    if (string.IsNullOrEmpty(result))
                    {
                        _logger.LogError($"TrackOutLot api {urlAdress}, ContainerName {item.SourceCode} return empty ");
                    }
                    else
                    {
                        var response = JsonSerializer.Deserialize<JwangResponseDto>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (response != null)
                        {
                            _logger.LogInformation($"TrackOutLot ContainerName {item.SourceCode}, ResultCode {response.ResultCode}, ResultMsg {response.ResultMsg}");

                            if (response.ResultCode == "0")
                            {
                                item.SignTrackOutTime = DateTime.Now;
                                updateList.Add(item);
                            }
                        }
                        else
                        {
                            _logger.LogError($"TrackOutLot api {urlAdress}, ContainerName {item.SourceCode} return null, result={result} ");
                        }
                    }
                }
                await _domainService.BulkUpdate(updateList);
            }
            catch (Exception e)
            {
                _logger.LogError($"SignTrackOutLot Error: Catch exceptions：{e}");
            }
        }

        public async Task GetAfterDrillFilePath()
        {
            try
            {
                var exterWorkDatas = await _externalWorkOrderService.QueryAsync(p => p.IsDeleted == 0 && string.IsNullOrEmpty(p.AfterDrillFilePath)
                && !string.IsNullOrEmpty(p.BeforeDrillFilePath), p => p.CreateTime, OrderByType.Desc);
                if (!exterWorkDatas.Any())
                {
                    return;
                }

                List<ExternalWorkOrder> updateWorkOrders = new List<ExternalWorkOrder>();
                List<WorkOrderAlterLog> addLogs = new List<WorkOrderAlterLog>();
                foreach (var wData in exterWorkDatas)
                {
                    var remarkArr = wData.Remark.Split(';');

                    if (await _innerWorkOrderService.VerifyBeforePath(wData.BeforeDrillFilePath))
                    {
                        wData.Remark = await SetWDataRemark(wData.Remark, remarkArr[0] + ";", $"处理失败：转换前路径填写错误! 已为转换后路径格式;");
                        continue;
                    }

                    var transResult = await _innerWorkOrderService.GetTransferDrillFilePath(wData.BeforeDrillFilePath);
                    if (transResult == null || transResult.Code != ResponseCode.Success)
                    {
                        if (transResult == null)
                        {
                            wData.Remark = await SetWDataRemark(wData.Remark, remarkArr[0] + ";", $"处理失败：调用钻带转换，返回null;");
                        }
                        else
                        {
                            wData.Remark = await SetWDataRemark(wData.Remark, remarkArr[0] + ";", $"处理失败：{transResult.Message}");
                        }
                        updateWorkOrders.Add(wData);
                        continue;
                    }

                    string afterFilePath = transResult.Data.verifyPath;
                    if (string.IsNullOrWhiteSpace(afterFilePath))
                    {
                        wData.Remark = await SetWDataRemark(wData.Remark, remarkArr[0] + ";", $"处理失败：调用钻带转换，verifyPath为空;");
                        updateWorkOrders.Add(wData);
                        continue;
                    }


                    if (remarkArr.Length > 0 && remarkArr[0].ToString().Contains("调用钻带转换"))
                    {
                        wData.Remark = wData.Remark.Replace(remarkArr[0].ToString(), "");
                        if (wData.Remark.StartsWith(";"))
                        {
                            wData.Remark = wData.Remark.Remove(0, 1);
                        }
                    }

                    wData.AfterDrillFilePath = afterFilePath;
                    wData.ModifyTime = DateTime.Now;
                    wData.ModifierId = UserId;

                    updateWorkOrders.Add(wData);
                    addLogs.Add(new WorkOrderAlterLog
                    {
                        WorkOrderCode = wData.Code,
                        DeviceCode = "",
                        ItemCode = wData.ItemCode,
                        ActionTime = DateTime.Now,
                        ActionDetail = $"GetAfterDrillFilePath WorkOrder {wData.Code} new AfterDrillFilePath {wData.AfterDrillFilePath} . ",
                        BeforeDrillFilePath = wData.BeforeDrillFilePath,
                        AfterDrillFilePath = "",
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        Status = (int)DataStatusEnum.Enable
                    });

                    await Task.Delay(500);
                }

                if (updateWorkOrders == null || updateWorkOrders.Count == 0) return;

                await _externalWorkOrderService.BulkUpdate(updateWorkOrders);
                await _workOrderAlterLogDomainService.BulkInsert(addLogs);
            }
            catch (Exception e)
            {
                _logger.LogError($"GetAfterDrillFilePath Error: Catch exceptions：{Environment.NewLine}{e}");
            }
        }

        private async Task<string> SetWDataRemark(string wDataR, string remarkArr, string newRemark)
        {
            if (string.IsNullOrEmpty(wDataR))
            {
                return newRemark;
            }
            else
            {
                if (remarkArr.ToString().Contains("调用钻带转换"))
                {
                    string str = wDataR.Replace(remarkArr.ToString(), "");
                    return newRemark + str;
                }
                else
                {
                    return newRemark + wDataR;
                }
            }
        }

        public async Task RebrushAfterDrillFilePath()
        {
            try
            {
                var workDatas = await _innerWorkOrderDomainService.QueryAsync(p => string.IsNullOrEmpty(p.AfterDrillFilePath)
                && !string.IsNullOrEmpty(p.BeforeDrillFilePath) && p.IsRebrush == 1, p => p.CreateTime, OrderByType.Desc);
                if (!workDatas.Any())
                {
                    return;
                }

                List<WorkOrder> updateWorkOrders = new List<WorkOrder>();
                List<WorkOrderAlterLog> addLogs = new List<WorkOrderAlterLog>();
                foreach (var wData in workDatas)
                {
                    if (await _innerWorkOrderService.VerifyBeforePath(wData.BeforeDrillFilePath))
                    {
                        _logger.LogInformation($"RebrushAfterDrillFilePath {wData.Code} BeforeDrillFilePath is contains _ConvDuo. {wData.BeforeDrillFilePath}");
                        continue;
                    }

                    string oldStr = wData.AfterDrillFilePath;

                    var transResult = await _innerWorkOrderService.GetTransferDrillFilePath(wData.BeforeDrillFilePath);
                    if (transResult == null || transResult.Code != ResponseCode.Success)
                    {
                        continue;
                    }

                    string afterFilePath = transResult.Data.verifyPath;
                    if (string.IsNullOrEmpty(afterFilePath))
                    {
                        continue;
                    }

                    wData.AfterDrillFilePath = afterFilePath;
                    wData.IsRebrush = 0;
                    wData.ModifyTime = DateTime.Now;
                    wData.ModifierId = UserId;

                    updateWorkOrders.Add(wData);
                    addLogs.Add(new WorkOrderAlterLog
                    {
                        WorkOrderCode = wData.Code,
                        DeviceCode = "",
                        ItemCode = wData.ItemCode,
                        ActionTime = DateTime.Now,
                        ActionDetail = $"Rebrush WorkOrder {wData.Code} old AfterDrillFilePath {oldStr} new AfterDrillFilePath {wData.AfterDrillFilePath} . ",
                        BeforeDrillFilePath = wData.BeforeDrillFilePath,
                        AfterDrillFilePath = oldStr,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        Status = (int)DataStatusEnum.Enable
                    });

                    await Task.Delay(500);
                }

                var updateResult = await _innerWorkOrderDomainService.BulkUpdate(updateWorkOrders);
                await _workOrderAlterLogDomainService.BulkInsert(addLogs);

                if (updateResult)
                {
                    var itemCodes = updateWorkOrders.Where(p => !string.IsNullOrEmpty(p.ItemCode))
                        .Select(p => p.ItemCode.ToLower()).Distinct().ToList();
                    if (itemCodes == null || itemCodes.Count == 0)
                    {
                        return;
                    }

                    var itemDatas = await _itemDomainService.QueryAsync(p => !string.IsNullOrEmpty(p.Code)
                    && itemCodes.Contains(p.Code.ToLower()), p => p.Code, OrderByType.Asc);
                    if (itemDatas == null || itemDatas.Count == 0)
                    {
                        return;
                    }

                    List<Item> updateItems = new List<Item>();
                    foreach (var item in itemDatas)
                    {
                        var workOrder = updateWorkOrders.Where(p => !string.IsNullOrEmpty(p.ItemCode)
                        && p.ItemCode.ToLower() == item.Code.ToLower()).OrderByDescending(p => p.ModifyTime).ToList();
                        if (workOrder == null || workOrder.Count == 0)
                        {
                            continue;
                        }

                        item.SpecGroup = workOrder[0].SpecGroup;
                        item.BeforeDrillFilePath = workOrder[0].BeforeDrillFilePath;
                        item.AfterDrillFilePath = workOrder[0].AfterDrillFilePath;
                        item.ModifierId = UserId;
                        item.ModifyTime = workOrder[0].ModifyTime;

                        updateItems.Add(item);
                    }
                    await _itemDomainService.BulkUpdate(updateItems);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"RebrushAfterDrillFilePath Error: Catch exceptions：{Environment.NewLine}{e}");
            }
        }

        public async Task RefreshWorkOrderToTask()
        {
            try
            {
                var query = _sqlSugarScope.Queryable<WorkTask, WorkOrder>
                ((t, w) => new object[]
                    {
                        JoinType.Left, t.WorkOrderCode.ToLower() == w.Code.ToLower()
                    });

                query = query.Where((t, w) => (string.IsNullOrEmpty(t.BeforeDrillFilePath) && !string.IsNullOrEmpty(w.BeforeDrillFilePath))
                || (string.IsNullOrEmpty(t.AfterDrillFilePath) && !string.IsNullOrEmpty(w.AfterDrillFilePath)));

                query = query.Where((t, w) => t.TaskStatus != TaskStatusEnum.FINISH);

                query = query.Where((t, w) => t.IsRebrush == 1);

                var tDatas = await query.Select((t, w) => new WorkTask
                {
                    Id = t.Id,
                    Name = t.Name,
                    Code = t.Code,
                    ParentId = t.ParentId,
                    WorkOrderId = t.WorkOrderId,
                    WorkOrderCode = t.WorkOrderCode,
                    WorkOrderName = t.WorkOrderName,
                    WorkStationId = t.WorkStationId,
                    WorkStationCode = t.WorkStationCode,
                    WorkStationName = t.WorkStationName,
                    BatchCode = t.BatchCode,
                    ProcessId = t.ProcessId,
                    ProcessCode = t.ProcessCode,
                    ProcessName = t.ProcessName,
                    ItemId = t.ItemId,
                    ItemCode = t.ItemCode,
                    ItemName = t.ItemName,
                    ItemTypeId = t.ItemTypeId,
                    SpecGroup = t.SpecGroup,
                    Specification = t.Specification,
                    UnitOfMeasure = t.UnitOfMeasure,
                    Quantity = t.Quantity,
                    QuantityProduced = t.QuantityProduced,
                    QuantityQuanlify = t.QuantityQuanlify,
                    QuantityUnquanlify = t.QuantityUnquanlify,
                    ClientId = t.ClientId,
                    ClientCode = t.ClientCode,
                    ClientName = t.ClientName,
                    Color = t.Color,
                    CreateTime = t.CreateTime,
                    CreatorId = t.CreatorId,
                    Duration = t.Duration,
                    RealDuration = t.RealDuration,
                    StartTime = t.StartTime,
                    RealStartTime = t.RealStartTime,
                    EndTime = t.EndTime,
                    RealEndTime = t.RealEndTime,
                    Status = t.Status,
                    IsDeleted = t.IsDeleted,
                    IsStarted = t.IsStarted,
                    IsUrgent = t.IsUrgent,
                    RequestDate = t.RequestDate,
                    RouteCode = t.RouteCode,
                    RouteId = t.RouteId,
                    RouteName = t.RouteName,
                    TaskStatus = t.TaskStatus,
                    KeyFlag = t.KeyFlag,
                    NowWadCount = t.NowWadCount,
                    PanelCount = t.PanelCount,
                    Ancestors = t.Ancestors,
                    IncodeNumber = w.IncodeNumber,
                    LayerNum = w.LayerNum,
                    ModifierId = UserId,
                    ModifyTime = DateTime.Now,
                    AfterDrillFilePath = w.AfterDrillFilePath,
                    BeforeDrillFilePath = w.BeforeDrillFilePath,
                    IsRebrush = 0,
                }).ToListAsync();

                if (!tDatas.Any())
                {
                    return;
                }

                await _taskDomainService.BulkUpdate(tDatas);
            }
            catch (Exception e)
            {
                _logger.LogError($"RefreshWorkOrderToTask Error: Catch exceptions：{Environment.NewLine}{e}");
            }
        }

        public async Task<ResponseDto<PageDto<ExternalWorkOrderDto>>> GetExterWorkOrderList(GetExterWorkOrderListReq req)
        {
            if (req == null)
            {
                return Fail<PageDto<ExternalWorkOrderDto>>("信息格式错误!");
            }
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ExternalWorkOrderDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ExternalWorkOrder>();
            if (!await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_NOT_WIP_DATA, SysConfigCategoryEnum.Kinwong, false))
            {
                where = where.And(p => p.IsDeleted == 0);
            }

            if (!string.IsNullOrEmpty(req.SourceCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SourceCode) && p.SourceCode.ToLower().Contains(req.SourceCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.SpecGroup))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SpecGroup) && p.SpecGroup.ToLower().Contains(req.SpecGroup.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower().Contains(req.Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.ToLower().Contains(req.ItemCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.RouteCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.RouteCode) && p.RouteCode.ToLower().Contains(req.RouteCode.ToLower()));
            }
            if (req.RequestDate != null)
            {
                where = where.And(s => s.RequestDate >= req.RequestDate.Value);
            }

            if (req.IsErrorData > -1)
            {
                where = where.And(p => p.IsErrorData == req.IsErrorData);
            }

            if (req.ExternalStatus.HasValue)
            {
                switch (req.ExternalStatus.Value)
                {
                    case HandleExternalWorkOrderStatusEnum.NotHandle:
                        where = where.And(p => p.Status == 0 && !p.Remark.StartsWith("处理失败"));
                        break;

                    case HandleExternalWorkOrderStatusEnum.HandleFail:
                        where = where.And(p => p.Remark.StartsWith("处理失败"));
                        break;

                    case HandleExternalWorkOrderStatusEnum.HandleSuccess:
                        where = where.And(p => p.Status == 1);
                        break;
                }
            }

            var result = await _domainService.QueryPageAsync(where, p => p.Id, OrderByType.Desc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<ExternalWorkOrder>, List<ExternalWorkOrderDto>>(result.ToList());
            var innerOrderIds = pageDto.List.Select(p => p.InnerOrderId).ToList();
            var innerOrders = await _innerWorkOrderDomainService.QueryAsync(s => innerOrderIds.Contains(s.Id), s => s.Id, OrderByType.Asc);
            var directBuildTaskSwitch = await _sysConfigManager.GetBoolValue(MESConfigConstants.DIRECT_BUILD_TASK, Model.Enum.SysConfigCategoryEnum.None, false);
            foreach (var item in pageDto.List)
            {
                item.Remark = item.IsErrorData == 0 ? "已处理" : item.Remark;//非自动排产，统一显示已处理
                item.InnerOrderStatus = innerOrders.FirstOrDefault(s => s.Id == item.InnerOrderId)?.ManuOrderStatus;
                if (string.IsNullOrEmpty(item.IsHold) || string.IsNullOrEmpty(item.IsInPlanWarehouse))
                {
                    continue;
                }
                if (item.IsHold.Contains("是"))
                {
                    continue;
                }

                if (item.IsInPlanWarehouse.Contains("否"))
                {
                    item.KwAGVStockInEnable = true;
                    item.KwHoldEnable = true;
                }
            }

            return Success(pageDto);
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
                List<ExternalWorkOrder> deleteList = new List<ExternalWorkOrder>();
                for (int i = 0; i < idList.Count; i++)
                {
                    var entity = await _domainService.QueryByID(idList[i]);
                    if (entity == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(entity.Code))
                    {
                        var isAnyStarted = await _taskDomainService.IsExistAsync(p => p.WorkOrderCode.ToLower() == entity.Code.ToLower()
                        && p.TaskStatus != TaskStatusEnum.DRAFT
                        && p.TaskStatus != TaskStatusEnum.COMMITED);

                        if (isAnyStarted)
                        {
                            return Fail($"外部工单号：{entity.SourceCode}、内部工单号：{entity.Code} 存在生产任务已开始调度，不能删除此工单");
                        }
                    }

                    deleteList.Add(entity);
                }

                if (deleteList.Count == 0)
                {
                    return Success();
                }

                _unitOfWork.BeginTran();

                var sourceCodeList = deleteList.Select(x => x.SourceCode.Trim().ToLower()).ToList();
                List<string> workOrderCodes = new List<string>();

                foreach (var item in deleteList)
                {
                    if (string.IsNullOrEmpty(item.Code))
                    {
                        continue;
                    }

                    if (item.InnerOrderId == null && await _innerWorkOrderDomainService.IsExistAsync(p => p.Code == item.Code))
                    {
                        var data = await _innerWorkOrderDomainService.FindSingleAsync(p => p.Code == item.Code);
                        if (data != null)
                        {
                            item.InnerOrderId = data.Id;
                        }
                    }

                    await _innerWorkOrderService.RobackTask(new AddOrUpdateTaskReq
                    {
                        WorkOrderCode = item.Code,
                    });

                    if (item.InnerOrderId != null)
                    {
                        await _innerWorkOrderService.UnCommit(new CommitWorkOrderReq { Id = item.InnerOrderId });
                    }

                    workOrderCodes.Add(item.Code.ToLower());
                }

                await _taskDomainService.DeleteAsync(p => workOrderCodes.Contains(p.WorkOrderCode.ToLower()));

                await _innerWorkOrderDomainService.DeleteAsync(p => workOrderCodes.Contains(p.Code.ToLower()));

                await _workOrderAndPanelDomainService.DeleteAsync(p => sourceCodeList.Contains(p.ExternalWorkerOrder.Trim().ToLower()));

                var result = await _domainService.DeleteAsync(p => sourceCodeList.Contains(p.SourceCode.Trim().ToLower()));
                if (!result)
                {
                    return Fail("删除失败！");
                }

                _unitOfWork.CommitTran();
            }
            return Success();
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteDataByID(long id)
        {
            var externalOrder = await _domainService.QueryByID(id);
            if (externalOrder == null)
            {
                return Fail("未找到此工单!");
            }
            return await DeleteData(externalOrder.SourceCode);
        }

        private async Task<ResponseDto<List<StockInfoQueryDataDto>>> GetStockInfoByConfig()
        {
            var returnData = new List<StockInfoQueryDataDto>();

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.URL_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(urlAdress))
            {
                return Fail<List<StockInfoQueryDataDto>>("未识别有效的请求地址，请检查配置项URLKinwongStockInfoQuery是否配置！");
            }

            var targetPosArea = await _sysConfigManager.GetStringValue(MESConfigConstants.STOCK_INFO_QUERY_TARGET_POSAREA, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(targetPosArea))
            {
                return Fail<List<StockInfoQueryDataDto>>("未识别有效的区域类型，请检查配置项StockInfoQueryTargetPosArea是否配置！");
            }

            var targetPosCodes = await _sysConfigManager.GetStringValue(MESConfigConstants.STOCK_INFO_QUERY_TARGET_POSAREA_CODE, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(targetPosCodes))
            {
                return Fail<List<StockInfoQueryDataDto>>("未识别有效的目标区域编码，请检查配置项StockInfoQueryTargetPosAreaCode是否配置！");
            }

            var enableWithoutArea = await _sysConfigManager.GetBoolValue(MESConfigConstants.STOCK_INFO_QUERY_WITHOUT_AREA_ENABLE);

            bool isExsitStock = false;
            StringBuilder sb = new StringBuilder();
            var targetPosCode = targetPosCodes.Split('|');
            if (targetPosCode.Length > 0)
            {
                for (int i = 0; i < targetPosCode.Length; i++)
                {
                    string targetPosAreaReq = targetPosCode[i] + targetPosArea;

                    if (enableWithoutArea && NoTargetAreas.Contains(targetPosAreaReq))
                    {
                        _logger.LogInformation($"查询区域{targetPosAreaReq}不存在，已被过滤！");
                        sb.Append($"{targetPosAreaReq}；");
                        continue;
                    }

                    var stockInfos = await GetStockInfos(urlAdress, targetPosAreaReq);
                    if (stockInfos == null || stockInfos.Count == 0)
                    {
                        sb.Append($"{targetPosAreaReq}；");
                        continue;
                    }

                    returnData.AddRange(stockInfos);
                    isExsitStock = true;
                }
            }

            if (!isExsitStock)
            {
                return Fail<List<StockInfoQueryDataDto>>($"目标区域：{sb.ToString()} 不存在至少一个料仓！");
            }
            return Success(returnData);
        }

        public async Task SendMoveSiloCommand()
        {
            var moveUrlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_GEN_AGV_SCHEDULING_TASK_URL,
                            SysConfigCategoryEnum.None, false);
            if (string.IsNullOrEmpty(moveUrlAdress))
            {
                _logger.LogError($"未识别有效的移动料仓请求地址，请检查配置项JingWangGenAgvSchedulingTaskUrl是否配置！");
                return;
            }

            var taskTyp = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_GEN_AGV_SCHEDULING_TASK_TASKTYP, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(taskTyp))
            {
                _logger.LogError($"未识别有效的移动料仓任务类型，请检查配置项JingWangGenAgvSchedulingTask_taskTyp是否配置！");
                return;
            }

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.URL_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(urlAdress))
            {
                _logger.LogError($"未识别有效的库存查询请求地址，请检查配置项URLKinwongStockInfoQuery是否配置！");
                return;
            }

            var needMoveWorkOrders = await _externalWorkOrderService.QueryAsync(p => p.IsDeleted == 0
            && !string.IsNullOrEmpty(p.IsInPlanWarehouse) && p.IsInPlanWarehouse.Contains("否")
            && !string.IsNullOrEmpty(p.IsHold) && p.IsHold.Contains("否"), p => p.Id, OrderByType.Asc);
            if (needMoveWorkOrders == null || needMoveWorkOrders.Count == 0)
            {
                return;
            }

            List<ExternalWorkOrder> updateList = new List<ExternalWorkOrder>();
            List<GenAgvSchedulingTaskReq> genReqs = new List<GenAgvSchedulingTaskReq>();

            await ProcessingData(urlAdress, needMoveWorkOrders, updateList, genReqs);

            if (genReqs.Count == 0)
            {
                return;
            }

            foreach (var genReq in genReqs)
            {
                genReq.reqCode = Guid.NewGuid().ToString();
                genReq.taskTyp = taskTyp;

                string genReqStr = JsonSerializer.Serialize(genReq);
                try
                {
                    var result = _apiHelper.RequestData(moveUrlAdress, "post", genReqStr);
                    if (string.IsNullOrEmpty(result))
                    {
                        _logger.LogError($"GenAgvSchedulingTask api {moveUrlAdress},reqStr {genReqStr}, return empty ");
                        continue;
                    }
                    _logger.LogInformation($"GenAgvSchedulingTask api {moveUrlAdress},request:{genReqStr}, result:{result}");

                    var jobject = JObject.Parse(result);
                    if (jobject.ContainsKey("code"))
                    {
                        if (jobject["code"].ToString() != "0")
                        {
                            string msg = jobject["message"].ToString();
                            _logger.LogError($"GenAgvSchedulingTask api {moveUrlAdress}, return Code{jobject["code"].ToString()}, Message {msg}");
                            continue;
                        }
                        else
                        {
                            IsSendedLots.Add(genReq.data.materialLot);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"GenAgvSchedulingTask api {moveUrlAdress} Error");
                }

                await Task.Delay(500);
            }

            await _externalWorkOrderService.BulkUpdate(updateList);
        }

        public async Task KwAGVStockVerifyAndIn()
        {
            var where = PredicateBuilder.True<ExternalWorkOrder>();
            where = where.And(p => p.IsDeleted == 0 && p.Status != (int)HandleExternalWorkOrderStatusEnum.HandleSuccess
            && !string.IsNullOrEmpty(p.IsInPlanWarehouse) && p.IsInPlanWarehouse == "否" && p.IsSendMoveInfo == 1
            && !string.IsNullOrEmpty(p.IsHold) && !p.IsHold.Contains("是"));

            var lstExternalWorkOrder = (await _domainService.QueryAsync(where, p => p.ModifyTime
                                     , OrderByType.Asc))
                                     .ToList()
                                    .OrderByDescending(p => p.IsUrgent).ToList();

            if (lstExternalWorkOrder == null || !lstExternalWorkOrder.Any()) return;

            var enableStockQuery = await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false);
            ResponseDto<List<StockInfoQueryDataDto>> exsitStockInfos = null;
            if (enableStockQuery)
            {
                var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_KW_AGV_STOCK_IN_URL, SysConfigCategoryEnum.Kinwong, false);
                if (string.IsNullOrEmpty(urlAdress))
                {
                    _logger.LogError($"未识别有效的mes转仓请求地址，请检查配置项JingWangkwAGVStockInUrl是否配置！");
                    return;
                }

                exsitStockInfos = await GetStockInfoByConfig();

                bool isEdit = false;
                foreach (var workOrder in lstExternalWorkOrder)
                {
                    var verifyResult = await VerifyStockQuantity(exsitStockInfos, workOrder);
                    if (verifyResult == null)
                    {
                        _logger.LogError($"KwAGVStockIn 校验库存信息VerifyStockQuantity，异常！");
                        continue;
                    }

                    if (verifyResult.Code != ResponseCode.Success || verifyResult.Data == null || verifyResult.Data.Count == 0)
                    {
                        _logger.LogError($"KwAGVStockIn 校验库存信息VerifyStockQuantity，{verifyResult.Message}！");
                        continue;
                    }

                    string currentPosArea = string.Empty;
                    if (!string.IsNullOrEmpty(workOrder.IsInPlanWarehouseRemark) && workOrder.IsInPlanWarehouseRemark.Contains(";"))
                    {
                        var arrInPlans = workOrder.IsInPlanWarehouseRemark.Split(';');
                        if (arrInPlans != null && arrInPlans.Length > 1)
                        {
                            currentPosArea = arrInPlans[1];
                        }
                    }
                    if (string.IsNullOrEmpty(currentPosArea))
                    {
                        _logger.LogError($"未识别有效的工单 {workOrder.SourceCode} 目标区域 {workOrder.IsInPlanWarehouseRemark}！");
                        continue;
                    }

                    KwAGVStockInReq kwAGVStockInReq = new KwAGVStockInReq
                    {
                        Container = workOrder.SourceCode,
                        Location = currentPosArea
                    };

                    var stockInResult = await KwAGVStockIn(urlAdress, kwAGVStockInReq);
                    if (stockInResult == null || stockInResult.ResultCode != "0")
                    {
                        var holdUrlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_HOLD_LOT_URL, SysConfigCategoryEnum.Kinwong, false);
                        if (string.IsNullOrEmpty(holdUrlAdress))
                        {
                            _logger.LogError($"未识别有效的mes暂停Lot请求地址，请检查配置项JingWangHoldLotUrl是否配置！");
                            continue;
                        }

                        HoldLotReq holdLotReq = new HoldLotReq
                        {
                            ContainerName = workOrder.SourceCode,
                            HoldReason = stockInResult.ResultMsg
                        };

                        var holdRsult = await KwHoldLot(holdUrlAdress, holdLotReq);
                        if (holdRsult == null)
                        {
                            _logger.LogError("Send KwHoldLot 异常");
                            continue;
                        }

                        if (holdRsult.ResultCode != "0")
                        {
                            _logger.LogError($"Send KwHoldLot 返回错误！{holdRsult.ResultMsg}");
                            continue;
                        }

                        workOrder.IsHold = "HOLD:是";
                        workOrder.IsErrorData = 1;
                        if (!workOrder.Remark.Contains("处理失败"))
                        {
                            workOrder.Remark = "处理失败：HOLD:是;";
                        }
                        else if (!workOrder.Remark.Contains("HOLD"))
                        {
                            workOrder.Remark = workOrder.Remark + "HOLD:是;";
                        }
                        workOrder.ModifyTime = DateTime.Now;
                    }

                    workOrder.IsInPlanWarehouse = "是";
                    workOrder.ModifyTime = DateTime.Now;
                    var remarks = workOrder.Remark.Split(';');
                    if (remarks != null)
                    {
                        foreach (var remark in remarks)
                        {
                            if (remark.Contains("Param02"))
                            {
                                workOrder.Remark = workOrder.Remark.Replace(remark, "");
                            }
                        }
                        workOrder.Remark = workOrder.Remark.Replace(";;", ";");
                        if (workOrder.Remark.StartsWith(";"))
                        {
                            workOrder.Remark = workOrder.Remark.Remove(0, 1);
                        }
                    }
                    if (workOrder.Remark.Split(';') == null || workOrder.Remark.Split(';').Length < 2 || string.IsNullOrEmpty(workOrder.Remark.Split(';')[0]))
                    {
                        workOrder.IsErrorData = 0;
                    }

                    isEdit = true;
                }

                if (isEdit)
                {
                    await _externalWorkOrderService.BulkUpdate(lstExternalWorkOrder);
                }
            }
        }

        public async Task<ResponseDto<string>> KwAGVStockInByData(string externalWorkOrderCode)
        {
            if (string.IsNullOrEmpty(externalWorkOrderCode))
            {
                return Fail("未识别有效的externalWorkOrderCode!");
            }

            var externalData = await _externalWorkOrderService.QueryAsync(p => p.SourceCode.ToLower() == externalWorkOrderCode.ToLower(), p => p.Id, OrderByType.Asc);
            if (externalData == null || externalData.Count == 0)
            {
                return Fail("未找到工单数据!");
            }

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_KW_AGV_STOCK_IN_URL, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(urlAdress))
            {
                _logger.LogError($"未识别有效的mes转仓请求地址，请检查配置项JingWangkwAGVStockInUrl是否配置！");
                return Fail($"未识别有效的mes转仓请求地址，请检查配置项JingWangkwAGVStockInUrl是否配置！");
            }

            var exsitStockInfos = await GetStockInfoByConfig();
            var verifyResult = await VerifyStockQuantity(exsitStockInfos, externalData[0]);
            if (verifyResult == null)
            {
                _logger.LogError($"KwAGVStockIn 校验库存信息VerifyStockQuantity，异常！");
                return Fail($"KwAGVStockIn 校验库存信息VerifyStockQuantity，异常！");
            }

            if (verifyResult.Code != ResponseCode.Success || verifyResult.Data == null || verifyResult.Data.Count == 0)
            {
                _logger.LogError($"KwAGVStockIn 校验库存信息VerifyStockQuantity，{verifyResult.Message}！");
                return Fail($"KwAGVStockIn 校验库存信息VerifyStockQuantity，{verifyResult.Message}！");
            }

            string currentPosArea = string.Empty;
            if (!string.IsNullOrEmpty(externalData[0].IsInPlanWarehouseRemark) && externalData[0].IsInPlanWarehouseRemark.Contains(";"))
            {
                var arrInPlans = externalData[0].IsInPlanWarehouseRemark.Split(';');
                if (arrInPlans != null && arrInPlans.Length > 1)
                {
                    currentPosArea = arrInPlans[1];
                }
            }
            if (string.IsNullOrEmpty(currentPosArea))
            {
                _logger.LogError($"未识别有效的工单 {externalData[0].SourceCode} 目标区域 {externalData[0].IsInPlanWarehouseRemark}！");
                return Fail($"未识别有效的工单 {externalData[0].SourceCode} 目标区域 {externalData[0].IsInPlanWarehouseRemark}！");
            }

            KwAGVStockInReq kwAGVStockInReq = new KwAGVStockInReq
            {
                Container = externalData[0].SourceCode,
                Location = currentPosArea
            };

            var stockInResult = await KwAGVStockIn(urlAdress, kwAGVStockInReq);
            if (stockInResult == null)
            {
                return Fail("Send KwAGVStockIn 异常");
            }

            if (stockInResult.ResultCode != "0")
            {
                return Fail($"Send KwAGVStockIn 返回错误！{stockInResult.ResultMsg}");
            }

            externalData[0].IsInPlanWarehouse = "是";
            externalData[0].ModifyTime = DateTime.Now;
            await _externalWorkOrderService.Update(externalData[0]);

            return Success();
        }

        public async Task<ResponseDto<string>> KwHoldLotByData(string externalWorkOrderCode)
        {
            if (string.IsNullOrEmpty(externalWorkOrderCode))
            {
                return Fail("未识别有效的externalWorkOrderCode!");
            }

            var externalData = await _externalWorkOrderService.QueryAsync(p => p.SourceCode.ToLower() == externalWorkOrderCode.ToLower(), p => p.Id, OrderByType.Asc);
            if (externalData == null || externalData.Count == 0)
            {
                return Fail("未找到工单数据!");
            }

            var holdUrlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_HOLD_LOT_URL, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(holdUrlAdress))
            {
                _logger.LogError($"未识别有效的mes暂停Lot请求地址，请检查配置项JingWangHoldLotUrl是否配置！");
                return Fail($"未识别有效的mes暂停Lot请求地址，请检查配置项JingWangHoldLotUrl是否配置！");
            }

            HoldLotReq holdLotReq = new HoldLotReq
            {
                ContainerName = externalData[0].SourceCode,
                HoldReason = "人工发起暂停"
            };

            var holdRsult = await KwHoldLot(holdUrlAdress, holdLotReq);
            if (holdRsult == null)
            {
                return Fail("Send KwHoldLot 异常");
            }

            if (holdRsult.ResultCode != "0")
            {
                return Fail($"Send KwHoldLot 返回错误！{holdRsult.ResultMsg}");
            }

            externalData[0].IsHold = "HOLD:是";
            externalData[0].IsErrorData = 1;
            if (!externalData[0].Remark.Contains("处理失败"))
            {
                externalData[0].Remark = "处理失败：HOLD:是;";
            }
            else if (!externalData[0].Remark.Contains("HOLD"))
            {
                externalData[0].Remark = externalData[0].Remark + "HOLD:是;";
            }
            externalData[0].ModifyTime = DateTime.Now;

            await _externalWorkOrderService.Update(externalData[0]);

            return Success();
        }

        public async Task<ResponseDto<string>> KwGenAgvSchedulingTask(string externalWorkOrderCode)
        {
            if (string.IsNullOrEmpty(externalWorkOrderCode))
            {
                return Fail("未识别有效的externalWorkOrderCode!");
            }

            var externalData = await _externalWorkOrderService.QueryAsync(p => p.SourceCode.ToLower() == externalWorkOrderCode.ToLower()
            && !string.IsNullOrEmpty(p.IsInPlanWarehouse) && p.IsInPlanWarehouse.Contains("否")
            && !string.IsNullOrEmpty(p.IsHold) && p.IsHold.Contains("否"), p => p.Id, OrderByType.Asc);
            if (externalData == null || externalData.Count == 0)
            {
                return Fail("未找到工单数据或该工单已被暂停或该工单的料仓已在钻孔计划仓!");
            }

            var moveUrlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_GEN_AGV_SCHEDULING_TASK_URL,
                            SysConfigCategoryEnum.None, false);
            if (string.IsNullOrEmpty(moveUrlAdress))
            {
                _logger.LogError($"未识别有效的移动料仓请求地址，请检查配置项JingWangGenAgvSchedulingTaskUrl是否配置！");
                return Fail($"未识别有效的移动料仓请求地址，请检查配置项JingWangGenAgvSchedulingTaskUrl是否配置！");
            }

            var taskTyp = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_GEN_AGV_SCHEDULING_TASK_TASKTYP, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(taskTyp))
            {
                _logger.LogError($"未识别有效的移动料仓任务类型，请检查配置项JingWangGenAgvSchedulingTask_taskTyp是否配置！");
                return Fail($"未识别有效的移动料仓任务类型，请检查配置项JingWangGenAgvSchedulingTask_taskTyp是否配置！");
            }

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.URL_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(urlAdress))
            {
                _logger.LogError($"未识别有效的库存查询请求地址，请检查配置项URLKinwongStockInfoQuery是否配置！");
                return Fail($"未识别有效的库存查询请求地址，请检查配置项URLKinwongStockInfoQuery是否配置！");
            }

            var needMoveWorkOrders = new List<ExternalWorkOrder>
            {
                externalData[0]
            };
            List<ExternalWorkOrder> updateList = new List<ExternalWorkOrder>();
            List<GenAgvSchedulingTaskReq> genReqs = new List<GenAgvSchedulingTaskReq>();

            await ProcessingData(urlAdress, needMoveWorkOrders, updateList, genReqs);

            if (genReqs.Count == 0)
            {
                return Fail($"未识别有效的移动料仓数据！");
            }

            StringBuilder sb = new StringBuilder();
            foreach (var genReq in genReqs)
            {
                genReq.reqCode = Guid.NewGuid().ToString();
                genReq.taskTyp = taskTyp;

                string genReqStr = JsonSerializer.Serialize(genReq);
                try
                {
                    var result = _apiHelper.RequestData(moveUrlAdress, "post", genReqStr);
                    if (string.IsNullOrEmpty(result))
                    {
                        _logger.LogError($"GenAgvSchedulingTask api {moveUrlAdress},reqStr {genReqStr}, return empty ");
                        sb.Append($"GenAgvSchedulingTask api {moveUrlAdress},reqStr {genReqStr}, return empty ");
                        continue;
                    }
                    _logger.LogInformation($"GenAgvSchedulingTask api {moveUrlAdress},request:{genReqStr}, result:{result}");

                    var jobject = JObject.Parse(result);
                    if (jobject.ContainsKey("code") && jobject["code"].ToString() != "0")
                    {
                        string msg = jobject["message"].ToString();
                        _logger.LogError($"GenAgvSchedulingTask api {moveUrlAdress}, return Code{jobject["code"].ToString()}, Message {msg}");
                        sb.Append($"GenAgvSchedulingTask api {moveUrlAdress}, return Code{jobject["code"].ToString()}, Message {msg}");
                        continue;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"GenAgvSchedulingTask api {moveUrlAdress} Error");
                    sb.Append($"GenAgvSchedulingTask api {moveUrlAdress} catch Exception!");
                }

                await Task.Delay(500);
            }

            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                return Fail(sb.ToString());
            }

            await _externalWorkOrderService.BulkUpdate(updateList);

            return Success();
        }

        public async Task<ResponseDto<List<StockInfoQueryDataDto>>> GetStockInfoList(MesStockInfoQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<StockInfoQueryDataDto>>("未识别有效的入参!");
            }

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.URL_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(urlAdress))
            {
                return Fail<List<StockInfoQueryDataDto>>("未识别有效的请求地址，请检查配置项URLKinwongStockInfoQuery是否配置！");
            }

            var enableWithoutArea = await _sysConfigManager.GetBoolValue(MESConfigConstants.STOCK_INFO_QUERY_WITHOUT_AREA_ENABLE);

            List<StockInfoQueryDataDto> resultDtos = new List<StockInfoQueryDataDto>();

            if (string.IsNullOrEmpty(req.TargetPosArea))
            {
                var targetPosArea = await _sysConfigManager.GetStringValue(MESConfigConstants.STOCK_INFO_QUERY_TARGET_POSAREA, SysConfigCategoryEnum.Kinwong, false);
                if (string.IsNullOrEmpty(targetPosArea))
                {
                    return Fail<List<StockInfoQueryDataDto>>("未识别有效的区域类型，请检查配置项StockInfoQueryTargetPosArea是否配置！");
                }

                var targetPosCodes = await _sysConfigManager.GetStringValue(MESConfigConstants.STOCK_INFO_QUERY_TARGET_POSAREA_CODE, SysConfigCategoryEnum.Kinwong, false);
                if (string.IsNullOrEmpty(targetPosCodes))
                {
                    return Fail<List<StockInfoQueryDataDto>>("未识别有效的目标区域编码，请检查配置项StockInfoQueryTargetPosAreaCode是否配置！");
                }

                var targetPosCode = targetPosCodes.Split('|');
                if (targetPosCode.Length > 0)
                {
                    for (int i = 0; i < targetPosCode.Length; i++)
                    {
                        string targetPosAreaReq = targetPosCode[i] + targetPosArea;

                        if (enableWithoutArea && NoTargetAreas.Contains(targetPosAreaReq))
                        {
                            _logger.LogInformation($"查询区域{targetPosAreaReq}不存在，已被过滤！");
                            continue;
                        }

                        var stockInfos = await GetStockInfos(urlAdress, targetPosAreaReq);
                        if (stockInfos == null || stockInfos.Count == 0)
                        {
                            continue;
                        }

                        resultDtos.AddRange(stockInfos);
                    }
                }
            }
            else
            {
                if (enableWithoutArea && NoTargetAreas.Contains(req.TargetPosArea))
                {
                    _logger.LogInformation($"查询区域{req.TargetPosArea}不存在，已被过滤！");
                    return Success(resultDtos);
                }

                var stockInfos = await GetStockInfos(urlAdress, req.TargetPosArea);
                if (stockInfos == null || stockInfos.Count == 0)
                {
                    return Success(resultDtos);
                }
                resultDtos = stockInfos;
            }

            if (!string.IsNullOrEmpty(req.Lot))
            {
                resultDtos = resultDtos.Where(p => !string.IsNullOrEmpty(p.lot) && p.lot.ToLower().Contains(req.Lot.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(req.product))
            {
                resultDtos = resultDtos.Where(p => !string.IsNullOrEmpty(p.product) && p.product.ToLower().Contains(req.product.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(req.address))
            {
                resultDtos = resultDtos.Where(p => !string.IsNullOrEmpty(p.address) && p.address.ToLower().Contains(req.address.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(req.addressName))
            {
                resultDtos = resultDtos.Where(p => !string.IsNullOrEmpty(p.addressName) && p.addressName.ToLower().Contains(req.addressName.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(req.podCode))
            {
                resultDtos = resultDtos.Where(p => !string.IsNullOrEmpty(p.podCode) && p.podCode.ToLower().Contains(req.podCode.ToLower())).ToList();
            }

            resultDtos = resultDtos.OrderBy(p => p.lot).ToList();

            return Success(resultDtos);
        }

        private async Task ProcessingData(string urlAdress, List<ExternalWorkOrder> needMoveWorkOrders, List<ExternalWorkOrder> updateList, List<GenAgvSchedulingTaskReq> genAgvSchedulingTaskReqs)
        {
            var enableWithoutArea = await _sysConfigManager.GetBoolValue(MESConfigConstants.STOCK_INFO_QUERY_WITHOUT_AREA_ENABLE);
            List<StockInfoQueryDataDto> nowPosAreaData = new List<StockInfoQueryDataDto>();
            List<string> nowPosArea = new List<string>();
            foreach (var order in needMoveWorkOrders)
            {
                if (!order.IsInPlanWarehouseRemark.Contains(";"))
                {
                    _logger.LogError($"SendMoveSiloCommand IsInPlanWarehouseRemark {order.IsInPlanWarehouseRemark} error !");
                    continue;
                }

                var arrInPlans = order.IsInPlanWarehouseRemark.Split(';');
                if (arrInPlans == null || arrInPlans.Length < 2)
                {
                    _logger.LogError($"SendMoveSiloCommand IsInPlanWarehouseRemark {order.IsInPlanWarehouseRemark} error !");
                    continue;
                }

                string currentPosArea = arrInPlans[0];
                string targetPosArea = arrInPlans[1];
                if (string.IsNullOrEmpty(currentPosArea) || string.IsNullOrEmpty(targetPosArea))
                {
                    _logger.LogError($"SendMoveSiloCommand IsInPlanWarehouseRemark {order.IsInPlanWarehouseRemark} error !");
                    continue;
                }

                if (!currentPosArea.EndsWith("${02}") && !currentPosArea.EndsWith("${04}"))
                {
                    currentPosArea = currentPosArea + "${04}";
                }

                if (!targetPosArea.EndsWith("${02}") && !targetPosArea.EndsWith("${04}"))
                {
                    targetPosArea = targetPosArea + "${04}";
                }

                if (enableWithoutArea && NoTargetAreas.Contains(currentPosArea))
                {
                    _logger.LogInformation($"查询区域{currentPosArea}不存在，已被过滤！");
                    continue;
                }

                if (!nowPosArea.Contains(currentPosArea))
                {
                    nowPosArea.Add(currentPosArea);

                    var stockInfos = await GetStockInfos(urlAdress, currentPosArea);
                    if (stockInfos == null || stockInfos.Count == 0)
                    {
                        continue;
                    }

                    nowPosAreaData.AddRange(stockInfos);
                }

                if (nowPosAreaData == null || nowPosAreaData.Count == 0)
                {
                    _logger.LogInformation($"SendMoveSiloCommand StockInfoQuery api {urlAdress}, data null !");
                    continue;
                }

                var containsLotDatas = nowPosAreaData.Where(p => !string.IsNullOrEmpty(p.lot) &&
                        p.lot.ToLower().StartsWith(order.SourceCode.ToLower())).ToList();
                if (containsLotDatas == null || containsLotDatas.Count == 0)
                {
                    _logger.LogInformation($"SendMoveSiloCommand StockInfoQuery api {urlAdress}, {currentPosArea} no data {order.SourceCode} !");
                    continue;
                }

                if (targetPosArea.Contains("${02}"))
                {
                    targetPosArea = targetPosArea.Replace("-", "_");
                }
                else if (targetPosArea.Contains("${04}"))
                {
                    targetPosArea = targetPosArea.Replace("_", "-");
                }

                foreach (var item in containsLotDatas)
                {
                    if (string.IsNullOrEmpty(item.lot))
                    {
                        continue;
                    }
                    if (IsSendedLots.Contains(item.lot))
                    {
                        continue;
                    }

                    genAgvSchedulingTaskReqs.Add(new GenAgvSchedulingTaskReq
                    {
                        userCallCodePath = new string[] { item.lot + "${01}", targetPosArea },
                        data = new GenAgvSchedulingTaskData
                        {
                            materialLot = item.lot,
                            sysLotNum = item.difQty.ToString(),
                            podLotNum = item.qty.ToString(),
                        }
                    });
                }

                order.IsSendMoveInfo = 1;
                if (!order.IsInPlanWarehouseRemark.Contains("已发送移动料仓到目标区域请求"))
                {
                    order.IsInPlanWarehouseRemark = order.IsInPlanWarehouseRemark + ";" + "已发送移动料仓到目标区域请求!";
                }
                order.ModifierId = UserId;
                order.ModifyTime = DateTime.Now;
                updateList.Add(order);
            }
        }

        private async Task<List<StockInfoQueryDataDto>> GetStockInfos(string urlAdress, string targetPosArea)
        {
            List<StockInfoQueryDataDto> resultDtos = new List<StockInfoQueryDataDto>();
            if (string.IsNullOrEmpty(urlAdress) || string.IsNullOrEmpty(targetPosArea))
            {
                return resultDtos;
            }

            if (targetPosArea.Contains("${02}"))
            {
                targetPosArea = targetPosArea.Replace("-", "_");
            }
            else if (targetPosArea.Contains("${04}"))
            {
                targetPosArea = targetPosArea.Replace("_", "-");
            }

            StockInfoQueryReq jWangRequest = new StockInfoQueryReq
            {
                ReqCode = Guid.NewGuid().ToString(),
                TargetPosArea = targetPosArea,
            };
            string reqStr = JsonSerializer.Serialize(jWangRequest);

            try
            {
                var result = _apiHelper.RequestData(urlAdress, "post", reqStr);
                if (string.IsNullOrEmpty(result))
                {
                    _logger.LogError($"StockInfoQuery api {urlAdress},reqStr {reqStr}, return empty ");
                    return resultDtos;
                }
                _logger.LogInformation($"StockInfoQuery api {urlAdress}, request:{reqStr}, result:{result}");

                var jobject = JObject.Parse(result);
                if (jobject.ContainsKey("code") && jobject["code"].ToString() != "0")
                {
                    string msg = jobject["message"].ToString();
                    if (msg.Contains("传入区域不存在"))
                    {
                        NoTargetAreas.Add(targetPosArea);
                    }
                    return resultDtos;
                }

                var stockInfoQueryDto = JsonSerializer.Deserialize<StockInfoQueryDto>(result);
                if (stockInfoQueryDto == null)
                {
                    _logger.LogError($"StockInfoQuery api {urlAdress}, analytical null {result}");
                    return resultDtos;
                }

                if (stockInfoQueryDto.code != "0")
                {
                    _logger.LogError($"StockInfoQuery api {urlAdress}, return Code {stockInfoQueryDto.code}");
                    return resultDtos;
                }

                if (stockInfoQueryDto.data == null || stockInfoQueryDto.data.Count == 0)
                {
                    _logger.LogError($"StockInfoQuery api {urlAdress}, data null {result}");
                    return resultDtos;
                }

                resultDtos.AddRange(stockInfoQueryDto.data);
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"StockInfoQuery api {urlAdress} Error");
            }

            return resultDtos;
        }

        private async Task<ResponseDto<List<StockInfoQueryDataDto>>> VerifyStockQuantity(ResponseDto<List<StockInfoQueryDataDto>> stockInfosByConfig, ExternalWorkOrder externalWorkOrder)
        {
            string msg = stockInfosByConfig != null ? stockInfosByConfig.Message : "";

            if (stockInfosByConfig == null)
            {
                stockInfosByConfig = new ResponseDto<List<StockInfoQueryDataDto>>();
            }

            string currentPosArea = string.Empty;
            if (!string.IsNullOrEmpty(externalWorkOrder.IsInPlanWarehouseRemark) && externalWorkOrder.IsInPlanWarehouseRemark.Contains(";"))
            {
                var arrInPlans = externalWorkOrder.IsInPlanWarehouseRemark.Split(';');
                if (arrInPlans != null && arrInPlans.Length > 1)
                {
                    currentPosArea = arrInPlans[1];
                }
            }

            if (!string.IsNullOrEmpty(currentPosArea))
            {
                currentPosArea = currentPosArea.Replace("_", "-");

                bool isReQueryStock = false;
                var targetPosConfig = await _sysConfigManager.GetStringValue(MESConfigConstants.STOCK_INFO_QUERY_TARGET_POSAREA_CODE, SysConfigCategoryEnum.Kinwong, false);
                if (!string.IsNullOrEmpty(targetPosConfig))
                {
                    targetPosConfig = targetPosConfig.Replace("_", "-").ToLower();
                    var targetPosCodes = targetPosConfig.Split('|');
                    if (!targetPosCodes.Contains(currentPosArea.ToLower()))
                    {
                        isReQueryStock = true;
                    }
                }
                else
                {
                    isReQueryStock = true;
                }

                if (isReQueryStock)
                {
                    if (!currentPosArea.EndsWith("${02}") && !currentPosArea.EndsWith("${04}"))
                    {
                        currentPosArea = currentPosArea + "${04}";
                    }

                    var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.URL_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false);
                    if (!string.IsNullOrEmpty(urlAdress))
                    {
                        var stockInfos = await GetStockInfos(urlAdress, currentPosArea);
                        if (stockInfos != null && stockInfos.Count > 0)
                        {
                            stockInfosByConfig.Data.AddRange(stockInfos);
                        }
                    }
                }
            }

            if (stockInfosByConfig.Data == null || stockInfosByConfig.Data.Count == 0)
            {
                if (!string.IsNullOrEmpty(currentPosArea))
                {
                    msg = msg + $"计划仓位 {currentPosArea} 不存在至少一个料仓!";
                }
                return Fail<List<StockInfoQueryDataDto>>($"处理失败：未识别有效的区域库存信息！{msg}");
            }

            var containsLotDatas = stockInfosByConfig.Data.Where(p => !string.IsNullOrEmpty(p.lot) &&
            p.lot.ToLower().StartsWith(externalWorkOrder.SourceCode.ToLower())).ToList();

            if (containsLotDatas == null || containsLotDatas.Count == 0)
            {
                return Fail<List<StockInfoQueryDataDto>>($"处理失败：未识别包含{externalWorkOrder.SourceCode}有效的区域库存信息！");
            }

            List<StockInfoQueryDataDto> resultDto = new List<StockInfoQueryDataDto>();
            int sumQty = 0;
            foreach (var item in containsLotDatas)
            {
                if (item.qty == null || item.qty < 0)
                {
                    continue;
                }
                sumQty += (int)item.qty;

                resultDto.Add(item);
            }

            if (externalWorkOrder.LotStockNum == null || externalWorkOrder.LotStockNum < sumQty)
            {
                if (!string.IsNullOrEmpty(externalWorkOrder.ItemCode)
                    && !await _innerWorkOrderDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.ItemCode)
                    && p.ItemCode.ToLower() == externalWorkOrder.ItemCode.ToLower()
                    && !(p.ManuOrderStatus == ManuOrderStatusEnum.DRAFT || p.ManuOrderStatus == ManuOrderStatusEnum.COMMITED)))
                {
                    externalWorkOrder.LotStockNum = sumQty;
                    externalWorkOrder.ModifyTime = DateTime.Now;
                    await _externalWorkOrderService.Update(externalWorkOrder);
                }
            }

            if (sumQty < externalWorkOrder.Quantity)
            {
                return Fail($"处理失败：{externalWorkOrder.SourceCode} 库存数量不够！库存数量{sumQty},生成需求数量{externalWorkOrder.Quantity}！", resultDto);
            }

            return Success(resultDto);
        }

        private async Task<JwangResponseDto> KwAGVStockIn(string urlAdress, KwAGVStockInReq kwAGVStockInReq)
        {
            JwangResponseDto returnDto = new JwangResponseDto();
            if (string.IsNullOrEmpty(urlAdress) || kwAGVStockInReq == null)
            {
                return returnDto;
            }

            try
            {
                string reqStr = JsonSerializer.Serialize(kwAGVStockInReq);
                var result = _apiHelper.RequestData(urlAdress, "post", reqStr);
                if (string.IsNullOrEmpty(result))
                {
                    _logger.LogError($"KwAGVStockIn api {urlAdress},reqStr {reqStr}, return empty ");
                    return returnDto;
                }
                _logger.LogInformation($"KwAGVStockIn api {urlAdress}, request:{reqStr}, result:{result}");

                returnDto = JsonSerializer.Deserialize<JwangResponseDto>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("Send KwAGVStockIn KwAGVStockIn 异常：" + ex.Message);
            }

            await Task.Delay(500);
            return returnDto;
        }

        private async Task<JwangResponseDto> KwHoldLot(string urlAdress, HoldLotReq holdLotReq)
        {
            JwangResponseDto returnDto = new JwangResponseDto();
            if (string.IsNullOrEmpty(urlAdress) || holdLotReq == null)
            {
                return returnDto;
            }
            try
            {
                string reqStr = JsonSerializer.Serialize(holdLotReq);
                var result = _apiHelper.RequestData(urlAdress, "post", reqStr);
                if (string.IsNullOrEmpty(result))
                {
                    _logger.LogError($"KwHoldLot api {urlAdress},reqStr {reqStr}, return empty ");
                    return returnDto;
                }
                _logger.LogInformation($"KwHoldLot api {urlAdress}, request:{reqStr}, result:{result}");

                returnDto = JsonSerializer.Deserialize<JwangResponseDto>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("Send KwAGVStockIn KwHoldLot 异常：" + ex.Message);
            }

            await Task.Delay(500);
            return returnDto;
        }

        public async Task RefreshLotStockNum()
        {
            if (!await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false))
            {
                return;
            }

            var externalOrders = await _externalWorkOrderService.QueryAsync(p =>

            //(p.LotStockNum == null || p.LotStockNum == 0)
            p.IsDeleted == 0
            //&& p.Status == (int)HandleExternalWorkOrderStatusEnum.NotHandle
            , p => p.SourceCode, OrderByType.Asc);

            if (externalOrders == null || externalOrders.Count == 0)
            {
                return;
            }

            List<StockInfoQueryDataDto> stockInfoByConfig = new List<StockInfoQueryDataDto>();
            var stockInfos = await GetStockInfoByConfig();
            if (stockInfos != null && stockInfos.Data != null)
            {
                stockInfoByConfig = stockInfos.Data;
            }

            List<WorkOrder> workOrders = new List<WorkOrder>();
            var itemCodeDatas = externalOrders.Where(p => !string.IsNullOrEmpty(p.ItemCode)).Select(p => p.ItemCode.ToLower()).ToList();
            if (itemCodeDatas != null && itemCodeDatas.Count > 0)
            {
                workOrders = await _innerWorkOrderDomainService.QueryAsync(p => !string.IsNullOrEmpty(p.ItemCode)
                && itemCodeDatas.Contains(p.ItemCode.ToLower())
                && !(p.ManuOrderStatus == ManuOrderStatusEnum.DRAFT || p.ManuOrderStatus == ManuOrderStatusEnum.COMMITED)
                , p => p.CreateTime, OrderByType.Asc);
            }

            List<ExternalWorkOrder> updateList = new List<ExternalWorkOrder>();
            foreach (var order in externalOrders)
            {
                var lotStockNum = await _externalGetStockInfo.GetStockQuantity(stockInfoByConfig, order.SourceCode);
                if (lotStockNum == 0)
                {
                    continue;
                }

                if (workOrders != null && workOrders.Exists(p => p.ItemCode.ToLower() == order.ItemCode.ToLower()))
                {
                    continue;
                }

                if (lotStockNum > order.LotStockNum || order.LotStockNum == null)
                {
                    order.LotStockNum = lotStockNum;
                    order.ModifyTime = DateTime.Now;

                    updateList.Add(order);
                }
            }

            if (updateList.Count > 0)
            {
                await _externalWorkOrderService.BulkUpdate(updateList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<BominDrillRecipesDto> GetDrillRecipesData(GetDrillRecipesReq req)
        {
            var result = new BominDrillRecipesDto();
            if (string.IsNullOrWhiteSpace(req.Lot) || string.IsNullOrWhiteSpace(req.ProcessCode))
            {
                return result;
            }
            string reqStr = JsonSerializer.Serialize(req);

            var url = await _sysConfigManager.GetStringValue(MESConfigConstants.BOMIN_DRILL_RECIPES, SysConfigCategoryEnum.None, false);
            var data = _apiHelper.RequestData(url, "post", reqStr);
            if (!string.IsNullOrWhiteSpace(data))
            {
                result = JsonSerializer.Deserialize<BominDrillRecipesDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return result;
        }


        public async Task<GetDrillRecipesDto> GetDrillRecipes(GetDrillRecipesReq req)
        {
            var result = new GetDrillRecipesDto();
            result.Data.DeviceId = req?.DeviceId;
            result.Data.Lot = req?.Lot;
            if (string.IsNullOrWhiteSpace(req.DeviceId) || string.IsNullOrWhiteSpace(req.Lot))
            {
                result.Data.Content = "钻机id或lot号不能为空";
                return result;
            }
            var tasks = await _taskDomainService.QueryAsync(s => s.ItemCode.ToLower() == req.Lot.ToLower()
                                                && s.WorkStationCode.ToLower() == req.DeviceId.ToLower(),
                                                s => s.CreateTime, OrderByType.Desc);
            if (tasks.Count() <= 0)
            {
                result.Data.Content = "查不到钻机任务";
                return result;
            }
            var task = tasks.OrderByDescending(p => p.Code).FirstOrDefault();
            var workOrderCode = task?.WorkOrderCode;
            var workOrders = await _innerWorkOrderDomainService.QueryAsync(s => s.Code.ToLower() == workOrderCode.ToLower()
                                                                        , s => s.Id, OrderByType.Asc);
            if (workOrders?.Count() <= 0)
            {
                result.Data.Content = "查不到钻机任务的工单";
                return result;
            }
            var processCode = workOrders?.FirstOrDefault()?.ProcessCode;
            if (string.IsNullOrWhiteSpace(processCode))
            {
                result.Data.Content = "钻机任务的工单上工序编码为空";
                return result;
            }
            var bominData = await GetDrillRecipesData(new GetDrillRecipesReq()
            {
                Lot = req.Lot,
                ProcessCode = processCode
            });
            if (!bominData.Data.Success)
            {
                result.Data.Content = "钻带文件接口请求失败";
            }
            result.Data.DiaFilePath = bominData.Data.DRCX;
            result.Data.ProgramFilePath = bominData.Data.DRZD;
            result.Data.FtpHost = bominData.Data.FtpBasicUrl.Replace("ftp://", "");
            result.Data.FtpPort = "21";
            result.Data.FtpUsername = bominData.Data.FtpAccount;
            result.Data.FtpPassword = bominData.Data.FtpPassword;
            result.Data.Success = true;

            task.DiaFilePath = bominData.Data.DRCX;
            task.ProgramFilePath = bominData.Data.DRZD;
            task.FtpHost = bominData.Data.FtpBasicUrl.Replace("ftp://", "");
            task.FtpPort = "21";
            task.FtpUsername = bominData.Data.FtpAccount;
            task.FtpPassword = bominData.Data.FtpPassword;
            await _taskDomainService.Update(task);
            return result;
        }


    }
}