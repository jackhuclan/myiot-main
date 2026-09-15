using Mapster;
using SqlSugar;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroupDetail;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class CutterGroupDetailDomainService : BaseDomainService<CutterGroupDetail>, ICutterCroupDetailDomainService
    {
        private readonly ICutterGroupDetailRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CutterGroupDetailDomainService(IUnitOfWork unitOfWork,
            ICutterGroupDetailRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public async Task<CutterGroupDetailDto> GetDetailByTaskCode(string taskCode)
        {
            var result = new CutterGroupDetailDto();
            if (string.IsNullOrWhiteSpace(taskCode))
            {
                return result;
            }
            var data = await _repository.QueryAsync(s => (s.TaskCode ?? "").ToLower() == taskCode.ToLower()
                                                            && s.IsDeleted == 0, s => s.Id, OrderByType.Asc);
            if (data?.Count > 0)
            {
                result = data.Adapt<List<CutterGroupDetailDto>>().First();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskCodes"></param>
        /// <returns></returns>
        public async Task<List<CutterGroupDetailDto>> GetDetailByTaskCode(List<string> taskCodes)
        {
            var result = new List<CutterGroupDetailDto>();
            if (taskCodes?.Count <= 0)
            {
                return result;
            }
            var data = await _repository.QueryAsync(s => s.IsDeleted == 0 && taskCodes!.Contains(s.TaskCode ?? ""), s => s.Id, OrderByType.Asc);
            if (data?.Count > 0)
            {
                result = data.Adapt<List<CutterGroupDetailDto>>();
            }
            return result;
        }
    }
}
