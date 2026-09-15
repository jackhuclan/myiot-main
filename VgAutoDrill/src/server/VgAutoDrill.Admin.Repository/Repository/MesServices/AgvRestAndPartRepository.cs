using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class AgvRestAndPartRepository : BaseRepository<AgvRestAndPart>, IAgvRestAndPartRepository
    {
        public AgvRestAndPartRepository(IUnitOfWork unitOfWork
            ) : base(unitOfWork)
        {

        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IPageList<RestAndPartDto>> GetList(GetRestAndPartListReq req)
        {
            var query = DBClient.Queryable<AgvRestAndPart, AgvRest>(
                (restAndpart, rest) => new object[]
                {
                    JoinType.Left, restAndpart.RestId == rest.Id
                });

            query = query.Where((restAndpart, rest) => restAndpart.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.PartCode))
            {
                query = query.Where((restAndpart, rest) => !string.IsNullOrEmpty(restAndpart.PartCode) && restAndpart.PartCode.Contains(req.PartCode));
            }
            if (!string.IsNullOrEmpty(req.PartName))
            {
                query = query.Where((restAndpart, rest) => !string.IsNullOrEmpty(restAndpart.PartName) && restAndpart.PartName.Contains(req.PartName));
            }
            if (!string.IsNullOrEmpty(req.RestCode))
            {
                query = query.Where((restAndpart, rest) => !string.IsNullOrEmpty(rest.Code) && rest.Code.Contains(req.RestCode));
            }
            if (req.Status > -1)
            {
                query = query.Where(p => p.Status == req.Status);
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select((restAndpart, rest) => new RestAndPartDto
            {
                Id = restAndpart.Id,
                PartCode = restAndpart.PartCode,
                PartName = restAndpart.PartName,
                RouteCode = restAndpart.RouteCode,
                RestCode = string.IsNullOrEmpty(rest.Code) ? "" : rest.Code,
                RestName = string.IsNullOrEmpty(rest.Name) ? "" : rest.Name,
                Point = rest.Point,
                Priority = restAndpart.Priority,
                AgvDeviceKind = restAndpart.AgvDeviceKind,
                Status = restAndpart.Status,
                CreateTime = restAndpart.CreateTime,
                CreatorId = restAndpart.CreatorId,
                ModifierId = restAndpart.ModifierId,
                ModifyTime = restAndpart.ModifyTime,
                PreBookAgv = rest.PreBookAgv,
                PreBookTime = rest.PreBookTime,
                CurrentAgv = rest.CurrentAgv,
                RestStatus = rest.Status,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((restAndpart, rest) => new RestAndPartDto
                {
                    Id = restAndpart.Id,
                    PartCode = restAndpart.PartCode,
                    PartName = restAndpart.PartName,
                    RouteCode = restAndpart.RouteCode,
                    RestCode = string.IsNullOrEmpty(rest.Code) ? "" : rest.Code,
                    RestName = string.IsNullOrEmpty(rest.Name) ? "" : rest.Name,
                    Point = rest.Point,
                    Priority = restAndpart.Priority,
                    AgvDeviceKind = restAndpart.AgvDeviceKind,
                    Status = restAndpart.Status,
                    CreateTime = restAndpart.CreateTime,
                    CreatorId = restAndpart.CreatorId,
                    ModifierId = restAndpart.ModifierId,
                    ModifyTime = restAndpart.ModifyTime,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<RestAndPartDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RestAndPartDto> QueryByID(long Id)
        {
            var query = DBClient.Queryable<AgvRestAndPart, AgvRest>
                        ((r, p) => new object[]
                        {
                              JoinType.Left, r.RestId == p.Id
                        });
            query = query.Where((r, p) => r.IsDeleted == 0 && p.IsDeleted == 0);

            query.Where((r, p) => r.Id == Id);

            var data = await query.Select((r, p) => new RestAndPartDto
            {
                Id = r.Id,
                PartCode = r.PartCode,
                PartName = r.PartName,
                RouteCode = r.RouteCode,
                RestCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                RestName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                Point = p.Point,
                Priority = r.Priority,
                AgvDeviceKind = r.AgvDeviceKind,
                Status = r.Status,
                CreateTime = r.CreateTime,
                CreatorId = r.CreatorId,
                ModifierId = r.ModifierId,
                ModifyTime = r.ModifyTime,
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
    }
}
