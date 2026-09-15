using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFile;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ItemDrillFileDomainService : BaseDomainService<ItemDrillFile>, IItemDrillFileDomainService
    {
        private readonly IItemDrillFileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ItemDrillFileDomainService(IUnitOfWork unitOfWork,
            IItemDrillFileRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<ItemDrillFileDto>> GetList(GetItemDrillFileListReq req)
        {
            var pageDto = new PageDto<ItemDrillFileDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<ItemDrillFileDto>>();
            return pageDto;
        }
    }
}