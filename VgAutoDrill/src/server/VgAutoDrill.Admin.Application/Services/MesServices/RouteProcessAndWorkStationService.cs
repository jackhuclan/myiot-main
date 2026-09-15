using AutoMapper;
using SqlSugar;
using System.Text;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 工序与工作站关系
    /// </summary>
    public class RouteProcessAndWorkStationService : BaseServiceWithoutTree<RouteProcessAndWorkStation, RouteProcessAndWorkStationDto, UpdateRouteProcessAndWorkStationReq>, IRouteProcessAndWorkStationService
    {
        private readonly IRouteProcessAndWorkStationDomainService _rdomainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarScope _sqlSugarScope;
        private readonly IWorkstationDomainService _workstationDomainService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public RouteProcessAndWorkStationService(
            IRouteProcessAndWorkStationDomainService domainService,
            IUnitOfWork unitOfWork,
            IWorkstationDomainService workstationDomainService,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _rdomainService = domainService;
            _unitOfWork = unitOfWork;
            _sqlSugarScope = unitOfWork.GetDbClient();
            _workstationDomainService = workstationDomainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RouteProcessAndWorkStationDto>>> GetList(GetRouteProcessAndWorkStationListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _rdomainService.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 根据工作站获取关联的工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RouteInfo>>> GetRoutesByWorkStation(GetRouteProcessAndWorkStationListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _rdomainService.GetRoutesByWorkStation(req);
            return Success(result);
        }

        public async Task<List<WorkstationDto>> GetDrillRouteCodes()
        {
            return await _rdomainService.GetDrillRouteCodes();
        }

        /// <summary>
        /// 根据工艺路线、工序获取工作站（按空闲时间排序）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<FitWorkStationDto>>> GetFitWorkStationListByRoute(GetFitWorkStationListByRouteReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            if (string.IsNullOrEmpty(req.RouteCode) || string.IsNullOrEmpty(req.ProcessCode))
            {
                return Fail<PageDto<FitWorkStationDto>>("RouteCode 、ProcessCode 未填写！");
            }

            var result = await _rdomainService.GetFitWorkStationListByRoute(req);
            return Success(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<RouteProcessAndWorkStationDto>> MultiQueryByID(long id)
        {
            var result = await _rdomainService.MultiQueryByID(id);
            if (result == null)
            {
                return Fail<RouteProcessAndWorkStationDto>("信息不存在!");
            }
            return Success(result);
        }

        /// <summary>
        /// 判断工作站是否绑定其他工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> ExsitWorkStation(AddRouteProcessAndWorkStationReq req)
        {
            if (req == null)
            {
                return Fail("信息错误!");
            }

            if (req.WorkStationIds == null || req.WorkStationIds.Count == 0)
            {
                return Fail("未填写工作站!");
            }
            var idList = req.WorkStationIds.Select(p => p.WorkStationId).ToList();
            if (idList == null || idList.Count == 0)
            {
                return Fail("未填写工作站!");
            }

            if (req.RouteAndProcessId == null || req.RouteAndProcessId == 0)
            {
                return Fail("未填写工艺组成!");
            }

            var query = _sqlSugarScope.Queryable<Route, RouteAndProcess, RouteProcessAndWorkStation, WorkStation>
                   ((r, rp, rpw, w) => new object[]
                   {
                      JoinType.Left, r.Id ==rp.RouteId,
                      JoinType.Left, rp.Id ==rpw.RouteAndProcessId,
                      JoinType.Left, rpw.WorkStationId ==w.Id,
                   }).Where((r, rp, rpw, w) => r.IsDeleted == 0 && w.IsDeleted == 0 && rpw.RouteAndProcessId != req.RouteAndProcessId);

            query = query.Where((r, rp, rpw, w) => idList.Contains(w.Id));
            var data = await query.Select((r, rp, rpw, w) => new RouteAndWorkStationInfo
            {
                RouteId = r.Id,
                RouteCode = r.Code,
                RouteName = r.Name,
                WorkStationId = w.Id,
                WorkStationCode = w.Code,
                WorkStationName = w.Name,
            }).ToListAsync();

            if (data == null || data.Count == 0)
            {
                return Success();
            }

            var wList = data.Select(p => p.WorkStationCode).Distinct().ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var item in wList)
            {
                sb.Append(item + " 已绑定工艺路线 ");
                var routeData = data.Where(p => p.WorkStationCode == item).Select(p => p.RouteCode).Distinct().ToList();
                foreach (var route in routeData)
                {
                    sb.Append(route + " ");
                }
                sb.Append('；');
            }

            return Fail(sb.ToString());
        }

        /// <summary>
        /// 添加
        /// 工艺路线下的工序关联多个工作站
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddRouteProcessAndWorkStationReq req)
        {
            if (req == null)
            {
                return Fail("信息错误!");
            }

            if (req.WorkStationIds == null || req.WorkStationIds.Count == 0)
            {
                return Fail("未填写工作站!");
            }

            if (req.RouteAndProcessId == null || req.RouteAndProcessId == 0)
            {
                return Fail("未填写工艺组成!");
            }

            //删除原有关联关系
            await _domainService.DeleteAsync(p => p.RouteAndProcessId == req.RouteAndProcessId);

            List<RouteProcessAndWorkStation> addList = new List<RouteProcessAndWorkStation>();
            foreach (var item in req.WorkStationIds)
            {
                var isExist = await _domainService.IsExistAsync(p => p.RouteAndProcessId == req.RouteAndProcessId && p.WorkStationId == item.WorkStationId);
                if (isExist)
                {
                    continue;
                }

                var addExsit = addList.Exists(p => p.RouteAndProcessId == req.RouteAndProcessId && p.WorkStationId == item.WorkStationId);
                if (addExsit)
                {
                    continue;
                }

                RouteProcessAndWorkStation model = new RouteProcessAndWorkStation();
                model.RouteAndProcessId = req.RouteAndProcessId;
                model.OrderNum = item.OrderNum;
                model.WorkStationId = item.WorkStationId;
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.Status = (int)DataStatusEnum.Enable;

                addList.Add(model);
            }

            await _domainService.BulkInsert(addList);

            return Success();
        }

        public async Task<ResponseDto<string>> ExsitWorkStationByRoute(AddRPAndWByWorkStationReq req)
        {
            if (req == null)
            {
                return Fail("信息错误!");
            }
            if (req.WorkStationId == null || req.WorkStationId == 0)
            {
                return Fail("未填写工作站ID !");
            }
            if (req.RouteInfos == null || req.RouteInfos.Count == 0)
            {
                return Fail("未填写关联的工艺路线 !");
            }

            var workStation = await _workstationDomainService.QueryByID(req.WorkStationId);
            if (workStation == null)
            {
                return Fail($"未找到工作站 {req.WorkStationId} !");
            }

            if (workStation.ProcessCode.ToLower().Equals("drill") && req.RouteInfos.Count > 1)
            {
                return Fail("钻机类型的工作站，只允许匹配一条工艺路线 !");
            }

            return Success();
        }

        /// <summary>
        /// 添加
        /// 工作站关联多个工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddDataByWorkStation(AddRPAndWByWorkStationReq req)
        {
            if (req == null)
            {
                return Fail("信息错误!");
            }
            if (req.WorkStationId == null || req.WorkStationId == 0)
            {
                return Fail("未填写工作站ID !");
            }
            if (req.RouteInfos == null || req.RouteInfos.Count == 0)
            {
                return Fail("未填写关联的工艺路线 !");
            }

            var RouteAndProcessIds = req.RouteInfos.Select(p => p.RouteAndProcessId).ToList();
            var isExsit = await VerifyOnlyRoute((long)req.WorkStationId, RouteAndProcessIds);
            if (!string.IsNullOrEmpty(isExsit))
            {
                return Fail(isExsit);
            }

            List<RouteProcessAndWorkStation> addList = new List<RouteProcessAndWorkStation>();
            for (int i = 0; i < req.RouteInfos.Count; i++)
            {
                var addExsit = addList.Exists(p => p.RouteAndProcessId == req.RouteInfos[i].RouteAndProcessId && p.WorkStationId == req.WorkStationId);
                if (addExsit)
                {
                    continue;
                }

                RouteProcessAndWorkStation model = new RouteProcessAndWorkStation();
                model.RouteAndProcessId = req.RouteInfos[i].RouteAndProcessId;
                model.WorkStationId = req.WorkStationId;
                model.OrderNum = 0;
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.Status = (int)DataStatusEnum.Enable;

                addList.Add(model);
            }

            _unitOfWork.BeginTran();

            var deleteResult = await _domainService.DeleteAsync(p => p.WorkStationId == req.WorkStationId);
            if (!deleteResult)
            {
                return Fail<string>("删除原有关联失败！");
            }

            var response = await _domainService.BulkInsert(addList);
            if (!response)
            {
                return Fail<string>("工作站添加关联工艺路线失败！");
            }

            _unitOfWork.CommitTran();

            return Success();
        }

        private async Task<string> VerifyOnlyRoute(long workStationId, List<long?> RouteAndProcessIds)
        {
            var isExist = await _domainService.QueryAsync(p => p.WorkStationId == workStationId
                        && !RouteAndProcessIds.Contains(p.RouteAndProcessId),
                        p => p.Id, OrderByType.Asc);
            if (isExist != null && isExist.Count > 0)
            {
                var rpIds = isExist.Select(p => p.RouteAndProcessId).ToList();
                var query = _sqlSugarScope.Queryable<Route, RouteAndProcess, RouteProcessAndWorkStation, WorkStation>
                   ((r, rp, rpw, w) => new object[]
                   {
                      JoinType.Left, r.Id ==rp.RouteId,
                      JoinType.Left, rp.Id ==rpw.RouteAndProcessId,
                      JoinType.Left, rpw.WorkStationId ==w.Id,
                   }).Where((r, rp, rpw) => r.IsDeleted == 0 && rpIds.Contains(rp.Id) && r.VettingStatus == 1);

                var data = await query.Select((r, rp, rpw, w) => new RouteAndWorkStationInfo
                {
                    RouteId = r.Id,
                    RouteCode = r.Code,
                    RouteName = r.Name,
                    WorkStationId = w.Id,
                    WorkStationName = w.Name,
                    WorkStationCode = w.Code,
                }).ToListAsync();

                if (data == null || data.Count == 0)
                {
                    return string.Empty;
                }

                StringBuilder sb = new StringBuilder();
                var exsitsOtherW = data.GroupBy(p => p.RouteCode).ToList();
                foreach (var item in exsitsOtherW)
                {
                    if (item.ToList().Count == 1)
                    {
                        sb.Append($"{item.Key},");
                    }
                }

                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    return $"该工作站是已审批工艺路线：{sb.ToString()} 下唯一的，不允许删除 !";
                }

            }
            return string.Empty;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(UpdateRouteProcessAndWorkStationReq req)
        {
            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            var isExist = await _domainService.IsExistAsync(p => p.RouteAndProcessId == req.RouteAndProcessId
            && p.WorkStationId == req.WorkStationId && p.Id != req.Id);
            if (isExist)
            {
                return Fail<string>("关联关系已存在!");
            }

            var model = _mapper.Map<RouteProcessAndWorkStation>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
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

            var isExist = await _domainService.QueryAsync(p => p.RouteAndProcessId == entity.RouteAndProcessId, p => p.CreateTime, OrderByType.Desc);
            if (isExist != null && isExist.Count == 1)
            {
                var routeList = await _rdomainService.GetRoutesByWorkStation(new GetRouteProcessAndWorkStationListReq
                {
                    WorkStationId = entity.WorkStationId,
                    RouteAndProcessId = entity.RouteAndProcessId,
                    PageNum = 1,
                    PageSize = 10,
                });

                if (routeList != null && routeList.List != null && routeList.List.Count > 0)
                {
                    var routeModel = routeList.List[0];
                    if (routeModel != null && routeModel.VettingStatus == 1)
                    {
                        return Fail("该工作站是已审批工艺路线： " + routeModel.Code + " 下唯一关联的，不允许删除 !");
                    }
                }
            }

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
                object[] deleteList = new object[idList.Count];
                for (int i = 0; i < idList.Count; i++)
                {
                    var entity = await _domainService.QueryByID(idList[i]);
                    if (entity == null)
                    {
                        continue;
                    }

                    var isExist = await _domainService.QueryAsync(p => p.RouteAndProcessId == entity.RouteAndProcessId, p => p.CreateTime, OrderByType.Desc);
                    if (isExist != null && isExist.Count == 1)
                    {
                        var routeList = await _rdomainService.GetRoutesByWorkStation(new GetRouteProcessAndWorkStationListReq
                        {
                            WorkStationId = entity.WorkStationId,
                            RouteAndProcessId = entity.RouteAndProcessId,
                            PageNum = 1,
                            PageSize = 10,
                        });

                        if (routeList != null && routeList.List != null && routeList.List.Count > 0)
                        {
                            var routeModel = routeList.List[0];
                            if (routeModel != null && routeModel.VettingStatus == 1)
                            {
                                return Fail("该工作站是已审批工艺路线： " + routeModel.Code + " 下唯一关联的，不允许删除 !");
                            }
                        }
                    }

                    deleteList[i] = idList[i];
                }

                var result = await _domainService.DeleteByIds(deleteList);
                if (result)
                {
                    return Success("");
                }
            }
            return Fail("删除失败");

        }

    }
}
