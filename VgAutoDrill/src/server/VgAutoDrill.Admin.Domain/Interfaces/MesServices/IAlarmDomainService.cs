using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IAlarmDomainService : IBaseDomainService<Alarm>
    {
        Task<List<AlarmToExcelDto>> GetToExcelList(GetAlarmListReq req);
    }
}
