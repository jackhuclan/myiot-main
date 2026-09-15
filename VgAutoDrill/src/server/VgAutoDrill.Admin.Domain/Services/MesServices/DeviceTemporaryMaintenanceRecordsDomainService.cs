using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceTemporaryMaintenanceRecordsDomainService : BaseDomainService<DeviceTemporaryMaintenanceRecords>, IDeviceTemporaryMaintenanceRecordsDomainService
    {


        private readonly IDeviceTemporaryMaintenanceRecordsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceTemporaryMaintenanceRecordsDomainService(IUnitOfWork unitOfWork,
            IDeviceTemporaryMaintenanceRecordsRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageDto<DeviceTemporaryMaintenanceRecordsDto>> GetList(GetDeviceTemporaryMaintenanceRecordsListReq req)
        {
            var pageDto = new PageDto<DeviceTemporaryMaintenanceRecordsDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<DeviceTemporaryMaintenanceRecordsDto>>();
            return pageDto;
        }


    }
}
