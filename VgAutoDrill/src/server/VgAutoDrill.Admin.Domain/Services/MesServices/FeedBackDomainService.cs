using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class FeedBackDomainService : BaseDomainService<FeedBack>, IFeedBackDomainService
    {
        private readonly IFeedBackRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public FeedBackDomainService(IUnitOfWork unitOfWork,
            IFeedBackRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<FeedBackDto>> GetList(GetFeedBackListReq req)
        {
            var pageDto = new PageDto<FeedBackDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<FeedBackDto>>();
            return pageDto;
        }

        public async Task<PageDto<TaskDto>> GetTaskList(GetTaskListReq req)
        {
            var pageDto = new PageDto<TaskDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetTaskList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<TaskDto>>();
            return pageDto;
        }
    }
}
