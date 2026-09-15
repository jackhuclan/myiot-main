using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceTemporaryMaintenanceRecordsService : BaseServiceWithoutTree<DeviceTemporaryMaintenanceRecords, DeviceTemporaryMaintenanceRecordsDto, AddOrUpdateDeviceTemporaryMaintenanceRecordsReq>, IDeviceTemporaryMaintenanceRecordsService
    {
        private readonly IDeviceTemporaryMaintenanceRecordsDomainService _deviceTemporaryMaintenanceRecordsDomainService;
        public DeviceTemporaryMaintenanceRecordsService(IDeviceTemporaryMaintenanceRecordsDomainService deviceTemporaryMaintenanceRecordsDomainService, IDeviceDomainService deviceDomainService, IMapper mapper)
            : base(deviceTemporaryMaintenanceRecordsDomainService, mapper)
        {
            _deviceTemporaryMaintenanceRecordsDomainService = deviceTemporaryMaintenanceRecordsDomainService;
        }


        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceTemporaryMaintenanceRecordsDto>>> GetList(GetDeviceTemporaryMaintenanceRecordsListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var result = await _deviceTemporaryMaintenanceRecordsDomainService.GetList(req);
            return Success(result);

        }


    }
}
