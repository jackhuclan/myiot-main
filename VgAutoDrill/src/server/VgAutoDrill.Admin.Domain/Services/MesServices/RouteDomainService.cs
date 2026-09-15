using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class RouteDomainService : BaseDomainService<Route>, IRouteDomainService
    {
        private readonly IRouteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RouteDomainService(IUnitOfWork unitOfWork,
            IRouteRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckKeyProcess(long Id)
        {
            var data = await _repository.CheckKeyProcess(Id);
            return data;
        }

        /// <summary>
        /// 校验是否存在已提交的任务与该工艺路线关联
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> ExsitTask(long Id)
        {
            var isExsit = await _repository.ExsitTask(Id);
            return isExsit;
        }
    }
}
