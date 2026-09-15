using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceRecordsDomainService : BaseDomainService<DeviceRecords>, IDeviceRecordsDomainService
    {
        private readonly IDeviceRecordsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceRecordsDomainService(IUnitOfWork unitOfWork,
            IDeviceRecordsRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<DeviceRecordsDto>> GetList(GetDeviceRecordsListReq req)
        {
            var pageDto = new PageDto<DeviceRecordsDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<DeviceRecordsDto>>();
            return pageDto;
        }

        public async Task<DateTime?> GetMinDate()
        {
            return await _repository.GetMinDate();
        }

        public async Task<List<DeviceRecords>> GetListByDate(DateTime startDate, DateTime endDate)
        {
            return await _repository.GetListByDate(startDate, endDate);
        }
    }
}
