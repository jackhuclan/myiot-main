using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceRecordsSummaryDomainService : BaseDomainService<DeviceRecordsSummary>, IDeviceRecordsSummaryDomainService
    {
        private readonly IDeviceRecordsSummaryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceRecordsSummaryDomainService(IUnitOfWork unitOfWork,
            IDeviceRecordsSummaryRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<DeviceRecordsSummaryDto>> GetList(GetDeviceRecordsSummaryListReq req)
        {
            var pageDto = new PageDto<DeviceRecordsSummaryDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<DeviceRecordsSummaryDto>>();
            return pageDto;
        }

        public async Task<DateTime?> GetMaxDate()
        {
            return await _repository.GetMaxDate();
        }

    }
}
