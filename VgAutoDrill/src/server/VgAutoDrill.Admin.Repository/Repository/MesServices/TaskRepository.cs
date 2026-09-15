using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class TaskRepository : BaseRepository<Model.Entites.Mes.WorkTask>, ITaskRepository
    {
        public TaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IPageList<TaskDto>> GetList(GetTaskListReq req)
        {
            var query = DBClient.Queryable<WorkTask>()
                .LeftJoin<Device>((p, d) => p.WorkStationCode == d.Code);
            if (req.IsAuto != null)
            {
                query = query.Where((p, d) => d.IsAuto == req.IsAuto);
            }
            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
            }
            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.StartsWith(req.ProcessCode.Trim()));
            }
            if (req.RouteCodes != null && req.RouteCodes.Count > 0)
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.RouteCode) && req.RouteCodes.Contains(p.RouteCode));
            }
            if (req.TaskStatusList != null && req.TaskStatusList.Count > 0)
            {
                query = query.Where((p, d) => p.TaskStatus != null && req.TaskStatusList.Contains((TaskStatusEnum)p.TaskStatus));
            }
            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.Contains(req.WorkStationCode));
            }
            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.Contains(req.WorkOrderCode));
            }
            if (req.WorkOrderId > 0)
            {
                query = query.Where((p, d) => p.WorkOrderId == req.WorkOrderId);
            }
            if (req.ProcessId > 0)
            {
                query = query.Where((p, d) => p.ProcessId == req.ProcessId);
            }

            if (req.RequestDate != null)
            {
                query = query.Where((p, d) => p.RequestDate != null && p.RequestDate.Value.Date.Equals(req.RequestDate.Value.Date));
            }

            if (req.QueryStartTime != null)
            {
                query = query.Where((p, d) => p.StartTime >= req.QueryStartTime.Value);
            }
            if (req.QueryEndTime != null)
            {
                query = query.Where((p, d) => p.StartTime <= req.QueryEndTime.Value);
            }

            if (req.Status > -1)
            {
                query = query.Where((p, d) => p.Status == req.Status);
            }

            if (req.IsUrgent != null)
            {
                query = query.Where((p, d) => p.IsUrgent == req.IsUrgent);
            }

            if (req.QueryOrderBy == null)
            {
                query = query.OrderBy((p, d) => p.TaskStatus);
                query = query.OrderByDescending((p, d) => p.Code);
            }
            else
            {
                switch (req.QueryOrderBy)
                {
                    case QueryOrderByEnum.OrderByCodeDesc:
                        query = query.OrderByDescending((p, d) => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCodeASC:
                        query = query.OrderBy((p, d) => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeDesc:
                        query = query.OrderByDescending((p, d) => p.CreateTime);
                        query = query.OrderByDescending((p, d) => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeASC:
                        query = query.OrderBy((p, d) => p.CreateTime);
                        query = query.OrderBy((p, d) => p.Code);
                        break;

                    default:
                        query = query.OrderBy((p, d) => p.TaskStatus);
                        query = query.OrderByDescending((p, d) => p.Code);
                        break;
                }
            }
            var totalCount = await query.CountAsync();
            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && totalCount > 0)
            {
                req.PageNum = pageCount;
            }
            var data = await query.Select((p, d) => new TaskDto()
            {
                PanelCount = p.PanelCount,
                Quantity = p.Quantity,
                QuantityProduced = p.QuantityProduced,
                QuantityQuanlify = p.QuantityQuanlify,
                QuantityUnquanlify = p.QuantityUnquanlify,
                AfterDrillFilePath = p.AfterDrillFilePath,
                Ancestors = p.Ancestors,
                WorkOrderCode = p.WorkOrderCode,
                BarCode = p.BarCode,
                BatchCode = p.BatchCode,
                BeforeDrillFilePath = p.BeforeDrillFilePath,
                ClientCode = p.ClientCode,
                ClientId = p.ClientId,
                ClientName = p.ClientName,
                Code = p.Code,
                Color = p.Color,
                CreateTime = p.CreateTime,
                CreatorId = p.CreatorId,
                CutterGroupNo = p.CutterGroupNo,
                DiaFilePath = p.DiaFilePath,
                Duration = p.Duration,
                EndTime = p.EndTime,
                Id = p.Id,
                IncodeNumber = p.IncodeNumber,
                WorkOrderId = p.WorkOrderId,
                WorkOrderName = p.WorkOrderName,
                WorkStationCode = p.WorkStationCode,
                WorkStationId = p.WorkStationId,
                WorkStationName = p.WorkStationName,
                Status = p.Status,
                IsRebrush = p.IsRebrush,
                ItemCode = p.ItemCode,
                ItemName = p.ItemName,
                NowWadCount = p.NowWadCount,
                UnitOfMeasure = p.UnitOfMeasure,
                IsUrgent = p.IsUrgent,
                Name = p.Name,
                ItemTypeId = p.ItemTypeId,
                ItemId = p.ItemId,
                KeyFlag = p.KeyFlag,
                LayerNum = p.LayerNum,
                ModifierId = p.ModifierId,
                ModifyTime = p.ModifyTime,
                ParentId = p.ParentId,
                StartTime = p.StartTime,
                SpecGroup = p.SpecGroup,
                TaskStatus = p.TaskStatus,
                RouteName = p.RouteName,
                ProcessCode = p.ProcessCode,
                RouteId = p.RouteId,
                RequestDate = p.RequestDate,
                RealStartTime = p.RealStartTime,
                ProgramFilePath = p.ProgramFilePath,
                ProcessName = p.ProcessName,
                RouteCode = p.RouteCode,
                RealEndTime = p.RealEndTime,
                RealDuration = p.RealDuration,
                ProcessId = p.ProcessId,
                IsAuto = d.IsAuto
            }).ToPageListAsync(req.PageNum, req.PageSize);
            var list = new PageList<TaskDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        public async Task<List<WorkTask>> GetDrillTaskList(GetDrillTaskReq req)
        {

            var query = DBClient.Queryable<WorkTask>().LeftJoin<Device>((p, d) => p.WorkStationCode == d.Code)
                .Where(p => p.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                query = query.Where((p, d) => p.ProcessCode == req.ProcessCode && p.StartTime >= req.StartDate
                && p.StartTime < req.StartDate.Value.Date.AddDays(req.TaskNumber.Value));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.ToLower().Contains(req.ItemCode.ToLower()));
            }

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                query = query.Where((p, d) => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.ToLower().Contains(req.WorkOrderCode.ToLower()));
            }

            if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
            {
                query = query.Where((p, d) => !string.IsNullOrWhiteSpace(p.RouteCode) && req.RouteCodeList.Contains(p.RouteCode.ToLower()));
            }
            query = query.OrderBy((p, d) => p.StartTime, OrderByType.Asc);
            return await query.Select((p, d) => p).ToListAsync();
        }

        public async Task<RouteInfoAndProcessInfo> GetRouteAndProcessList(GetRouteAndProcessByItemReq req)
        {
            RouteInfoAndProcessInfo result = new RouteInfoAndProcessInfo();

            var queryRoute = DBClient.Queryable<Item, RouteAndProductCategory, Route>
                ((i, rpc, r) => new object[]
                    {
                        JoinType.Left, i.ProductCategoryId == rpc.ProductCategoryId,
                        JoinType.Left, rpc.RouteId == r.Id
                    });
            queryRoute = queryRoute.Where((i, rpc, r) => i.IsDeleted == 0 && rpc.IsDeleted == 0 && r.IsDeleted == 0);

            if (req.ItemId > 0)
            {
                queryRoute = queryRoute.Where((i, rpc, r) => i.Id == req.ItemId);
            }

            if (req.RouteId > 0)
            {
                queryRoute = queryRoute.Where((i, rpc, r) => r.Id == req.RouteId);
            }

            if (req.VettingStatus != null)
            {
                queryRoute = queryRoute.Where((i, rpc, r) => r.VettingStatus == req.VettingStatus);
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                queryRoute = queryRoute.Where((i, rpc, r) => i.Code == req.ItemCode);
            }

            queryRoute = queryRoute.OrderBy((i, rpc, r) => rpc.OrderNum);

            var routeList = await queryRoute.Select((i, rpc, r) => new RouteDto
            {
                Id = r.Id,
                Name = r.Name,
                Code = r.Code,
                Remark = r.Remark,
                RouteDesc = r.RouteDesc,
                VettingStatus = r.VettingStatus,
            }).ToListAsync();

            if (routeList == null || routeList.Count == 0)
            {
                return result;
            }

            result.RouteInfos = routeList;

            var query = DBClient.Queryable<RouteAndProcess, Process>
                ((rp, p) => new object[]
                    {
                        JoinType.Left, rp.ProcessId == p.Id
                    });

            query = query.Where((rp, p) => rp.IsDeleted == 0 && p.IsDeleted == 0);

            query = query.Where((rp, p) => rp.RouteId == routeList[0].Id);

            query = query.OrderBy((rp, p) => rp.OrderNum);

            var processList = await query.Select((rp, p) => new RouteAndProcessDto
            {
                Id = rp.Id,
                RouteId = rp.RouteId,
                ProcessId = rp.ProcessId,
                ProcessCode = string.IsNullOrEmpty(p.Code) ? "" : p.Code,
                ProcessName = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                OrderNum = rp.OrderNum,
                Color = rp.Color,
                KeyFlag = rp.KeyFlag,
                RequiredTime = rp.RequiredTime,
                IsManualCheck = rp.IsManualCheck,
                SelfCheckNum = rp.SelfCheckNum,
            }).ToListAsync();

            result.ProcessInfos = processList;

            return result;
        }

        public async Task<IPageList<Model.Entites.Mes.WorkTask>> GetEquipmentList(GetTaskListReq req)
        {
            var query = DBClient.Queryable<Model.Entites.Mes.WorkTask, ItemType>
                ((p, i) => new object[]
                    {
                        JoinType.Left, p.ItemTypeId == i.Id
                    });

            query = query.Where((p, i) => p.IsDeleted == 0 && i.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
            }
            if (req.TaskStatusList != null && req.TaskStatusList.Count > 0)
            {
                query = query.Where((p, i) => p.TaskStatus != null && req.TaskStatusList.Contains((TaskStatusEnum)p.TaskStatus));
            }
            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.StartsWith(req.ProcessCode.Trim()));
            }
            if (req.RouteCodes != null && req.RouteCodes.Count > 0)
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.RouteCode) && req.RouteCodes.Contains(p.RouteCode));
            }
            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.Contains(req.WorkStationCode));
            }
            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.Contains(req.WorkOrderCode));
            }
            if (req.RequestDate != null)
            {
                query = query.Where((p, i) => p.RequestDate != null && p.RequestDate.Value.Date.Equals(req.RequestDate.Value.Date));
            }
            if (req.QueryStartTime != null)
            {
                query = query.Where((p, i) => p.CreateTime >= req.QueryStartTime.Value);
            }
            if (req.QueryEndTime != null)
            {
                query = query.Where((p, i) => p.CreateTime <= req.QueryEndTime.Value);
            }
            if (req.WorkOrderId > 0)
            {
                query = query.Where((p, i) => p.WorkOrderId == req.WorkOrderId);
            }
            if (req.ProcessId > 0)
            {
                query = query.Where((p, i) => p.ProcessId == req.ProcessId);
            }
            if (req.Status > -1)
            {
                query = query.Where((p, i) => p.Status == req.Status);
            }

            if (req.ItemTypeId > 0)
            {
                var sql = $"select id from t_item_type where find_in_set({req.ItemTypeId},ancestors)";
                var typeIdList = DBClient.SqlQueryable<ItemType>(sql).Select(d => d.Id).ToList();

                query = query.Where((p, i) => p.ItemTypeId == req.ItemTypeId
                            || (p.ItemTypeId.HasValue && typeIdList.Contains(p.ItemTypeId.Value)));
            }

            if (req.QueryOrderBy == null)
            {
                query = query.OrderByDescending((p, i) => p.Code);
            }
            else
            {
                switch (req.QueryOrderBy)
                {
                    case QueryOrderByEnum.OrderByCodeDesc:
                        query = query.OrderByDescending((p, i) => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCodeASC:
                        query = query.OrderBy((p, i) => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeDesc:
                        query = query.OrderByDescending((p, i) => p.CreateTime);
                        query = query.OrderByDescending((p, i) => p.Code);
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeASC:
                        query = query.OrderBy((p, i) => p.CreateTime);
                        query = query.OrderBy((p, i) => p.Code);
                        break;

                    default:
                        query = query.OrderByDescending((p, i) => p.Code);
                        break;
                }
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select((p, i) => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((p, i) => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<Model.Entites.Mes.WorkTask>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IPageList<FitWorkStationDto>> GetFitWorkStationList(GetFitWorkStationListReq req)
        {
            List<FitWorkStationDto> workStations = await GetAllFitList(req);

            if (req.FitCount > 0)
            {
                workStations = workStations.Take(req.FitCount).ToList();
            }

            var data = workStations.Skip(req.PageSize * (req.PageNum - 1)).Take(req.PageSize).ToList();

            var list = new PageList<FitWorkStationDto>(data, req.PageNum, req.PageSize, workStations.Count);

            return list;
        }

        /// <summary>
        /// 根据工单的工艺路线和工序获取全部机台，按空闲时间进行排序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private async Task<List<FitWorkStationDto>> GetAllFitList(GetFitWorkStationListReq req)
        {
            var query = DBClient.Queryable<WorkOrder, RouteAndProcess, Process, RouteProcessAndWorkStation, WorkStation, Device>
                            ((wo, rp, p, rpw, ws, d) => new object[]
                                {
                        JoinType.Inner, wo.RouteId == rp.RouteId,
                        JoinType.Inner, rp.ProcessId == p.Id,
                        JoinType.Inner, rp.Id == rpw.RouteAndProcessId,
                        JoinType.Inner, rpw.WorkStationId == ws.Id,
                        JoinType.Inner, ws.Id == d.WorkStationId,
                                });

            query = query.Where((wo, rp, p, rpw, ws, d) => ws.IsDeleted == 0 && wo.IsDeleted == 0 && rp.IsDeleted == 0
            && rpw.IsDeleted == 0 && p.IsDeleted == 0 && d.IsDeleted == 0 && ws.Status == 1);

            if (req.WorkOrderId > 0)
            {
                query = query.Where((wo, rp, p, rpw, ws, d) => wo.Id == req.WorkOrderId);
            }

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                query = query.Where((wo, rp, p, rpw, ws, d) => wo.Code == req.WorkOrderCode);
            }

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                query = query.Where((wo, rp, p, rpw, ws, d) => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == req.ProcessCode.ToLower());
            }

            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                query = query.Where((wo, rp, p, rpw, ws, d) => !string.IsNullOrEmpty(ws.Code) && ws.Code.Contains(req.WorkStationCode));
            }

            if (!string.IsNullOrEmpty(req.WorkStationName))
            {
                query = query.Where((wo, rp, p, rpw, ws, d) => !string.IsNullOrEmpty(ws.Name) && ws.Name.Contains(req.WorkStationName));
            }

            var allData = await query.Select((wo, rp, p, rpw, ws, d) => new FitWorkStationDto
            {
                Name = ws.Name,
                Code = ws.Code,
                Id = ws.Id,
                WorkshopCode = ws.WorkshopCode,
                WorkshopId = ws.WorkshopId,
                WorkshopName = ws.WorkshopName,
                BeginFreeTime = DateTime.Today,
                CreateTime = ws.CreateTime,
                DeviceStatus = d.DeviceStatus,
                DeviceCode = d.Code,
                ToDoTaskCount = 0,
            }).ToListAsync();

            if (allData == null || allData.Count == 0)
            {
                return new List<FitWorkStationDto>();
            }

            allData = allData.DistinctBy(p => p.Id).ToList();

            if (req.DeviceStatusList != null && req.DeviceStatusList.Count > 0)
            {
                allData = allData.Where(p => p.DeviceStatus != null && req.DeviceStatusList.Contains((DeviceStatus)p.DeviceStatus)).ToList();
            }

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

            return workStations;
        }

        /// <summary>
        /// 根据设备编码查询Commit状态的任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IPageList<Model.Entites.Mes.WorkTask>> GetTaskByDevice(GetDrillOrAgvDeviceInfoReq req)
        {
            var query = DBClient.Queryable<Model.Entites.Mes.WorkTask>();

            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                query = query.Where((t) => !string.IsNullOrEmpty(t.WorkStationCode) && t.WorkStationCode.ToLower().Equals(req.DeviceCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where(t => !string.IsNullOrEmpty(t.ItemCode) && t.ItemCode.ToLower() == req.ItemCode.ToLower());
            }

            if (req.TaskStatusList == null || !req.TaskStatusList.Any())
            {
                query = query.Where((t) => t.TaskStatus != null && t.TaskStatus == TaskStatusEnum.COMMITED);
            }
            else
            {
                query = query.Where((t) => t.TaskStatus != null && req.TaskStatusList.Contains(t.TaskStatus.Value));
            }

            if (req.IsVerifyAfterDrillPath)
            {
                query = query.Where((t) => !string.IsNullOrEmpty(t.AfterDrillFilePath));
            }
            if (req.IsManualCallAgvRequest)
            {
                query = query.OrderBy(s => new { s.CutterGroupNo, s.StartTime });
            }
            else
            {
                query = query.OrderBy((t) => t.StartTime);
            }
            RefAsync<int> totalCount = 0;
            var data = await query.Select((t) => t).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((t) => t).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }
            var list = new PageList<Model.Entites.Mes.WorkTask>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }
    }
}
