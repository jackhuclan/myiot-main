using AutoMapper;
using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceDomainService : BaseDomainService<Device>, IDeviceDomainService
    {
        private readonly IDeviceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeviceDomainService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IDeviceRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<DeviceDto>> PageList(GetDeviceListReq req)
        {
            var pageDto = new PageDto<DeviceDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DeviceDto>>(result.ToList());
            return pageDto;
        }

        public async Task<List<RouteAndProcessListByDevice>> GetRouteAndProcessList(string agvDeviceCode, string anyDeviceCode)
        {
            var result = await _repository.GetRouteAndProcessList(agvDeviceCode, anyDeviceCode);
            return result;
        }

        /// <summary>
        /// 获取在线设备绑定的工艺路线
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        public async Task<List<CentralOnlineDeviceDto>> GetOnlineDeviceInfo(List<CentralOnlineDeviceDto> devices)
        {
            var result = await _repository.GetOnlineDeviceInfo(devices);
            return result;
        }

        public async Task<IPageList<Device>> GetDrills(GetDeviceListReq req)
        {
            var result = await _repository.GetDrills(req);
            return result;
        }

        public async Task<List<DrillDeviceTaskDto>> GetDrillDeviceTask(List<CentralOnlineDeviceDto> devices)
        {
            var result = await _repository.GetDrillDeviceTask(devices);
            return result;
        }

        public async Task<List<Schedule>> GetSchedules()
        {
            var result = await _repository.GetSchedules();
            return result;
        }

        public async Task<List<Schedule>> GetSchedulesByStatus(ScheduledTaskStatus scheduledTaskStatus)
        {
            var result = await _repository.GetSchedulesByStatus(scheduledTaskStatus);
            return result;
        }

        public async Task<List<ScheduleDto>> GetLatestSchedule(List<CentralOnlineDeviceDto> devices, int queryCount)
        {
            var result = await _repository.GetLatestSchedule(devices, queryCount);
            return result.Adapt<List<ScheduleDto>>();
        }

        public async Task<List<RouteAndProcessListByDevice>> GetRPListByDeviceCode(string deviceCode)
        {
            var result = await _repository.GetRPListByDeviceCode(deviceCode);
            return result;
        }
    }
}
