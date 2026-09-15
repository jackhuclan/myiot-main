using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDrillRateFactorDomainService : IBaseDomainService<DrillRateFactor>
    {
        Task<List<DrillRateFactorSummaryDto>> GetListByDate(List<string> deviceCodes, DateTime startDate, DateTime endDate);
        Task<List<DrillRateFactorDto>> GetRateReasonDetails(string deviceCode, DateTime startDate, DateTime endDate);
        Task<List<DrillRateFactor>> GetLastRateReasonDatas(List<string> deviceCodes, DateTime startDate, DateTime endDate);
    }
}
