using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class AlarmSettingRepository : BaseRepository<AlarmSetting>, IAlarmSettingRepository
    {
        public AlarmSettingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
