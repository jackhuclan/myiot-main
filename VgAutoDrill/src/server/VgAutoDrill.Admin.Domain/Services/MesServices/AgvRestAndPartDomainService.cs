using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class AgvRestAndPartDomainService : BaseDomainService<AgvRestAndPart>, IAgvRestAndPartDomainService
    {
        private readonly IAgvRestAndPartRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AgvRestAndPartDomainService(IUnitOfWork unitOfWork,
            IAgvRestAndPartRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageDto<RestAndPartDto>> GetList(GetRestAndPartListReq req)
        {
            var pageDto = new PageDto<RestAndPartDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<RestAndPartDto>>();
            return pageDto;
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<RestAndPartDto> QueryByID(long id)
        {
            var result = await _repository.QueryByID(id);
            return result;
        }
    }
}
