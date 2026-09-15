using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class RouteProcessAndWorkStationRepository : BaseRepository<RouteProcessAndWorkStation>, IRouteProcessAndWorkStationRepository
    {
        public RouteProcessAndWorkStationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageList<RouteProcessAndWorkStationDto>> GetList(GetRouteProcessAndWorkStationListReq req)
        {
            var query = DBClient.Queryable<RouteProcessAndWorkStation, WorkStation>
                ((r, p) => new object[]
                    {
                        JoinType.Left, r.WorkStationId == p.Id
                    });

            query = query.Where((r, p) => r.IsDeleted == 0 && p.IsDeleted == 0);

            if (req.RouteAndProcessId > 0)
            {
                query = query.Where((r, p) => r.RouteAndProcessId == req.RouteAndProcessId);
            }

            if (req.WorkStationId > 0)
            {
                query = query.Where((r, p) => r.WorkStationId == req.WorkStationId);
            }

            query = query.OrderBy((r, p) => p.Code);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((r, p) => new RouteProcessAndWorkStationDto
            {
                Id = r.Id,
                RouteAndProcessId = r.RouteAndProcessId,
                WorkStationId = r.WorkStationId,
                WorkStationCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                WorkStationName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                OrderNum = r.OrderNum,
                Status = r.Status
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((r, p) => new RouteProcessAndWorkStationDto
                {
                    Id = r.Id,
                    RouteAndProcessId = r.RouteAndProcessId,
                    WorkStationId = r.WorkStationId,
                    WorkStationCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                    WorkStationName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                    OrderNum = r.OrderNum,
                    Status = r.Status
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<RouteProcessAndWorkStationDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<RouteProcessAndWorkStationDto> MultiQueryByID(long Id)
        {
            var query = DBClient.Queryable<RouteProcessAndWorkStation, WorkStation>
                ((r, p) => new object[]
                    {
                        JoinType.Left, r.WorkStationId == p.Id,
                    });

            query = query.Where((r, p) => r.IsDeleted == 0 && p.IsDeleted == 0);

            query.Where((r, p) => r.Id == Id);

            var data = await query.Select((r, p) => new RouteProcessAndWorkStationDto
            {
                Id = r.Id,
                RouteAndProcessId = r.RouteAndProcessId,
                WorkStationId = r.WorkStationId,
                WorkStationCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                WorkStationName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                OrderNum = r.OrderNum,
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

        public async Task<List<WorkstationDto>> GetDrillRouteCodes()
        {
            var query = DBClient.Queryable<RouteProcessAndWorkStation, RouteAndProcess, Route, WorkStation>
                ((rpw, rp, r, w) => new object[]
                    {
                        JoinType.Left, rpw.RouteAndProcessId == rp.Id,
                        JoinType.Left, rp.RouteId == r.Id,
                        JoinType.Left, rpw.WorkStationId == w.Id,
                    });

            query = query.Where((rpw, rp, r, w) => rpw.IsDeleted == 0 && rp.IsDeleted == 0 && r.IsDeleted == 0);

            var drillAndRoutes = await query.Select((rpw, rp, r, w) => new { DeviceId = w.Code, RouteCode = r.Code }).ToListAsync();

            return drillAndRoutes.GroupBy(x => x.DeviceId).Select(x => new WorkstationDto { Code = x.Key, RouteCodes = x.Select(x => x.RouteCode ?? string.Empty).ToList() }).ToList();
        }

        public async Task<PageList<RouteInfo>> GetRoutesByWorkStation(GetRouteProcessAndWorkStationListReq req)
        {
            var query = DBClient.Queryable<RouteProcessAndWorkStation, RouteAndProcess, Route, WorkStation>
                ((rpw, rp, r, w) => new object[]
                    {
                        JoinType.Left, rpw.RouteAndProcessId == rp.Id,
                        JoinType.Left, rp.RouteId == r.Id,
                        JoinType.Left, rpw.WorkStationId == w.Id,
                    });

            query = query.Where((rpw, rp, r, w) => rpw.IsDeleted == 0 && rp.IsDeleted == 0 && r.IsDeleted == 0);

            if (req.RouteAndProcessId > 0)
            {
                query = query.Where((rpw, rp, r, w) => rpw.RouteAndProcessId == req.RouteAndProcessId);
            }

            if (req.WorkStationId > 0)
            {
                query = query.Where((rpw, rp, r, w) => rpw.WorkStationId == req.WorkStationId);
            }
            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                query = query.Where((rpw, rp, r, w) => !string.IsNullOrEmpty(w.Code) && w.Code.ToLower() == req.WorkStationCode.ToLower());
            }

            query = query.OrderByDescending((rpw, rp, r, w) => r.CreateTime);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((rpw, rp, r, w) => new RouteInfo
            {
                Id = r.Id,
                Code = r.Code,
                Name = r.Name,
                RouteDesc = r.RouteDesc,
                Remark = r.Remark,
                VettingStatus = r.VettingStatus,
                Status = r.Status,
                CreateTime = r.CreateTime,
                CreatorId = r.CreatorId,
                ProcessId = rp.ProcessId,
                RouteAndProcessId = rpw.RouteAndProcessId,
                RouteProcessAndWorkStationId = rpw.Id,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((rpw, rp, r, w) => new RouteInfo
                {
                    Id = r.Id,
                    Code = r.Code,
                    Name = r.Name,
                    RouteDesc = r.RouteDesc,
                    Remark = r.Remark,
                    VettingStatus = r.VettingStatus,
                    Status = r.Status,
                    CreateTime = r.CreateTime,
                    CreatorId = r.CreatorId,
                    ProcessId = rp.ProcessId,
                    RouteAndProcessId = rpw.RouteAndProcessId,
                    RouteProcessAndWorkStationId = rpw.Id,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var newData = data.DistinctBy(p => p.Code).ToList();
            totalCount = newData.Count;

            var list = new PageList<RouteInfo>(newData, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 根据工艺路线和工序获取工作站，按空闲时间进行排序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageList<FitWorkStationDto>> GetFitWorkStationListByRoute(GetFitWorkStationListByRouteReq req)
        {
            List<FitWorkStationDto> workStations = await GetAllFitList(req);

            var data = workStations.Skip(req.PageSize * (req.PageNum - 1)).Take(req.PageSize).ToList();

            var list = new PageList<FitWorkStationDto>(data, req.PageNum, req.PageSize, workStations.Count);

            return list;
        }

        private async Task<List<FitWorkStationDto>> GetAllFitList(GetFitWorkStationListByRouteReq req)
        {
            var query = DBClient.Queryable<Route, RouteAndProcess, Process, RouteProcessAndWorkStation, WorkStation, Device>
                            ((r, rp, p, rpw, ws, de) => new object[]
                                {
                        JoinType.Inner, r.Id == rp.RouteId,
                        JoinType.Inner, rp.ProcessId == p.Id,
                        JoinType.Inner, rp.Id == rpw.RouteAndProcessId,
                        JoinType.Inner, rpw.WorkStationId == ws.Id,
                        JoinType.Inner, de.WorkStationId == rpw.WorkStationId,
                                });

            query = query.Where((r, rp, p, rpw, ws) => ws.IsDeleted == 0 && r.IsDeleted == 0 && rp.IsDeleted == 0
            && rpw.IsDeleted == 0 && p.IsDeleted == 0 && ws.Status == 1);

            if (!string.IsNullOrEmpty(req.RouteCode))
            {
                query = query.Where((r, rp, p, rpw, ws) => r.Code == req.RouteCode);
            }

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                query = query.Where((r, rp, p, rpw, ws) => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == req.ProcessCode.ToLower());
            }
            if (req.DeviceStatusList != null && req.DeviceStatusList.Count > 0)
            {
                query = query.Where((r, rp, p, rpw, ws, de) => de.DeviceStatus != null && req.DeviceStatusList.Contains((DeviceStatus)de.DeviceStatus));
            }

            var allData = await query.Select((r, rp, p, rpw, ws, de) => new FitWorkStationDto
            {
                Name = ws.Name,
                Code = ws.Code,
                Id = ws.Id,
                WorkshopCode = ws.WorkshopCode,
                WorkshopId = ws.WorkshopId,
                WorkshopName = ws.WorkshopName,
                BeginFreeTime = DateTime.Today,
                CreateTime = ws.CreateTime,
                DeviceStatus = de.DeviceStatus,
            }).ToListAsync();

            List<FitWorkStationDto> workStations = new List<FitWorkStationDto>();

            var queryTask = DBClient.Queryable<Model.Entites.Mes.WorkTask>();

            queryTask = queryTask.Where(t => t.IsDeleted == 0 && t.EndTime > DateTime.Today);
            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                queryTask = queryTask.Where((t) => !string.IsNullOrEmpty(t.ProcessCode) && t.ProcessCode.ToLower() == req.ProcessCode.ToLower());
            }
            queryTask = queryTask.OrderBy(t => t.EndTime);

            var taskData = await queryTask.Select(t => t).ToListAsync();
            if (taskData == null || taskData.Count == 0)
            {
                workStations.AddRange(allData);
            }
            else
            {
                foreach (var workStation in allData)
                {
                    if (!taskData.Exists(p => p.WorkStationId == workStation.Id))
                    {
                        workStations.Add(workStation);
                    }
                }

                var useTaskData = (from t in taskData
                                   where allData.Exists(p => p.Id == t.WorkStationId) && t.RealEndTime is null
                                   orderby t.EndTime descending
                                   select t).DistinctBy(t => t.WorkStationId).ToList();
                if (useTaskData != null)
                {
                    for (int i = useTaskData.Count; i > 0; i--)
                    {
                        var model = allData.SingleOrDefault(p => p.Id == useTaskData[i - 1].WorkStationId);
                        if (model != null)
                        {
                            model.BeginFreeTime = useTaskData[i - 1].EndTime;
                            workStations.Add(model);
                        }
                    }
                }

                var useEndTaskData = (from t in taskData
                                      where allData.Exists(p => p.Id == t.WorkStationId) && t.RealEndTime is not null
                                      orderby t.RealEndTime descending
                                      select t).DistinctBy(t => t.WorkStationId).ToList();
                if (useEndTaskData != null)
                {
                    for (int i = useEndTaskData.Count; i > 0; i--)
                    {
                        if (workStations.Exists(p => p.Id == useEndTaskData[i - 1].WorkStationId))
                        {
                            var model = workStations.SingleOrDefault(p => p.Id == useEndTaskData[i - 1].WorkStationId);
                            if (model != null)
                            {
                                if (model.BeginFreeTime < useEndTaskData[i - 1].RealEndTime)
                                {
                                    model.BeginFreeTime = useEndTaskData[i - 1].RealEndTime;
                                }
                            }
                        }
                        else
                        {
                            var model = allData.SingleOrDefault(p => p.Id == useEndTaskData[i - 1].WorkStationId);
                            if (model != null)
                            {
                                if (useEndTaskData[i - 1].RealEndTime > DateTime.Today)
                                {
                                    model.BeginFreeTime = useEndTaskData[i - 1].RealEndTime;
                                }
                                else
                                {
                                    if (model.BeginFreeTime == null || model.BeginFreeTime < DateTime.Today)
                                    {
                                        model.BeginFreeTime = DateTime.Today;
                                    }
                                }
                                workStations.Add(model);
                            }
                        }
                    }
                }
            }

            workStations = workStations.OrderByDescending(p => p.BeginFreeTime).DistinctBy(p => p.Code).ToList();

            workStations = workStations.OrderBy(p => p.BeginFreeTime).ThenBy(p => p.Code).ToList();

            if (taskData != null && taskData.Count > 0)
            {
                foreach (var item in workStations)
                {
                    var commitTask = taskData.FindAll(p => p.WorkStationId == item.Id && p.TaskStatus == TaskStatusEnum.COMMITED);
                    if (commitTask == null)
                    {
                        continue;
                    }
                    item.ToDoTaskCount = commitTask.Count;
                }
            }
            else
            {
                for (int i = 0; i < workStations.Count(); i++)
                {
                    workStations[i].ToDoTaskCount = GetTaskCount(workStations[i]);
                }
            }
            return workStations;
        }

        private int? GetTaskCount(FitWorkStationDto fWDto)
        {
            var queryTask = DBClient.Queryable<Model.Entites.Mes.WorkTask>();
            int count = queryTask.Where(t => t.TaskStatus == TaskStatusEnum.COMMITED && t.WorkOrderName == fWDto.Name && t.WorkStationCode == fWDto.Code && t.StartTime >= fWDto.BeginFreeTime).Count();
            return 0;
        }
    }
}
