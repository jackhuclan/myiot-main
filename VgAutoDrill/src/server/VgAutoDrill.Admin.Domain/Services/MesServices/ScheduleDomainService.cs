using SqlSugar;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Admin.Repository.Repository.Department;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class ScheduleDomainService : BaseDomainService<Schedule>, IScheduleDomainService
    {
        private readonly IScheduleRepository _repository;
        private readonly IRackRepository _rackrepository;
        private readonly IDeviceRepository _devicerepository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleDomainService(IUnitOfWork unitOfWork,
             IRackRepository rackrepository,
             IDeviceRepository devicerepository,
        IScheduleRepository repository)
        {
            _repository = repository;
            _rackrepository = rackrepository;
             _devicerepository= devicerepository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ExsistSchedule(GetScheduleListReq req)
        {
            return await _repository.ExsistSchedule(req);
        }

        public async Task<PageDto<ScheduleDto>> GetList(GetScheduleListReq req)
        {
            var pageDto = new PageDto<ScheduleDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList();
            if (pageDto.List?.Count > 0)
            {
                var subDeviceCodes = pageDto.List.Where(s => !string.IsNullOrWhiteSpace(s.SubDeviceCode)).Select(s => s.SubDeviceCode).Distinct().ToList();
                //var routeCodes = pageDto.List.Where(s => !string.IsNullOrWhiteSpace(s.RouteCode)).Select(s => s.RouteCode).Distinct().ToList();
                //var agvs = await _devicereAndRouteRepository.QueryAsync(s=> routeCodes.Contains(s.RouteCode),r=>r.Id,OrderByType.Asc);
                //var agvCodes = agvs.Select(s => s.DeviceCode).ToList();
                var racks = await _rackrepository.QueryAsync(s => subDeviceCodes.Contains(s.Code), r => r.Id, OrderByType.Asc);
                var requireDevices = pageDto.List.Where(s => !string.IsNullOrWhiteSpace(s.RequireDeviceId)).Select(s => s.RequireDeviceId).Distinct().ToList();
                var devices = await _devicerepository.QueryAsync(s => requireDevices.Contains(s.Code), r => r.Id, OrderByType.Asc);
                foreach (var item in pageDto.List)
                {
                    var positionCodes = racks.Where(s => s.Code?.ToLower() == item.SubDeviceCode?.ToLower() && !string.IsNullOrWhiteSpace(s.PositionCode)).Select(s => s.PositionCode).Distinct().ToList();
                    item.PositionCodes = positionCodes?.Count > 0 ? string.Join(",", positionCodes) : "";
                    item.DeviceStatus = devices.FirstOrDefault(s => item.RequireDeviceId == s.Code)?.DeviceStatus;
                }
            }
            return pageDto;
        }
        public async Task<PageDto<ScheduleInfo>> GetFullDataList(GetScheduleListReq req)
        {
            var pageDto = new PageDto<ScheduleInfo>(req.PageNum, req.PageSize);
            var result = await _repository.GetFullDataList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList();
            return pageDto;
        }
        public async Task<PageDto<ScheduleDto>> GetScheduleWithRequestList(GetScheduleListReq req)
        {
            var pageDto = new PageDto<ScheduleDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetScheduleWithRequestList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList();
            return pageDto;
        }

        /// <summary>
        /// 根据scheduleId更新库位panel信息
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="scheduleLocationPanels"></param>
        /// <returns></returns>
        public async Task<bool> UpdateLocationPanels(long scheduleId, List<ScheduleLocationPanel> scheduleLocationPanels)
        {
            return await _repository.UpdateLocationPanels(scheduleId, scheduleLocationPanels);
        }


    }
}
