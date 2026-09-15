using SqlSugar;
using SqlSugar.Extensions;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IPageList<Device>> GetList(GetDeviceListReq req)
        {
            var query = DBClient.Queryable<Device, DeviceType>
                ((device, deviceType) => new object[]
                    {
                        JoinType.Left, device.DeviceTypeId == deviceType.Id
                    });

            query = query.Where((device, deviceType) => device.IsDeleted == 0);
            if (req.Status.HasValue)
            {
                query = query.Where((device, deviceType) => device.Status == req.Status);
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((device, deviceType) => !string.IsNullOrEmpty(device.Name) && device.Name.Contains(req.Name));
            }
            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((device, deviceType) => !string.IsNullOrEmpty(device.Code) && device.Code.Contains(req.Code));
            }
            if (req.IsAuto != null)
            {
                query = query.Where((device, deviceType) => device.IsAuto == req.IsAuto);
            }
            if (req.DeviceStatusList != null && req.DeviceStatusList.Count > 0)
            {
                query = query.Where((device, deviceType) => device.DeviceStatus != null && req.DeviceStatusList.Contains((DeviceStatus)device.DeviceStatus));
            }
            if (req.DeviceKindList != null && req.DeviceKindList.Count > 0)
            {
                query = query.Where((device, deviceType) => device.DeviceKind != null && req.DeviceKindList.Contains((DeviceKind)device.DeviceKind));
            }

            if (req.DeviceTypeId > 0)
            {
                var sql = $"select id from t_device_type where find_in_set({req.DeviceTypeId},ancestors)";
                var typeIdList = DBClient.SqlQueryable<DeviceType>(sql).Select(d => d.Id).ToList();

                query = query.Where((device, deviceType) => device.DeviceTypeId == req.DeviceTypeId
                            || (device.DeviceTypeId.HasValue && typeIdList.Contains(device.DeviceTypeId.Value)));
            }
            if (!string.IsNullOrEmpty(req.DeviceTypeCode))
            {
                query = query.Where((device, deviceType) => !string.IsNullOrEmpty(deviceType.Code) && deviceType.Code.ToLower().Equals(req.DeviceTypeCode.ToLower()));
            }

            query = query.OrderBy((device, deviceType) => device.Code);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((device, deviceType) => device).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((device, deviceType) => device).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<Device>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        public async Task<IPageList<Device>> GetDrills(GetDeviceListReq req)
        {
            var query = DBClient.Queryable<Device, WorkStation, RouteProcessAndWorkStation, RouteAndProcess, Route>
                            ((d, w, rpW, rp, r) => new object[]
                            {
                                JoinType.Inner, d.WorkStationId ==w.Id,
                                JoinType.Inner, w.Id == rpW.WorkStationId,
                                JoinType.Inner, rpW.RouteAndProcessId == rp.Id,
                                JoinType.Inner,rp.RouteId ==r.Id,
                            });

            query = query.Where((d, w, rpW, rp, r) => !string.IsNullOrEmpty(d.DeviceTypeCode) && d.DeviceTypeCode.ToLower().Contains("drill"));

            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((d, w, rpW, rp, r) => d.Code!.ToLower().Contains(req.Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((d, w, rpW, rp, r) => d.Name!.ToLower().Contains(req.Name.ToLower()));
            }
            if (req.DeviceStatusList != null && req.DeviceStatusList.Count > 0)
            {
                query = query.Where((d, w, rpW, rp, r) => d.DeviceStatus != null && req.DeviceStatusList.Contains((DeviceStatus)d.DeviceStatus));
            }
            if (req.DeviceKindList != null && req.DeviceKindList.Count > 0)
            {
                query = query.Where((d, w, rpW, rp, r) => d.DeviceKind != null && req.DeviceKindList.Contains((DeviceKind)d.DeviceKind));
            }
            if (req.Status != null)
            {
                query = query.Where((d, w, rpW, rp, r) => d.Status == req.Status);
            }
            if (req.RouteCodes != null && req.RouteCodes.Any())
            {
                query = query.Where((d, w, rpW, rp, r) => req.RouteCodes.Contains(r.Code!.ToLower()));
            }
            query = query.OrderBy((d, w, rpW, rp, r) => d.Code);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((d, w, rpW, rp, r) => d).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((d, w, rpW, rp, r) => d).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<Device>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        public async Task<List<RouteAndProcessListByDevice>> GetRouteAndProcessList(string agvDeviceCode, string anyDeviceCode)
        {
            var dataByAGVDeviceCode = await GetRPListByAGVDevice(agvDeviceCode);

            List<RouteAndProcessListByDevice> dataByAnyDeviceCode = new List<RouteAndProcessListByDevice>();

            var query = DBClient.Queryable<Device>();
            query = query.Where(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower().Equals(anyDeviceCode.ToLower())
             && !string.IsNullOrEmpty(p.DeviceTypeCode) && p.DeviceTypeCode.ToLower().Equals("agv"));

            var data = await query.SingleAsync();
            if (data == null)  //第二个设备非AGV
            {
                dataByAnyDeviceCode = await GetRPListByNotAGVDevice(anyDeviceCode);
            }
            else
            {
                dataByAnyDeviceCode = await GetRPListByAGVDevice(anyDeviceCode);
            }

            if (dataByAGVDeviceCode == null || dataByAGVDeviceCode.Count == 0 || dataByAnyDeviceCode == null || dataByAnyDeviceCode.Count == 0)
            {
                return new List<RouteAndProcessListByDevice>();
            }

            if (data == null)  //第二个设备非AGV
            {
                var result = dataByAGVDeviceCode.Where(x => dataByAnyDeviceCode.Any(p => p.RouteId == x.RouteId && p.WorkStationId == x.WorkStationId)).ToList();
                return result;
            }
            else
            {
                var result = dataByAGVDeviceCode.Where(x => dataByAnyDeviceCode.Any(p => p.RouteId == x.RouteId)).ToList();
                return result;
            }
        }

        /// <summary>
        /// 根据AGV设备获取配置的工艺路线、工序以及工作站
        /// </summary>
        /// <param name="DevieCode"></param>
        /// <returns></returns>
        private async Task<List<RouteAndProcessListByDevice>> GetRPListByAGVDevice(string DevieCode)
        {
            var query = DBClient.Queryable<DeviceAndRoute, Route, RouteAndProcess, Process, RouteProcessAndWorkStation, WorkStation>
                            ((dr, r, rp, p, rpW, w) => new object[]
                            {
                                JoinType.Left, dr.RouteId == r.Id,
                                JoinType.Left, dr.RouteId == rp.RouteId,
                                JoinType.Left, rp.ProcessId == p.Id,
                                JoinType.Left, rp.Id == rpW.RouteAndProcessId,
                                JoinType.Left,rpW.WorkStationId == w.Id,
                            });

            query = query.Where((dr, r, rp, p, rpW, w) => r.VettingStatus == 1 && r.Status == 1 && p.Status == 1 && w.Status == 1);

            if (!string.IsNullOrEmpty(DevieCode))
            {
                query = query.Where((dr, r, rp, p, rpW, w) => !string.IsNullOrEmpty(dr.DeviceCode) && dr.DeviceCode.ToLower().Equals(DevieCode.ToLower()));
            }

            query = query.OrderBy((dr, r, rp, p, rpW, w) => dr.RouteCode);

            var data = await query.Select((dr, r, rp, p, rpW, w) => new RouteAndProcessListByDevice
            {
                RouteId = dr.RouteId,
                RouteCode = dr.RouteCode,
                RouteName = dr.RouteName,
                ProcessId = rp.ProcessId,
                ProcessName = p.Name,
                ProcessCode = p.Code,
                WorkStationId = rpW.WorkStationId,
                WorkStationCode = w.Code,
                WorkStationName = w.Name,
            }).ToListAsync();

            return data;
        }

        /// <summary>
        /// 根据非AGV设备获取配置的工艺路线、工序以及工作站
        /// </summary>
        /// <param name="DeviceCode"></param>
        /// <returns></returns>
        private async Task<List<RouteAndProcessListByDevice>> GetRPListByNotAGVDevice(string DeviceCode)
        {
            var query = DBClient.Queryable<Device, WorkStation, RouteProcessAndWorkStation, RouteAndProcess, Route, Process>
                            ((d, w, rpW, rp, r, p) => new object[]
                            {
                                JoinType.Left, d.WorkStationId ==w.Id,
                                JoinType.Left, w.Id == rpW.WorkStationId,
                                JoinType.Left, rpW.RouteAndProcessId == rp.Id,
                                JoinType.Left,rp.RouteId ==r.Id,
                                JoinType.Left,rp.ProcessId ==p.Id,
                            });

            query = query.Where((d, w, rpW, rp, r, p) => r.VettingStatus == 1 && r.Status == 1 && p.Status == 1 && w.Status == 1 && d.Status == 1);

            if (!string.IsNullOrEmpty(DeviceCode))
            {
                query = query.Where((d, w, rpW, rp, r, p) => !string.IsNullOrEmpty(d.Code) && d.Code.ToLower().Equals(DeviceCode.ToLower()));
            }

            query = query.OrderBy((d, w, rpW, rp, r, p) => r.Code);

            var data = await query.Select((d, w, rpW, rp, r, p) => new RouteAndProcessListByDevice
            {
                RouteId = rp.RouteId,
                RouteCode = r.Code,
                RouteName = r.Name,
                ProcessId = rp.ProcessId,
                ProcessName = p.Name,
                ProcessCode = p.Code,
                WorkStationId = d.WorkStationId,
                WorkStationCode = w.Code,
                WorkStationName = w.Name,
            }).ToListAsync();

            return data;
        }

        /// <summary>
        /// 获取在线设备详细信息
        /// 绑定的工艺路线
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        public async Task<List<CentralOnlineDeviceDto>> GetOnlineDeviceInfo(List<CentralOnlineDeviceDto> devices)
        {
            if (devices == null || devices.Count == 0) return devices;

            var deviceCodes = devices.Where(p => p.DeviceId != null).Select(p => p.DeviceId.ToLower()).Distinct();
            var queryNotAGV = DBClient.Queryable<Device, WorkStation, RouteProcessAndWorkStation, RouteAndProcess, Route>
                            ((d, w, rpW, rp, r) => new object[]
                            {
                                JoinType.Left, d.WorkStationId ==w.Id,
                                JoinType.Left, w.Id == rpW.WorkStationId,
                                JoinType.Left, rpW.RouteAndProcessId == rp.Id,
                                JoinType.Left,rp.RouteId ==r.Id,
                            });

            queryNotAGV = queryNotAGV.Where((d, w, rpW, rp, r) => !string.IsNullOrEmpty(d.Code) && deviceCodes.Contains(d.Code.ToLower()));

            var dataNotAGV = await queryNotAGV.Select((d, w, rpW, rp, r) => new CentralOnlineDeviceDto
            {
                RouteCode = r.Code,
                RouteName = r.Name,
                DeviceId = d.Code,
            }).ToListAsync();

            var queryAGV = DBClient.Queryable<DeviceAndRoute, Route>
                            ((dr, r) => new object[]
                            {
                                JoinType.Left, dr.RouteId == r.Id,
                            });

            queryAGV = queryAGV.Where((dr, r) => !string.IsNullOrEmpty(dr.DeviceCode) && deviceCodes.Contains(dr.DeviceCode.ToLower()));

            var dataAGV = await queryAGV.Select((dr, r) => new CentralOnlineDeviceDto
            {
                RouteCode = r.Code,
                RouteName = r.Name,
                DeviceId = dr.DeviceCode,

            }).ToListAsync();

            if (dataNotAGV == null && dataAGV == null)
            {
                return devices;
            }

            foreach (var item in devices)
            {
                if (string.IsNullOrEmpty(item.DeviceId))
                {
                    continue;
                }
                List<CentralOnlineDeviceDto>? modelList;
                if (!string.IsNullOrEmpty(item.ProductId) && item.ProductId.ToLower().Equals("agv") && dataAGV != null)
                {
                    modelList = dataAGV.FindAll(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower().Equals(item.DeviceId.ToLower()));
                }
                else
                {
                    if (dataNotAGV != null)
                    {
                        modelList = dataNotAGV.FindAll(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower().Equals(item.DeviceId.ToLower()));
                    }
                    else
                    {
                        modelList = new List<CentralOnlineDeviceDto>();
                    }
                }

                if (modelList == null || modelList.Count == 0)
                {
                    continue;
                }

                foreach (var model in modelList)
                {
                    if (string.IsNullOrEmpty(model.RouteCode) || string.IsNullOrEmpty(model.RouteName))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(item.RouteName))
                    {
                        item.RouteName = model.RouteName;
                    }
                    else
                    {
                        item.RouteName = $"{item.RouteName},{model.RouteName}";
                    }

                    if (string.IsNullOrEmpty(item.RouteCode))
                    {
                        item.RouteCode = model.RouteCode;
                    }
                    else
                    {
                        item.RouteCode = $"{item.RouteCode},{model.RouteCode}";
                    }

                    if (!string.IsNullOrEmpty(item.RouteCode))
                    {
                        item.RouteCode = item.RouteCode.TrimEnd(',');
                    }
                }
            }

            return devices;
        }

        /// <summary>
        /// 获取在线Drill下Commit任务
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        public async Task<List<DrillDeviceTaskDto>> GetDrillDeviceTask(List<CentralOnlineDeviceDto> devices)
        {
            List<DrillDeviceTaskDto> result = new List<DrillDeviceTaskDto>();
            foreach (var device in devices)
            {
                var model = new DrillDeviceTaskDto
                {
                    DeviceId = device.DeviceId,
                    DeviceStatus = device.DeviceStatus,
                    DeviceConsoleAddress = device.Descriptor != null ? device.Descriptor.HostAddress : "",
                    OverView = "剩余 0 条任务",
                    RouteCode = device.RouteCode,
                };
                if (device.Properties != null)
                {
                    model.IsWarning = device.Properties.ContainsKey("IsWarning") ? device.Properties["IsWarning"].ObjToBool() : false;
                    model.IsLoadingOrUnLoading = device.Properties.ContainsKey("IsLoadingOrUnLoading") ? device.Properties["IsLoadingOrUnLoading"].ObjToBool() : false;
                    model.CallAgvMessage = device.Properties.ContainsKey("CallAgvMessage") ? device.Properties["CallAgvMessage"].ToString() : "";
                }
                result.Add(model);
            }

            List<string> deviceCodes = devices.Where(p => !string.IsNullOrEmpty(p.DeviceId))
                .Select(p => p.DeviceId).Distinct().ToList().ConvertAll(t => t.ToLower());
            if (deviceCodes == null || deviceCodes.Count == 0)
            {
                return result;
            }

            var query = DBClient.Queryable<Device, Model.Entites.Mes.WorkTask>
                            ((d, t) => new object[]
                            {
                                JoinType.Left, d.WorkStationId == t.WorkStationId,
                            });

            query = query.Where((d, t) => !string.IsNullOrEmpty(d.Code) && deviceCodes.Contains(d.Code.ToLower()));

            query = query.Where((d, t) => t.TaskStatus != null && t.TaskStatus == Model.Enum.TaskStatusEnum.COMMITED);

            query = query.OrderBy((d, t) => t.StartTime);

            var data = await query.Select((d, t) => new DrillDeviceTaskInfo
            {
                DeviceId = d.Code,
                WorkStationCode = t.WorkStationCode,
                WorkStationId = t.WorkStationId,
                WorkStationName = t.WorkStationName,
                Code = t.Code,
                Name = t.Name,
                WorkOrderCode = t.WorkOrderCode,
                WorkOrderName = t.WorkOrderName,
                WorkOrderId = t.WorkOrderId,
                ItemCode = t.ItemCode,
                ItemId = t.ItemId,
                ItemName = t.Name,
                PanelCount = t.PanelCount,
                NowWadCount = t.NowWadCount,
                RouteCode = t.RouteCode,
                RouteId = t.RouteId,
                RouteName = t.RouteName,
                RealDuration = t.RealDuration,
                UnitOfMeasure = t.UnitOfMeasure,

            }).ToListAsync();

            if (data == null || data.Count == 0)
            {
                return result;
            }

            foreach (var item in deviceCodes)
            {
                var tasks = data.FindAll(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower().Equals(item));
                if (tasks == null || tasks.Count == 0)
                {
                    continue;
                }

                var model = new DrillDeviceTaskDto
                {
                    DeviceId = tasks.First().DeviceId,
                    OverView = string.Format("剩余 {0} 条任务", tasks.Count),
                    FirstTask = new Model.ViewModels.Mes.ProTask.TaskDto()
                    {
                        WorkStationCode = tasks.First().WorkStationCode,
                        WorkStationId = tasks.First().WorkStationId,
                        WorkStationName = tasks.First().WorkStationName,
                        Code = tasks.First().Code,
                        Name = tasks.First().Name,
                        WorkOrderCode = tasks.First().WorkOrderCode,
                        WorkOrderName = tasks.First().WorkOrderName,
                        WorkOrderId = tasks.First().WorkOrderId,
                        ItemCode = tasks.First().ItemCode,
                        ItemId = tasks.First().ItemId,
                        ItemName = tasks.First().Name,
                        PanelCount = tasks.First().PanelCount,
                        NowWadCount = tasks.First().NowWadCount,
                        RouteCode = tasks.First().RouteCode,
                        RouteId = tasks.First().RouteId,
                        RouteName = tasks.First().RouteName,
                        RealDuration = tasks.First().RealDuration,
                        UnitOfMeasure = tasks.First().UnitOfMeasure,
                    }
                };

                if (result.Exists(p => p.DeviceId.ToLower().Equals(model.DeviceId.ToLower())))
                {
                    result.RemoveAll(p => p.DeviceId.ToLower().Equals(model.DeviceId.ToLower()));
                }

                var deviceData = devices.SingleOrDefault(p => p.DeviceId.ToLower().Equals(model.DeviceId.ToLower()));
                if (deviceData != null)
                {
                    model.DeviceStatus = deviceData.DeviceStatus;
                    model.RouteCode = deviceData.RouteCode;
                    if (deviceData.Descriptor != null)
                    {
                        model.DeviceConsoleAddress = deviceData.Descriptor.HostAddress;
                    }

                    if (deviceData.Properties != null)
                    {
                        model.Properties = deviceData.Properties;
                        model.IsWarning = deviceData.Properties.ContainsKey("IsWarning") ? deviceData.Properties["IsWarning"].ObjToBool() : false;
                        model.IsLoadingOrUnLoading = deviceData.Properties.ContainsKey("IsLoadingOrUnLoading")
                            ? deviceData.Properties["IsLoadingOrUnLoading"].ObjToBool() : false;
                        model.CallAgvMessage = deviceData.Properties.ContainsKey("CallAgvMessage") ? deviceData.Properties["CallAgvMessage"].ToString() : "";
                        model.Percentage = deviceData.Properties.ContainsKey("Percentage") ? deviceData.Properties["Percentage"].ToInt() : 0;
                    }
                }

                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 获取近7天AGV（完成/异常状态）调度记录
        /// </summary>
        /// <returns></returns>
        public async Task<List<Schedule>> GetSchedules()
        {
            var query = DBClient.Queryable<Device, Schedule>
                           ((d, s) => new object[]
                           {
                                JoinType.Left, d.Code == s.RequireDeviceId,
                           });
            query = query.Where((d, s) => d.IsDeleted == 0 && s.IsDeleted == 0
            && !string.IsNullOrEmpty(d.DeviceTypeCode) && d.DeviceTypeCode.ToLower().Equals("agv"));

            query = query.Where((d, s) => s.ScheduledTaskStatus == ScheduledTaskStatus.Failed
            || s.ScheduledTaskStatus == ScheduledTaskStatus.Completed);

            query = query.Where((d, s) => (s.CompletedTime >= DateTime.Today.AddDays(-6) && s.CompletedTime < DateTime.Today.AddDays(1))
            || (s.FailedTime >= DateTime.Today.AddDays(-6) && s.FailedTime < DateTime.Today.AddDays(1)));

            query = query.OrderBy((d, s) => d.Code);

            var data = await query.Select((d, s) => s).ToListAsync();
            return data;
        }

        /// <summary>
        /// 根据调度状态，统计近七天设备调度数量
        /// </summary>
        /// <param name="scheduledTaskStatus"></param>
        /// <returns></returns>
        public async Task<List<Schedule>> GetSchedulesByStatus(ScheduledTaskStatus scheduledTaskStatus)
        {
            var query = DBClient.Queryable<Device, Schedule>
                           ((d, s) => new object[]
                           {
                                JoinType.Left, d.Code == s.RequireDeviceId,
                           });
            query = query.Where((d, s) => d.IsDeleted == 0 && s.IsDeleted == 0
            && !string.IsNullOrEmpty(d.DeviceTypeCode) && d.DeviceTypeCode.ToLower().Equals("agv"));

            bool isQueryData = true;
            switch (scheduledTaskStatus)
            {
                case ScheduledTaskStatus.Failed:
                    query = query.Where((d, s) => s.ScheduledTaskStatus == ScheduledTaskStatus.Failed);
                    query = query.Where((d, s) => s.FailedTime >= DateTime.Today.AddDays(-6) && s.FailedTime < DateTime.Today.AddDays(1));
                    break;
                case ScheduledTaskStatus.Completed:
                    query = query.Where((d, s) => s.ScheduledTaskStatus == ScheduledTaskStatus.Completed);
                    query = query.Where((d, s) => s.CompletedTime >= DateTime.Today.AddDays(-6) && s.CompletedTime < DateTime.Today.AddDays(1));
                    break;
                case ScheduledTaskStatus.Canceled:
                    query = query.Where((d, s) => s.ScheduledTaskStatus == ScheduledTaskStatus.Canceled);
                    query = query.Where((d, s) => s.CanceledTime >= DateTime.Today.AddDays(-6) && s.CanceledTime < DateTime.Today.AddDays(1));
                    break;
                default:
                    isQueryData = false;
                    break;
            }

            if (!isQueryData)
            {
                return new List<Schedule>();
            }

            var data = await query.Select((d, s) => s).ToListAsync();
            return data;
        }

        /// <summary>
        /// 获取AGV最近调度记录
        /// </summary>
        /// <param name="devices"></param>
        /// <param name="queryCount"></param>
        /// <returns></returns>
        public async Task<List<Schedule>> GetLatestSchedule(List<CentralOnlineDeviceDto> devices, int queryCount)
        {
            List<Schedule> result = new List<Schedule>();

            List<string> deviceCodes = devices.Where(p => !string.IsNullOrEmpty(p.DeviceId))
                .Select(p => p.DeviceId).Distinct().ToList().ConvertAll(t => t.ToLower());
            if (deviceCodes == null || deviceCodes.Count == 0)
            {
                return result;
            }

            var query = DBClient.Queryable<Schedule>();

            query = query.Where(s => !string.IsNullOrEmpty(s.RequireDeviceId) && deviceCodes.Contains(s.RequireDeviceId.ToLower()));

            query = query.Where(s => s.ScheduledTaskStatus != ScheduledTaskStatus.Canceled);

            query = query.OrderByDescending(s => s.AllocateTime);

            var data = await query.Select(s => new Schedule()).ToListAsync();

            if (data == null || data.Count == 0)
            {
                return result;
            }

            foreach (var deviceCode in deviceCodes)
            {
                var scheduleData = data.FindAll(p => !string.IsNullOrEmpty(p.RequireDeviceId) && p.RequireDeviceId.ToLower().Equals(deviceCode.ToLower()));
                if (scheduleData == null || scheduleData.Count == 0)
                {
                    continue;
                }
                result.AddRange(scheduleData.Take(queryCount));
            }

            return result;
        }

        /// <summary>
        /// 根据设备Code获取绑定的工艺路线和工序（非AGV）
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public async Task<List<RouteAndProcessListByDevice>> GetRPListByDeviceCode(string deviceCode)
        {
            if (string.IsNullOrEmpty(deviceCode))
            {
                return new List<RouteAndProcessListByDevice> { };
            }

            var data = await GetRPListByNotAGVDevice(deviceCode);
            return data;
        }
    }
}
