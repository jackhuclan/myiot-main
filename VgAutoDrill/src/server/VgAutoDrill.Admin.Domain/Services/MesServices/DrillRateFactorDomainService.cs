using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DrillRateFactorDomainService : BaseDomainService<DrillRateFactor>, IDrillRateFactorDomainService
    {
        private readonly IDrillRateFactorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DrillRateFactorDomainService(IUnitOfWork unitOfWork,
            IDrillRateFactorRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DrillRateFactorSummaryDto>> GetListByDate(List<string> deviceCodes, DateTime startDate, DateTime endDate)
        {
            return await _repository.GetListByDate(deviceCodes, startDate, endDate);
        }

        public async Task<List<DrillRateFactorDto>> GetRateReasonDetails(string deviceCode, DateTime startDate, DateTime endDate)
        {
            return await _repository.GetRateReasonDetails(deviceCode, startDate, endDate);
        }

        public async Task<List<DrillRateFactor>> GetLastRateReasonDatas(List<string> deviceCodes, DateTime startDate, DateTime endDate)
        {
            return await _repository.GetLastRateReasonDatas(deviceCodes, startDate, endDate);
        }
    }
}
