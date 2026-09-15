using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ItemAtpFileDomainService : BaseDomainService<ItemAtpFile>, IItemAtpFileDomainService
    {
        private readonly IItemAtpFileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ItemAtpFileDomainService(IUnitOfWork unitOfWork,
            IItemAtpFileRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<ItemAtpFileDto>> GetList(GetItemAtpFileListReq req)
        {
            var pageDto = new PageDto<ItemAtpFileDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<ItemAtpFileDto>>();
            return pageDto;
        }
    }
}
