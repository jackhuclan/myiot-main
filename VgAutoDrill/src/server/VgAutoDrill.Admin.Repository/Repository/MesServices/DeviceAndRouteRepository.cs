using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceAndRouteRepository : BaseRepository<DeviceAndRoute>, IDeviceAndRouteRepository
    {
        public DeviceAndRouteRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageList<DeviceInfoAndRouteInfo>> GetList(GetDeviceAndRouteListReq req)
        {
            var query = DBClient.Queryable<DeviceAndRoute, Route>
                ((dr, r) => new object[]
                    {
                        JoinType.Left, dr.RouteId == r.Id
                    });

            query = query.Where((dr, r) => dr.IsDeleted == 0 && r.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                query = query.Where((dr, r) => !string.IsNullOrEmpty(dr.DeviceCode) && dr.DeviceCode.Contains(req.DeviceCode));
            }
            if (!string.IsNullOrEmpty(req.DeviceName))
            {
                query = query.Where((dr, r) => !string.IsNullOrEmpty(dr.DeviceName) && dr.DeviceName.Contains(req.DeviceName));
            }
            if (!string.IsNullOrEmpty(req.DeviceTypeCode))
            {
                query = query.Where((dr, r) => !string.IsNullOrEmpty(dr.DeviceTypeCode) && dr.DeviceTypeCode.Contains(req.DeviceTypeCode));
            }
            if (!string.IsNullOrEmpty(req.RouteCode))
            {
                query = query.Where((dr, r) => !string.IsNullOrEmpty(dr.RouteCode) && dr.RouteCode.Contains(req.RouteCode));
            }
            if (!string.IsNullOrEmpty(req.RouteName))
            {
                query = query.Where((dr, r) => !string.IsNullOrEmpty(dr.RouteName) && dr.RouteName.Contains(req.RouteName));
            }
            if (req.DeviceId > 0)
            {
                query = query.Where((dr, r) => dr.DeviceId == req.DeviceId);
            }
            if (req.DeviceTypeId > 0)
            {
                query = query.Where((dr, r) => dr.DeviceTypeId == req.DeviceTypeId);
            }
            if (req.RouteId > 0)
            {
                query = query.Where((dr, r) => dr.RouteId == req.RouteId);
            }

            query = query.OrderBy((dr, r) => dr.DeviceCode);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((dr, r) => new DeviceInfoAndRouteInfo
            {
                Id = dr.Id,
                DeviceCode = dr.DeviceCode,
                DeviceId = dr.DeviceId,
                DeviceName = dr.DeviceName,
                DeviceTypeCode = dr.DeviceTypeCode,
                DeviceTypeId = dr.DeviceTypeId,
                RouteId = dr.RouteId,
                RouteCode = r.Code,
                RouteName = r.Name,
                RouteDesc = r.RouteDesc,
                RouteRemark = r.Remark,
                RouteVettingStatus = r.VettingStatus,
                Status = r.Status,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((dr, r) => new DeviceInfoAndRouteInfo
                {
                    Id = dr.Id,
                    DeviceCode = dr.DeviceCode,
                    DeviceId = dr.DeviceId,
                    DeviceName = dr.DeviceName,
                    DeviceTypeCode = dr.DeviceTypeCode,
                    DeviceTypeId = dr.DeviceTypeId,
                    RouteId = dr.RouteId,
                    RouteCode = r.Code,
                    RouteName = r.Name,
                    RouteDesc = r.RouteDesc,
                    RouteRemark = r.Remark,
                    RouteVettingStatus = r.VettingStatus,
                    Status = r.Status,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<DeviceInfoAndRouteInfo>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        public async Task<PageList<DeviceFullDataAndRouteInfo>> GetDeviceAndRouteList(GetDeviceFullDataAndRouteInfoReq req)
        {
            List<DeviceFullDataAndRouteInfo> resultList = new List<DeviceFullDataAndRouteInfo>();

            var query = DBClient.Queryable<Device, DeviceType>
                ((device, deviceType) => new object[]
                    {
                        JoinType.Left, device.DeviceTypeId == deviceType.Id
                    });

            query = query.Where((device, deviceType) => device.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DeviceName))
            {
                query = query.Where((device, deviceType) => !string.IsNullOrEmpty(device.Name) && device.Name.Contains(req.DeviceName));
            }
            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                query = query.Where((device, deviceType) => !string.IsNullOrEmpty(device.Code) && device.Code.Contains(req.DeviceCode));
            }
            if (req.DeviceStatusList != null && req.DeviceStatusList.Count > 0)
            {
                query = query.Where((device, deviceType) => device.DeviceStatus != null && req.DeviceStatusList.Contains((DeviceStatus)device.DeviceStatus));
            }
            if (req.RequestDeviceKindList != null && req.RequestDeviceKindList.Count > 0)
            {
                query = query.Where((device, deviceType) => device.DeviceKind != null && req.RequestDeviceKindList.Contains((DeviceKind)device.DeviceKind));
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
            List<Device> deviceData = new List<Device>();

            if (!string.IsNullOrEmpty(req.RouteCode))
            {
                deviceData = await query.Select((device, deviceType) => device).ToListAsync();
            }
            else
            {
                deviceData = await query.Select((device, deviceType) => device).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

                int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
                if (pageCount < req.PageNum && (deviceData == null || deviceData.Count == 0))
                {
                    req.PageNum = pageCount;
                    deviceData = await query.Select((device, deviceType) => device).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
                }
            }

            if (deviceData == null || deviceData.Count == 0)
            {
                return new PageList<DeviceFullDataAndRouteInfo>(resultList, req.PageNum, req.PageSize, totalCount);
            }
            foreach (var item in deviceData)
            {
                resultList.Add(new DeviceFullDataAndRouteInfo
                {
                    Id = (int)item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    DeviceTypeId = item.DeviceTypeId,
                    DeviceTypeCode = item.DeviceTypeCode,
                    DeviceStatus = item.DeviceStatus,
                    WorkStationId = item.WorkStationId,
                    CreateTime = item.CreateTime,
                    DeviceBrand = item.DeviceBrand,
                    DeviceSpec = item.DeviceSpec,
                    DeviceVendorId = item.DeviceVendorId,
                    MaintainPeriodDays = item.MaintainPeriodDays,
                    Parameters = item.Parameters,
                    ParentId = (int)item.ParentId,
                    Status = item.Status,
                    ProductionTime = item.ProductionTime,
                    DeviceKind = item.DeviceKind,
                    SpindleNum = item.SpindleNum,
                });
            }

            var queryDR = DBClient.Queryable<DeviceAndRoute>();

            if (!string.IsNullOrEmpty(req.DeviceName))
            {
                queryDR = queryDR.Where(p => !string.IsNullOrEmpty(p.DeviceName) && p.DeviceName.Contains(req.DeviceName));
            }
            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                queryDR = queryDR.Where(p => !string.IsNullOrEmpty(p.DeviceCode) && p.DeviceCode.Contains(req.DeviceCode));
            }

            if (!string.IsNullOrEmpty(req.DeviceTypeCode))
            {
                queryDR = queryDR.Where(p => !string.IsNullOrEmpty(p.DeviceTypeCode) && p.DeviceTypeCode.ToLower().Equals(req.DeviceTypeCode.ToLower()));
            }
            if (req.DeviceTypeId > 0)
            {
                queryDR = queryDR.Where(p => p.DeviceTypeId == req.DeviceTypeId);
            }

            var DRData = await queryDR.Select(p => new DeviceAndRoute { }).ToListAsync();

            if (DRData != null && DRData.Count > 0)
            {
                foreach (var item in resultList)
                {
                    var drByDeviceList = DRData.Where(p => p.DeviceId == item.Id).ToList();
                    if (drByDeviceList == null || drByDeviceList.Count == 0)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(req.RouteCode))
                    {
                        bool isExsit = drByDeviceList.Exists(p => !string.IsNullOrEmpty(p.RouteCode) && p.RouteCode.Equals(req.RouteCode));
                        if (!isExsit)
                        {
                            continue;
                        }
                    }

                    foreach (var routeInfo in drByDeviceList)
                    {
                        if (string.IsNullOrEmpty(item.RouteNameDescription))
                        {
                            item.RouteNameDescription = routeInfo.RouteCode;
                        }
                        else
                        {
                            item.RouteNameDescription = $"{item.RouteNameDescription},{routeInfo.RouteCode}";
                        }
                    }
                }

            }

            if (!string.IsNullOrEmpty(req.RouteCode))
            {
                resultList = resultList.Where(p => !string.IsNullOrEmpty(p.RouteNameDescription)).ToList();
                totalCount = resultList.Count;

                resultList = resultList.Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();
            }

            return new PageList<DeviceFullDataAndRouteInfo>(resultList, req.PageNum, req.PageSize, totalCount);
        }

    }
}
