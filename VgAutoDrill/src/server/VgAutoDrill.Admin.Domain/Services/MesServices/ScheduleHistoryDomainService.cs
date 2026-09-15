using SqlSugar;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ScheduleHistoryDomainService : BaseDomainService<ScheduleHistory>, IScheduleHistoryDomainService
    {
        private readonly IScheduleHistoryRepository _repository;
        private readonly IRackRepository _rackrepository;
        private readonly IDeviceRepository _devicerepository;
        private readonly IUnitOfWork _unitOfWork;


        public ScheduleHistoryDomainService(IUnitOfWork unitOfWork,
             IRackRepository rackrepository,
             IDeviceRepository devicerepository,
        IScheduleHistoryRepository repository)
        {
            _repository = repository;
            _devicerepository = devicerepository;
            _rackrepository = rackrepository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
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


    }
}