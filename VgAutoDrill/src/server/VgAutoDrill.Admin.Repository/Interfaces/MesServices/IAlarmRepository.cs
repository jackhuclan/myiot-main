using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;

namespace VgAutoDrill.Admin.Repository.Interfaces.Equipment
{
    public interface IAlarmRepository : IBaseRepository<Alarm>
    {
        Task<List<AlarmToExcelDto>> GetToExcelList(GetAlarmListReq req);
    }
}
