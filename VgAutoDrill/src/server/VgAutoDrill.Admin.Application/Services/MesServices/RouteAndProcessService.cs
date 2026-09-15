using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 工艺路线与工序关系
    /// </summary>
    public class RouteAndProcessService : BaseServiceWithoutTree<RouteAndProcess, RouteAndProcessDto, AddOrUpdateRouteAndProcessReq>, IRouteAndProcessService
    {
        private readonly IRouteAndProcessDomainService _routeAndProcessService;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public RouteAndProcessService(IRouteAndProcessDomainService domainService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _routeAndProcessService = domainService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RouteAndProcessDto>>> GetList(GetRouteAndProcessListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = int.MaxValue;

            var result = await _routeAndProcessService.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<RouteAndProcessDto>> MultiQueryByID(long Id)
        {
            var result = await _routeAndProcessService.MultiQueryByID(Id);
            if (result == null)
            {
                return Fail<RouteAndProcessDto>("信息不存在!");
            }
            return Success(result);
        }

        /// <summary>
        /// 根据工序ID获取工艺路线
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RouteInfo>>> GetRoutesByProcess(GetRoutesByProcessReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = int.MaxValue;

            if (string.IsNullOrEmpty(req.ProcessCode))
            {
                return Fail<PageDto<RouteInfo>>("未填写工序编码！");
            }

            var result = await _routeAndProcessService.GetRoutesByProcess(req);
            return Success(result);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateRouteAndProcessReq req)
        {
            var isExist = await _domainService.IsExistAsync(p => p.RouteId == req.RouteId && p.ProcessId == req.ProcessId);
            if (isExist)
            {
                return Fail<string>("已添加过该工序!");
            }

            var db = _unitOfWork.GetDbClient();
            //工序的序号不允许相同
            var queryUniqueOrderNum =
                db.Queryable<RouteAndProcess, Process>((rp, p) =>
                new object[]
                {
                    JoinType.Inner, rp.ProcessId == p.Id
                });
            queryUniqueOrderNum = queryUniqueOrderNum.Where((rp, p) => rp.RouteId == req.RouteId
                && rp.OrderNum == req.OrderNum
                 && rp.ProcessId != req.ProcessId);
            if (await queryUniqueOrderNum.AnyAsync())
            {
                return Fail<string>("工序的序号不允许相同!");
            }

            //新增的是关键工序时，才需要检查
            if (req.KeyFlag == "1")
            {
                var queryExsitKey =
                    db.Queryable<RouteAndProcess, Process>((rp, p) =>
                    new object[]
                    {
                        JoinType.Inner, rp.ProcessId == p.Id
                    });
                queryExsitKey = queryExsitKey.Where((rp, p) =>
                    rp.RouteId == req.RouteId
                    && rp.ProcessId != req.ProcessId
                    && rp.KeyFlag.Equals("1"));
                if (await queryExsitKey.AnyAsync())
                {
                    return Fail("有且只能有一个关键工序!");
                }
            }

            var model = _mapper.Map<RouteAndProcess>(req);
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
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateRouteAndProcessReq req)
        {
            if (req == null)
            {
                return Fail("未提供输入信息!");
            }
            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            var db = _unitOfWork.GetDbClient();

            //工序的序号不允许相同
            var queryUniqueOrderNum =
                db.Queryable<RouteAndProcess, Process>((rp, p) =>
                new object[]
                {
                    JoinType.Inner, rp.ProcessId == p.Id
                });
            queryUniqueOrderNum = queryUniqueOrderNum.Where((rp, p) => rp.RouteId == req.RouteId
                && rp.OrderNum == req.OrderNum
                 && rp.ProcessId != req.ProcessId);
            if (await queryUniqueOrderNum.AnyAsync())
            {
                return Fail<string>("工序的序号不允许相同!");
            }

            //新增的是关键工序时，才需要检查
            if (req.KeyFlag == "1")
            {
                var queryExsitKey =
                    db.Queryable<RouteAndProcess, Process>((rp, p) =>
                    new object[]
                    {
                        JoinType.Inner, rp.ProcessId == p.Id
                    });
                queryExsitKey = queryExsitKey.Where((rp, p) =>
                    rp.RouteId == req.RouteId
                    && rp.ProcessId != req.ProcessId
                    && rp.KeyFlag.Equals("1"));
                if (await queryExsitKey.AnyAsync())
                {
                    return Fail("有且只能有一个关键工序!");
                }
            }

            var model = _mapper.Map<RouteAndProcess>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);

            return Success();
        }
    }
}
