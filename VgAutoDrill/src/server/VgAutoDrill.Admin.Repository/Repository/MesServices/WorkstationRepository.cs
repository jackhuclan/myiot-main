using SqlSugar;
using System.Text;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class WorkstationRepository : BaseRepository<WorkStation>, IWorkstationRepository
    {
        public WorkstationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// 设备负载查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IPageList<WorkStationLoadTaskDto>> GetWorkStationLoadTask(GetWorkStationLoadTaskReq req)
        {
            var query = DBClient.Queryable<Device, DeviceType, WorkStation, RouteProcessAndWorkStation, RouteAndProcess, Route>
                ((d, dt, w, rpw, rp, r) => new object[]
                    {
                        JoinType.Left, d.DeviceTypeId == dt.Id,
                        JoinType.Left, d.WorkStationId == w.Id,
                        JoinType.Left, w.Id == rpw.WorkStationId,
                        JoinType.Left, rpw.RouteAndProcessId == rp.Id,
                        JoinType.Left, rp.RouteId == r.Id,
                    });
            query = query.Where((d, dt, w, rpw, rp, r) => dt.IsManufacture == 1 && d.IsDeleted == 0 && w.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                query = query.Where((d, dt, w, rpw, rp, r) => !string.IsNullOrEmpty(w.ProcessCode) && w.ProcessCode.ToLower().Equals(req.ProcessCode.ToLower()));
            }

            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                query = query.Where((d, dt, w, rpw, rp, r) => !string.IsNullOrEmpty(w.Code) && w.Code.Contains(req.WorkStationCode));
            }

            if (req.DeviceStatusList != null && req.DeviceStatusList.Count > 0)
            {
                query = query.Where((d, dt, w, rpw, rp, r) => d.DeviceStatus != null && req.DeviceStatusList.Contains((DeviceStatus)d.DeviceStatus));
            }
            if (req.RequestDeviceKindList != null && req.RequestDeviceKindList.Count > 0)
            {
                query = query.Where((d, dt, w, rpw, rp, r) => d.DeviceKind != null && req.RequestDeviceKindList.Contains((DeviceKind)d.DeviceKind));
            }
            query = query.OrderBy((d, dt, w, rpw, rp, r) => w.Code);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((d, dt, w, rpw, rp, r) => new WorkStationLoadTaskDto
            {
                Id = w.Id,
                Code = w.Code,
                Name = w.Name,
                ProcessCode = w.ProcessCode,
                ProcessId = w.ProcessId,
                ProcessName = w.ProcessName,
                RouteCode = r.Code,
                RouteId = r.Id,
                RouteName = r.Name,
                DeviceStatus = d.DeviceStatus,
                CreateTime = w.CreateTime,
                DeviceKind = d.DeviceKind,
            }).ToListAsync();

            if (data == null || data.Count == 0)
            {
                var list = new PageList<WorkStationLoadTaskDto>(new List<WorkStationLoadTaskDto>(), req.PageNum, req.PageSize, totalCount);
                return list;
            }

            List<WorkStationLoadTaskDto> workStationsByRoute = data;

            if (!string.IsNullOrEmpty(req.RouteCode))
            {
                workStationsByRoute = data.FindAll(p => !string.IsNullOrEmpty(p.RouteCode) && p.RouteCode.ToLower().Equals(req.RouteCode.ToLower()));
            }

            if (workStationsByRoute == null || workStationsByRoute.Count == 0)
            {
                var list = new PageList<WorkStationLoadTaskDto>(new List<WorkStationLoadTaskDto>(), req.PageNum, req.PageSize, totalCount);
                return list;
            }

            var dataDistinct = workStationsByRoute.DistinctBy(p => p.Code).ToList();

            totalCount = dataDistinct.Count;
            var workStationList = dataDistinct.Skip(req.PageSize * (req.PageNum - 1)).Take(req.PageSize).ToList();

            List<string> workStationCodes = new List<string>();
            foreach (var item in workStationList)
            {
                if (string.IsNullOrEmpty(item.Code))
                {
                    continue;
                }
                workStationCodes.Add(item.Code.ToLower());

                var WSRoutes = data.FindAll(p => p.Code == item.Code).OrderBy(p => p.RouteCode).ToList();
                if (WSRoutes == null || WSRoutes.Count == 0)
                {
                    continue;
                }
                WSRoutes = WSRoutes.DistinctBy(p => p.RouteId).ToList();

                StringBuilder routeCodeStr = new StringBuilder();
                StringBuilder routeNameStr = new StringBuilder();
                foreach (var route in WSRoutes)
                {
                    if (!string.IsNullOrEmpty(route.RouteCode))
                    {
                        routeCodeStr.Append(route.RouteCode + ",");
                    }

                    if (!string.IsNullOrEmpty(route.RouteName))
                    {
                        routeNameStr.Append(route.RouteName + ",");
                    }
                }
                item.RouteCode = routeCodeStr.ToString().TrimEnd(',');
                item.RouteName = routeNameStr.ToString().TrimEnd(',');
            }

            var queryTask = DBClient.Queryable<Model.Entites.Mes.WorkTask>();
            #region queryTask查询条件
            if (!string.IsNullOrEmpty(req.TaskCode))
            {
                queryTask = queryTask.Where(t => !string.IsNullOrEmpty(t.Code) && t.Code.Contains(req.TaskCode));
            }

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                queryTask = queryTask.Where(t => !string.IsNullOrEmpty(t.WorkOrderCode) && t.WorkOrderCode.Contains(req.WorkOrderCode));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                queryTask = queryTask.Where(t => !string.IsNullOrEmpty(t.ItemCode) && t.ItemCode.Contains(req.ItemCode));
            }

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                queryTask = queryTask.Where(t => !string.IsNullOrEmpty(t.ProcessCode) && t.ProcessCode.StartsWith(req.ProcessCode.Trim()));
            }

            if (!string.IsNullOrEmpty(req.RouteCode))
            {
                queryTask = queryTask.Where(t => !string.IsNullOrEmpty(t.RouteCode) && t.RouteCode.StartsWith(req.RouteCode.Trim()));
            }

            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                queryTask = queryTask.Where(t => !string.IsNullOrEmpty(t.WorkStationCode) && t.WorkStationCode.Contains(req.WorkStationCode));
            }

            if (req.TaskStatusList != null && req.TaskStatusList.Count > 0)
            {
                queryTask = queryTask.Where(t => t.TaskStatus != null && req.TaskStatusList.Contains((TaskStatusEnum)t.TaskStatus));
            }

            if (req.QueryStartTime != null)
            {
                queryTask = queryTask.Where(t => t.CreateTime >= req.QueryStartTime.Value);
            }
            if (req.QueryEndTime != null)
            {
                queryTask = queryTask.Where(t => t.CreateTime <= req.QueryEndTime.Value);
            }

            if (workStationCodes != null && workStationCodes.Count > 0)
            {
                queryTask = queryTask.Where(t => !string.IsNullOrEmpty(t.WorkStationCode) && workStationCodes.Contains(t.WorkStationCode));
            }
            #endregion

            var taskData = await queryTask.ToListAsync();
            if (taskData == null || taskData.Count == 0)
            {
                return new PageList<WorkStationLoadTaskDto>(workStationList, req.PageNum, req.PageSize, totalCount);
            }

            foreach (var item in workStationList)
            {
                var taskList = taskData.FindAll(p => p.WorkStationCode == item.Code);
                if (taskList == null || taskList.Count == 0)
                {
                    continue;
                }

                item.TaskCount = taskList.Count;
            }

            return new PageList<WorkStationLoadTaskDto>(workStationList, req.PageNum, req.PageSize, totalCount);
        }
    }
}
