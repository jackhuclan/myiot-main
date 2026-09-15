using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndWorkStation;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 生产工单和工作站关联关系表
    /// </summary>
    public class WorkOrderAndWorkStationService : BaseServiceWithoutTree<WorkOrderAndWorkStation, WorkOrderAndWorkStationDto, AddOrUpdateWorkOrderAndWorkStationReq>, IWorkOrderAndWorkStationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public WorkOrderAndWorkStationService(IWorkOrderAndWorkStationDomainService domainService, IMapper mapper, IUnitOfWork unitOfWork)
            : base(domainService, mapper)
        {
            _sqlSugarScope = unitOfWork.GetDbClient();
            _unitOfWork = unitOfWork;
        }
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarScope _sqlSugarScope;

        protected ISqlSugarClient DBClient => _sqlSugarScope;

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<WorkOrderAndWorkStationDto>>> GetList(GetWorkOrderAndWorkStationListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<WorkOrderAndWorkStationDto>(req.PageNum, req.PageSize);

            if (req == null || req.WorkOrderId == 0 || req.WorkOrderId is null)
            {
                return Success(pageDto);
            }

            var where = PredicateBuilder.True<WorkOrderAndWorkStation>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.Contains(req.WorkOrderCode));
            }

            if (req.WorkOrderId > 0 || req.WorkOrderId is not null)
            {
                where = where.And(p => req.WorkOrderId != 0 && p.WorkOrderId == req.WorkOrderId);
            }

            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.Contains(req.WorkStationCode));
            }

            if (!string.IsNullOrEmpty(req.WorkStationName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkStationName) && p.WorkStationName.Contains(req.WorkStationName));
            }

            if (req.WorkStationId > 0)
            {
                where = where.And(p => p.WorkStationId == req.WorkStationId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.WorkStationCode, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<WorkOrderAndWorkStation>, List<WorkOrderAndWorkStationDto>>(result.ToList());
            for (int i = 0; i < pageDto.List.Count; i++)
            {
                pageDto.List[i].WaitTaskCount = GetWaitTask(pageDto.List[i]);
            }
            return Success(pageDto);
        }

        private int? GetWaitTask(WorkOrderAndWorkStationDto req)
        {
            var result = DBClient.Queryable<Model.Entites.Mes.WorkTask, WorkOrder, RouteAndProcess, Process, RouteProcessAndWorkStation, WorkStation, Device>
                            ((t, wo, rp, p, rpw, ws, d) => new object[]
                                {
                        JoinType.Inner,t.WorkOrderCode == wo.Code,
                        JoinType.Inner, wo.RouteId == rp.RouteId,
                        JoinType.Inner, rp.ProcessId == p.Id,
                        JoinType.Inner, rp.Id == rpw.RouteAndProcessId,
                        JoinType.Inner, rpw.WorkStationId == ws.Id,
                        JoinType.Inner, ws.Id == d.WorkStationId,
                                });
            var count = result.Where((t, wo, rp, p, rpw, ws, d) => ws.IsDeleted == 0 && wo.IsDeleted == 0 && rp.IsDeleted == 0
            && rpw.IsDeleted == 0 && p.IsDeleted == 0 && d.IsDeleted == 0 && ws.Status == 1 && t.TaskStatus == TaskStatusEnum.COMMITED
                                   && t.WorkStationId == req.WorkStationId && t.ProcessId == p.Id && t.WorkStationId == ws.Id).Count();
            return count;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateDataReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (req.workStationDatas == null || req.workStationDatas.Count == 0)
            {
                return Fail("未识别有效的生产工单号和工作站集合!");
            }

            if (string.IsNullOrEmpty(req.WorkOrderCode) && req.WorkOrderId == 0 && req.WorkOrderId is null)
            {
                return Fail("未识别有效的生产工单号!");
            }

            if (!string.IsNullOrEmpty(req.WorkOrderCode) && req.WorkOrderId is null ||
                 !string.IsNullOrEmpty(req.WorkOrderCode) && req.WorkOrderId == 0)
            {
                var db = _unitOfWork.GetDbClient();
                var wo = db.Queryable<WorkOrder>();
                wo = wo.Where(t => t.Code == req.WorkOrderCode);
                var WorkOrderId = wo.OrderByDescending(t => t.Id).Select(t => t.Id).First();
                req.WorkOrderId = WorkOrderId;
            }

            if (string.IsNullOrEmpty(req.WorkOrderCode) && req.WorkOrderId > 0)
            {
                var db = _unitOfWork.GetDbClient();
                var wo = db.Queryable<WorkOrder>();
                wo = wo.Where(t => t.Id == req.WorkOrderId);
                var WorkOrderCode = wo.OrderByDescending(t => t.Code).Select(t => t.Code).First();
                req.WorkOrderCode = WorkOrderCode;
            }

            //先删除再添加
            await _domainService.DeleteAsync(p => p.WorkOrderCode == req.WorkOrderCode);

            List<WorkOrderAndWorkStation> addList = new List<WorkOrderAndWorkStation>();
            foreach (var item in req.workStationDatas)
            {
                addList.Add(new WorkOrderAndWorkStation
                {
                    WorkOrderId = req.WorkOrderId,
                    WorkOrderCode = req.WorkOrderCode,
                    WorkStationCode = item.WorkStationCode,
                    WorkStationId = item.WorkStationId,
                    WorkStationName = item.WorkStationName,
                    CreateTime = DateTime.Now,
                    CreatorId = UserId,
                    Status = (int)DataStatusEnum.Enable
                });
            }

            await _domainService.BulkInsert(addList);
            return Success();
        }
    }
}

