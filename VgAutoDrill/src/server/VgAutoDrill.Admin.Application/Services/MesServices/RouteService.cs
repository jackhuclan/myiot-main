using AutoMapper;
using SqlSugar;
using System.Text;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 工艺路线
    /// </summary>
    public class RouteService : BaseServiceWithoutTree<Route, RouteDto, AddOrUpdateRouteReq>, IRouteService
    {
        private readonly IRouteDomainService _routeService;
        private readonly IWorkOrderDomainService _workOrderDomainService;
        private readonly IRouteAndProcessDomainService _routeAndProcessDomainService;
        private readonly IRouteAndProductCategoryDomainService _routeAndProductCategoryDomainService;
        private readonly IRouteProcessAndWorkStationDomainService _routeAndProcessWorkStationDomainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductCategoryDomainService _productCategoryDomainService;
        private readonly IProcessDomainService _processDomainService;
        private readonly IRouteAndProductCategoryRepository _routeAndProductCategoryRepository;
        private readonly IRouteAndProcessRepository _routeAndProcessRepository;
        private readonly IRouteProcessAndWorkStationRepository _routeProcessAndWorkStationRepository;
        private readonly ISysConfigManager _sysConfigManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="workOrderDomainService"></param>
        /// <param name="routeAndProcessDomainService"></param>
        /// <param name="routeAndProductCategoryDomainService"></param>
        /// <param name="routeAndProcessWorkStationDomainService"></param>
        /// <param name="productCategoryDomainService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public RouteService(IRouteDomainService domainService,
            IWorkOrderDomainService workOrderDomainService,
            IRouteAndProcessDomainService routeAndProcessDomainService,
            IRouteAndProductCategoryDomainService routeAndProductCategoryDomainService,
            IRouteProcessAndWorkStationDomainService routeAndProcessWorkStationDomainService,
            IProductCategoryDomainService productCategoryDomainService,
            IProcessDomainService processDomainService,
            ISysConfigManager sysConfigManager,
            IRouteAndProductCategoryRepository routeAndProductCategoryRepository,
            IRouteAndProcessRepository routeAndProcessRepository,
            IRouteProcessAndWorkStationRepository routeProcessAndWorkStationRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _routeService = domainService;
            _workOrderDomainService = workOrderDomainService;
            _routeAndProcessDomainService = routeAndProcessDomainService;
            _routeAndProductCategoryDomainService = routeAndProductCategoryDomainService;
            _routeAndProcessWorkStationDomainService = routeAndProcessWorkStationDomainService;
            _productCategoryDomainService = productCategoryDomainService;
            _processDomainService = processDomainService;
            _unitOfWork = unitOfWork;
            _sysConfigManager = sysConfigManager;
            _routeAndProductCategoryRepository = routeAndProductCategoryRepository;
            _routeAndProcessRepository = routeAndProcessRepository;
            _routeProcessAndWorkStationRepository = routeProcessAndWorkStationRepository;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RouteDto>>> GetList(GetRouteListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = int.MaxValue;
            var pageDto = new PageDto<RouteDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<Route>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (req.VettingStatus != null)
            {
                where = where.And(p => p.VettingStatus == req.VettingStatus);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<Route>, List<RouteDto>>(result.ToList());

            for (int i = 0; i < pageDto.List.Count(); i++)
            {
                var list1 = new List<object>();
                var id = pageDto.List[i].Id;
                var routeAndProductCategoryListReq = new GetRouteAndProductCategoryListReq
                {
                    RouteId = id,
                    PageNum = 0,
                    PageSize = int.MaxValue,
                };
                var routeStyle = await _routeAndProductCategoryRepository.GetList(routeAndProductCategoryListReq);
                var list = routeStyle.ToList();
                //无论有没有匹配 产品大类 ， 返回结构中，都要包含这个结构
                var x = new
                {
                    Name = "产品大类",
                    List = list,
                };
                list1.Add(x);

                var getRouteAndProcess = new GetRouteAndProcessListReq
                {
                    RouteId = id
                };
                var processSettings = await _routeAndProcessRepository.GetList(getRouteAndProcess);
                foreach (var processSetting in processSettings)
                {
                    var routeProcessAndWorkStationListReq = new GetRouteProcessAndWorkStationListReq
                    {
                        RouteAndProcessId = processSetting.Id,
                        PageNum = 0,
                        PageSize = int.MaxValue,
                    };
                    var stationQuery = await _routeProcessAndWorkStationRepository.GetList(routeProcessAndWorkStationListReq);
                    var stationList = stationQuery.ToList();
                    var proce = new
                    {
                        Name = processSetting.ProcessName,
                        List = stationList,
                    };
                    list1.Add(proce);
                }

                pageDto.List[i].Children = list1.ToList();
            }
            return Success<PageDto<RouteDto>>(pageDto);
        }
        public async Task<List<PartitionConfig>> GetRouteAndPartitionSetting()
        {
            var partitionSettingConfigString = await _sysConfigManager.GetStringValue("PartitionSetting");
            var partitionSettings = new List<PartitionConfig>();
            if (!string.IsNullOrWhiteSpace(partitionSettingConfigString))
            {
                var settings = partitionSettingConfigString.Split(";").ToList();//分号分隔
                foreach (var item in settings)
                {
                    var list = item.Split(":").ToList();//冒号分割
                    if (list?.Count > 1)
                    {
                        var routes = list.LastOrDefault().Split(",");
                        foreach (var route in routes)
                        {
                            partitionSettings.Add(new PartitionConfig()
                            {
                                RouteCode = route,
                                PartitionCode = list.FirstOrDefault()
                            });
                        }
                    }
                }
            }
            return partitionSettings;
        }


        public async Task<ResponseDto<List<DropSelectDto>>> GetDropSelectDatas(GetRouteListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = int.MaxValue;
            var pageDto = new PageDto<RouteDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<Route>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (req.VettingStatus != null)
            {
                where = where.And(p => p.VettingStatus == req.VettingStatus);
            }

            var datas = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            if (datas == null || datas.Count == 0)
            {
                return Success(new List<DropSelectDto>());
            }
            var partitionSettings = await GetRouteAndPartitionSetting();
            var result = new List<DropSelectDto>();
            foreach (var data in datas)
            {
                var partitionSetting = partitionSettings.FirstOrDefault(s => s.RouteCode.ToLower() == data.Code.ToLower());
                result.Add(new DropSelectDto
                {
                    Id = data.Id,
                    Code = data.Code,
                    Name = data.Name,
                    PartitionCode= partitionSetting?.PartitionCode,
                    Label = $"{partitionSetting?.PartitionCode}({data.Code})",//分区编码（路线编码）
                });
            }

            return Success(result);
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<RouteDto>> GetListDetail(GetRouteListReq req)
        {
            RouteDto routeDto = new RouteDto();


            return Success<RouteDto>(routeDto);
        }
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateRouteReq req)
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

            var model = _mapper.Map<Route>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);
            return Success();
        }

        /// <summary>
        /// 更新时校验:
        /// 是否有且只包含一个关键工序；
        /// 存在引用的生产工单，不允许变更编码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> VerifyUpdate(AddOrUpdateRouteReq req)
        {
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

            //var isExsit = await _routeService.ExsitTask(req.Id);
            //if (isExsit)
            //{
            //    return Fail("存在已提交的生产任务，不允许变更工艺路线!");
            //}

            var check = await _routeService.CheckKeyProcess(req.Id);
            if (!check)
            {
                return Fail("有且只能有一个关键工序!");
            }
            else
            {
                var model = _mapper.Map<Route>(req);
                model.CreateTime = entity.CreateTime;
                model.CreatorId = entity.CreatorId;
                model.VettingStatus = entity.VettingStatus;
                model.ModifierId = UserId;
                model.ModifyTime = DateTime.Now;

                bool isExsitWorkOrder = await _workOrderDomainService.IsExistAsync(p => p.RouteId == req.Id);
                if (isExsitWorkOrder)
                {
                    model.Code = entity.Code;
                }

                await _domainService.Update(model);
                return Success();
            }
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

            if (entity.VettingStatus == 1)
            {
                return Fail("已审批，不允许删除!");
            }

            bool isExsitWorkOrder = await _workOrderDomainService.IsExistAsync(p => p.RouteId == id);
            if (isExsitWorkOrder)
            {
                return Fail("存在引用的生产工单，不允许删除!");
            }

            var RAndProductList = await _routeAndProductCategoryDomainService.QueryAsync(p => p.Status == 1, p => p.Id, OrderByType.Asc);
            if (RAndProductList != null && RAndProductList.Count > 0)
            {
                var exsitP = RAndProductList.FindAll(p => p.RouteId == id).DistinctBy(t => t.ProductCategoryId).ToList();
                if (exsitP != null && exsitP.Count > 0)
                {
                    foreach (var productID in exsitP.Select(p => p.ProductCategoryId))
                    {
                        if (productID == null)
                        {
                            continue;
                        }

                        if (RAndProductList.FindAll(p => p.ProductCategoryId == productID).Count != 1)
                        {
                            continue;
                        }

                        var pData = await _productCategoryDomainService.QueryByID(productID);
                        if (pData != null && pData.VettingStatus == 1)
                        {
                            return Fail("该工艺路线是产品大类 " + pData.Code + " 关联唯一的工艺路线，且产品大类已审批，不允许删除!");
                        }
                    }
                }
            }

            var result = await _domainService.DeleteById(id);
            if (result)
            {
                //同步删除关联关系表
                var db = _unitOfWork.GetDbClient();
                StringBuilder sb = new StringBuilder();
                sb.Append(@"DELETE  from t_device_and_route where route_id=@routeId;");
                sb.Append(@"DELETE  from t_route_and_process where route_id=@routeId;");
                sb.Append(@"DELETE  from t_route_and_product_category where route_id=@routeId;");
                sb.Append(@"DELETE  from t_route_process_and_work_station where route_and_process_id not in (select id  from t_route_and_process);");
                var paramList = new List<SugarParameter>
                {
                    new SugarParameter("@routeId",id,System.Data.DbType.Int64),
                };
                await db.Ado.ExecuteCommandAsync(sb.ToString(), paramList);

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
                var RAndProductList = await _routeAndProductCategoryDomainService.QueryAsync(p => p.Status == 1, p => p.Id, OrderByType.Asc);

                object[] deleteList = new object[idList.Count];
                List<long> sqlDelete = new List<long>();
                for (int i = 0; i < idList.Count; i++)
                {
                    var entity = await _domainService.QueryByID(idList[i]);
                    if (entity == null)
                    {
                        continue;
                    }

                    if (entity.VettingStatus == 1)
                    {
                        return Fail(idList[i] + " 已审批，不允许删除!");
                    }

                    bool isExsitWorkOrder = await _workOrderDomainService.IsExistAsync(p => p.RouteId == idList[i]);
                    if (isExsitWorkOrder)
                    {
                        return Fail(idList[i] + " 存在引用的生产工单，不允许删除!");
                    }

                    if (RAndProductList != null && RAndProductList.Count > 0)
                    {
                        var exsitP = RAndProductList.FindAll(p => p.RouteId == idList[i]).DistinctBy(t => t.ProductCategoryId).ToList();
                        if (exsitP != null && exsitP.Count > 0)
                        {
                            foreach (var productID in exsitP.Select(p => p.ProductCategoryId))
                            {
                                if (productID == null)
                                {
                                    continue;
                                }

                                if (RAndProductList.FindAll(p => p.ProductCategoryId == productID).Count != 1)
                                {
                                    continue;
                                }

                                var pData = await _productCategoryDomainService.QueryByID(productID);
                                if (pData != null && pData.VettingStatus == 1)
                                {
                                    return Fail("工艺路线 " + entity.Code + " 是产品大类 " + pData.Code + " 关联唯一的工艺路线，且产品大类已审批，不允许删除!");
                                }
                            }
                        }
                    }

                    deleteList[i] = idList[i];
                    sqlDelete.Add(idList[i]);
                }

                var result = await _domainService.DeleteByIds(deleteList);
                if (result)
                {
                    if (sqlDelete.Count > 0)
                    {
                        foreach (var item in sqlDelete)
                        {
                            //同步删除关联关系表
                            var db = _unitOfWork.GetDbClient();
                            StringBuilder sb = new StringBuilder();
                            sb.Append(@"DELETE  from t_device_and_route where route_id=@routeId;");
                            sb.Append(@"DELETE  from t_route_and_process where route_id=@routeId;");
                            sb.Append(@"DELETE  from t_route_and_product_category where route_id=@routeId;");
                            sb.Append(@"DELETE  from t_route_process_and_work_station where route_and_process_id not in (select id  from t_route_and_process);");
                            var paramList = new List<SugarParameter>
                            {
                                new SugarParameter("@routeId",item, System.Data.DbType.Int64),
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
        /// 审批工艺路线
        /// 校验工艺路线是否匹配工序和产品大类，工序是否匹配工作站
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> VettingRoute(VettingRouteReq req)
        {
            if (req == null || req.VettingRoutes == null || req.VettingRoutes.Count == 0)
            {
                return Fail("信息格式错误!");
            }

            var routeList = await _domainService.QueryAsync(p => p.VettingStatus == 0, p => p.Id, OrderByType.Asc);
            if (routeList == null || routeList.Count == 0)
            {
                return Fail("未查询到未审批的工艺路线");
            }

            var vettingRoutes = routeList.FindAll(p => req.VettingRoutes.Any(c => c.RouteId == p.Id));
            if (vettingRoutes == null || vettingRoutes.Count == 0)
            {
                return Fail("未查询到未审批的工艺路线");
            }

            List<Route> updateList = new List<Route>();
            foreach (var item in vettingRoutes)
            {
                var exsitRAndPD = await _routeAndProductCategoryDomainService.IsExistAsync(p => p.RouteId == item.Id);
                if (!exsitRAndPD)
                {
                    return Fail(item.Code + " 工艺路线未匹配产品大类");
                }

                var exsitRAndPData = await _routeAndProcessDomainService.QueryAsync(p => p.RouteId == item.Id, p => p.Id, OrderByType.Asc);
                if (exsitRAndPData == null || exsitRAndPData.Count == 0)
                {
                    return Fail(item.Code + " 工艺路线未匹配工序");
                }

                foreach (var rAndPModel in exsitRAndPData)
                {
                    if (!await _processDomainService.IsExistAsync(p => p.Id == rAndPModel.ProcessId))
                    {
                        return Fail(item.Code + " 对应的工序 " + rAndPModel.ProcessId + " 不存在");
                    }
                    var exsitRPAndW = await _routeAndProcessWorkStationDomainService.IsExistAsync(p => p.RouteAndProcessId == rAndPModel.Id);
                    if (!exsitRPAndW)
                    {
                        return Fail(item.Code + " 对应的工序 " + rAndPModel.ProcessId + " 未匹配工作站");
                    }
                }

                item.VettingStatus = 1;
                item.ModifierId = UserId;
                item.ModifyTime = DateTime.Now;
                updateList.Add(item);
            }

            var result = await _domainService.BulkUpdate(updateList);
            if (!result)
            {
                return Fail("审批 工艺路线 失败！");
            }
            return Success();
        }

        /// <summary>
        /// 取消审批工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> CancelVettingRoute(VettingRouteReq req)
        {
            if (req == null || req.VettingRoutes == null || req.VettingRoutes.Count == 0)
            {
                return Fail("信息格式错误!");
            }

            var routeList = await _domainService.QueryAsync(p => p.VettingStatus == 1, p => p.Id, OrderByType.Asc);
            if (routeList == null || routeList.Count == 0)
            {
                return Fail("未查询到取消审批的工艺路线");
            }

            var cancelVettingRoutes = routeList.FindAll(p => req.VettingRoutes.Any(c => c.RouteId == p.Id));
            if (cancelVettingRoutes == null || cancelVettingRoutes.Count == 0)
            {
                return Fail("未查询到取消审批的工艺路线");
            }

            List<Route> updateList = new List<Route>();
            foreach (var route in cancelVettingRoutes)
            {
                bool isExsitWorkOrder = await _workOrderDomainService.IsExistAsync(p => p.RouteId == route.Id);
                if (isExsitWorkOrder)
                {
                    return Fail("存在引用的生产工单，不允许取消审批!");
                }

                route.VettingStatus = 0;
                route.ModifierId = UserId;
                route.ModifyTime = DateTime.Now;
                updateList.Add(route);
            }

            var result = await _domainService.BulkUpdate(updateList);
            if (!result)
            {
                return Fail("取消审批 工艺路线 失败！");
            }

            return Success();
        }

    }
}
