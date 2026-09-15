using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceAndSubjectDomainService : BaseDomainService<DeviceAndSubject>, IDeviceAndSubjectDomainService
    {
        private readonly IDeviceAndSubjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceAndSubjectDomainService(IUnitOfWork unitOfWork,
            IDeviceAndSubjectRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<PageDto<DeviceAndSubjectDto>> GetList(GetDeviceAndSubjectListReq req)
        {
            var pageDto = new PageDto<DeviceAndSubjectDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<DeviceAndSubjectDto>>();
            return pageDto;
        }
    }
}
