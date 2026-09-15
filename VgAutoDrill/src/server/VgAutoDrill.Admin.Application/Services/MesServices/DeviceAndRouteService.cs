using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceAndRouteService : BaseServiceWithoutTree<DeviceAndRoute, DeviceAndRouteDto, AddOrUpdateDeviceAndRouteReq>, IDeviceAndRouteService
    {
        private readonly IDeviceAndRouteDomainService _deviceAndRouteDomainService;
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public DeviceAndRouteService(IDeviceAndRouteDomainService domainService, IUnitOfWork unitOfWork, IMapper mapper)
            : base(domainService, mapper)
        {
            _deviceAndRouteDomainService = domainService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceInfoAndRouteInfo>>> GetList(GetDeviceAndRouteListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _deviceAndRouteDomainService.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 根据设备返回 工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceAndRouteInfo>>> GetInfosByDevice(GetDeviceAndRouteListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DeviceAndRouteInfo>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DeviceAndRoute>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DeviceCode) && p.DeviceCode.StartsWith(req.DeviceCode.Trim()));
            }
            if (!string.IsNullOrEmpty(req.DeviceName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DeviceName) && p.DeviceName.Contains(req.DeviceName));
            }
            if (req.DeviceId > 0)
            {
                where = where.And(p => p.DeviceId == req.DeviceId);
            }

            var data = await _domainService.QueryAsync(where, p => p.CreateTime, OrderByType.Desc);
            if (data == null || data.Count == 0)
            {
                pageDto.Total = 0;
                pageDto.List = new List<DeviceAndRouteInfo>();
                return Success(pageDto);
            }

            var dataByDevice = data.DistinctBy(p => p.DeviceCode).Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();

            List<DeviceAndRouteInfo> result = new List<DeviceAndRouteInfo>();
            foreach (var item in dataByDevice)
            {
                var dataInfos = data.FindAll(p => p.DeviceCode == item.DeviceCode);
                foreach (var info in dataInfos)
                {
                    if (result.Exists(p => p.DeviceCode == info.DeviceCode))
                    {
                        var model = result.SingleOrDefault(p => p.DeviceCode == info.DeviceCode);
                        if (model != null && !string.IsNullOrEmpty(info.RouteName))
                        {
                            model.RouteNameDescription = $"{model.RouteNameDescription},{info.RouteName}";
                        }
                    }
                    else
                    {
                        result.Add(new DeviceAndRouteInfo
                        {
                            DeviceCode = info.DeviceCode,
                            DeviceId = info.DeviceId,
                            DeviceName = info.DeviceName,
                            DeviceTypeCode = info.DeviceTypeCode,
                            DeviceTypeId = info.DeviceTypeId,
                            RouteNameDescription = info.RouteName,
                            Id = info.Id,
                            CreateTime = info.CreateTime,
                            CreatorId = info.CreatorId,
                            ModifierId = info.ModifierId,
                            ModifyTime = info.ModifyTime,
                            Status = info.Status,
                        });
                    }
                }
            }

            pageDto.Total = result.Count;
            pageDto.List = result;
            return Success(pageDto);
        }

        /// <summary>
        /// 获取调度配置设备列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceFullDataAndRouteInfo>>> GetDeviceAndRouteList(GetDeviceFullDataAndRouteInfoReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _deviceAndRouteDomainService.GetDeviceAndRouteList(req);
            return Success(result);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddDeviceAndRouteListReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (req.RouteList == null || req.RouteList.Count == 0)
            {
                return Fail("未识别有效的工艺路线!");
            }

            if (string.IsNullOrEmpty(req.DeviceCode) || req.DeviceId == null || req.DeviceId == 0)
            {
                return Fail("未识别有效的设备!");
            }

            List<DeviceAndRoute> addList = new List<DeviceAndRoute>();
            foreach (var item in req.RouteList)
            {
                if (item.VettingStatus == 0)
                {
                    return Fail(item.Code + " 工艺路线未审批!");
                }

                addList.Add(new DeviceAndRoute
                {
                    DeviceId = req.DeviceId,
                    DeviceCode = req.DeviceCode,
                    DeviceName = req.DeviceName,
                    DeviceTypeCode = req.DeviceTypeCode,
                    DeviceTypeId = req.DeviceTypeId,
                    RouteCode = item.Code,
                    RouteName = item.Name,
                    RouteId = item.Id,
                    CreateTime = DateTime.Now,
                    CreatorId = UserId,
                    Status = (int)DataStatusEnum.Enable
                });
            }
            _unitOfWork.BeginTran();

            await _domainService.DeleteAsync(p => p.DeviceId == req.DeviceId);

            var result = await _domainService.BulkInsert(addList);
            if (!result)
            {
                return Fail("添加失败！");
            }

            _unitOfWork.CommitTran();
            return Success();
        }
    }
}
