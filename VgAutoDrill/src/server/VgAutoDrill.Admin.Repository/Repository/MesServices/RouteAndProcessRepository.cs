using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class RouteAndProcessRepository : BaseRepository<RouteAndProcess>, IRouteAndProcessRepository
    {
        public RouteAndProcessRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageList<RouteAndProcessDto>> GetList(GetRouteAndProcessListReq req)
        {
            var query = DBClient.Queryable<RouteAndProcess, Process>
                ((r, p) => new object[]
                    {
                        JoinType.Left, r.ProcessId == p.Id
                    });

            query = query.Where((r, p) => r.IsDeleted == 0 && p.IsDeleted == 0);

            if (req.RouteId > 0)
            {
                query = query.Where((r, p) => r.RouteId == req.RouteId);
            }

            if (req.ProcessId > 0)
            {
                query = query.Where((r, p) => r.ProcessId == req.ProcessId);
            }

            query = query.OrderBy((r, p) => r.OrderNum);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((r, p) => new RouteAndProcessDto
            {
                Id = r.Id,
                RouteId = r.RouteId,
                ProcessId = r.ProcessId,
                ProcessCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                ProcessName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                OrderNum = r.OrderNum,
                Color = r.Color,
                KeyFlag = r.KeyFlag,
                RequiredTime = r.RequiredTime,
                IsManualCheck = r.IsManualCheck,
                SelfCheckNum = r.SelfCheckNum,
                Status = r.Status
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((r, p) => new RouteAndProcessDto
                {
                    Id = r.Id,
                    RouteId = r.RouteId,
                    ProcessId = r.ProcessId,
                    ProcessCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                    ProcessName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                    OrderNum = r.OrderNum,
                    Color = r.Color,
                    KeyFlag = r.KeyFlag,
                    RequiredTime = r.RequiredTime,
                    IsManualCheck = r.IsManualCheck,
                    SelfCheckNum = r.SelfCheckNum,
                    Status = r.Status
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<RouteAndProcessDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<RouteAndProcessDto> MultiQueryByID(long Id)
        {
            var query = DBClient.Queryable<RouteAndProcess, Process>
                ((r, p) => new object[]
                    {
                        JoinType.Left, r.ProcessId == p.Id,
                    });

            query = query.Where((r, p) => r.IsDeleted == 0 && p.IsDeleted == 0);

            query.Where((r, p) => r.Id == Id);

            var data = await query.Select((r, p) => new RouteAndProcessDto
            {
                Id = r.Id,
                RouteId = r.RouteId,
                ProcessId = r.ProcessId,
                ProcessCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                ProcessName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                OrderNum = r.OrderNum,
                Color = r.Color,
                KeyFlag = r.KeyFlag,
                RequiredTime = r.RequiredTime,
                IsManualCheck = r.IsManualCheck,
                SelfCheckNum = r.SelfCheckNum,
                Status = r.Status
            }).ToListAsync();

            if (data != null && data.Count > 0)
            {
                return data[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<PageList<RouteInfo>> GetRoutesByProcess(GetRoutesByProcessReq req)
        {
            var query = DBClient.Queryable<RouteAndProcess, Route, Process>
               ((rp, r, p) => new object[]
                   {
                        JoinType.Left, rp.RouteId == r.Id,
                        JoinType.Left, rp.ProcessId == p.Id,
                   });

            query = query.Where((rp, r, p) => rp.IsDeleted == 0 && r.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                query = query.Where((rp, r, p) => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower().Equals(req.ProcessCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.RouteName))
            {
                query = query.Where((rp, r, p) => !string.IsNullOrEmpty(r.Name) && r.Name.Contains(req.RouteName));
            }
            if (!string.IsNullOrEmpty(req.RouteCode))
            {
                query = query.Where((rp, r, p) => !string.IsNullOrEmpty(r.Code) && r.Code.Contains(req.RouteCode));
            }

            query = query.OrderBy((rp, r, p) => rp.RouteId);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((rp, r, p) => new RouteInfo
            {
                Id = r.Id,
                Name = r.Name,
                Code = r.Code,
                Remark = r.Remark,
                VettingStatus = r.VettingStatus,
                RouteDesc = r.RouteDesc,
                Status = r.Status,
                CreateTime = r.CreateTime,
                CreatorId = r.CreatorId,
                RouteAndProcessId = rp.Id,
                ProcessId = p.Id,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((rp, r, p) => new RouteInfo
                {
                    Id = r.Id,
                    Name = r.Name,
                    Code = r.Code,
                    Remark = r.Remark,
                    VettingStatus = r.VettingStatus,
                    RouteDesc = r.RouteDesc,
                    Status = r.Status,
                    CreateTime = r.CreateTime,
                    CreatorId = r.CreatorId,
                    RouteAndProcessId = rp.Id,
                    ProcessId = p.Id,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<RouteInfo>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }
    }
}
