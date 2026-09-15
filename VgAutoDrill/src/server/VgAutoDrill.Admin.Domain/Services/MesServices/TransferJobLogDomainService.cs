using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class TransferJobLogDomainService : BaseDomainService<TransferJobLog>, ITransferJobLogDomainService
    {
        private readonly ITransferJobLogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferJobLogDomainService(IUnitOfWork unitOfWork,
            ITransferJobLogRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<TransferJobLogDto>> GetList(GetTransferJobLogListReq req)
        {
            var pageDto = new PageDto<TransferJobLogDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<TransferJobLogDto>>();
            return pageDto;
        }
    }
}
