
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Application.Services.MesServices.External;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices.External;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAlterLog;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Application.Services;

/// <summary>
/// 
/// </summary>
public class WorkOrderService : BaseServiceWithTree<WorkOrder, WorkOrderTreeDto, WorkOrderDto, AddOrUpdateWorkOrderReq>, IWorkOrderService
{
    private readonly IWorkOrderDomainService _workOrderService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IWorkOrderAndWorkStationDomainService _workOrderAndWorkStationService;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IItemDomainService _itemDomainService;
    private readonly ICutterGroupService _cutterGroupService;
    private readonly ITaskService _taskServiceProvider;
    private readonly SqlSugarScope _sqlSugarScope;
    private readonly IAPIHelper _apiHelper;
    private readonly IExternalWorkOrderDomainService _externalWorkOrderDomainService;
    private readonly ITaskDomainService _taskDomainService;
    private readonly IWorkOrderAlterLogService _workOrderAlterLogService;
    private readonly ILogger<WorkOrderService> _logger;
    private readonly IWorkOrderAlterLogDomainService _workOrderAlterLogDomainService;
    private readonly IExternalGetStockInfo _externalGetStockInfo;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="domainService"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="mapper"></param>
    public WorkOrderService(IWorkOrderDomainService domainService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IWorkOrderAndWorkStationDomainService workOrderAndWorkStationService,
        ISysConfigManager sysConfigManager,
        IMapper mapper,
        ITaskService taskServiceProvider,
        IItemDomainService itemDomainService,
        IExternalWorkOrderDomainService externalWorkOrderDomainService,
        ITaskDomainService taskDomainService,
        IAPIHelper apiHelper,
        ICutterGroupService cutterGroupService,
        ILogger<WorkOrderService> logger,
        IWorkOrderAlterLogDomainService workOrderAlterLogDomainService,
        IWorkOrderAlterLogService workOrderAlterLogService,
        IExternalGetStockInfo externalGetStockInfo)
        : base(domainService, mapper)
    {
        _workOrderService = domainService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _workOrderAndWorkStationService = workOrderAndWorkStationService;
        _sysConfigManager = sysConfigManager;
        _itemDomainService = itemDomainService;
        _apiHelper = apiHelper;
        _cutterGroupService = cutterGroupService;
        _sqlSugarScope = unitOfWork.GetDbClient();
        _taskServiceProvider = taskServiceProvider;
        _externalWorkOrderDomainService = externalWorkOrderDomainService;
        _taskDomainService = taskDomainService;
        _workOrderAlterLogService = workOrderAlterLogService;
        _logger = logger;
        _workOrderAlterLogDomainService = workOrderAlterLogDomainService;
        _externalGetStockInfo = externalGetStockInfo;
    }

    protected ISqlSugarClient DBClient => _sqlSugarScope;
    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<PageDto<WorkOrderDto>>> GetList(GetWorkOrderListReq req)
    {
        if (req.PageNum < 1) req.PageNum = 1;
        if (req.PageSize < 1) req.PageSize = 10;
        var pageDto = new PageDto<WorkOrderDto>(req.PageNum, req.PageSize);

        var where = PredicateBuilder.True<WorkOrder>();
        where = where.And(p => p.IsDeleted == 0);

        if (!string.IsNullOrEmpty(req.Code))
        {
            where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
        }
        if (!string.IsNullOrEmpty(req.Name))
        {
            where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
        }

        if (!string.IsNullOrEmpty(req.ClientName))
        {
            where = where.And(p => !string.IsNullOrEmpty(p.ClientName) && p.ClientName.Contains(req.ClientName));
        }
        if (!string.IsNullOrEmpty(req.ClientCode))
        {
            where = where.And(p => !string.IsNullOrEmpty(p.ClientCode) && p.ClientCode.Contains(req.ClientCode));
        }

        if (!string.IsNullOrEmpty(req.OrderSource))
        {
            where = where.And(p => !string.IsNullOrEmpty(p.OrderSource) && p.OrderSource.Contains(req.OrderSource));
        }

        if (!string.IsNullOrEmpty(req.SourceCode))
        {
            where = where.And(p => !string.IsNullOrEmpty(p.SourceCode) && p.SourceCode.Contains(req.SourceCode));
        }

        if (!string.IsNullOrEmpty(req.ItemCode))
        {
            where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
        }
        if (!string.IsNullOrEmpty(req.ItemName))
        {
            where = where.And(p => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
        }

        if (!string.IsNullOrEmpty(req.BatchCode))
        {
            where = where.And(p => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
        }

        if (req.ManuOrderStatusList != null && req.ManuOrderStatusList.Count > 0)
        {
            where = where.And(p => p.ManuOrderStatus != null && req.ManuOrderStatusList.Contains(p.ManuOrderStatus.Value));
        }

        if (req.RequestDate != null)
        {
            where = where.And(p => p.RequestDate != null && p.RequestDate.Value.Date.Equals(req.RequestDate.Value.Date));
        }

        if (req.IsAddWorkOrder != null)
        {
            where = where.And(p => p.IsAddWorkOrder != null && p.IsAddWorkOrder == req.IsAddWorkOrder);
        }

        if (req.Status > -1)
        {
            where = where.And(p => p.Status == req.Status);
        }

        if (req.PanelCount > 0)
        {
            where = where.And(p => p.PanelCount == req.PanelCount);
        }
        if (req.DrillCount > 0)
        {
            where = where.And(p => p.DrillCount == req.DrillCount);
        }
        if (req.WadCount > 0)
        {
            where = where.And(p => p.WadCount == req.WadCount);
        }

        if (req.IsUrgent != null)
        {
            where = where.And(p => p.IsUrgent == req.IsUrgent);
        }

        if (req.LayerNumList != null && req.LayerNumList.Any())
        {
            where = where.And(p => req.LayerNumList.Contains((int)p.LayerNum));
        }

        if (req.IsExternal != null)
        {
            where = where.And(p => p.IsExternal == req.IsExternal);
        }

        if (req.QueryOrderBy == null)
        {
            var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<WorkOrder>, List<WorkOrderDto>>(result.ToList());
        }
        else
        {
            switch (req.QueryOrderBy)
            {
                case QueryOrderByEnum.OrderByCodeDesc:
                    var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);
                    pageDto.Total = result.TotalCount;
                    pageDto.List = _mapper.Map<List<WorkOrder>, List<WorkOrderDto>>(result.ToList());
                    break;

                case QueryOrderByEnum.OrderByCodeASC:
                    var result1 = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
                    pageDto.Total = result1.TotalCount;
                    pageDto.List = _mapper.Map<List<WorkOrder>, List<WorkOrderDto>>(result1.ToList());
                    break;

                case QueryOrderByEnum.OrderByCreateTimeDesc:
                    var result2 = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);
                    pageDto.Total = result2.TotalCount;
                    pageDto.List = _mapper.Map<List<WorkOrder>, List<WorkOrderDto>>(result2.ToList());
                    break;

                case QueryOrderByEnum.OrderByCreateTimeASC:
                    var result3 = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
                    pageDto.Total = result3.TotalCount;
                    pageDto.List = _mapper.Map<List<WorkOrder>, List<WorkOrderDto>>(result3.ToList());
                    break;

                default:
                    var result0 = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);
                    pageDto.Total = result0.TotalCount;
                    pageDto.List = _mapper.Map<List<WorkOrder>, List<WorkOrderDto>>(result0.ToList());
                    break;
            }
        }

        return Success<PageDto<WorkOrderDto>>(pageDto);
    }

    /// <summary>
    /// 根据ItemTypeId获取数据列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<PageDto<WorkOrderDto>>> GetEquipmentList(GetWorkOrderListReq req)
    {
        if (req.PageNum < 1) req.PageNum = 1;
        if (req.PageSize < 1) req.PageSize = 10;

        var result = await _workOrderService.GetList(req);
        return Success(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseDto<List<WorkOrderTreeDto>>> GetTreeList()
    {
        var list = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1, q => q.Id, SqlSugar.OrderByType.Asc);
        var result = new ResponseDto<List<WorkOrderTreeDto>>();
        if (list == null || !list.Any())
        {
            return result;
        }

        var allCodes = AddChildN(list, 0);

        result.Data = allCodes;

        return result;
    }

    /// <summary>
    /// 根据ID获取信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseDto<WorkOrderDto>> QueryDataByID(long id)
    {
        var entity = await _domainService.QueryByID(id);
        if (entity == null)
        {
            return Fail<WorkOrderDto>("信息不存在!");
        }
        if (entity.IsDeleted == 1)
        {
            return Fail<WorkOrderDto>("信息错误!");
        }
        var model = _mapper.Map<WorkOrderDto>(entity);
        model.DrillTaskShaftCount = await _sysConfigManager.GetIntValue(MESConfigConstants.DRILL_TASK_SHAFT_COUNT);
        model.RemarkColor = entity.RemarkColor;
        return Success(model);
    }

    /// <summary>
    /// 添加信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> AddData(AddOrUpdateWorkOrderReq req)
    {
        if (req == null)
        {
            return Fail("信息格式错误!");
        }

        if (await _domainService.IsExistAsync(x => x.Code.ToLower() == req.Code.ToLower()))
        {
            return Fail($"生产工单号，{req.Code}，已存在此工单号，不能重复!");
        }

        if (await _externalWorkOrderDomainService.IsExistAsync(p => p.Code.ToLower() == req.Code.ToLower() || p.SourceCode.ToLower() == req.Code.ToLower()))
        {
            return Fail($"外部工单列表中，已存在此生产工单号，{req.Code}!");
        }

        if (req.RequestDate < DateTime.Now.Date)
        {
            return Fail($"需求日期，{req.RequestDate}，不能早于今天!");
        }

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_TRANSFER_DRILL_FILE_ENABLE_WORKORDER)
            && (string.IsNullOrEmpty(req.BeforeDrillFilePath) || string.IsNullOrEmpty(req.SpecGroup)))
        {
            return Fail("已开启获取钻带参数配置项，工艺分组和转换前路径不可为空 !");
        }

        if (!string.IsNullOrEmpty(req.BeforeDrillFilePath))
        {
            req.BeforeDrillFilePath = Regex.Replace(req.BeforeDrillFilePath.Trim(), @"[\r\n]", "");
            var verifyResult = await VerifyBeforePath(req.BeforeDrillFilePath);
            if (verifyResult)
            {
                return Fail("转换前路径填写错误! 已为转换后路径格式，请确认!");
            }
        }
        if (!string.IsNullOrEmpty(req.SpecGroup))
        {
            req.SpecGroup = req.SpecGroup.Trim();
        }
        if (!string.IsNullOrEmpty(req.AfterDrillFilePath))
        {
            req.AfterDrillFilePath = req.AfterDrillFilePath.Trim();
        }
        var model = _mapper.Map<WorkOrder>(req);
        model.CreateTime = DateTime.Now;
        model.CreatorId = UserId;
        model.Status = (int)DataStatusEnum.Enable;
        if (!string.IsNullOrEmpty(req.BeforeDrillFilePath))
        {
            model.IsRebrush = 1;
            model.AfterDrillFilePath = "";
        }

        await _domainService.Add(model);
        return Success();
    }

    /// <summary>
    /// 修改信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> UpdateData(AddOrUpdateWorkOrderReq req)
    {
        if (req == null)
        {
            return Fail("信息格式错误!");
        }

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_TRANSFER_DRILL_FILE_ENABLE_WORKORDER)
            && (string.IsNullOrEmpty(req.BeforeDrillFilePath) || string.IsNullOrEmpty(req.SpecGroup)))
        {
            return Fail("已开启获取钻带参数配置项，工艺分组和转换前路径不可为空 !");
        }

        if (!string.IsNullOrEmpty(req.BeforeDrillFilePath))
        {
            req.BeforeDrillFilePath = Regex.Replace(req.BeforeDrillFilePath.Trim(), @"[\r\n]", "");
            var verifyResult = await VerifyBeforePath(req.BeforeDrillFilePath);
            if (verifyResult)
            {
                return Fail("转换前路径填写错误! 已为转换后路径格式，请确认!");
            }
        }

        var entity = await _domainService.QueryByID(req.Id);
        if (entity == null)
        {
            return Fail("信息不存在!");
        }
        if (!string.IsNullOrEmpty(req.SpecGroup))
        {
            req.SpecGroup = req.SpecGroup.Trim();
        }
        if (!string.IsNullOrEmpty(req.AfterDrillFilePath))
        {
            req.AfterDrillFilePath = req.AfterDrillFilePath.Trim();
        }

        var model = _mapper.Map<WorkOrder>(req);
        model.CreateTime = entity.CreateTime;
        model.CreatorId = entity.CreatorId;
        model.ModifierId = UserId;
        model.ModifyTime = DateTime.Now;
        model.RemarkColor = req.RemarkColor;
        if (model.BeforeDrillFilePath != entity.BeforeDrillFilePath)
        {
            if (!string.IsNullOrEmpty(model.BeforeDrillFilePath))
            {
                model.IsRebrush = 1;
            }
            model.AfterDrillFilePath = "";
        }

        await _domainService.Update(model);

        return Success();
    }

    public async Task<ResponseDto<string>> VerifyWIPItemNum(AddOrUpdateWorkOrderReq req)
    {
        if (req == null
            || string.IsNullOrEmpty(req.ItemCode)
            || !await _sysConfigManager.GetBoolValue(MESConfigConstants.JINGWANG_VERIFY_ITEM_NUM_WORKORDER)
            || !await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_KINWONG_STOCK_INFO_QUERY))
        {
            return Success();
        }

        var latestLotNum = await _externalGetStockInfo.GetStockQuantityByLot(req.ItemCode);

        var where = PredicateBuilder.True<WorkOrder>();
        where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.ToLower() == req.ItemCode.ToLower());
        var allOrders = await _workOrderService.QueryAsync(where, p => p.Id, OrderByType.Asc);
        var todoOrderStatus = new List<ManuOrderStatusEnum?>
        {
            ManuOrderStatusEnum.DRAFT,
            ManuOrderStatusEnum.COMMITED,
            ManuOrderStatusEnum.SCHEDULED,
            //ManuOrderStatusEnum.BEGIN,
        };
        var sumTodoQuantity = allOrders.Where(x => todoOrderStatus.Contains(x.ManuOrderStatus)).Sum(x => x.Quantity);
        if (sumTodoQuantity == 0)
        //var datas = await _workOrderService.QueryAsync(where, p => p.Code, OrderByType.Desc);
        //if (datas == null || datas.Count == 0)
        {
            if (req.Quantity > latestLotNum)
            {
                return Fail($"{req.ItemCode} 料号 的生产数量 {req.Quantity} 超过了 {req.ItemCode} 料号的库存数量 {latestLotNum}，是否继续创建？");
            }
        }
        else
        {
            //var dataItemNum = datas.Select(p => p.Quantity).Sum();
            if ((sumTodoQuantity + req.Quantity) > latestLotNum)
            {
                return Fail($"{req.ItemCode} 料号 的生产数量 {req.Quantity} + 已创建工单的生产数量 {sumTodoQuantity} ，超过了 {req.ItemCode} 料号的库存数量 {latestLotNum}，是否继续创建？");
            }
        }

        var externalDatas = await _externalWorkOrderDomainService.QueryAsync(p => !string.IsNullOrEmpty(p.ItemCode)
                && p.ItemCode.ToLower() == req.ItemCode.ToLower() && p.LotStockNum > 0, p => p.CreateTime, OrderByType.Desc);

        var savedExternalLotStockNum = externalDatas.Select(p => p.LotStockNum).Sum();

        if (latestLotNum < savedExternalLotStockNum && savedExternalLotStockNum > 0)
        {
            var sumAllQuantity = allOrders.Sum(x => x.Quantity);
            if (sumAllQuantity == 0)
            {
                if (req.Quantity > savedExternalLotStockNum)
                {
                    return Fail($"{req.ItemCode} 料号 的生产数量 {req.Quantity} 超过了外部工单 {req.ItemCode} 料号的库存数量 {savedExternalLotStockNum}，是否继续创建？");
                }
            }
            else
            {
                if ((sumAllQuantity + req.Quantity) > savedExternalLotStockNum)
                {
                    return Fail($"{req.ItemCode} 料号 的生产数量 {req.Quantity} + 已创建工单的生产数量 {sumTodoQuantity} ，超过了外部工单 {req.ItemCode} 料号的库存数量 {savedExternalLotStockNum}，是否继续创建？");
                }
            }
        }

        return Success();
    }

    public async Task<bool> VerifyBeforePath(string beforeDrillFilePath)
    {
        if (string.IsNullOrEmpty(beforeDrillFilePath))
        {
            return false;
        }

        string str = string.Empty;
        if (beforeDrillFilePath.Length > 20)
        {
            str = beforeDrillFilePath.Substring(beforeDrillFilePath.Length - 20, 20).ToLower();
        }
        else
        {
            str = beforeDrillFilePath.ToLower();
        }

        if (str.Contains("_convduo"))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 提交
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> Commit(CommitWorkOrderReq req)
    {
        var entity = await _domainService.QueryByID(req.Id);
        if (entity == null)
        {
            return Fail("信息不存在!");
        }
        if (entity.ManuOrderStatus.HasValue && entity.ManuOrderStatus != ManuOrderStatusEnum.DRAFT)
        {
            return Fail($"生产工单已经提交过，当前状态是{entity.ManuOrderStatus.ToString()}!");
        }

        if (!await _workOrderAndWorkStationService.IsExistAsync(p => p.WorkOrderCode == entity.Code))
        {
            return Fail("工作站未绑定机台，不允许审批!");
        }

        if (string.IsNullOrEmpty(entity.ItemCode))
        {
            return Fail("未选择物料");
        }

        if (!await (_itemDomainService.IsExistAsync(p => p.Code == entity.ItemCode)))
        {
            return Fail("物料编码不存在");
        }

        entity.ManuOrderStatus = ManuOrderStatusEnum.COMMITED;
        entity.ModifierId = UserId;
        entity.ModifyTime = DateTime.Now;
        entity.IsUrgent = req.IsUrgent;
        await _domainService.Update(entity);
        return Success();
    }
    public async Task<ResponseDto<string>> UnCommit(CommitWorkOrderReq req)
    {
        if (!req.Id.HasValue)
        {
            return Fail("req.Id is null!");
        }
        var entity = await _domainService.QueryByID(req.Id);
        if (entity == null)
        {
            return Fail("信息不存在!");
        }
        if (entity.ManuOrderStatus.HasValue
            && entity.ManuOrderStatus != ManuOrderStatusEnum.COMMITED
            && entity.ManuOrderStatus != ManuOrderStatusEnum.SCHEDULED)
        {
            return Fail($"撤销提交状态失败，当前状态是{entity.ManuOrderStatus.ToString()}!");
        }

        entity.ManuOrderStatus = ManuOrderStatusEnum.DRAFT;
        entity.ModifierId = UserId;
        entity.ModifyTime = DateTime.Now;
        await _domainService.Update(entity);
        return Success();
    }
    /// <summary>
    /// 批量添加
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> BulkInsert(List<WorkOrderToExcelDto> list)
    {
        if (list == null || list.Count == 0)
        {
            return Fail("未识别有效的数据！");
        }

        bool importStatus = await _sysConfigManager.GetBoolValue(MESConfigConstants.IMPORT_STATUS);

        int failCount = 0;
        StringBuilder sb = new StringBuilder();
        List<WorkOrder> workOrders = new List<WorkOrder>();
        foreach (var item in list)
        {
            if (string.IsNullOrEmpty(item.Code) || string.IsNullOrEmpty(item.Name))
            {
                sb.Append("编码：" + item.Code + " 或名称：" + item.Name + " 为空；");
                sb.Append("\r\n");
                failCount++;
                continue;
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == item.Code);
            if (isExsitCode)
            {
                sb.Append("编码" + item.Code + " 数据库已存在；");
                sb.Append("\r\n");
                failCount++;
                continue;
            }

            if (workOrders.Exists(p => p.Code == item.Code))
            {
                sb.Append("编码" + item.Code + " 导入列表中已存在；");
                sb.Append("\r\n");
                failCount++;
                continue;
            }

            if (item.QuantityChanged % item.PanelCount != 0)
            {
                sb.Append("编码" + item.Code + " 调整数量必须是叠板层数的整数倍；");
                sb.Append("\r\n");
                failCount++;
                continue;
            }

            WorkOrder model = new WorkOrder();
            model.Code = item.Code;
            model.Name = item.Name;
            model.ParentId = item.ParentId;
            model.BatchCode = item.BatchCode;
            model.ClientCode = item.ClientCode;
            model.ClientId = item.ClientId;
            model.ClientName = item.ClientName;
            model.ItemCode = item.ItemCode;
            model.ItemId = item.ItemId;
            model.ItemName = item.ItemName;
            model.ItemTypeId = item.ItemTypeId;
            model.OrderSource = item.OrderSource;
            model.Quantity = item.Quantity;
            model.QuantityProduced = item.QuantityProduced;
            model.QuantityScheduled = item.QuantityScheduled;
            model.QuantityChanged = item.QuantityChanged;
            model.RequestDate = item.RequestDate;
            model.SourceCode = item.SourceCode;
            model.Specification = item.Specification;
            model.UnitOfMeasure = item.UnitOfMeasure;
            model.PanelCount = item.PanelCount;
            model.WadCount = item.WadCount;
            model.DrillCount = item.DrillCount;
            model.RouteId = item.RouteId;
            model.RouteName = item.RouteName;
            model.RouteCode = item.RouteCode;
            model.DispenseMachines = item.DispenseMachines;
            if (importStatus)
            {
                model.Status = 1;
            }
            else
            {
                model.Status = 0;
            }
            model.CreatorId = UserId;
            model.CreateTime = DateTime.Now;
            model.IsAddWorkOrder = 0;
            workOrders.Add(model);
        }
        var result = await _domainService.BulkInsert(workOrders);
        if (!result)
        {
            return Fail("导入失败！");
        }

        string str = string.Format("预计导入：{0} 条；成功导入：{1} 条；失败：{2} 条；\r\n", list.Count, list.Count - failCount, failCount);
        str = str + sb.ToString();

        return Success(str);
    }

    /// <summary>
    /// 删除
    /// （已提交审批后，不允许删除）
    /// 删除后同步删除相关联的表数据
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

        if (entity.IsExternal == 1)
        {
            return Fail("外部导入的工单，不允许删除!");
        }

        if (entity.ManuOrderStatus.HasValue && entity.ManuOrderStatus != ManuOrderStatusEnum.DRAFT)
        {
            return Fail("已提交审批后，不允许删除!");
        }

        var result = await _domainService.DeleteById(id);
        if (result)
        {
            //同步删除关联表
            var db = _unitOfWork.GetDbClient();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"DELETE  from t_check_records where work_order_id=@workOrderID;");
            sb.Append(@"DELETE  from t_drill_work_order where workorder_id=@workOrderID;");
            sb.Append(@"DELETE  from t_feedback where workorder_id=@workOrderID;");
            sb.Append(@"DELETE  from t_material_stock where workorder_id=@workOrderID;");
            sb.Append(@"DELETE  from t_task where work_order_id=@workOrderID;");
            sb.Append(@"DELETE  from t_trans_order where work_order_id=@workOrderID;");
            sb.Append(@"DELETE  from t_workorder_and_workstation where work_order_code=@workOrderCode;");
            var paramList = new List<SugarParameter>
            {
                new SugarParameter("@workOrderID",id,System.Data.DbType.Int64),
                new SugarParameter("@workOrderCode",entity.Code,System.Data.DbType.String),
            };
            await db.Ado.ExecuteCommandAsync(sb.ToString(), paramList);
            return Success("");
        }
        return Fail("删除失败");
    }

    /// <summary>
    /// 删除集合
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> DeleteDataList(List<long> idList)
    {
        if (idList != null)
        {
            List<WorkOrder> sqlDelete = new List<WorkOrder>();
            object[] deleteList = new object[idList.Count];
            for (int i = 0; i < idList.Count; i++)
            {
                var entity = await _domainService.QueryByID(idList[i]);
                if (entity == null)
                {
                    continue;
                }

                if (entity.IsExternal == 1)
                {
                    return Fail(idList[i] + " 外部导入的工单，不允许删除!");
                }

                if (entity.ManuOrderStatus.HasValue && entity.ManuOrderStatus != ManuOrderStatusEnum.DRAFT)
                {
                    return Fail(idList[i] + " 已提交审批后，不允许删除!");
                }
                deleteList[i] = idList[i];
                sqlDelete.Add(entity);
            }

            var result = await _domainService.DeleteByIds(deleteList);
            if (result)
            {
                //同步删除关联表
                if (sqlDelete.Count > 0)
                {
                    for (int i = 0; i < sqlDelete.Count; i++)
                    {
                        var db = _unitOfWork.GetDbClient();
                        StringBuilder sb = new StringBuilder();
                        sb.Append(@"DELETE  from t_check_records where work_order_id=@workOrderID;");
                        sb.Append(@"DELETE  from t_drill_work_order where workorder_id=@workOrderID;");
                        sb.Append(@"DELETE  from t_feedback where workorder_id=@workOrderID;");
                        sb.Append(@"DELETE  from t_material_stock where workorder_id=@workOrderID;");
                        sb.Append(@"DELETE  from t_task where work_order_id=@workOrderID;");
                        sb.Append(@"DELETE  from t_trans_order where work_order_id=@workOrderID;");
                        sb.Append(@"DELETE  from t_workorder_and_workstation where work_order_code=@workOrderCode;");
                        var paramList = new List<SugarParameter>
                        {
                            new SugarParameter("@workOrderID",sqlDelete[i].Id,System.Data.DbType.Int64),
                            new SugarParameter("@workOrderCode",sqlDelete[i].Code,System.Data.DbType.String),
                        };
                        await db.Ado.ExecuteCommandAsync(sb.ToString(), paramList);
                    }

                }
                return Success("");
            }
        }
        return Fail("删除失败");
    }

    /// <summary>
    /// 更新工艺路线
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> UpdateRoute(UpdateRouteReq req)
    {
        var entity = await _domainService.QueryByID(req.Id);
        if (entity == null)
        {
            return Fail("信息不存在!");
        }

        if (entity.ManuOrderStatus.HasValue &&
            entity.ManuOrderStatus != ManuOrderStatusEnum.DRAFT && entity.ManuOrderStatus != ManuOrderStatusEnum.COMMITED)
        {
            return Fail("只有草稿/审批状态的生产工单，才可以变更工艺路线!");
        }

        entity.RouteId = req.RouteId;
        entity.RouteCode = req.RouteCode;
        entity.RouteName = req.RouteName;
        entity.ModifierId = UserId;
        entity.ModifyTime = DateTime.Now;

        await _domainService.Update(entity);
        return Success();
    }

    /// <summary>
    /// 根据工单编码，清空工单绑定的工作站
    /// </summary>
    /// <param name="workOrderCode"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> ClearWorkStation(string workOrderCode)
    {
        if (string.IsNullOrEmpty(workOrderCode))
        {
            return Fail("未识别有效的WorkOrderCode");
        }

        var entity = await _domainService.FindSingleAsync(p => p.Code == workOrderCode);
        if (entity != null && entity.ManuOrderStatus.HasValue &&
            entity.ManuOrderStatus != ManuOrderStatusEnum.DRAFT && entity.ManuOrderStatus != ManuOrderStatusEnum.COMMITED)
        {
            return Fail("只有草稿/审批状态的生产工单，才可以变更工艺路线!");
        }

        if (!await _workOrderAndWorkStationService.IsExistAsync(p => p.WorkOrderCode == workOrderCode))
        {
            return Fail("未找到" + workOrderCode + "绑定的工作站");
        }

        var result = await _workOrderAndWorkStationService.DeleteAsync(p => p.WorkOrderCode == workOrderCode);
        if (result)
        {
            return Success();
        }
        else
        {
            return Fail("清空失败！");
        }
    }

    public async Task<ResponseDto<DrillWorkOrderDto>> GetMOTaskList(GetMOTaskReq req)
    {
        if (req == null)
        {
            return Fail<DrillWorkOrderDto>("未识别有效的入参！");
        }
        try
        {
            if (req.RouteCodeList.Any())
            {
                List<string> routeCodes = req.RouteCodeList.Select(p => p.ToLower()).ToList();

                req.RouteCodeList = routeCodes;
            }

            var db = _unitOfWork.GetDbClient();
            var where = PredicateBuilder.True<WorkOrder>();
            where = where.And(p => p.IsDeleted == 0);
            DrillWorkOrderDto drillTaskDto = new DrillWorkOrderDto();
            var result = await _domainService.QueryAsync(where, q => q.RequestDate, SqlSugar.OrderByType.Asc);
            if (result == null)
            {
                return Success<DrillWorkOrderDto>(drillTaskDto);
            }
            var list = new List<Dictionary<String, Object>>();
            var workStationList = await GetWorkStationLists(req);
            for (var i = req.TaskNumber - 1; i >= 0; i--)
            {
                var dates = req.StartDate.Value.Date.AddDays((double)i).ToString("yyyy-MM-dd");
                var arr = new Dictionary<String, Object>();
                arr = await GetMOInfor(req, workStationList, db, false, dates, i.Value, false);
                list.Add(arr);
            }

            if (req.IsHistory == true)
            {
                var arrhistory = new Dictionary<String, Object>();
                arrhistory = await GetMOInfor(req, workStationList, db, true, null, 0, req.IsFinish.ToBool());
                list.Add(arrhistory);
            }

            if (req.IsFinish == true)
            {
                var arrFinish = new Dictionary<String, Object>();
                arrFinish = await GetMOInfor(req, workStationList, db, false, null, 0, true);
                list.Add(arrFinish);
            }

            drillTaskDto.WorkStationList = workStationList;
            list.Reverse();
            drillTaskDto.List = list.Distinct();
            return Success<DrillWorkOrderDto>(drillTaskDto);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            return Fail<DrillWorkOrderDto>(msg);
        }
    }

    private async Task<Dictionary<string, object>> GetMOInfor(GetMOTaskReq req, List<WorkStationLoadTaskDto> workStationList, SqlSugarScope db, bool IsHistory, string dates, int qty, bool IsFinish)
    {
        var arr = new Dictionary<String, Object>();

        if (IsHistory)
        {
            arr.Add("date", "History");
        }
        else
        {
            arr.Add("date", dates);
        }

        var children = new List<Object>();
        foreach (var station in workStationList)
        {
            var childrenObj = new Dictionary<String, Object>();
            var detailArr = new List<Object>();
            if (station.Code != null)
            {
                childrenObj.Add("stationCode", station.Code);
            }
            var res = db.Queryable<Model.Entites.Mes.WorkTask, WorkOrder>
              ((t, wo) => new object[]
              {
                             JoinType.Inner, t.WorkOrderCode == wo.Code,
               });
            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                res = res.Where(t => t.WorkOrderCode.ToLower().Contains(req.WorkOrderCode.ToLower()));
            }
            if (string.IsNullOrEmpty(req.ProcessCode))
            {
                req.ProcessCode = _configuration["AppConfig:ProcessCode"];
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                res = res.Where(t => t.ItemCode.ToLower().Contains(req.ItemCode.ToLower()));
            }
            if (req.RouteCodeList.Any())
            {
                res = res.Where(t => req.RouteCodeList.Contains(t.RouteCode.ToLower()));
            }

            if (IsHistory)
            {
                res = res.Where((t, wo) => t.WorkStationCode == station.Code && wo.Code == t.WorkOrderCode);
            }
            else
            {
                res = res.Where((t, wo) => t.WorkStationCode == station.Code && wo.Code == t.WorkOrderCode
                                && t.StartTime.Value.Date.ToString("yyyy-MM-dd") == dates);
            }

            if (!IsFinish)
            {
                res = res.Where((t, wo) => t.WorkStationCode == station.Code && wo.Code == t.WorkOrderCode && wo.ManuOrderStatus != ManuOrderStatusEnum.FINISH);
            }

            var temp = res.ToList();
            if (temp == null || !temp.Any())
            {
                var tmp = new List<Object>();
                childrenObj.Add("detail", tmp);
                children.Add(childrenObj);
                continue;
            }

            res = res.Distinct();
            var dataByRoute = await res.Select((t, wo) => new GetMOTaskReq
            {
                WorkOrderId = t.WorkOrderId,
                WorkOrderCode = t.WorkOrderCode,
                WorkOrderName = wo.Name,
                ItemCode = t.ItemCode,
                ItemTypeId = t.ItemTypeId,
                ProcessCode = t.ProcessCode,
                RouteCode = t.RouteCode,
                RouteName = t.RouteName,
                IsUrgent = t.IsUrgent,
                RemarkColor = wo.RemarkColor,
                manuOrderStatus = (wo.ManuOrderStatus == null ? ManuOrderStatusEnum.DRAFT : wo.ManuOrderStatus),
            }).ToListAsync();

            dataByRoute.OrderBy(t => t.IsUrgent).OrderBy(t => t.StartDate);
            dataByRoute.DistinctBy(t => t.WorkOrderCode).OrderBy(t => t.StartDate).Min(t => t.EndDate);
            var mo = dataByRoute.Select(t => t.WorkOrderCode).Distinct().ToList();
            if (mo.Count > 0)
            {
                foreach (var item in mo)
                {
                    var end = temp.OrderBy(t => t.EndTime).Where(t => t.WorkOrderCode == item);
                    var endData = end.Select(t => t.EndTime).Max();
                    var start = temp.OrderBy((t) => t.StartTime).Where(t => t.WorkOrderCode == item); ;
                    var startData = start.Select(t => t.StartTime).Min();
                    if (endData.Value.Date.ToString("yyyy-MM-dd") == req.StartDate.Value.Date.AddDays((double)(qty + 1)).ToString("yyyy-MM-dd"))
                    {
                        endData = DateTime.Parse(endData.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59");
                    }

                    var count = temp.Where(t => t.WorkOrderCode == item).DistinctBy(t => t.Code).Count();
                    var commitTask = temp.Where(t => t.WorkOrderCode == item && t.TaskStatus == TaskStatusEnum.COMMITED).DistinctBy(t => t.Code).Count();
                    var draftTask = temp.Where(t => t.WorkOrderCode == item && t.TaskStatus == TaskStatusEnum.DRAFT).DistinctBy(t => t.Code).Count();
                    var sendingTask = temp.Where(t => t.WorkOrderCode == item && t.TaskStatus == TaskStatusEnum.SENDING).DistinctBy(t => t.Code).Count();
                    var bufferedTask = temp.Where(t => t.WorkOrderCode == item && t.TaskStatus == TaskStatusEnum.BUFFERED).DistinctBy(t => t.Code).Count();
                    var beginTask = temp.Where(t => t.WorkOrderCode == item && t.TaskStatus == TaskStatusEnum.BEGIN).DistinctBy(t => t.Code).Count();
                    var finishTask = temp.Where(t => t.WorkOrderCode == item && t.TaskStatus == TaskStatusEnum.FINISH).DistinctBy(t => t.Code).Count();

                    foreach (var t in dataByRoute)
                    {
                        if (item == t.WorkOrderCode)
                        {
                            t.EndDate = endData;
                            t.StartDate = startData;
                            t.TaskDraft = draftTask;
                            t.TaskCommit = commitTask;
                            t.TaskCount = count;
                            t.TaskSending = sendingTask;
                            t.TaskBuffered = bufferedTask;
                            t.TaskBegin = beginTask;
                            t.TaskFinish = finishTask;
                            break;
                        }
                    }
                }
            }
            dataByRoute = dataByRoute.OrderBy(t => t.StartDate).ToList();
            childrenObj.Add("detail", dataByRoute);
            children.Add(childrenObj);
        }
        children.Distinct();
        arr.Add("des", children);
        return arr;
    }

    public async Task<List<WorkStationLoadTaskDto>> GetWorkStationLists(GetMOTaskReq req)
    {
        var db = _unitOfWork.GetDbClient();

        var queryByRoute = db.Queryable<Device, DeviceType, WorkStation, RouteProcessAndWorkStation, RouteAndProcess, Route, Process>
           ((d, dt, w, rpw, rp, r, p) => new object[]
               {
                    JoinType.Left, d.DeviceTypeId == dt.Id,
                    JoinType.Left, d.WorkStationId == w.Id,
                    JoinType.Left, w.Id == rpw.WorkStationId,
                    JoinType.Left, rpw.RouteAndProcessId == rp.Id,
                    JoinType.Left, rp.RouteId == r.Id,
                    JoinType.Inner,rp.ProcessId == p.Id
               });

        queryByRoute = queryByRoute.Where((d, dt, w, rpw, rp, r, p) => dt.IsDeleted == 0 && d.IsDeleted == 0 && w.IsDeleted == 0);
        queryByRoute = queryByRoute.Where((d, dt, w, rpw, rp, r, p) => r.IsDeleted == 0 && rpw.IsDeleted == 0 && rp.IsDeleted == 0
        && p.IsDeleted == 0 && p.Code == _configuration["AppConfig:ProcessCode"]);

        if (req.RouteCodeList.Any())
        {
            queryByRoute = queryByRoute.Where((d, dt, w, rpw, rp, r, p) => !string.IsNullOrEmpty(r.Code) && req.RouteCodeList.Contains(r.Code.ToLower()));
        }

        queryByRoute = queryByRoute.OrderBy((d, dt, w, rpw, rp, r, p) => w.Code);

        var dataByRoute = await queryByRoute.Select((d, dt, w, rpw, rp, r, p) => new WorkStationLoadTaskDto
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

        if (dataByRoute != null && dataByRoute.Count() > 0)
        {
            dataByRoute.DistinctBy(p => p.Code);
        }

        var workStationList = dataByRoute;
        List<WorkStationLoadTaskDto> lst = new List<WorkStationLoadTaskDto>();
        List<string> workStationCodes = new List<string>();
        foreach (var item in workStationList)
        {
            if (string.IsNullOrEmpty(item.Code))
            {
                continue;
            }
            if (workStationCodes.Count() == 0)
            {
                workStationCodes.Add(item.Code.ToLower());
            }
            else
            {
                if (!workStationCodes[workStationCodes.Count() - 1].Contains(item.Code.ToLower()))
                {
                    workStationCodes.Add(item.Code.ToLower());
                }
                else
                {
                    continue;
                }
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
            lst.Add(item);
        }
        return lst;
    }
    private async Task<List<WorkStation>> GetWorkStationList(GetMOTaskReq req)
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

            queryByRoute = queryByRoute.Where((r, rp, p, rpw, w) => !string.IsNullOrEmpty(r.Code) && routeCodes.Contains(r.Code));

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
                result = dataByRoute.DistinctBy(p => p.Id).ToList();
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

    public async Task<ResponseDto<string>> RobackTask(AddOrUpdateTaskReq req)
    {
        var rest = await UpdateTaskData(req, TaskStatusEnum.COMMITED, TaskStatusEnum.DRAFT);
        if (rest.Code.ToString() != "Success")
        {
            return Fail("撤回工单任务失败");
        }
        return Success();
    }

    public async Task<ResponseDto<string>> commitTask(AddOrUpdateTaskReq req)
    {
        var rest = await UpdateTaskData(req, TaskStatusEnum.DRAFT, TaskStatusEnum.COMMITED);
        if (rest.Code.ToString() != "Success")
        {
            return Fail("提交工单任务失败");
        }
        return Success();
    }

    public async Task<ResponseDto<string>> UpdateTaskData(AddOrUpdateTaskReq req, TaskStatusEnum oldStatus, TaskStatusEnum newStatus)
    {
        if (req == null)
        {
            return Fail("信息不存在!");
        }

        var taskDatas = await _taskDomainService.QueryAsync(t => t.IsDeleted == 0 && t.WorkOrderCode.ToLower() == req.WorkOrderCode.ToLower()
        && t.TaskStatus == oldStatus, t => t.Code, OrderByType.Asc);

        if (taskDatas == null || taskDatas.Count == 0)
        {
            return Fail("修改的信息状态不正确，不能修改!");
        }

        var updateResult = await _taskDomainService.UpdateAsync(t => new WorkTask()
        {
            ModifierId = UserId,
            ModifyTime = DateTime.Now,
            TaskStatus = newStatus
        }, t => t.IsDeleted == 0 && t.WorkOrderCode.ToLower() == req.WorkOrderCode.ToLower() && t.TaskStatus == oldStatus);
        if (newStatus == TaskStatusEnum.DRAFT && oldStatus == TaskStatusEnum.COMMITED)//由COMMITED到DRAFT，需要删除对应的配刀计划
        {
            var cutterGroupNos = taskDatas.Where(s => !string.IsNullOrWhiteSpace(s.CutterGroupNo)).Select(s => s.CutterGroupNo).ToList();
            if (cutterGroupNos?.Count > 0)
            {
                await _cutterGroupService.DeleteItemAndDetails(cutterGroupNos!);
            }
        }
        if (!updateResult)
        {
            return Fail("数据库更新任务状态失败，请稍后重试!");
        }

        //任务提交，取消钻机的调度记录
        if (newStatus == TaskStatusEnum.COMMITED)
        {
            var workStationCodes = taskDatas.Where(p => !string.IsNullOrEmpty(p.WorkStationCode)).Select(p => p.WorkStationCode.ToLower()).ToList();

            if (workStationCodes != null && workStationCodes.Count > 0)
            {
                var where = PredicateBuilder.True<Schedule>();
                where = where.And(p => !string.IsNullOrEmpty(p.SourceDeviceId) && workStationCodes.Contains(p.SourceDeviceId.ToLower()));

                where = where.And(p => p.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                    || p.ScheduledTaskStatus == ScheduledTaskStatus.Created);

                await _taskServiceProvider.CanceledSchedule(where);
            }
        }
        //任务取消，取消任务的调度记录
        else if (newStatus == TaskStatusEnum.DRAFT)
        {
            var taskCodes = taskDatas.Where(p => !string.IsNullOrEmpty(p.Code)).Select(p => p.Code.ToLower()).ToList();

            if (taskCodes != null && taskCodes.Count > 0)
            {
                var where = PredicateBuilder.True<Schedule>();
                where = where.And(p => taskCodes.Contains(p.TaskId));

                where = where.And(p => p.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                    || p.ScheduledTaskStatus == ScheduledTaskStatus.Created);

                await _taskServiceProvider.CanceledSchedule(where);
            }
        }

        return Success();
    }

    public async Task<ResponseDto<string>> MoColorRemark(AddOrUpdateWorkOrderReq req)
    {
        if (req == null)
        {
            return Fail("信息不存在");
        }
        if (req.Id < 1)
        {
            return Fail($"ID{req.Id}不存在");
        }
        var result = _domainService.QueryByID(req.Id);
        if (result == null)
        {
            return Fail("信息不存在!");
        }
        if (result.Result.IsDeleted == 1)
        {
            return Fail("此工单状态错误，不能修改!");
        }
        var model = _mapper.Map<WorkOrder>(result.Result);
        model.CreateTime = result.Result.CreateTime;
        model.CreatorId = result.Result.CreatorId;
        model.ModifierId = UserId;
        model.ModifyTime = DateTime.Now;
        model.RemarkColor = req.RemarkColor;
        await _domainService.Update(model);
        return Success();
    }

    public async Task<ResponseDto<string>> SetMoveInTime(string workOrderCode, DateTime moveInTime)
    {
        if (string.IsNullOrEmpty(workOrderCode))
        {
            return Fail("未识别有效的WorkOrderCode！");
        }

        var data = await _domainService.FindSingleAsync(p => p.Code.ToLower() == workOrderCode.ToLower());
        if (data == null)
        {
            return Fail("未找到对应工单！");
        }

        if (data.MoveInTime.HasValue)
        {
            return Fail("已设置过MoveInTime！");
        }

        data.MoveInTime = moveInTime;
        data.ModifierId = UserId;
        data.ModifyTime = DateTime.Now;
        await _domainService.Update(data);

        if (!string.IsNullOrEmpty(data.SourceCode))
        {
            await _externalWorkOrderDomainService.UpdateAsync(p => new ExternalWorkOrder
            { MoveInTime = data.MoveInTime, ModifierId = UserId, ModifyTime = DateTime.Now },
            p => p.SourceCode.ToLower() == data.SourceCode.ToLower());
        }

        return Success();
    }

    public async Task<ResponseDto<string>> SetMoveOutTime(string workOrderCode, DateTime moveOutTime)
    {
        if (string.IsNullOrEmpty(workOrderCode))
        {
            return Fail("未识别有效的WorkOrderCode！");
        }

        var data = await _domainService.FindSingleAsync(p => p.Code.ToLower() == workOrderCode.ToLower());
        if (data == null)
        {
            return Fail("未找到对应工单！");
        }

        if (!data.TrackOutTime.HasValue)
        {
            return Fail("不能Move out, 必须先执行Track out！");
        }

        if (data.MoveOutTime.HasValue)
        {
            return Fail("已设置过MoveOutTime！");
        }

        data.MoveOutTime = moveOutTime;
        data.ModifierId = UserId;
        data.ModifyTime = DateTime.Now;
        await _domainService.Update(data);

        if (!string.IsNullOrEmpty(data.SourceCode))
        {
            await _externalWorkOrderDomainService.UpdateAsync(p => new ExternalWorkOrder
            { MoveOutTime = data.MoveOutTime, ModifierId = UserId, ModifyTime = DateTime.Now },
            p => p.SourceCode.ToLower() == data.SourceCode.ToLower());
        }

        return Success();
    }

    public async Task<ResponseDto<string>> SetTrackInTime(string workOrderCode, DateTime trackInTime)
    {
        if (string.IsNullOrEmpty(workOrderCode))
        {
            return Fail("未识别有效的WorkOrderCode！");
        }

        var data = await _domainService.FindSingleAsync(p => p.Code.ToLower() == workOrderCode.ToLower());
        if (data == null)
        {
            return Fail("未找到对应工单！");
        }
        if (!data.MoveInTime.HasValue)
        {
            return Fail("不能Track In, 必须先执行Move In！");
        }
        if (data.TrackInTime.HasValue)
        {
            return Fail("已设置过TrackInTime！");
        }

        data.TrackInTime = trackInTime;
        data.ModifierId = UserId;
        data.ModifyTime = DateTime.Now;
        await _domainService.Update(data);

        if (!string.IsNullOrEmpty(data.SourceCode))
        {
            await _externalWorkOrderDomainService.UpdateAsync(p => new ExternalWorkOrder
            { TrackInTime = data.TrackInTime, ModifierId = UserId, ModifyTime = DateTime.Now },
            p => p.SourceCode.ToLower() == data.SourceCode.ToLower());
        }

        return Success();
    }

    public async Task<ResponseDto<string>> SetTrackOutTime(string workOrderCode, DateTime trackOutTime)
    {
        if (string.IsNullOrEmpty(workOrderCode))
        {
            return Fail("未识别有效的WorkOrderCode！");
        }

        var data = await _domainService.FindSingleAsync(p => p.Code.ToLower() == workOrderCode.ToLower());
        if (data == null)
        {
            return Fail("未找到对应工单！");
        }
        if (!data.TrackInTime.HasValue)
        {
            return Fail("不能Track Out, 必须先执行Track In！");
        }
        if (data.TrackOutTime.HasValue)
        {
            return Fail("已设置过TrackOutTime！");
        }

        data.TrackOutTime = trackOutTime;
        data.ModifierId = UserId;
        data.ModifyTime = DateTime.Now;
        await _domainService.Update(data);

        if (!string.IsNullOrEmpty(data.SourceCode))
        {
            await _externalWorkOrderDomainService.UpdateAsync(p => new ExternalWorkOrder
            { TrackOutTime = data.TrackOutTime, ModifierId = UserId, ModifyTime = DateTime.Now },
            p => p.SourceCode.ToLower() == data.SourceCode.ToLower());
        }

        return Success();
    }

    public async Task<bool> Exsist(string code)
    {
        return await _domainService.IsExistAsync(x => x.Code.ToLower() == code.ToLower());
    }

    public async Task<WorkOrder> FindSingle(string code)
    {
        return await _domainService.FindSingleAsync(x => x.Code.ToLower() == code.ToLower());
    }

    public async Task<bool> ExsistAfterDrillFilePath(string code, string deviceCode, string beforeDrillFilePath)
    {
        if (string.IsNullOrEmpty(beforeDrillFilePath) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(deviceCode))
        {
            return false;
        }

        try
        {
            if (!await _workOrderService.IsExistAsync(p => p.Code.ToLower() == code.ToLower() && p.BeforeDrillFilePath == beforeDrillFilePath))
            {
                _logger.LogInformation("ExsistAfterDrillFilePath can not identify workOrder data!");
                return false;
            }
            var wData = await _workOrderService.FindSingleAsync(p => p.Code.ToLower() == code.ToLower() && p.BeforeDrillFilePath == beforeDrillFilePath);
            if (wData == null)
            {
                _logger.LogInformation("ExsistAfterDrillFilePath can not identify workOrder data!");
                return false;
            }

            var transResult = await GetTransferDrillFilePath(beforeDrillFilePath);
            if (transResult == null || transResult.Code != ResponseCode.Success)
            {
                await RefreshAfterDrillPath(code, deviceCode, beforeDrillFilePath, wData);
                return false;
            }

            string afterFilePath = transResult.Data.verifyPath;
            if (string.IsNullOrEmpty(afterFilePath))
            {
                await RefreshAfterDrillPath(code, deviceCode, beforeDrillFilePath, wData);
                return false;
            }

            if (afterFilePath != wData.AfterDrillFilePath)
            {
                string oldStr = wData.AfterDrillFilePath;

                wData.AfterDrillFilePath = afterFilePath;
                wData.ModifierId = UserId;
                wData.ModifyTime = DateTime.Now;
                await _workOrderService.Update(wData);

                await _workOrderAlterLogService.Add(new AddOrUpdateWorkOrderAlterLogReq
                {
                    WorkOrderCode = wData.Code,
                    DeviceCode = deviceCode,
                    ItemCode = wData.ItemCode,
                    ActionTime = DateTime.Now,
                    ActionDetail = $"Refresh WorkOrder {wData.Code} old AfterDrillFilePath {oldStr}, new AfterDrillFilePath {wData.AfterDrillFilePath}. ",
                    BeforeDrillFilePath = beforeDrillFilePath,
                    AfterDrillFilePath = oldStr
                });

                await _taskDomainService.UpdateAsync(p => new WorkTask
                {
                    AfterDrillFilePath = afterFilePath,
                    ModifierId = UserId,
                    ModifyTime = DateTime.Now
                }, p => p.WorkStationCode.ToLower() == deviceCode.ToLower() && p.WorkOrderCode.ToLower() == code.ToLower());
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"ExsistAfterDrillFilePath catch exception : {e.Message}!");
            return false;
        }

        return true;
    }

    private async Task RefreshAfterDrillPath(string code, string deviceCode, string beforeDrillFilePath, WorkOrder wData)
    {
        string oldStr = wData.AfterDrillFilePath;

        wData.AfterDrillFilePath = "";
        wData.IsRebrush = 1;
        wData.ModifierId = UserId;
        wData.ModifyTime = DateTime.Now;
        await _workOrderService.Update(wData);

        await _workOrderAlterLogService.Add(new AddOrUpdateWorkOrderAlterLogReq
        {
            WorkOrderCode = wData.Code,
            DeviceCode = deviceCode,
            ItemCode = wData.ItemCode,
            ActionTime = DateTime.Now,
            ActionDetail = $"Clear WorkOrder {wData.Code} old AfterDrillFilePath {oldStr} . ",
            BeforeDrillFilePath = beforeDrillFilePath,
            AfterDrillFilePath = oldStr
        });

        await _taskDomainService.UpdateAsync(p => new WorkTask
        {
            AfterDrillFilePath = "",
            IsRebrush = 1,
            ModifierId = UserId,
            ModifyTime = DateTime.Now
        }, p => p.Code.ToLower() == deviceCode.ToLower() && p.WorkOrderCode.ToLower() == code.ToLower() && p.TaskStatus != TaskStatusEnum.FINISH);
    }

    public async Task<ResponseDto<JWConversionPathResponse>> GetTransferDrillFilePath(string beforeDrillFilePath)
    {
        if (string.IsNullOrEmpty(beforeDrillFilePath))
        {
            return Fail<JWConversionPathResponse>("调用钻带转换，无法识别beforeDrillFilePath;");
        }

        string urlMOGetAfterDrillPath = await _sysConfigManager.GetStringValue(MESConfigConstants.JINGWANG_TRANSFER_DRILL_FILE_URL);
        if (string.IsNullOrEmpty(urlMOGetAfterDrillPath))
        {
            _logger.LogError("GetAfterDrillFilePath can not identify JingWangTransferDrillFileUrl!");
            return Fail<JWConversionPathResponse>("调用钻带转换，无法识别请求地址 JingWangTransferDrillFileUrl;");
        }

        try
        {
            string urlAddress = urlMOGetAfterDrillPath + beforeDrillFilePath;
            var result = _apiHelper.RequestData(urlAddress, "Get");

            if (string.IsNullOrEmpty(result))
            {
                _logger.LogError("GetAfterDrillFilePath api result null!");
                return Fail<JWConversionPathResponse>("调用钻带转换，返回null;");
            }
            _logger.LogInformation($"GetAfterDrillFilePath request: {urlAddress}, result: {result} .");

            var response = JsonSerializer.Deserialize<JWConversionPathResponse>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (response == null)
            {
                return Fail<JWConversionPathResponse>("调用钻带转换，返回值转换失败;");
            }
            if (string.IsNullOrEmpty(response.state) || !response.state.ToLower().Equals("verifyok")
                || !response.success || string.IsNullOrEmpty(response.verifyPath))
            {
                string errorLog = response == null ? "" : $"state:{response.state},success:{response.success},verifyPath:{response.verifyPath}";
                _logger.LogError($"GetAfterDrillFilePath api response null or {errorLog}!");
                return Fail<JWConversionPathResponse>($"调用钻带转换，{response.state} {response.stateInfo};");
            }
            return Success(response);

        }
        catch (Exception ex)
        {
            _logger.LogError($"GetAfterDrillFilePath catch exception : {ex.Message}!");
            return Fail<JWConversionPathResponse>("调用钻带转换，异常中断;");
        }
    }
}
