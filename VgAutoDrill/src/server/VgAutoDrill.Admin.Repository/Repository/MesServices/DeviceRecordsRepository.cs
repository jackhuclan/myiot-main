using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceRecordsRepository : BaseRepository<DeviceRecords>, IDeviceRecordsRepository
    {
        public DeviceRecordsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageList<DeviceRecordsDto>> GetList(GetDeviceRecordsListReq req)
        {
            var query = DBClient.Queryable<DeviceRecords, Device, WorkStation, RouteProcessAndWorkStation, RouteAndProcess, Route>
                            ((dr, d, w, rpW, rp, r) => new object[]
                            {
                                JoinType.Left, dr.DeviceCode == d.Code,
                                JoinType.Left, d.WorkStationId == w.Id,
                                JoinType.Left, w.Id == rpW.WorkStationId,
                                JoinType.Left, rpW.RouteAndProcessId == rp.Id,
                                JoinType.Left,rp.RouteId ==r.Id,
                            });

            query = query.Where((dr, d, w, rpW, rp, r) => dr.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                query = query.Where((dr, d, w, rpW, rp, r) => !string.IsNullOrEmpty(dr.DeviceCode) && dr.DeviceCode.ToLower().Contains(req.DeviceCode.ToLower()));
            }

            if (req.QueryStartTime != null)
            {
                query = query.Where((dr, d, w, rpW, rp, r) => dr.CreateTime >= req.QueryStartTime.Value);
            }
            if (req.QueryEndTime != null)
            {
                query = query.Where((dr, d, w, rpW, rp, r) => dr.CreateTime <= req.QueryEndTime.Value);
            }

            if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
            {
                query = query.Where((dr, d, w, rpW, rp, r) => !string.IsNullOrEmpty(r.Code) && req.RouteCodeList.Contains(r.Code));
            }

            if (req.QueryOrderBy == null)
            {
                query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime);
                query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.DeviceCode);
            }
            else
            {
                switch (req.QueryOrderBy)
                {
                    case QueryOrderByEnum.OrderByCodeDesc:
                        query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                        break;

                    case QueryOrderByEnum.OrderByCodeASC:
                        query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeDesc:
                        query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.CreateTime);
                        query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeASC:
                        query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime);
                        query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                        break;

                    default:
                        query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime);
                        query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                        break;
                }
            }

            if (req.OrderByWaitTime != null)
            {
                switch (req.OrderByWaitTime)
                {
                    case 0:
                        query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.WaitTime);
                        break;

                    case 1:
                        query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.WaitTime);
                        break;
                }
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select((dr, d, w, rpW, rp, r) => new DeviceRecordsDto
            {
                DeviceCode = dr.DeviceCode,
                DateString = dr.DateString,
                Duty = dr.Duty,
                CollectClearTime = dr.CollectClearTime,
                CreateTime = dr.CreateTime,
                CreatorId = dr.CreatorId,
                RouteCode = r.Code,
                EndToStartTime = dr.EndToStartTime,
                ErrorTime = dr.ErrorTime,
                Id = dr.Id,
                ModifierId = dr.ModifierId,
                ModifyTime = dr.ModifyTime,
                OpenTime = dr.OpenTime,
                WaitTime = dr.WaitTime,
                WorkTime = dr.WorkTime,
                Status = dr.Status,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((dr, d, w, rpW, rp, r) => new DeviceRecordsDto
                {
                    DeviceCode = dr.DeviceCode,
                    DateString = dr.DateString,
                    Duty = dr.Duty,
                    CollectClearTime = dr.CollectClearTime,
                    CreateTime = dr.CreateTime,
                    CreatorId = dr.CreatorId,
                    RouteCode = r.Code,
                    EndToStartTime = dr.EndToStartTime,
                    ErrorTime = dr.ErrorTime,
                    Id = dr.Id,
                    ModifierId = dr.ModifierId,
                    ModifyTime = dr.ModifyTime,
                    OpenTime = dr.OpenTime,
                    WaitTime = dr.WaitTime,
                    WorkTime = dr.WorkTime,
                    Status = dr.Status,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }
            var list = new PageList<DeviceRecordsDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        public async Task<DateTime?> GetMinDate()
        {
            var query = DBClient.Queryable<DeviceRecords>();

            query = query.Where(p => !string.IsNullOrEmpty(p.DeviceCode));
            query = query.OrderBy(p => p.CreateTime);

            var data = await query.FirstAsync();
            if (data == null)
            {
                return null;
            }

            return data.CreateTime;
        }

        public async Task<List<DeviceRecords>> GetListByDate(DateTime startDate, DateTime endDate)
        {
            var resultData = new List<DeviceRecords>();

            var list = await DBClient.Queryable<DeviceRecords>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceCode) && p.CreateTime >= startDate && p.CreateTime < endDate)
                            .GroupBy(p => new { p.DeviceCode })
                            .Select(p => new
                            {
                                deviceCode = p.DeviceCode,
                                data = SqlFunc.Subqueryable<DeviceRecords>()
                                .Where(s => s.DeviceCode == p.DeviceCode && s.CreateTime >= startDate && s.CreateTime < endDate)
                                .OrderByDesc(s => s.OpenTime.Length)
                                .OrderByDesc(s => s.OpenTime)
                                .First()
                            }).ToListAsync();

            if (list == null || list.Count == 0)
            {
                return resultData;
            }

            foreach (var item in list)
            {
                resultData.Add(item.data);
            }
            return resultData;
        }
    }
}
